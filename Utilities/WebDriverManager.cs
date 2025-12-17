// WebDriver management utility (singleton pattern)
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace selenium_xunit_reqnroll_framework.Utilities;

public static class WebDriverManager
{
    private static IWebDriver? driver;

    public static IWebDriver Driver
    {
        get
        {
            if (driver == null || IsDriverDisposed())
            {
                switch (ConfigManager.Browser.ToLower())
                {
                    case "edge":
                        new DriverManager().SetUpDriver(new EdgeConfig());
                        driver = new EdgeDriver();
                        break;
                    case "firefox":
                        new DriverManager().SetUpDriver(new FirefoxConfig());
                        driver = new FirefoxDriver();
                        break;
                    case "chrome":
                    default:
                        // Prefer explicit ChromeDriverPath if set
                        if (!string.IsNullOrWhiteSpace(ConfigManager.ChromeDriverPath) && File.Exists(ConfigManager.ChromeDriverPath))
                        {
                            Console.WriteLine($"Using explicit chromedriver path from config: {ConfigManager.ChromeDriverPath}");
                            var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(ConfigManager.ChromeDriverPath), Path.GetFileName(ConfigManager.ChromeDriverPath));
                            driver = new ChromeDriver(service, new ChromeOptions());
                            break;
                        }
                        // Prefer explicit ChromeDriverVersion if set
                        if (!string.IsNullOrWhiteSpace(ConfigManager.ChromeDriverVersion))
                        {
                            try
                            {
                                Console.WriteLine($"Requesting chromedriver for version {ConfigManager.ChromeDriverVersion} from config");
                                new DriverManager().SetUpDriver(new ChromeConfig(), ConfigManager.ChromeDriverVersion);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"WebDriverManager setup for config version failed: {ex.Message}");
                                new DriverManager().SetUpDriver(new ChromeConfig());
                            }
                        }
                        // Use WebDriverManager with MatchingBrowser strategy to automatically find the correct driver version
                        try 
                        {
                            Console.WriteLine("Attempting to set up ChromeDriver using MatchingBrowser strategy...");
                            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"WebDriverManager MatchingBrowser strategy failed: {ex.Message}");
                            Console.WriteLine("Falling back to Selenium Manager (new ChromeDriver())...");
                        }

                        driver = new ChromeDriver(new ChromeOptions());

                        break;
                }

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
            if (driver == null)
                return true;
            if (driver is ChromeDriver chrome)
                return chrome.SessionId == null;
            if (driver is EdgeDriver edge)
                return edge.SessionId == null;
            if (driver is FirefoxDriver firefox)
                return firefox.SessionId == null;
            return false;
        }
        catch
        {
            return true;
        }
    }
}
