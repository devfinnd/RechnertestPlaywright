using Microsoft.Playwright;
using RechnerTests.Data;

namespace RechnerTests.Facades;

public sealed class Step2Facade(IFrameLocator page)
{
    public DeliveryAddressFacade DeliveryAddress => new(page);
    public SwitchFacade Switch => new(page);
    public MeterInformationFacade MeterInformation => new(page);

    public async Task Submit()
    {
        await Task.Delay(1000);
        await page.GetByRole(AriaRole.Button, new FrameLocatorGetByRoleOptions { Name = "Speichern und weiter" }).ClickAsync();
    }
}

public sealed class DeliveryAddressFacade(IFrameLocator page)
{
    public async Task SetGender(Gender gender)
    {
        ILocator radioButton = gender switch
        {
            Gender.Male => page.GetByText("Herr"),
            Gender.Female => page.GetByText("Frau"),
            Gender.Diverse => page.GetByText("k.A."),
        };

        await radioButton.ClickAsync();
    }

    public async Task SetFirstName(string firstName)
    {
        await page.Locator("#deliveryFirstName").FillAsync(firstName);
    }

    public async Task SetLastName(string lastName)
    {
        await page.Locator("#deliveryLastName").FillAsync(lastName);
    }

    public async Task SetStreet(string street)
    {
        await page.Locator("#DeliveryStreet").ClickAsync();
        await page.Locator("#DeliveryStreetInput").FillAsync(street);
        await page.GetByRole(AriaRole.Option, new FrameLocatorGetByRoleOptions { Name = street }).ClickAsync();
    }

    public async Task SetHouseNumber(string houseNumber)
    {
        await page.Locator("#deliveryHouseNumber").FillAsync(houseNumber);
    }
}

public sealed class SwitchFacade(IFrameLocator page)
{
    public async Task SelectReasonAndDate(SwitchReason reason)
    {
        ILocator radioButton = reason switch
        {
            ProviderChange => page.GetByText("Versorger wechseln"),
            Move => page.GetByText("Umzug/Einzug"),
            PriceChange => page.GetByText("Preiserhöhung zum"),
            _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
        };

        await radioButton.ClickAsync();
    }

    private async Task Handle(ProviderChange reason)
    {
        await page.GetByText("Versorger wechseln").ClickAsync();
        if (reason.Date is not SpecificDate specificDate)
        {
            await page.Locator("#button_dateNow").ClickAsync();
            return;
        }

        await page.Locator("#button_dateWish").ClickAsync();
        await page.Locator("#switchDate").FillAsync(specificDate.Date.ToString("dd.MM.yyyy"));
    }

    private async Task Handle(Move reason)
    {
        await page.Locator("#switchDate").FillAsync(reason.Date.ToString("dd.MM.yyyy"));
    }

    private async Task Handle(PriceChange reason)
    {
        await page.Locator("#switchDate").FillAsync(reason.Date.ToString("dd.MM.yyyy"));
    }
}

public sealed class MeterInformationFacade(IFrameLocator page)
{
    public async Task SetMeterNumber(MeterNumber meterNumber)
    {
        if (meterNumber is not SpecificMeterNumber specificMeterNumber)
        {
            await page.GetByText("Zählernummer nachreichen").ClickAsync();
            return;
        }

        await page.Locator("#meterNumber").FillAsync(specificMeterNumber.Value);
    }

    public async Task SetCustomerNumber(string meterReading)
    {
        await page.Locator("#customerId").FillAsync(meterReading);
    }
}
