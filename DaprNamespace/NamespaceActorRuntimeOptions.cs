using Dapr.Actors.Runtime;
using DaprActor = Dapr.Actors.Runtime.Actor;

namespace DaprActorMultiTenancy.DaprNamespace;

public sealed class NamespaceActorRuntimeOptions
{
    private readonly ActorRuntimeOptions _options;
    private readonly DaprConfig _config;

    public NamespaceActorRuntimeOptions(ActorRuntimeOptions options, DaprConfig config)
    {
        _options = options;
        _config = config;
    }

    public void RegisterActor<TActor>(string actorTypeName, Action<ActorRegistration>? configure = null)
        where TActor : DaprActor
    {
        var namespacedActorTypeName = ActorNamespace.GetNamespacedActorType(_config, actorTypeName);
        _options.Actors.RegisterActor<TActor>(namespacedActorTypeName, configure);
    }
}
