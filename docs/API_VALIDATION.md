# Weather validation walkthrough — superseded

## Disposition — 2026-11-02
Cancelled as unnecessary scaffold work by owner agreement. Do not implement
the proposed days query, weather-specific validation tests, or the associated
OpenAPI response additions. This was a TECH-003 subtask, not a separate issue;
it is cancelled, not completed or verified.

Existing versioned routing, Problem Details, OpenAPI, and persistence tests
remain useful foundation checks. No source removal is authorized or performed
by this documentation update. Replace weather coverage when real endpoints
provide equivalent checks; removal belongs with that future implementation.

Request validation remains required. It is transferred to
[AUTH-002 (#10)](https://github.com/Ravynnox/Calliqo/issues/10) for the first
real sign-in/team request endpoint: valid requests, field-specific 400
Problem Details with traceId, and invalid-input contract tests.
If endpoint sequencing changes, transfer this criterion explicitly to the
first real write endpoint before closing AUTH-002.

[TECH-003 (#7)](https://github.com/Ravynnox/Calliqo/issues/7) stays In Progress
for GitHub Actions and verified CI checks. Its completion no longer requires
extending the weather example. The proposed 16-test target is retired; the
last owner-reported passing suite contains the four existing backend tests.
