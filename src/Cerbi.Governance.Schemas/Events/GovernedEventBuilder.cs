namespace Cerbi.Governance.Schemas.Events;

public static class GovernedEventBuilder
{
    public static GovernedEvent Create(
        string eventName,
        string category,
        string message,
        Dictionary<string, object?>? properties = null)
    {
        return new GovernedEvent
        {
            EventName = eventName,
            Category = category,
            Message = message,
            Properties = properties ?? new Dictionary<string, object?>()
        };
    }

    public static GovernedEvent Create(
        string eventName,
        string category,
        string message,
        string? governanceProfile,
        string templateVersion,
        Dictionary<string, object?>? properties = null)
    {
        return new GovernedEvent
        {
            EventName = eventName,
            Category = category,
            Message = message,
            GovernanceProfile = governanceProfile,
            TemplateVersion = templateVersion,
            Properties = properties ?? new Dictionary<string, object?>()
        };
    }
}
