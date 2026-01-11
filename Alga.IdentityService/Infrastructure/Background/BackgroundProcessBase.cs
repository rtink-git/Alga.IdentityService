using System;

namespace Alga.IdentityService.Infrastructure.Background;

public abstract class BackgroundProcessBase : IBackgroundProcess
{
    protected readonly ILogger? Logger;
    protected readonly NATS.Client.Core.INatsConnection? Bus;
    protected readonly IHttpClientFactory? HttpClientFactory;

    public string Name { get; }
    public bool IsDebug { get; }

    protected BackgroundProcessBase(
        string name,
        ILogger? logger,
        NATS.Client.Core.INatsConnection? bus = null)
    {
        Name = name;
        Bus = bus;
        Logger = logger;
        IsDebug = System.Diagnostics.Debugger.IsAttached;
    }

    public abstract Task ExecuteAsync(CancellationToken token);
}