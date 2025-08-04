# Module 1: Development Environment Setup

## Overview
This module establishes the development environment for building AI-powered Windows applications using the Windows AI Foundry AI APIs. Designed for enterprise developers, system integrators, and PC manufacturers creating demo applications.

## System Requirements
- Windows 11 Pro/Enterprise (Build 22000 or later)
- Administrator privileges for development tools installation
- High-speed internet connection for SDK and model downloads
- Minimum 16GB RAM recommended for AI model processing

## Step 1: Install Visual Studio 2022

### 1.1 Download Visual Studio
1. Go to [Visual Studio Downloads](https://visualstudio.microsoft.com/downloads/)
2. Download **Visual Studio 2022 Professional** or **Enterprise** edition
3. Run the installer with administrator privileges

### 1.2 Select Workloads
During installation, ensure you select these workloads:
- ✅ **.NET desktop development**
- ✅ **Universal Windows Platform development**

### 1.3 Individual Components (Important!)
In the **Individual components** tab, make sure to select:
- ✅ **Windows 11 SDK (10.0.26100.0)** or latest
- ✅ **.NET 9.0 Runtime** or latest
- ✅ **Git for Windows**

## Step 2: Acquire the Implementation Codebase

### 2.1 Open Visual Studio
1. Launch Visual Studio 2022
2. On the start screen, select **"Clone a repository"**

### 2.2 Clone Enterprise Repository
1. In the repository URL field, enter your organization's repository:
   ```
   https://github.com/your-enterprise-org/InspectionReporter
   ```
2. Select appropriate local development directory
3. Click **"Clone"**

*Alternative: Enterprise Git workflow*
```bash
git clone https://github.com/your-enterprise-org/InspectionReporter.git
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
You should see the Inspection Reporter application with:
- ✅ Report management panel with sample inspection data
- ✅ Document editing interface with rich text capabilities
- ✅ AI processing panel (features initially disabled until implementation)
- ✅ Global search interface
- ✅ Feature configuration controls

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
**✅ Module Complete!** Your development environment is configured and the foundation application is ready for AI feature implementation.