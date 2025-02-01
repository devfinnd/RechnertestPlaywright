namespace RechnerTests.Data;

public abstract record SwitchReason
{
    public static SwitchReason ProviderChange(SwitchDate date) => new ProviderChange(date);
    public static SwitchReason Move(DateTimeOffset date) => new Move(date);
    public static SwitchReason PriceChange(DateTimeOffset date) => new PriceChange(date);
}

public sealed record ProviderChange(SwitchDate Date) : SwitchReason;

public sealed record Move(DateTimeOffset Date) : SwitchReason;

public sealed record PriceChange(DateTimeOffset Date) : SwitchReason;
