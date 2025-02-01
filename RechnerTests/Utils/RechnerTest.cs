using System.Text;
using Microsoft.Playwright;
using RechnerTests.Data;
using RechnerTests.Facades;

namespace RechnerTests.Utils;

public abstract class RechnerTest : PageTest
{
    protected IFrameLocator Container => Page.FrameLocator("iframe[title=\"Wechselpilot Rechner - Jedes Jahr automatisch sparen mit unserem Wechselservice\"]");
    protected IntroFacade Intro => new(Container);
    protected Step1Facade Step1 => new(Container);
    protected Step2Facade Step2 => new(Container);
    protected Step3Facade Step3 => new(Container);
    protected FinalizeFacade Finalize => new(Container);
    protected SuccessFacade Success => new(Container);

    protected async Task SetupRechner(RechnerSetupParameters parameters)
    {
        StringBuilder urlBuilder = new StringBuilder("https://rechner-staging.wechselpilot.com")
            .Append($"?widgetType={parameters.Type.Value}")
            .Append($"&branch={parameters.Branch.Value}")
            .Append($"&environment={parameters.Environment.Value}");

        await Page.GotoAsync(urlBuilder.ToString());
    }
}
