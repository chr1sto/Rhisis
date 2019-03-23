@ECHO off
SET VSVER=0
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\" SET VSVER=Enterprise
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\" SET VSVER=Professional
IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\" SET VSVER=Community

IF %VSVER% equ 0 (
	echo You need to install Visual Studio to compile Rhisis.
) ELSE (
	"C:\Program Files (x86)\Microsoft Visual Studio\2017\%VSVER%\MSBuild\15.0\Bin\MSBuild.exe" ../Rhisis.sln
)