using Microsoft.Playwright;

namespace RechnerTests.Facades;

public sealed class FinalizeFacade(IFrameLocator page)
{
    public VoucherCodeFacade Voucher => new(page);
    public TermsAndConditionsFacade TermsAndConditions => new(page);
    public RightToCancelFacade RightToCancel => new(page);

    public async Task Submit()
    {
        await page.GetByRole(AriaRole.Button, new FrameLocatorGetByRoleOptions { Name = "Wechselpilot Beauftragen" }).ClickAsync();
    }
}

public sealed class VoucherCodeFacade(IFrameLocator page)
{
    public async Task SetVoucherCode(string voucherCode)
    {
        await page.Locator("#CouponCode").FillAsync(voucherCode);
        await page.GetByRole(AriaRole.Button, new FrameLocatorGetByRoleOptions { Name = "Einlösen" }).ClickAsync();
    }
}

public sealed class TermsAndConditionsFacade(IFrameLocator page)
{
    public async Task Accept()
    {
        if (await page.Locator("#toggleAcceptAGB").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Hiermit akzeptiere ich die geänderten").ClickAsync();
    }

    public async Task Decline()
    {
        if (!await page.Locator("#toggleAcceptAGB").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Hiermit akzeptiere ich die geänderten").ClickAsync();
    }
}

public sealed class RightToCancelFacade(IFrameLocator page)
{
    public async Task Accept()
    {
        if (await page.Locator("#toggleAcceptWiderruf").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Mein Widerrufsrecht habe ich zur Kenntnis genommen").ClickAsync();
    }

    public async Task Decline()
    {
        if (!await page.Locator("#toggleAcceptWiderruf").IsCheckedAsync())
        {
            return;
        }

        await page.GetByText("Mein Widerrufsrecht habe ich zur Kenntnis genommen").ClickAsync();
    }
}
