namespace Alga.IdentityService.Core.Entities.Email;

internal readonly struct GuidVO : IEquatable<GuidVO>
{
    public Guid Value { get; }

    public GuidVO(Guid value) => Value = value;

    public static implicit operator Guid(GuidVO id) => id.Value;
    public static explicit operator GuidVO(Guid value) => new(value);

    public bool Equals(GuidVO other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is GuidVO other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}