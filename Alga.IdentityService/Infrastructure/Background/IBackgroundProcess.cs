namespace Alga.IdentityService.Infrastructure.Background;

public interface IBackgroundProcess
{
    string Name { get; }
    Task ExecuteAsync(CancellationToken stoppingToken);
}
