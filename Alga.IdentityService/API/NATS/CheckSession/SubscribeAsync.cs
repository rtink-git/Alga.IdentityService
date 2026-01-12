using System.Text.Json;
using NATS.Client.Core;
using Alga.IdentityService.Infrastructure.Background;

namespace Alga.IdentityService.API.NATS.CheckSession;

public class SubscribeAsync : BackgroundProcessBase
{
    INatsConnection _bus;
    Alga.sessions.IProvider _sessionProvider;
    public SubscribeAsync(ILogger<SubscribeAsync> logger, INatsConnection bus, Alga.sessions.IProvider sessionProvider) : base(nameof(SubscribeAsync), logger)
    {
        _bus = bus;
        _sessionProvider = sessionProvider;
    }

    public override async Task ExecuteAsync(CancellationToken token)
    {
        // await foreach (var msg in _bus.SubscribeAsync<string>($"{Constants.EnvironmentName}.auth_db_service.session.check.request"))
        //     try
        //     {
        //         if (msg.Data == null || msg.ReplyTo == null) continue;

        //         await _bus.PublishAsync(msg.ReplyTo, new Application.Handlers.SessionCheck.H(_sessionProvider).Execute(msg.Data));

        //         await Task.CompletedTask;
        //     }
        //     catch (Exception ex) { Logger?.LogError(ex, $"{Name}. Error while processing message."); }
    }
}
