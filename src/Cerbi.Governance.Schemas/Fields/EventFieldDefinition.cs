namespace Cerbi.Governance.Schemas.Fields;

public class EventFieldDefinition
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = "string";

    public bool Sensitive { get; set; }

    public bool Required { get; set; }
}
