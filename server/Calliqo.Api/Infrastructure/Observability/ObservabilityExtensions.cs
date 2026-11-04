using System.Diagnostics;
using Microsoft.AspNetCore.Routing;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Calliqo.Api.Infrastructure.Observability;

public static class ObservabilityExtensions
{
    private const string RequestLogCategory = "Calliqo.Requests";

    public static WebApplicationBuilder AddCalliqoObservability(
        this WebApplicationBuilder builder)
    {
        builder.Logging.AddJsonConsole(options =>
        {
            options.IncludeScopes = false;
            options.UseUtcTimestamp = true;
            options.TimestampFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        });

        if (!builder.Configuration.GetValue<bool>("Observability:Enabled"))
        {
            return builder;
        }

        var endpointText = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

        if (!Uri.TryCreate(endpointText, UriKind.Absolute, out var endpoint) ||
            endpoint.Scheme is not ("http" or "https"))
        {
            throw new InvalidOperationException(
                "Observability requires an absolute HTTP(S) OTLP endpoint.");
        }

        builder.Logging.AddFilter<OpenTelemetryLoggerProvider>(
            (category, level) =>
                category == RequestLogCategory &&
                level >= LogLevel.Information);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService("Calliqo.Api")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment.name"] =
                        builder.Environment.EnvironmentName
                }))
            .WithTracing(tracing => tracing
                    .AddSource("Npgsql")
                    .AddProcessor(new NpgsqlTracePrivacyProcessor())
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = false;

                    options.EnrichWithHttpResponse = (activity, response) =>
                    {
                        var route = GetRoute(response.HttpContext);

                        activity.DisplayName =
                            $"{response.HttpContext.Request.Method} {route}";

                        activity.SetTag("url.path", null);
                        activity.SetTag("url.query", null);
                        activity.SetTag("user_agent.original", null);
                    };
                }))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation())
            .WithLogging(
                configureBuilder: null,
                configureOptions: options =>
                {
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = false;
                })
            .UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, endpoint);

        return builder;
    }

    public static WebApplication UseCalliqoRequestLogging(
        this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger(RequestLogCategory);

        app.Use(async (HttpContext context, RequestDelegate next) =>
        {
            var started = Stopwatch.GetTimestamp();
            var statusCode = StatusCodes.Status500InternalServerError;

            try
            {
                await next(context);
                statusCode = context.Response.StatusCode;
            }
            finally
            {
                var elapsedMs =
                    Stopwatch.GetElapsedTime(started).TotalMilliseconds;

                var traceId =
                    Activity.Current?.TraceId.ToString()
                    ?? context.TraceIdentifier;

                logger.Log(
                    statusCode >= 500
                        ? LogLevel.Error
                        : LogLevel.Information,
                    new EventId(1000, "RequestCompleted"),
                    "HTTP {Method} {Route} responded {StatusCode} " +
                    "in {ElapsedMs} ms TraceId={TraceId}",
                    context.Request.Method,
                    GetRoute(context),
                    statusCode,
                    elapsedMs,
                    traceId);
            }
        });

        return app;
    }

    private static string GetRoute(HttpContext context) =>
        (context.GetEndpoint() as RouteEndpoint)?
            .RoutePattern.RawText ?? "unmatched";
}