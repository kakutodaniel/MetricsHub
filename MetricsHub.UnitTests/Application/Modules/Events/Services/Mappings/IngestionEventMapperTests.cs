using MetricsHub.Application.Modules.Events.DTOs;
using MetricsHub.Application.Modules.Events.Services.Mappings;

namespace MetricsHub.UnitTests.Application.Modules.Events.Services.Mappings
{
    public class IngestionEventMapperTests
    {
        [Fact]
        public void ToEvent_ShouldMapAlertPayload_Correctly()
        {
            // Arrange
            var alertPayload = new AlertPayload(
                AlertId: "alert-123",
                FiredAt: "2026-05-11T10:30:00Z",
                CorrelationId: "corr-456",
                Rule: new Rule(
                    Name: "HighCPU",
                    Threshold: 65.0,
                    Actual: 68.5,
                    Metric: "cpu_pct",
                    Severity: "warning"),
                Resource: new Resource(
                    Name: "NODE-01",
                    Group: "eu-west",
                    Environment: "production"
                )
            );

            // Act
            var result = alertPayload.ToEvent();

            // Assert
            Assert.NotNull(result);

            Assert.Equal("alert-123", result.EventId);
            Assert.Equal("alert", result.Source);

            Assert.Equal(
                DateTimeOffset.Parse("2026-05-11T10:30:00Z").UtcDateTime,
                result.Timestamp
            );

            Assert.Equal("node-01", result.ResourceName);
            Assert.Equal("eu-west", result.Region);

            Assert.NotNull(result.Attributes);

            Assert.Equal("production", result.Attributes[nameof(alertPayload.Resource.Environment)]);

            var rule = Assert.IsType<Rule>(result.Attributes[nameof(alertPayload.Rule)]);
            Assert.Equal("HighCPU", rule.Name);
            Assert.Equal(65.0, rule.Threshold);
            Assert.Equal(68.5, rule.Actual);
            Assert.Equal("cpu_pct", rule.Metric);
            Assert.Equal("warning", rule.Severity);

            Assert.Equal("corr-456", result.CorrelationId);
        }

        [Fact]
        public void ToEvent_ShouldMapPulsePayload_Correctly()
        {
            // Arrange
            var pulsePayload = new PulsePayload(
                PulseId: "pulse-789",
                Ts: "2026-05-11T12:00:00Z",
                Node: "NODE-02",
                Region: "us-east",
                Metrics: new Metrics(
                    CpuPct: 82.5,
                    MemMb: 4096,
                    ActiveConns: 120,
                    Status: "healthy"
                ),
                Tags: ["api", "prod"]
            );

            // Act
            var result = pulsePayload.ToEvent();

            // Assert
            Assert.NotNull(result);

            Assert.Equal("pulse-789", result.EventId);
            Assert.Equal("pulse", result.Source);

            Assert.Equal(
                DateTimeOffset.Parse("2026-05-11T12:00:00Z").UtcDateTime,
                result.Timestamp
            );

            Assert.Equal("node-02", result.ResourceName);
            Assert.Equal("us-east", result.Region);

            Assert.NotNull(result.Metrics);

            Assert.Equal(82.5, result.Metrics[nameof(pulsePayload.Metrics.CpuPct)]);
            Assert.Equal(4096, result.Metrics[nameof(pulsePayload.Metrics.MemMb)]);
            Assert.Equal(120, result.Metrics[nameof(pulsePayload.Metrics.ActiveConns)]);

            Assert.NotNull(result.Attributes);

            Assert.Equal("healthy", result.Attributes[nameof(pulsePayload.Metrics.Status)]);

            Assert.Equal(pulsePayload.Tags, result.Attributes[nameof(pulsePayload.Tags)]);
        }
    }
}
