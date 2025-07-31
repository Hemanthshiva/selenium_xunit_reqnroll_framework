// Hooks for test setup and teardown
using Allure.Net.Commons;
using OpenQA.Selenium;
using Reqnroll;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private static readonly AllureLifecycle _allure = AllureLifecycle.Instance;
        private string? _testUuid;

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Initialize Allure results directory
            var allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
            Directory.CreateDirectory(allureResultsPath);
            Console.WriteLine($"Allure results will be generated at: {allureResultsPath}");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _testUuid = Guid.NewGuid().ToString();
            var testResult = new TestResult
            {
                uuid = _testUuid,
                name = _scenarioContext.ScenarioInfo.Title,
                fullName = _scenarioContext.ScenarioInfo.Title,
                description = _scenarioContext.ScenarioInfo.Description,
                start = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };
            _allure.StartTestCase(testResult);
        }

        [AfterStep]
        public void AfterStep()
        {
            var stepInfo = _scenarioContext.StepContext.StepInfo;
            var stepType = stepInfo.StepDefinitionType.ToString();
            var stepText = stepInfo.Text;
            var error = _scenarioContext.TestError;
            var stepName = $"{stepType} {stepText}";

            var stepResult = new StepResult
            {
                name = stepName,
                status = error == null ? Status.passed : Status.failed,
                start = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                stop = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            if (error != null)
            {
                stepResult.statusDetails = new StatusDetails { message = error.Message };
                
                // Capture screenshot if WebDriver is available
                try
                {
                    var driver = selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Driver;
                    if (driver is IWebDriver webDriver)
                    {
                        var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                        AllureApi.AddAttachment("Error Screenshot", "image/png", screenshot.AsByteArray);
                    }
                }
                catch
                {
                    // Continue without screenshot if capture fails
                }
            }

            _allure.StartStep(stepResult);
            _allure.StopStep();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_testUuid != null)
            {
                var status = _scenarioContext.TestError == null ? Status.passed : Status.failed;
                _allure.UpdateTestCase(x => {
                    x.status = status;
                    x.stop = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                });
                _allure.StopTestCase();
                _allure.WriteTestCase();
            }
            
            try
            {
                selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Quit();
            }
            catch
            {
                // Suppress any errors during browser cleanup
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("Test run completed. Allure results generated.");
        }
    }
}
