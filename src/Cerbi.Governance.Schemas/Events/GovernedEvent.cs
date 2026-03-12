namespace Cerbi.Governance.Schemas.Events;

public class GovernedEvent
{
    public string EventName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public Dictionary<string, object?> Properties { get; set; } = new();

    public string? GovernanceProfile { get; set; }

    public string TemplateVersion { get; set; } = "1.0.0";

    public bool GovernanceRelaxed { get; set; }

    public List<string> GovernanceViolations { get; set; } = new();
}
