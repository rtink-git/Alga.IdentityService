using System.Text.Json;
using NATS.Client.Core;
using Alga.IdentityService.Infrastructure.Background;

namespace Alga.IdentityService.API.NATS.CreateSession;

public class SubscribeAsync : BackgroundProcessBase
{
    INatsConnection _bus;
    Alga.sessions.Provider _sessionProvider;
    public SubscribeAsync(ILogger<SubscribeAsync> logger, INatsConnection bus, Alga.sessions.Provider sessionProvider) : base(nameof(SubscribeAsync), logger)
    {
        _bus = bus;
        _sessionProvider = sessionProvider;
    }

    public override async Task ExecuteAsync(CancellationToken token)
    {
        // await foreach (var msg in _bus.SubscribeAsync<string>($"{Constants.EnvironmentName}.auth_db_service.session.create.request"))
        //     try
        //     {
        //         if (msg.Data == null || msg.ReplyTo == null) continue;

        //         var req = JsonSerializer.Deserialize<Application.Handlers.SessionCreate.Req>(msg.Data);
        //         var res = await new Application.Handlers.SessionCreate.H(_sessionProvider).Execute(req);

        //         await _bus.PublishAsync(msg.ReplyTo, res);

        //         await Task.CompletedTask;
        //     }
        //     catch (Exception ex) { Logger.LogError(ex, $"{Name}. Error while processing message."); }
    }
}
