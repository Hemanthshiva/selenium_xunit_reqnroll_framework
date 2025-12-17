@echo off
echo Generating Allure Report...
echo.

REM Check if allure-results directory exists
if not exist "bin\Debug\net8.0\allure-results" (
    echo Error: No test results found. Please run tests first using 'dotnet test'
    pause
    exit /b 1
)

REM Generate static report
echo Generating static Allure report...
powershell -ExecutionPolicy Bypass -Command "allure generate 'bin\Debug\net8.0\allure-results' -o allure-report --clean"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✓ Allure report generated successfully!
    echo.
    echo You can now:
    echo 1. Open allure-report\index.html in your browser
    echo 2. Or run 'allure serve "bin/Debug/net8.0/allure-results"' for live server
    echo.
) else (
    echo.
    echo ✗ Failed to generate Allure report
    echo Make sure Allure CLI is installed: npm install -g allure-commandline
    echo.
)

pause