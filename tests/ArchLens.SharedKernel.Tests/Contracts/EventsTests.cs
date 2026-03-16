using ArchLens.Contracts.Enums;
using ArchLens.Contracts.Events;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Contracts;

public class EventsTests
{
    [Fact]
    public void DiagramUploadedEvent_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        var evt = new DiagramUploadedEvent
        {
            DiagramId = id,
            FileName = "diagram.png",
            FileHash = "abc123",
            StoragePath = "/uploads/diagram.png",
            UserId = "user-1",
            Timestamp = timestamp
        };

        evt.DiagramId.Should().Be(id);
        evt.FileName.Should().Be("diagram.png");
        evt.FileHash.Should().Be("abc123");
        evt.StoragePath.Should().Be("/uploads/diagram.png");
        evt.UserId.Should().Be("user-1");
        evt.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void DiagramUploadedEvent_DefaultValues_ShouldBeEmpty()
    {
        var evt = new DiagramUploadedEvent();

        evt.FileName.Should().BeEmpty();
        evt.FileHash.Should().BeEmpty();
        evt.StoragePath.Should().BeEmpty();
        evt.UserId.Should().BeNull();
    }

    [Fact]
    public void ProcessingStartedEvent_ShouldSetProperties()
    {
        var analysisId = Guid.NewGuid();
        var diagramId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        var evt = new ProcessingStartedEvent
        {
            AnalysisId = analysisId,
            DiagramId = diagramId,
            StoragePath = "/path",
            Timestamp = timestamp
        };

        evt.AnalysisId.Should().Be(analysisId);
        evt.DiagramId.Should().Be(diagramId);
        evt.StoragePath.Should().Be("/path");
        evt.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void AnalysisCompletedEvent_ShouldSetProperties()
    {
        var providers = new List<string> { "OpenAI", "Gemini" };

        var evt = new AnalysisCompletedEvent
        {
            AnalysisId = Guid.NewGuid(),
            DiagramId = Guid.NewGuid(),
            ResultJson = "{\"result\": true}",
            ProvidersUsed = providers,
            ProcessingTimeMs = 1500,
            Timestamp = DateTime.UtcNow
        };

        evt.ResultJson.Should().Be("{\"result\": true}");
        evt.ProvidersUsed.Should().BeEquivalentTo(providers);
        evt.ProcessingTimeMs.Should().Be(1500);
    }

    [Fact]
    public void AnalysisCompletedEvent_DefaultValues()
    {
        var evt = new AnalysisCompletedEvent();

        evt.ResultJson.Should().BeEmpty();
        evt.ProvidersUsed.Should().BeEmpty();
        evt.ProcessingTimeMs.Should().Be(0);
    }

    [Fact]
    public void AnalysisFailedEvent_ShouldSetProperties()
    {
        var failedProviders = new List<string> { "OpenAI" };

        var evt = new AnalysisFailedEvent
        {
            AnalysisId = Guid.NewGuid(),
            DiagramId = Guid.NewGuid(),
            ErrorMessage = "Timeout",
            FailedProviders = failedProviders,
            Timestamp = DateTime.UtcNow
        };

        evt.ErrorMessage.Should().Be("Timeout");
        evt.FailedProviders.Should().BeEquivalentTo(failedProviders);
    }

    [Fact]
    public void AnalysisFailedEvent_DefaultValues()
    {
        var evt = new AnalysisFailedEvent();

        evt.ErrorMessage.Should().BeEmpty();
        evt.FailedProviders.Should().BeEmpty();
    }

    [Fact]
    public void StatusChangedEvent_ShouldSetProperties()
    {
        var evt = new StatusChangedEvent
        {
            AnalysisId = Guid.NewGuid(),
            OldStatus = "Received",
            NewStatus = "Processing",
            Timestamp = DateTime.UtcNow
        };

        evt.OldStatus.Should().Be("Received");
        evt.NewStatus.Should().Be("Processing");
    }

    [Fact]
    public void StatusChangedEvent_DefaultValues()
    {
        var evt = new StatusChangedEvent();

        evt.OldStatus.Should().BeEmpty();
        evt.NewStatus.Should().BeEmpty();
    }

    [Fact]
    public void ReportGeneratedEvent_ShouldSetProperties()
    {
        var reportId = Guid.NewGuid();
        var analysisId = Guid.NewGuid();
        var diagramId = Guid.NewGuid();

        var evt = new ReportGeneratedEvent
        {
            ReportId = reportId,
            AnalysisId = analysisId,
            DiagramId = diagramId,
            Timestamp = DateTime.UtcNow
        };

        evt.ReportId.Should().Be(reportId);
        evt.AnalysisId.Should().Be(analysisId);
        evt.DiagramId.Should().Be(diagramId);
    }

    [Fact]
    public void ReportFailedEvent_ShouldSetProperties()
    {
        var evt = new ReportFailedEvent
        {
            AnalysisId = Guid.NewGuid(),
            DiagramId = Guid.NewGuid(),
            ErrorMessage = "PDF generation failed",
            Timestamp = DateTime.UtcNow
        };

        evt.ErrorMessage.Should().Be("PDF generation failed");
    }

    [Fact]
    public void ReportFailedEvent_DefaultValues()
    {
        var evt = new ReportFailedEvent();

        evt.ErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public void GenerateReportCommand_ShouldSetProperties()
    {
        var providers = new List<string> { "OpenAI" };

        var cmd = new GenerateReportCommand
        {
            AnalysisId = Guid.NewGuid(),
            DiagramId = Guid.NewGuid(),
            UserId = "user-123",
            ResultJson = "{\"data\": 1}",
            ProvidersUsed = providers,
            ProcessingTimeMs = 2000,
            Timestamp = DateTime.UtcNow
        };

        cmd.UserId.Should().Be("user-123");
        cmd.ResultJson.Should().Be("{\"data\": 1}");
        cmd.ProvidersUsed.Should().BeEquivalentTo(providers);
        cmd.ProcessingTimeMs.Should().Be(2000);
    }

    [Fact]
    public void GenerateReportCommand_DefaultValues()
    {
        var cmd = new GenerateReportCommand();

        cmd.UserId.Should().BeNull();
        cmd.ResultJson.Should().BeEmpty();
        cmd.ProvidersUsed.Should().BeEmpty();
        cmd.ProcessingTimeMs.Should().Be(0);
    }

    [Fact]
    public void UserAccountDeletedEvent_ShouldSetProperties()
    {
        var userId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        var evt = new UserAccountDeletedEvent
        {
            UserId = userId,
            Timestamp = timestamp
        };

        evt.UserId.Should().Be(userId);
        evt.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void AnalysisStatus_ShouldHaveExpectedValues()
    {
        ((int)AnalysisStatus.Received).Should().Be(0);
        ((int)AnalysisStatus.Processing).Should().Be(1);
        ((int)AnalysisStatus.Analyzed).Should().Be(2);
        ((int)AnalysisStatus.Error).Should().Be(3);
    }
}
