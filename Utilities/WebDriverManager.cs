// WebDriver management utility (singleton pattern)
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace selenium_xunit_reqnroll_framework.Utilities
{
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
                            // Detect installed Chrome full and major version
                            string? fullVersion = null;
                            string? majorVersion = null;
                            try
                            {
                                var chromePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
                                if (!File.Exists(chromePath))
                                    chromePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";

                                if (File.Exists(chromePath))
                                {
                                    var info = FileVersionInfo.GetVersionInfo(chromePath);
                                    fullVersion = info.FileVersion; // e.g. 138.0.7204.51
                                    if (!string.IsNullOrEmpty(fullVersion))
                                        majorVersion = fullVersion.Split('.').FirstOrDefault();
                                    Console.WriteLine($"Detected Chrome version: {fullVersion}");
                                }
                            }
                            catch (System.Exception ex) { Console.WriteLine($"Failed to detect Chrome version: {ex.Message}"); }

                            var mgr = new DriverManager();
                            try
                            {
                                // Attempt to download driver matching full version, fallback to major version
                                if (!string.IsNullOrEmpty(fullVersion))
                                {
                                    try { mgr.SetUpDriver(new ChromeConfig(), fullVersion); Console.WriteLine($"Requested chromedriver for full version {fullVersion}"); }
                                    catch { if (!string.IsNullOrEmpty(majorVersion)) { try { mgr.SetUpDriver(new ChromeConfig(), majorVersion); Console.WriteLine($"Requested chromedriver for major version {majorVersion}"); } catch { mgr.SetUpDriver(new ChromeConfig()); Console.WriteLine("Requested chromedriver default"); } } else { mgr.SetUpDriver(new ChromeConfig()); Console.WriteLine("Requested chromedriver default"); } }
                                }
                                else
                                {
                                    mgr.SetUpDriver(new ChromeConfig());
                                    Console.WriteLine("Requested chromedriver default");
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Console.WriteLine($"WebDriverManager setup failed: {ex.Message}");
                                try { mgr.SetUpDriver(new ChromeConfig()); } catch { }
                            }

                            // Try to locate chromedriver.exe in WebDriverManager cache for the detected version
                            string? driverExePath = null;
                            try
                            {
                                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                var wdmBase = Path.Combine(userProfile, ".wdm", "drivers", "chromedriver", "win32");
                                if (Directory.Exists(wdmBase))
                                {
                                    var subdirs = Directory.GetDirectories(wdmBase);
                                    // Inspect exe file versions to find a match for majorVersion
                                    string? match = null;
                                    foreach (var d in subdirs.OrderByDescending(d => d))
                                    {
                                        var exe = Path.Combine(d, "chromedriver.exe");
                                        if (File.Exists(exe))
                                        {
                                            try
                                            {
                                                var v = FileVersionInfo.GetVersionInfo(exe).FileVersion;
                                                Console.WriteLine($"Found chromedriver at {exe} with version {v}");
                                                if (!string.IsNullOrEmpty(majorVersion) && v?.Split('.').FirstOrDefault() == majorVersion)
                                                {
                                                    match = d;
                                                    break;
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    // if no exact major match, pick latest
                                    if (match == null)
                                        match = subdirs.OrderByDescending(d => d).FirstOrDefault();

                                    if (!string.IsNullOrEmpty(match))
                                    {
                                        var exe = Path.Combine(match, "chromedriver.exe");
                                        if (File.Exists(exe)) driverExePath = exe;
                                    }
                                }
                            }
                            catch { /* ignore */ }

                            try
                            {
                                if (!string.IsNullOrEmpty(driverExePath))
                                {
                                    Console.WriteLine($"Using chromedriver.exe at: {driverExePath}");
                                    var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(driverExePath), Path.GetFileName(driverExePath));
                                    driver = new ChromeDriver(service, new ChromeOptions());
                                }
                                else
                                {
                                    Console.WriteLine("chromedriver.exe not found in WDM cache, creating default ChromeDriver");
                                    driver = new ChromeDriver();
                                }
                            }
                            catch (OpenQA.Selenium.WebDriverException ex)
                            {
                                Console.WriteLine($"ChromeDriver session creation failed: {ex.Message}");
                                // If session creation fails due to version mismatch, try to download driver for the major version and retry
                                if (!string.IsNullOrEmpty(majorVersion))
                                {
                                    try
                                    {
                                        Console.WriteLine($"Retrying WebDriverManager setup for major version {majorVersion}");
                                        mgr.SetUpDriver(new ChromeConfig(), majorVersion);
                                        // try to find exe again
                                        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                        var wdmBase = Path.Combine(userProfile, ".wdm", "drivers", "chromedriver", "win32");
                                        if (Directory.Exists(wdmBase))
                                        {
                                            var subdirs = Directory.GetDirectories(wdmBase).OrderByDescending(d => d);
                                            foreach (var d in subdirs)
                                            {
                                                var exe = Path.Combine(d, "chromedriver.exe");
                                                if (File.Exists(exe))
                                                {
                                                    var v = FileVersionInfo.GetVersionInfo(exe).FileVersion;
                                                    if (!string.IsNullOrEmpty(v) && v.Split('.').FirstOrDefault() == majorVersion)
                                                    {
                                                      driverExePath = exe; break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(driverExePath))
                                        {
                                            var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(driverExePath), Path.GetFileName(driverExePath));
                                            driver = new ChromeDriver(service, new ChromeOptions());
                                        }
                                    }
                                    catch (System.Exception retryEx)
                                    {
                                        Console.WriteLine($"Retry failed: {retryEx.Message}");
                                    }
                                }
                                if (driver == null)
                                    throw; // rethrow original
                            }

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
}
