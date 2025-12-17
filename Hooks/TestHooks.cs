// Hooks for test setup and teardown
using Allure.Net.Commons;
using OpenQA.Selenium;
using Reqnroll;


namespace selenium_xunit_reqnroll_framework.Hooks;

    [Binding]
    public class TestHooks(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Allure lifecycle is managed by the Allure.Reqnroll plugin.
            // Configuration is read from allureConfig.json in the output directory.
        }



        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    if (selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Driver is ITakesScreenshot driver)
                    {
                        var screenshot = driver.GetScreenshot();
                        AllureApi.AddAttachment("Error Screenshot", "image/png", screenshot.AsByteArray);
                    }
                }
                catch { }
            }
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            try { selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Quit(); }
            catch { }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("Test run completed. Allure results generated.");
        }
    }

