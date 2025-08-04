# Lab Guide 1: Development Environment Setup

## Overview
This guide will walk you through setting up your development environment for building AI-powered Windows applications using Windows AI APIs.

## Prerequisites
- Windows 11 (Build 22000 or later)
- Administrator access for package installation
- Internet connection for downloading dependencies

## Step 1: Install Visual Studio 2022

### 1.1 Download Visual Studio
1. Go to [Visual Studio Downloads](https://visualstudio.microsoft.com/downloads/)
2. Download **Visual Studio 2022 Community** (free) or higher edition
3. Run the installer

### 1.2 Select Workloads
During installation, ensure you select these workloads:
- ✅ **.NET desktop development**
- ✅ **Universal Windows Platform development**

### 1.3 Individual Components (Important!)
In the **Individual components** tab, make sure to select:
- ✅ **Windows 11 SDK (10.0.26100.0)** or latest
- ✅ **.NET 9.0 Runtime** or latest
- ✅ **Git for Windows**

## Step 2: Clone the Repository

### 2.1 Open Visual Studio
1. Launch Visual Studio 2022
2. On the start screen, select **"Clone a repository"**

### 2.2 Clone from GitHub
1. In the repository URL field, enter:
   ```
   https://github.com/your-org/InspectionReporter
   ```
2. Choose a local path for the project
3. Click **"Clone"**

*Alternative: Use Git command line*
```bash
git clone https://github.com/your-org/InspectionReporter.git
cd InspectionReporter
```

## Step 3: Verify Project Configuration

### 3.1 Open the Solution
1. Navigate to the cloned folder
2. Double-click `InspectionReporter.sln` to open in Visual Studio

### 3.2 Check Windows App SDK Reference
1. In **Solution Explorer**, expand your project
2. Expand **Dependencies** → **Packages**
3. Verify you see:
   - ✅ **Microsoft.WindowsAppSDK** (1.8.x experimental)
   - ✅ **Microsoft.Windows.SDK.BuildTools**

### 3.3 Verify Target Framework
1. Right-click the project in Solution Explorer
2. Select **Properties**
3. In **Application** tab, verify:
   - **Target framework**: `net9.0-windows10.0.26100.0`
   - **Target OS version**: `10.0.26100.0`

## Step 4: First Build and Deploy

### 4.1 Set Startup Project
1. In Solution Explorer, right-click the main project
2. Select **"Set as Startup Project"**

### 4.2 Select Target Architecture
1. In the toolbar, change the architecture dropdown from **"Any CPU"** to:
   - **x64** (for Intel/AMD processors)
   - **ARM64** (for ARM processors like Surface Pro X)

### 4.3 Build and Run
1. Press **F5** or click the **"Start Debugging"** button
2. First run will trigger:
   - NuGet package restoration
   - Project compilation
   - Package deployment
   - App launch

### 4.4 Expected Result
You should see the AI Inspection Report application with:
- ✅ Left panel with sample reports
- ✅ Middle panel with document editor
- ✅ Right panel with AI tools (initially disabled)
- ✅ Top search bar
- ✅ Settings button at bottom left

## Step 5: Verify Skeleton UI

### 5.1 Test Basic Functionality
1. **Report Selection**: Click on different reports in the left panel
2. **Document Editing**: Type in the document editor
3. **Search Bar**: Click the search box (should show "not implemented" dialog)
4. **Settings**: Click settings button at bottom left

### 5.2 Check AI Features (Should be Disabled)
1. **Add Image**: Button should be present but may be disabled
2. **Summarize Report**: Button should be present but may be disabled
3. **Generate Ticket**: Buttons should be present but may be disabled

## Troubleshooting

### Build Errors
**Error**: "Windows App SDK not found"
- **Solution**: Install Windows App SDK from [Microsoft Store](https://apps.microsoft.com/store/detail/windows-app-runtime/9P7JF9WZNV1C) or via Visual Studio Installer

**Error**: "Target framework not supported"
- **Solution**: Update Visual Studio to latest version and install .NET 9.0 SDK

### Runtime Errors
**Error**: "Package deployment failed"
- **Solution**: Enable Developer Mode in Windows Settings → Privacy & Security → For developers

**Error**: "App crashes on startup"
- **Solution**: This is expected in the skeleton - AI services are not implemented yet

### Performance Issues
**Issue**: Slow first build
- **Solution**: This is normal - subsequent builds will be faster

## Next Steps
Once your environment is set up and the skeleton app runs successfully, you're ready to proceed to:
- **Lab Guide 2**: Implementing OCR and Image Description
- **Lab Guide 3**: Adding Text Summarization
- **Lab Guide 4**: Standard Ticket Generation with Phi Silica
- **Lab Guide 5**: LoRA-Enhanced Ticket Generation

## Verification Checklist
Before proceeding to the next lab:
- [ ] Visual Studio 2022 installed with correct workloads
- [ ] Project cloned and opens without errors
- [ ] Windows App SDK references verified
- [ ] App builds and runs successfully (F5)
- [ ] Skeleton UI displays correctly
- [ ] Basic UI interactions work (clicking, typing)

---
**🎓 Lab Complete!** You now have a working development environment and skeleton app ready for AI feature implementation.