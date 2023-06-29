namespace DaprActorMultiTenancy.DaprNamespace;

public class DaprConfig
{
    public const string SectionName = "Dapr";
    public string Namespace { get; init; } = string.Empty;
}