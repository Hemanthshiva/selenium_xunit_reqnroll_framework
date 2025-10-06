#!/usr/bin/env pwsh

Write-Host "Generating Allure Report..." -ForegroundColor Green
Write-Host ""

# Check if allure-results directory exists
$resultsPath = "bin/Debug/net8.0/allure-results"
if (-not (Test-Path $resultsPath)) {
    Write-Host "Error: No test results found at $resultsPath" -ForegroundColor Red
    Write-Host "Please run tests first using 'dotnet test'" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Check if Allure CLI is installed
try {
    $allureVersion = & allure --version 2>$null
    Write-Host "Using Allure CLI version: $allureVersion" -ForegroundColor Cyan
} catch {
    Write-Host "Error: Allure CLI not found" -ForegroundColor Red
    Write-Host "Please install Allure CLI: npm install -g allure-commandline" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Generate static report
Write-Host "Generating static Allure report..." -ForegroundColor Yellow
try {
    & allure generate $resultsPath -o allure-report --clean
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✓ Allure report generated successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "You can now:" -ForegroundColor Cyan
        Write-Host "1. Open allure-report/index.html in your browser" -ForegroundColor White
        Write-Host "2. Or run 'allure serve `"bin/Debug/net8.0/allure-results`"' for live server" -ForegroundColor White
        Write-Host ""
        
        # Ask if user wants to open the report
        $openReport = Read-Host "Would you like to open the report now? (y/n)"
        if ($openReport -eq 'y' -or $openReport -eq 'Y') {
            if (Test-Path "allure-report/index.html") {
                Start-Process "allure-report/index.html"
            }
        }
    } else {
        Write-Host ""
        Write-Host "✗ Failed to generate Allure report" -ForegroundColor Red
    }
} catch {
    Write-Host "Error generating report: $_" -ForegroundColor Red
}

Read-Host "Press Enter to exit"