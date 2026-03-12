namespace Cerbi.Governance.Schemas.Profiles;

public class GovernanceProfileDefinition
{
    public string ProfileName { get; set; } = string.Empty;

    public List<string> RequiredFields { get; set; } = new();

    public List<string> DisallowedFields { get; set; } = new();

    public Dictionary<string, string> FieldSeverities { get; set; } = new();

    public Dictionary<string, string> EncryptionRules { get; set; } = new();

    public string Version { get; set; } = "1.0.0";
}
