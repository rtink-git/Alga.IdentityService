namespace Alga.IdentityService.Infrastructure.Background;

public class BackgroundHost : BackgroundService
{
    private readonly ILogger<BackgroundHost> _logger;
    private readonly IEnumerable<IBackgroundProcess> _processes;
    private readonly List<Task> _running = new();

    public BackgroundHost(
        ILogger<BackgroundHost> logger,
        IEnumerable<IBackgroundProcess> processes)

    {
        _logger = logger;
        _processes = processes;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_processes.Any())
        {
            _logger.LogWarning("BackgroundHost started with no registered background processes.");
            return Task.CompletedTask;
        }

        foreach (var process in _processes)
        {
            _logger.LogInformation("Starting background process: {Name}", process.Name);
            _running.Add(RunProcess(process, stoppingToken));
        }

        return Task.WhenAll(_running);
    }

    private async Task RunProcess(IBackgroundProcess process, CancellationToken token)
    {
        try { await process.ExecuteAsync(token); }
        catch (OperationCanceledException) { _logger.LogInformation("Background process cancelled: {Name}", process.Name); }
        catch (Exception ex) { _logger.LogError(ex, "Unhandled error in background process: {Name}", process.Name); }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping BackgroundHost...");

        await base.StopAsync(cancellationToken);

        _logger.LogInformation("BackgroundHost stopped.");
    }
}