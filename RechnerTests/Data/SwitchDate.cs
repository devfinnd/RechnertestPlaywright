namespace RechnerTests.Data;

public abstract record SwitchDate
{
    public static readonly SwitchDate Immediately = new Immediately();
    public static SwitchDate Specific(DateTimeOffset date) => new SpecificDate(date);
}

public sealed record Immediately : SwitchDate;
public sealed record SpecificDate(DateTimeOffset Date) : SwitchDate;
