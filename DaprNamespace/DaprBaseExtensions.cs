using Dapr.Actors.Client;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Options;

namespace DaprActorMultiTenancy.DaprNamespace;

public static class DaprBaseExtensions
{
    public static void AddActorsWithNamespace(this WebApplicationBuilder builder, Action<NamespaceActorRuntimeOptions>? configure = null)
    {
        var daprConfig = builder.Configuration.GetSection(DaprConfig.SectionName).Get<DaprConfig>() ?? new DaprConfig();

        builder.Services.Configure<DaprConfig>(builder.Configuration.GetSection(DaprConfig.SectionName));
        // Very important to register this first before calling AddActors, since AddActors will check 
        // if IActorProxyFactory is already present, and if not, register the default one
        builder.Services.AddSingleton<IActorProxyFactory>(
            serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ActorRuntimeOptions>>().Value;
                var daprBaseConfig = serviceProvider.GetRequiredService<IOptions<DaprConfig>>().Value;
                var factory = new ActorProxyFactory(
                    new ActorProxyOptions
                    {
                        JsonSerializerOptions = options.JsonSerializerOptions,
                        DaprApiToken = options.DaprApiToken,
                        HttpEndpoint = options.HttpEndpoint
                    });

                return new NamespaceActorProxyFactory(factory, daprBaseConfig);
            });

        builder.Services.AddActors(options => configure?.Invoke(new NamespaceActorRuntimeOptions(options, daprConfig)));
    }
}
