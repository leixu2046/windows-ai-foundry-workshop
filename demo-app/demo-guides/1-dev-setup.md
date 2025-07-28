# Step 1: Set up your development environment

In this step, you'll set up your development environment to build and run the **Field Report Assistant** app. This is a native **WPF application** designed for Windows Copilot+ PCs. It includes a **skeleton UI** — your job during the workshop is to progressively integrate **local AI capabilities** using Windows AI Foundry APIs.

> 💡 The UI shell is already provided in this project. You’ll focus on wiring up local AI features such as OCR, Image Description, Semantic Search, Phi Silica, and LoRA step by step.

---

## Prerequisites

Before proceeding, make sure your development environment includes the following:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the following workloads:
  - ✅ **.NET Desktop Development**
  - ✅ **Universal Windows Platform (UWP) Development**
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- [GitHub Desktop](https://desktop.github.com/) (or Git CLI) to clone the repo

---

## Open the project in Visual Studio

1. Launch **Visual Studio 2022**
2. Select **Open a project or solution**
3. Navigate to the cloned repo and open the solution: TODO: windows-ai-foundry-workshop/demo-app/FieldReportAssistant.sln
4. If prompted, allow Visual Studio to **restore NuGet packages**

---

## Install or update Windows App SDK dependencies

This app uses **Windows App SDK** to enable Windows-native APIs such as OCR, Image Description, and other AI-related features.

1. In **Solution Explorer**, right-click the project and select **Manage NuGet Packages**
2. Under the **Installed** tab, verify that the following packages are present:

- `Microsoft.WindowsAppSDK`
- `Microsoft.Windows.SDK.Contracts`

3. If any packages are missing or outdated:
- Go to the **Browse** tab
- TODO: which build? Search and install the latest **stable version** of each package

For more information, refer to the [Windows App SDK documentation](https://learn.microsoft.com/windows/apps/windows-app-sdk/).

---

## Set the target framework and OS version

1. Right-click the project > **Properties**
2. Under the **Application** tab:
- **Target framework**: `.NET 8.0`
- **Target OS version**: `10.0.22621.0` or later (Windows 11)

---

## Confirm WinAppSDK initialization

In your `App.xaml.cs` or program entry point, ensure the app is correctly initialized to use Windows App SDK. You should see something like:

```csharp
Microsoft.WindowsAppSDK.Runtime.AppInstance appInstance = 
 Microsoft.WindowsAppSDK.Runtime.AppInstance.FindOrRegisterForKey("FieldReportAssistant");
```

If this is missing, refer to the [WinAppSDK setup guide](https://learn.microsoft.com/windows/apps/windows-app-sdk/set-up-project).

## Build and run the app

1. Ensure the **WPF project** is set as the **Startup Project**
2. Press **F5** or select **Start Debugging** in Visual Studio
3. The app should launch and display the **Field Report Assistant** UI, which includes:
   - A file picker or drag-and-drop zone
   - Placeholder buttons for AI actions
   - Output display areas for results

If the app builds successfully and the shell UI loads — you're ready to begin integrating local AI features!

