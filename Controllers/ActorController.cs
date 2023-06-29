using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DaprActorMultiTenancy.Controllers;

[ApiController]
[Route("[controller]")]
public class ActorController : ControllerBase
{
    // Very important to inject IActorProxyFactory and not rely on the static one
    // ActorProxy.Create<> which will use the DefaultActorProxy that doesn't prefix state 
    // with namespace
    private readonly IActorProxyFactory _proxyFactory;

    public ActorController(IActorProxyFactory proxyFactory)
    {
        _proxyFactory = proxyFactory;
    }
    
    [HttpGet(Name = "set-state")]
    public async Task<int> SetActorState(string actorId)
    {
        var actorProxy = _proxyFactory.CreateActorProxy<ITestActor>(new ActorId(actorId), ActorTest.ActorType);
        await actorProxy.SetState();

        return 42;
    }
}