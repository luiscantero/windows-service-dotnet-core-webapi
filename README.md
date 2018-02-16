# Windows Service hosting ASP.NET Core Web API v2.0
## Description
* Visual Studio 2017 project to demo how to host a Web API service as a Windows Service.

## Steps
1. Create projects
- Windows Service (.NET Framework)
- ASP.NET Core Web API (.NET Framework)

2. NuGet references
- Install references from Web API in Windows Service, e.g. for ASP.NET Core 2.0:
  - Microsoft.AspNetCore
  - Microsoft.AspNetCore.MVC

- To run Webhost as Windows Service:
  - Microsoft.AspNetCore.Hosting.WindowsServices

3. Windows Service
- Rename Service.cs to actual service name, e.g. "FileWebApiSvc"
- Open context menu for FileWebApiSvc.cs, choose View Designer
- In Properties window, set ServiceName to FileWebApiSvc

- Copy files/folders from Web API to Windows Service and correct namespaces:
Startup.cs
appsettings.json
appsettings.Development.json
Controllers

4. Installer
- Open context menu for FileWebApiSvc.cs, choose View Designer
- Right-click on Designer window, choose Add Installer
- In the Designer for ProjectInstaller, choose serviceInstaller1
- In Properties window, set ServiceName to FileWebApiSvc
- Set Description
- Set DisplayName
- Set StartType
- In the Designer, choose serviceProcessInstaller1, set Account property to LocalSystem

5. Copy Web API bootstrapping code from Program.cs to Windows Service, make sure an IWebHost is returned to be able to call ".RunAsService()".

6. Install/uninstall service
- Open Admin Command Prompt
cd to folder containing "FileWinSvcWebApi.exe", e.g. bin\<Debug|Release>\

- Copy "libuv.dll" from \packages\Libuv.1.10.0\runtimes\win-<x86|x64>\native to compile output folder
- Use x86 version when compiling Windows Service as x68 or Any CPU, otherwise x64, configure under Properties --> Build --> Platform Target

- Run
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe FileWinSvcWebApi.exe
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe FileWinSvcWebApi.exe /u

7. Start/stop service
sc start FileWebApiSvc
sc stop FileWebApiSvc
-OR-
net start FileWebApiSvc
net stop FileWebApiSvc
-OR-
Using UI: start services.msc

8. Debug start issues
- Event viewer: eventvwr
- Windows protocolls --> Application --> Error with Source ".NET Runtime"
- Recompile and restart service, reinstalling not necessary

### License
[MIT](http://opensource.org/licenses/MIT)


## Reference
* [Walkthrough: Creating a Windows Service Application in the Component Designer](https://msdn.microsoft.com/en-us/library/zt39148a(v=vs.110).aspx)

## See also: Windows 10 Toast Notifications:
* Reference for Win10 (also available for 8.1 in another path under "Windows Kits\8.1"):
* C:\Program Files (x86)\Windows Kits\10\UnionMetadata\Windows.winmd