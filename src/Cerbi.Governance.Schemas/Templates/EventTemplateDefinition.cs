using Cerbi.Governance.Schemas.Fields;

namespace Cerbi.Governance.Schemas.Templates;

public class EventTemplateDefinition
{
    public string EventName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string ProfileName { get; set; } = string.Empty;

    public string DefaultMessage { get; set; } = string.Empty;

    public string Version { get; set; } = "1.0.0";

    public List<EventFieldDefinition> RequiredFields { get; set; } = new();

    public List<EventFieldDefinition> OptionalFields { get; set; } = new();
}
