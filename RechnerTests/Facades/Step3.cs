using Microsoft.Playwright;
using RechnerTests.Data;

namespace RechnerTests.Facades;

public sealed class Step3Facade(IFrameLocator page)
{
    public BillingAddressFacade BillingAddress => new(page);
    public BankingFacade Banking => new(page);

    public async Task Submit()
    {
        await Task.Delay(1000);
        await page.GetByRole(AriaRole.Button, new FrameLocatorGetByRoleOptions { Name = "Speichern und weiter" }).ClickAsync();
    }
}

public sealed class BillingAddressFacade(IFrameLocator page)
{
    public async Task SetDifferentBillingAddress(bool enabled)
    {
        ILocator locator = page.GetByText("Abweichende Rechnungsanschrift");
        bool isChecked = await locator.IsCheckedAsync();
        if (isChecked != enabled)
        {
            await locator.ClickAsync();
        }
    }

    public async Task SetGender(Gender gender)
    {
        await SetDifferentBillingAddress(true);
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
        await SetDifferentBillingAddress(true);
        await page.Locator("#firstName").FillAsync(firstName);
    }

    public async Task SetLastName(string lastName)
    {
        await SetDifferentBillingAddress(true);
        await page.Locator("#lastName").FillAsync(lastName);
    }

    public async Task SetPostcode(string postcode)
    {
        await SetDifferentBillingAddress(true);
        await page.Locator("#postcode").FillAsync(postcode);
    }

    public async Task SetCity(string city)
    {
        await SetDifferentBillingAddress(true);
        ILocator citySelect = page.Locator("#billing");
        await citySelect.ClickAsync();
        await citySelect.SelectOptionAsync(new SelectOptionValue{ Label = city});
    }

    public async Task SetStreet(string street)
    {
        await SetDifferentBillingAddress(true);
        await page.Locator("#BillingStreet").ClickAsync();
        await page.Locator("#BillingStreetInput").FillAsync(street);
        await page.GetByRole(AriaRole.Option, new FrameLocatorGetByRoleOptions { Name = street }).ClickAsync();
    }

    public async Task SetHouseNumber(string houseNumber)
    {
        await SetDifferentBillingAddress(true);
        await page.Locator("#houseNumber").FillAsync(houseNumber);
    }

    public async Task SetEmail(string email)
    {
        await page.Locator("#email").FillAsync(email);
    }

    public async Task SetPhone(string phone)
    {
        await page.Locator("#phoneNumber").FillAsync(phone);
    }

    public async Task SetBirthday(DateTimeOffset birthday)
    {
        await page.Locator("#birthDate").FillAsync(birthday.ToString("dd.MM.yyyy"));
    }
}

public sealed class BankingFacade(IFrameLocator page)
{
    public async Task SetIban(string iban)
    {
        await page.Locator("#bankingIBAN").FillAsync(iban);
    }

    public async Task SetAccountHolder(string firstName, string lastName)
    {
        await page.Locator("#bankingFirstName").FillAsync(firstName);
        await page.Locator("#bankingLastName").FillAsync(lastName);
    }


    public SepaManadateFacade SepaMandate => new(page);
}

public sealed class SepaManadateFacade(IFrameLocator page)
{
    public async Task Accept()
    {
        if (await page.Locator("#toggleAcceptIBAN").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Hiermit ermächtigt der Kontoinhaber").ClickAsync();
    }

    public async Task Decline()
    {
        if (!await page.Locator("#toggleAcceptIBAN").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Hiermit ermächtigt der Kontoinhaber").ClickAsync();
    }
}
