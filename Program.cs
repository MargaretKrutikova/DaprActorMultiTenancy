using DaprActorMultiTenancy.Controllers;
using DaprActorMultiTenancy.DaprNamespace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDaprClient();

builder.AddActorsWithNamespace(options =>
{
    options.RegisterActor<ActorTest>(ActorTest.ActorType);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    app.MapActorsHandlers();
});

app.MapControllers();
app.Run();