:: Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
:: Title: Rhisis install script for Windows
:: Description: This script will install the Rhisis emulator for Windows Operating Systems.
@echo OFF

:: Define messages constats
set MSBUILD_COMMAND=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
set DOTNET_COMMAND=dotnet
set DOTNET_NOT_FOUND="%DOTNET_COMMAND% command was not found. Please install the .NET Core 2.0 SDK."
set WRONG_FOLDER=You must sart the install script at the root folder of Rhisis.
set EXIT_MESSAGE=Press any key to exit installation script.

:: Check if dotnet command exists
WHERE %DOTNET_COMMAND% >nul 2>nul
if %ERRORLEVEL% neq 0 (
    call:displayErrorMessage %DOTNET_NOT_FOUND%
)

:: Check if we are at the root folder of Rhisis project
set IS_DIRECTORY_VALID=1

if not exist bin set IS_DIRECTORY_VALID=0
if not exist src set IS_DIRECTORY_VALID=0

if %IS_DIRECTORY_VALID% equ 0 (
    call:displayErrorMessage %WRONG_FOLDER%
)

:: Create dist folder if not exists
if not exist dist\bin md dist\bin
if not exist dist\bin\login md dist\bin\login
if not exist dist\bin\cluster md dist\bin\cluster
if not exist dist\bin\world md dist\bin\world

:: Compile Rhisis
%DOTNET_COMMAND% restore
%DOTNET_COMMAND% build src\Rhisis.Core\ --configuration Release
%DOTNET_COMMAND% build src\Rhisis.Database\ --configuration Release
%DOTNET_COMMAND% build src\Rhisis.Login\ --configuration Release
%DOTNET_COMMAND% build src\Rhisis.Cluster\ --configuration Release
%DOTNET_COMMAND% build src\Rhisis.World\ --configuration Release

:: Copy binaries to dist\bin folders
xcopy /E src\Rhisis.Login\bin\Release\netcoreapp2.0 dist\bin\login\
xcopy /E src\Rhisis.Cluster\bin\Release\netcoreapp2.0 dist\bin\cluster\
xcopy /E src\Rhisis.World\bin\Release\netcoreapp2.0 dist\bin\world\

:: Copy start scripts to dist folder
xcopy script\windows\login-server.bat dist\
xcopy script\windows\cluster-server.bat dist\
xcopy script\windows\world-server.bat dist\

:: End of the script, goto the End Of File
goto EOF

:: Useful methods
:displayErrorMessage
echo %~1
echo %EXIT_MESSAGE%
pause > nul
exit

:EOF