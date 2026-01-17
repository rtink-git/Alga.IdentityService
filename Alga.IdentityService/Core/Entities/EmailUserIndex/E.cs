namespace Alga.IdentityService.Core.Entities.EmailUserIndex;

public class E
{
    class SR : Infrastructure.KVA.AdbBase { public SR() : base("email_user_table") { } }

    internal static async Task<User.GuidVO?> GetVAsync(Email.GuidVO? guid)
    {
        if (!guid.HasValue) return null;

        var v = await new SR().GetValueAsync(guid.Value);

        return v == null ? null : (User.GuidVO)(Guid)v;
    }

    static readonly string _logAddVAsync = $"Log (error): {typeof(E).FullName}.{nameof(AddVAsync)}()";

    internal static async Task<bool> AddVAsync(Email.GuidVO guid, User.GuidVO value)
    {
        if (guid == Guid.Empty) throw new ArgumentException($"{_logAddVAsync} - the 'guid' cannot be: {Guid.Empty}");
        if (value == Guid.Empty) throw new ArgumentException($"{_logAddVAsync} - the 'value' cannot be: {Guid.Empty}");

        if (!await new SR().SetValueAsync(guid, value)) { Console.WriteLine($"{_logAddVAsync} - the 'value' was not added to collection"); return false; }
        return true;
    }
}