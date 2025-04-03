# Makefile for building a .NET app and deploying to USB drives
# Assumes Windows environment with PowerShell

.PHONY: all build deploy clean

# Path variables
SOURCE_EXE = C:\Users\kasper\OneDrive - Innlandet fylkeskommune\Dokumenter\GitHub\Elever-ProgramOppsett\ProgramApp\bin\Release\net8.0\win-x64\publish\ProgramApp.exe
PROJECT_PATH = C:\Users\kasper\OneDrive - Innlandet fylkeskommune\Dokumenter\GitHub\Elever-ProgramOppsett\ProgramApp

# Default target
all: build deploy

# Build the application
build:
	@echo Building .NET application...
	cd $(PROJECT_PATH) && dotnet publish -c Release -r win-x64 --self-contained
	@echo Build completed.

# Deploy to all USB drives
deploy:
	@echo Deploying to USB drives...
	powershell -Command "$$drives = Get-WmiObject Win32_LogicalDisk | Where-Object { $$_.DriveType -eq 2 } | Select-Object -ExpandProperty DeviceID; if ($$drives) { foreach ($$drive in $$drives) { if (Test-Path '$(SOURCE_EXE)') { Copy-Item -Path '$(SOURCE_EXE)' -Destination \"$$drive\\ProgramApp.exe\" -Force; echo \"Deployed to $$drive\" } else { echo \"Source executable not found!\" } } } else { echo \"No USB drives detected!\" }"
	@echo Deployment completed.

# # Clean build artifacts
# clean:
# 	@echo Cleaning build artifacts...
# 	cd $(PROJECT_PATH) && dotnet clean -c Release
# 	@echo Clean completed.