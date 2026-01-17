namespace Alga.IdentityService.Core.Entities.User.LongId;

public static class E
{
    class SR : Infrastructure.KVA.AdbBase { public SR() : base("user_longid_table") { } }

    internal static async Task<long?> GetVAsync(GuidVO? guid) => !guid.HasValue ? null : (await new SR().GetValueAsync(guid.Value) is not { } v) ? null : (long)v;
}
