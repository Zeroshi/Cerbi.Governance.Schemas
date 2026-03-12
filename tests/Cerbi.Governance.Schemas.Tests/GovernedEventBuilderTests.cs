using Cerbi.Governance.Schemas.Events;

namespace Cerbi.Governance.Schemas.Tests;

public class GovernedEventBuilderTests
{
    [Fact]
    public void Create_SetsEventName()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.Equal("Test.Event", evt.EventName);
    }

    [Fact]
    public void Create_SetsCategory()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.Equal("Test", evt.Category);
    }

    [Fact]
    public void Create_SetsMessage()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.Equal("A test event", evt.Message);
    }

    [Fact]
    public void Create_WithProperties_PopulatesProperties()
    {
        var props = new Dictionary<string, object?>
        {
            ["userId"] = "user-1",
            ["tenantId"] = "tenant-1"
        };

        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event", props);

        Assert.Equal("user-1", evt.Properties["userId"]);
        Assert.Equal("tenant-1", evt.Properties["tenantId"]);
    }

    [Fact]
    public void Create_WithoutProperties_HasEmptyDictionary()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.NotNull(evt.Properties);
        Assert.Empty(evt.Properties);
    }

    [Fact]
    public void Create_DefaultsGovernanceRelaxedToFalse()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.False(evt.GovernanceRelaxed);
    }

    [Fact]
    public void Create_DefaultsGovernanceViolationsToEmpty()
    {
        var evt = GovernedEventBuilder.Create("Test.Event", "Test", "A test event");

        Assert.NotNull(evt.GovernanceViolations);
        Assert.Empty(evt.GovernanceViolations);
    }

    [Fact]
    public void Create_WithProfileAndVersion_SetsFields()
    {
        var evt = GovernedEventBuilder.Create(
            "Test.Event", "Test", "A test event",
            "SecurityProfile", "2.0.0");

        Assert.Equal("SecurityProfile", evt.GovernanceProfile);
        Assert.Equal("2.0.0", evt.TemplateVersion);
    }
}
