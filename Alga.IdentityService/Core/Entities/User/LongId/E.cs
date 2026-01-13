namespace Alga.IdentityService.Core.Entities.User.LongId;

internal static class E
{
    public static async Task<long?> GetValueAsync(GuidVO? guid) => !guid.HasValue ? null : (await new SR().GetValueAsync(guid.Value) is not { } v) ? null : (long)v;
}
