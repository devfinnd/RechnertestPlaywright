using Microsoft.Playwright;
using RechnerTests.Data;

namespace RechnerTests.Facades;

public sealed class IntroFacade(IFrameLocator page)
{
    private ILocator PostcodeInput => page.Locator("#postcode");
    private ILocator CitySelect => page.Locator("#delivery");
    private ILocator ConsumptionInput => page.Locator("#EstimatedAnnualConsumptionHT");
    public IntroTariffFacade Tariff => new(page);
    public IntroOptionsFacade Options => new(page);

    public async Task SetPostcode(string postCode)
    {
        await PostcodeInput.ClickAsync();
        await PostcodeInput.FillAsync(postCode);
    }

    public async Task SetCity(string city)
    {
        await CitySelect.ClickAsync();
        await CitySelect.SelectOptionAsync(new SelectOptionValue{ Label = city});
    }

    public async Task SetConsumption(int consumptionKwh)
    {
        await ConsumptionInput.ClickAsync();
        await ConsumptionInput.FillAsync(consumptionKwh.ToString());
    }

    public async Task Submit()
    {
        await page.Locator("#calculateSavings").ClickAsync();
    }
}

public sealed class IntroTariffFacade(IFrameLocator page)
{
    private ILocator CurrentProviderSelect => page.Locator("#WpProviderIdInput");
    private ILocator CurrentTariffSelect => page.Locator("#WpTariffSelectInput");

    public async Task SetCurrentProvider(string provider)
    {
        await CurrentProviderSelect.ClickAsync();
        await CurrentProviderSelect.FillAsync(provider);
        await page.GetByRole(AriaRole.Option, new FrameLocatorGetByRoleOptions { Name = provider, Exact = true }).ClickAsync();
    }

    public async Task SetCurrentTariff(string tariff)
    {
        await CurrentTariffSelect.ClickAsync();
        await CurrentTariffSelect.FillAsync(tariff);
        await page.GetByRole(AriaRole.Option, new FrameLocatorGetByRoleOptions { Name = tariff }).ClickAsync();
    }
}

public sealed class IntroOptionsFacade(IFrameLocator page)
{
    public async Task Open()
    {
        await page.GetByText("Mehr Optionen anzeigen").ClickAsync();
    }

    public async Task Close()
    {
        await page.GetByText("Weniger Optionen anzeigen").ClickAsync();
    }

    public async Task SetBonusPreference(BonusPreference bonusPreference)
    {
        ILocator locator = bonusPreference switch
        {
            BonusPreference.All => page.GetByText("Alle Boni"),
            BonusPreference.NoBonus => page.GetByText("Kein Bonus"),
            _ => throw new ArgumentOutOfRangeException(nameof(bonusPreference), bonusPreference, null)
        };

        await locator.CheckAsync();
    }

    public async Task SetEcoPreference(EcoPreference ecoPreference)
    {
        ILocator locator = ecoPreference switch
        {
            EcoPreference.All => page.GetByText("Alle Tarife"),
            EcoPreference.OnlyGreen => page.GetByText("Nur Öko-Tarife"),
            _ => throw new ArgumentOutOfRangeException(nameof(ecoPreference), ecoPreference, null)
        };

        await locator.CheckAsync();
    }

    public async Task SetCustomerType(CustomerType customerType)
    {
        ILocator locator = customerType switch
        {
            CustomerType.Private => page.GetByText("Privat", new FrameLocatorGetByTextOptions{ Exact = true }),
            CustomerType.Commercial => page.GetByText("Gewerblich", new FrameLocatorGetByTextOptions{ Exact = true }),
            _ => throw new ArgumentOutOfRangeException(nameof(customerType), customerType, null)
        };

        await locator.CheckAsync();
    }
}
