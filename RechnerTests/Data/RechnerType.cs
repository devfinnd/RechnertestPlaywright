namespace RechnerTests.Data;

public sealed class RechnerType
{
    public static readonly RechnerType Simple = new("simple");
    public static readonly RechnerType Advanced = new("advanced");
    public static readonly RechnerType Kundenportal = new("kuko");
    public static readonly RechnerType Partnerportal = new("papo");

    public string Value { get; }

    private RechnerType(string value) => Value = value;
}