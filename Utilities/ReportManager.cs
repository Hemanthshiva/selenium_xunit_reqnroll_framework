// Allure reporting integration utility
using Allure.Net.Commons;

namespace selenium_xunit_reqnroll_framework.Utilities;

public static class AllureReportManager
{
    public static void StartTest(string testName, string? description = null)
    {
        // Test will be started automatically by Allure attributes
    }

    public static void PassStep(string stepName)
    {
        AllureApi.Step(stepName);
    }

    public static void FailStep(string stepName, string errorMessage)
    {
        AllureApi.Step(stepName, () => {
            throw new Exception(errorMessage);
        });
    }

    public static void AttachScreenshot(byte[] screenshot, string name = "Screenshot")
    {
        AllureApi.AddAttachment(name, "image/png", screenshot);
    }

    public static void StopTest(Status status = Status.passed)
    {
        // Test case will be automatically stopped by Allure
    }
}
