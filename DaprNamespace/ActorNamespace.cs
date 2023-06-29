namespace DaprActorMultiTenancy.DaprNamespace;

public static class ActorNamespace
{
    public static string GetNamespacedActorType(DaprConfig config, string actorType)
        => !string.IsNullOrWhiteSpace(config.Namespace) ? $"{actorType}.{config.Namespace}" : actorType;
}
