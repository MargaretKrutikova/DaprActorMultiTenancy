using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace DaprActorMultiTenancy.Controllers;
using DaprActor = Actor;

public class ActorTest : DaprActor, ITestActor
{
    public const string ActorType = nameof(ActorTest);
    public ActorTest(ActorHost host) : base(host)
    {
    }
    
    public async Task SetState()
    {
        await StateManager.SetStateAsync("some-state", Id.GetId());
    }
}

public interface ITestActor : IActor
{
    public Task SetState();
}