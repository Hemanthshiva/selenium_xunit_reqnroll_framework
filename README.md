# Selenium xUnit Reqnroll Automation Framework

A comprehensive C# test automation framework built with **Selenium WebDriver**, **xUnit**, and **Reqnroll** (formerly SpecFlow) for behavior-driven development (BDD) testing. This framework provides a robust foundation for automated web testing with integrated reporting, database management, and API testing capabilities.

## 🚀 Features

- **BDD Testing**: Gherkin-based test scenarios using Reqnroll
- **Cross-Browser Support**: Chrome WebDriver with extensible architecture
- **Page Object Model**: Clean, maintainable page object implementation
- **Allure Reporting**: Rich test reports with screenshots and detailed execution logs
- **Database Integration**: SQLite database for test data management
- **API Testing**: REST API integration for user registration
- **Configuration Management**: JSON-based configuration system
- **Parallel Execution**: xUnit test runner with parallel execution support
- **Cookie Handling**: Automatic cookie consent dialog management
- **Screenshot Capture**: Automatic screenshot capture on test failures

## 🏗️ Architecture

### Project Structure
```
selenium-xunit-reqnroll-framework/
├── Features/                    # Gherkin feature files
│   ├── ContactUs.feature
│   ├── LoginUser.feature
│   └── RegisterUser.feature
├── StepDefinitions/            # Step definition implementations
│   ├── GivenSteps.cs
│   ├── WhenSteps.cs
│   └── ThenSteps.cs
├── PageObjects/                # Page Object Model classes
│   ├── BasePage.cs
│   ├── HomePage.cs
│   └── ...
├── Utilities/                  # Utility classes
│   ├── WebDriverManager.cs
│   ├── ConfigManager.cs
│   ├── UserCredentialDb.cs
│   └── ...
├── Hooks/                      # Test hooks and lifecycle management
│   └── TestHooks.cs
├── appsettings.json           # Configuration file
├── allureConfig.json          # Allure reporting configuration
├── reqnroll.json              # Reqnroll and plugin configuration
└── README.md
```

### Key Components

#### 1. **WebDriver Management**
- Singleton pattern implementation for WebDriver lifecycle
- Automatic driver setup and teardown
- Configurable timeouts and browser settings

#### 2. **Page Object Model**
- Base page class with common functionality
- Static methods for better performance and easier usage
- Robust element interaction with error handling (e.g., handling overlays)

#### 3. **Configuration Management**
- JSON-based configuration system
- Environment-specific settings
- Centralized configuration access

#### 4. **Database Integration**
- SQLite database for test data storage
- User credential management
- Automated database initialization

#### 5. **Reporting System**
- Allure framework integration
- Automatic screenshot capture on failures
- Step-by-step execution tracking
- Rich HTML reports with timeline and history

## 🛠️ Prerequisites

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** or **Visual Studio Code**
- **Chrome Browser** (latest version)
- **Allure CLI** (for report generation)

### Installing Allure CLI
```bash
# Using npm
npm install -g allure-commandline --save-dev

# Using Scoop (Windows)
scoop install allure

# Or download from https://github.com/allure-framework/allure2/releases
```

## 📦 Dependencies

The framework uses the following NuGet packages:

```xml
<PackageReference Include="Selenium.WebDriver" Version="4.35.0" />
<PackageReference Include="Selenium.Support" Version="4.35.0" />
<PackageReference Include="Selenium.WebDriver.ChromeDriver" Version="138.0.7204.18300" />
<PackageReference Include="WebDriverManager" Version="2.17.6" />
<PackageReference Include="Reqnroll" Version="2.4.1" />
<PackageReference Include="Reqnroll.xUnit" Version="2.4.1" />
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="Allure.Net.Commons" Version="2.12.1" />
<PackageReference Include="Allure.Reqnroll" Version="2.12.1" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="9.0.9" />
<PackageReference Include="RestSharp" Version="112.1.0" />
<PackageReference Include="Bogus" Version="35.6.3" />
```

