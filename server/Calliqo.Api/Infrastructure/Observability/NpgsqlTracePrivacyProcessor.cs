using System.Diagnostics;
using OpenTelemetry;

namespace Calliqo.Api.Infrastructure.Observability;

public sealed class NpgsqlTracePrivacyProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        if (activity.Source.Name != "Npgsql")
        {
            return;
        }

        activity.DisplayName = "PostgreSQL";

        activity.SetTag("db.query.text", null);
        activity.SetTag("db.statement", null);
        activity.SetTag("db.npgsql.data_source", null);

        if (activity.Status == ActivityStatusCode.Error)
        {
            activity.SetStatus(ActivityStatusCode.Error);
        }
    }
}