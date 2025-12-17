---
description: Repository Information Overview
alwaysApply: true
---

# Selenium xUnit Reqnroll Automation Framework Information

## Summary
A comprehensive C# test automation framework built with Selenium WebDriver, xUnit, and Reqnroll (formerly SpecFlow) for behavior-driven development (BDD) testing. The framework provides automated web testing with integrated reporting, database management, and API testing capabilities.

## Structure
- **Features/**: Gherkin feature files defining test scenarios
- **StepDefinitions/**: C# implementations of Gherkin steps
- **PageObjects/**: Page Object Model classes for web page interactions
- **Utilities/**: Helper classes for WebDriver, configuration, and database management
- **Hooks/**: Test lifecycle management and setup/teardown logic

## Language & Runtime
**Language**: C#
**Version**: .NET 8.0
**Build System**: MSBuild
**Package Manager**: NuGet

## Dependencies
**Main Dependencies**:
- Selenium.WebDriver (4.33.0)
- Selenium.Support (4.33.0)
- Reqnroll (2.4.1)
- Reqnroll.xUnit (2.4.1)
- xunit (2.9.3)
- Microsoft.NET.Test.Sdk (17.14.1)
- Allure.Net.Commons (2.12.1)
- Microsoft.Data.Sqlite (9.0.6)
- RestSharp (112.1.0)
- Bogus (35.6.3)

**Development Dependencies**:
- coverlet.collector (6.0.4)
- xunit.runner.visualstudio (3.1.1)

## Build & Installation
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test
```

## Testing
**Framework**: xUnit with Reqnroll (BDD)
**Test Location**: Features/*.feature
**Naming Convention**: Feature-based naming (e.g., RegisterUser.feature)
**Configuration**: reqnroll.json
**Run Command**:
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "ContactUsForm"

# Generate and serve Allure report
allure serve allure-results
```

## Key Components

### WebDriver Management
The framework implements a singleton pattern for WebDriver lifecycle management in `WebDriverManager.cs`, providing automatic driver setup, configuration, and cleanup.

### Page Object Model
Implements the Page Object pattern with a `BasePage.cs` class that provides common functionality for all page objects, with specialized page classes for different website sections.

### BDD Testing
Uses Reqnroll for BDD-style testing with Gherkin syntax, allowing for readable test scenarios defined in feature files with corresponding step definitions in C#.

### Reporting
Integrates Allure reporting framework for comprehensive test reports with screenshots, step-by-step execution tracking, and detailed test results.

### Database Integration
Utilizes SQLite for test data management, particularly for storing user credentials and test data through the `UserCredentialDb.cs` class.

### API Testing
Includes REST API integration for user registration and other operations using RestSharp through the `ApiUserRegistrationClient.cs` class.