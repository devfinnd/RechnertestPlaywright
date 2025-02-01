namespace RechnerTests.Data;

public record RechnerSetupParameters
{
    public RechnerType Type { get; init; } = RechnerType.Simple;
    public RechnerBranch Branch { get; init; } = RechnerBranch.Master;
    public PartnerApiEnvironment Environment { get; init; } = PartnerApiEnvironment.Staging;
    public Guid? ApiToken { get; init; }
}
