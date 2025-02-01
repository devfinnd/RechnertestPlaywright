using Microsoft.Playwright;
using RechnerTests.Data;
using RechnerTests.Utils;

namespace RechnerTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : RechnerTest
{
    [Test]
    public async Task RunSimpleCalculator()
    {
        await SetupRechner(new RechnerSetupParameters
        {
            Type = RechnerType.Advanced,
            Branch = RechnerBranch.Master,
            Environment = PartnerApiEnvironment.Staging,
        });

        await Intro.SetPostcode("23898");
        await Intro.SetCity("Wentorf");
        await Intro.SetConsumption(3500);

        await Intro.Tariff.SetCurrentProvider("E.ON Energie Deutschland GmbH");
        await Intro.Tariff.SetCurrentTariff("Grundvers");

        await Intro.Options.Open();
        await Intro.Options.SetBonusPreference(BonusPreference.NoBonus);
        await Intro.Options.SetEcoPreference(EcoPreference.All);
        await Intro.Options.SetCustomerType(CustomerType.Private);

        await Intro.Submit();

        await Step1.TariffsList.Recommended.Select();

        await Step2.DeliveryAddress.SetGender(Gender.Male);
        await Step2.DeliveryAddress.SetFirstName("John");
        await Step2.DeliveryAddress.SetLastName("Doe");
        await Step2.DeliveryAddress.SetStreet("Kronika");
        await Step2.DeliveryAddress.SetHouseNumber("1");
        await Step2.Switch.SelectReasonAndDate(SwitchReason.ProviderChange(SwitchDate.Immediately));
        await Step2.Switch.SelectReasonAndDate(SwitchReason.ProviderChange(SwitchDate.Specific(DateTimeOffset.Now.AddDays(35))));
        await Step2.MeterInformation.SetMeterNumber(MeterNumber.Specific("1234567890"));
        await Step2.MeterInformation.SetMeterNumber(MeterNumber.ProvideItLater);

        await Step2.Submit();

        await Step3.BillingAddress.SetGender(Gender.Female);
        await Step3.BillingAddress.SetFirstName("Jane");
        await Step3.BillingAddress.SetLastName("Doe");
        await Step3.BillingAddress.SetPostcode("21107");
        await Step3.BillingAddress.SetCity("Hamburg");
        await Step3.BillingAddress.SetStreet("Veringstr");
        await Step3.BillingAddress.SetHouseNumber("27");
        await Step3.BillingAddress.SetDifferentBillingAddress(false);

        string emailAddress = $"rechner-test-{Guid.CreateVersion7()}@wechselpilot.com";
        await Step3.BillingAddress.SetEmail(emailAddress);
        await Step3.BillingAddress.SetBirthday(DateTime.Now.AddYears(-30));

        await Step3.Banking.SetAccountHolder("John", "Doe");
        await Step3.Banking.SetIban("DE43500105171747755412");
        await Step3.Banking.SepaMandate.Accept();

        await Step3.Submit();

        await Finalize.Voucher.SetVoucherCode("DAVE");
        await Finalize.TermsAndConditions.Accept();
        await Finalize.RightToCancel.Accept();
        await Finalize.Submit();


        // Expect the page to contain the text "Wechselpilot erfolgreich beauftragt"
        await Expect(Container.GetByRole(AriaRole.Heading, new FrameLocatorGetByRoleOptions { Name = "Wechselpilot erfolgreich" }))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions{ Timeout = (float)TimeSpan.FromSeconds(60).TotalMilliseconds });
    }
}
