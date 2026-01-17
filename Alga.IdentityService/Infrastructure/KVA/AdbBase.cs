namespace Alga.IdentityService.Infrastructure.KVA;

public abstract class AdbBase : Alga.transport.Providers.Postgres.KVAClientBase { public AdbBase(string storeName) : base(Context.Res.PostgresConnectionString, storeName) { } }

// public abstract class AdbBase : Alga.transport.Providers.Postgres.KVAClientBase, Background.IBackgroundProcess
// {
//     public string Name { get; }
//     public AdbBase(string storeName) : base(Context.Res.PostgresConnectionString, storeName) { Name = typeof(AdbBase).FullName ?? string.Empty; }

//     public virtual async Task ExecuteAsync(CancellationToken token) { }
// }







// public abstract class FhBase : Alga.transport.Providers.Postgres.KVAClientBase, Background.IBackgroundProcess
// {
//     public string Name { get; }
//     public FhBase(string storeName) : base(Application.Context.FohousePostgresConnectionString, storeName) { Name = typeof(FhBase).FullName ?? string.Empty; }

//     public virtual async Task ExecuteAsync(CancellationToken token) { }
// }