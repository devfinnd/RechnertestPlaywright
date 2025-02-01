namespace RechnerTests.Data;

public sealed class PartnerApiEnvironment
{
    public static readonly PartnerApiEnvironment Staging = new("staging");
    public static readonly PartnerApiEnvironment Production = new("production");

    public string Value { get; }

    private PartnerApiEnvironment(string value) => Value = value;
}