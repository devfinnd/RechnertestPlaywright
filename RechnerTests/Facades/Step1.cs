using Microsoft.Playwright;

namespace RechnerTests.Facades;

public sealed class Step1Facade(IFrameLocator page)
{
    public TariffListFacade TariffsList => new(page);
}

public sealed class TariffListFacade(IFrameLocator page)
{
    public RecommendedTariffFacade Recommended => new(page.Locator("#tariffBox-empfehlung"));
    public EcoTariffFacade Eco => new(page.Locator("#tariffBox-oko-empfehlung"));
    public CheapestTariffFacade Cheapest => new(page.Locator("#tariffBox-empfehlung"));
}

public abstract class TariffListItemFacade(ILocator page)
{
    public async Task Select()
    {
        await page.GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Tarif wählen" }).ClickAsync();
    }
}

public sealed class RecommendedTariffFacade(ILocator page) : TariffListItemFacade(page);

public sealed class EcoTariffFacade(ILocator page) : TariffListItemFacade(page);

public sealed class CheapestTariffFacade(ILocator page) : TariffListItemFacade(page);
