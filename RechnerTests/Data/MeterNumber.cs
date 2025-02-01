namespace RechnerTests.Data;

public abstract record MeterNumber
{
    public static readonly ProvideItLater ProvideItLater = new();
    public static MeterNumber Specific(string value) => new SpecificMeterNumber(value);
}

public sealed record ProvideItLater : MeterNumber;
public sealed record SpecificMeterNumber(string Value) : MeterNumber;
