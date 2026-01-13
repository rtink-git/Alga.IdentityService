namespace Alga.IdentityService.Core.Entities.EmailUserIndex;

internal class E
{
    private static readonly string LogBaseTryAdd = $"ERROR: {typeof(E).FullName}.{nameof(TryAddValue)}()";

    public static async Task<User.GuidVO?> GetValueAsync(Email.GuidVO? guid)
    {
        if (!guid.HasValue) return null;

        var v = await new SR().GetValueAsync(guid.Value);

        return v == null ? null : (User.GuidVO)(Guid)v;
    }

    public static async Task<bool> TryAddValue(Email.GuidVO guid, User.GuidVO value)
    {
        if (guid == Guid.Empty) throw new ArgumentException($"{LogBaseTryAdd} - the 'guid' cannot be: {Guid.Empty}");
        if (value == Guid.Empty) throw new ArgumentException($"{LogBaseTryAdd} - the 'value' cannot be: {Guid.Empty}");

        if (!await new SR().SetValueAsync(guid, value)) { Console.WriteLine($"{LogBaseTryAdd} - the 'value' was not added to collection"); return false; }
        return true;
    }
}