## ⚙️ Configuration

### appsettings.json
```json
{
  "BaseUrl": "https://automationexercise.com",
  "Browser": "Chrome",
  "Timeout": 15
}
```

### allureConfig.json
```json
{
  "allure": {
    "directory": "allure-results",
    "links": []
  }
}
```

### reqnroll.json
```json
{
  "plugins": [
    { "name": "Reqnroll.xUnit" },
    { "name": "Allure" }
  ]
}
```

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd selenium-xunit-reqnroll-framework
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

### 4. Run Tests
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "ContactUsForm"

# Run with verbose output
dotnet test --verbosity normal
```

### 5. Generate Allure Reports

After running the tests, the results will be generated in `bin/Debug/net8.0/allure-results`.

```bash
# Navigate to the output directory
cd bin/Debug/net8.0

# Generate and serve report (opens in browser)
allure serve allure-results

# Or generate static report
allure generate allure-results -o allure-report --clean
```

**Note**: If you want to run `allure serve` from the project root, you must point to the correct results path:
```bash
allure serve bin/Debug/net8.0/allure-results
```

## 📊 Reporting Features

The framework uses **Allure** for comprehensive test reporting:

- **Test Results**: Pass/Fail status for each scenario
- **Step Details**: Individual step execution results with Gherkin steps
- **Screenshots**: Automatic screenshot capture on failures
- **Timeline**: Test execution timeline visualization
- **Categories**: Test categorization and filtering
- **History**: Test execution history tracking
- **Attachments**: File uploads and error logs

### Troubleshooting Allure
If reports are blank:
1. Ensure `reqnroll.json` is copied to the output directory (handled by `.csproj`).
2. Ensure `Allure.Reqnroll` plugin is correctly configured in `reqnroll.json`.
3. Verify `allure-results` folder contains `.json` files after test execution.

## 🔄 CI/CD Pipeline

The project is configured with a **GitHub Actions** workflow that automatically builds, tests, and generates reports on every push to the `main` branch.

### Workflow Overview
The pipeline defined in `.github/workflows/build-and-test.yml` performs the following steps:

1.  **Build & Test** (Windows Environment):
    *   Sets up .NET 8 and Node.js
    *   Installs Allure CLI
    *   Builds the solution in Release mode
    *   Executes all tests using `dotnet test`
2.  **Report Generation**:
    *   Generates a static Allure HTML report from the test results
    *   Uploads the report as a workflow artifact
3.  **Deployment** (Ubuntu Environment):
    *   Downloads the generated report
    *   Deploys the report to **GitHub Pages**

### Accessing CI Reports
Once the pipeline completes successfully, the latest test report is automatically published to GitHub Pages:

1.  Navigate to your repository's **Settings** > **Pages** to see the live URL.
2.  The URL format is: `https://hemanthshiva.github.io/selenium_xunit_reqnroll_framework/`

## 📝 Test Scenarios

### Contact Us Feature
```gherkin
Feature: Contact Us
  As a user
  I want to contact Automation Exercise
  So that I can submit my query

Scenario: Contact Us Form
  Given I am on the home page
  Then the page title should contain "Automation Exercise"
  When I click on Contact Us button
  Then GET IN TOUCH is visible
  When I enter contact name, email, subject and message
  And I upload a file
  And I click Submit button
  And I accept the alert
  Then Success message is visible
  When I click Home button
  Then I am on the home page
```

## 🔧 Key Features Explained

### 1. **Cookie Consent Handling**
The framework automatically handles cookie consent dialogs via `HomePage.HandleCookiePopup()`, using `WebDriverWait` to ensure the popup is interactable before clicking.

### 2. **Robust Element Interaction**
Elements are clicked with fallback to JavaScript execution to handle overlays and interception issues.

### 3. **Database Integration**
SQLite database is used for storing test user credentials, allowing data-driven testing with known states.

### 4. **API Integration**
REST API calls are used for fast user registration setup in preconditions (`Given 5 users are registered via API`).

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
