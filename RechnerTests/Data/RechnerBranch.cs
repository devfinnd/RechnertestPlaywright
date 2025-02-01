namespace RechnerTests.Data;

public sealed class RechnerBranch
{
    public static readonly RechnerBranch Master = new("master");
    public static readonly RechnerBranch Develop = new("develop");

    public string Value { get; }

    private RechnerBranch(string value) => Value = value;
}