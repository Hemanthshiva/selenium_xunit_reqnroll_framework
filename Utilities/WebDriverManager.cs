// WebDriver management utility (singleton pattern)
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_xunit_reqnroll_framework.Utilities
{
    public static class WebDriverManager
    {
        private static ChromeDriver? driver;

        public static IWebDriver Driver
        {
            get
            {
                if (driver == null || IsDriverDisposed())
                {
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = ConfigManager.Browser.ToLower() switch
                    {
                        _ => new ChromeDriver(),
                    };
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigManager.Timeout);
                    driver.Manage().Window.Maximize();
                }
                return driver;
            }
        }

        public static void Quit()
        {
            try
            {
                if (driver != null && !IsDriverDisposed())
                {
                    driver.Quit();
                }
            }
            catch { /* Suppress errors */ }
            finally
            {
                driver = null;
            }
        }

        private static bool IsDriverDisposed()
        {
            try
            {
                var _ = driver?.SessionId;
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
