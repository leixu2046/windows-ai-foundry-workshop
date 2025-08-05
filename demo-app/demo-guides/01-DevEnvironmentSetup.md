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

### 2.1 Clone the Repository
1. Clone the complete repository from GitHub:
   ```bash
   git clone https://github.com/leixu2046/windows-ai-foundry-workshop.git
   cd windows-ai-foundry-workshop
   ```

### 2.2 Navigate to Demo App Folder
```bash
cd demo-app
```

The demo-app folder contains three components:
- **demo-guides/**: Implementation documentation (including this guide)
- **demo-app-start/**: Skeleton application for hands-on implementation  
- **demo-app-final/**: Complete working application with all AI features

### 2.3 Choose Your Starting Point

**Option A: Start with Skeleton (Recommended for Implementation)**
```bash
cd demo-app-start
```
- Contains UI framework and basic structure
- Ready for step-by-step AI feature implementation
- Follow the implementation guides in `../demo-guides/`

**Option B: Examine Complete Solution**
```bash
cd demo-app-final  
```
- Contains fully implemented application with all AI features
- Use for reference, comparison, or immediate demonstration
- All modules already implemented and working

### 2.4 Open in Visual Studio
1. Launch Visual Studio 2022
2. Open the solution file in your chosen folder:
   - **demo-app-start**: `InspectionReporter.sln` (skeleton)
   - **demo-app-final**: `InspectionReporter.sln` (complete)

## Step 3: Verify Project Configuration

### 3.1 Verify Solution Structure
The solution should be open in Visual Studio. Depending on your choice:
- **demo-app-start**: Basic UI framework, AI services stubbed out, ready for implementation
- **demo-app-final**: Complete implementation with all AI features working

You can also explore the repository root:
- **LoRA Fine-Tuning Training Data/**: Sample datasets for training custom LoRA adapters

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

### 4.4 Expected Result Based on Your Choice

**If using demo-app-start (skeleton):**
You should see the Inspection Reporter application with:
- ✅ Report management panel with sample inspection data
- ✅ Document editing interface with rich text capabilities  
- ✅ AI processing panel (features disabled/not implemented yet)
- ✅ Global search interface
- ✅ Feature configuration controls

**If using demo-app-final (complete):**
You should see the fully functional application with:
- ✅ All UI components working
- ✅ All AI features operational (OCR, image description, summarization, ticket generation)
- ✅ Settings panel with working feature toggles
- ✅ Complete error handling and user feedback

## Step 5: Verify Application State

### 5.1 Test Basic UI Functions
1. **Report Selection**: Click on different reports in the left panel
2. **Document Editing**: Type in the document editor  
3. **Search Bar**: Click the search box (shows "not implemented" in skeleton)
4. **Settings**: Click settings button at bottom left

### 5.2 AI Features Status
**demo-app-start (skeleton):**
- **Add Image**: Button present but disabled or shows placeholder messages
- **Summarize Report**: Button present but not functional yet
- **Generate Ticket**: Buttons present but not implemented yet

**demo-app-final (complete):**
- **Add Image**: Fully functional with OCR and description generation
- **Summarize Report**: Working AI summarization
- **Generate Ticket**: Both standard and LoRA ticket generation working

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
- **Solution**: Verify that you have the correct configuration set (x64 or ARM64)
### Performance Issues
**Issue**: Slow first build
- **Solution**: This is normal - subsequent builds will be faster

## Next Steps

### If Using demo-app-start (Implementation Path):
Ready to build AI features step by step:
- **Module 2**: Implementing OCR and Image Description
- **Module 3**: Adding Text Summarization  
- **Module 4**: Standard Ticket Generation with Phi Silica
- **Module 5**: LoRA-Enhanced Ticket Generation

### If Using demo-app-final (Demo Path):
Explore the complete working application:
- Examine implemented AI services in the `Services/` folder
- Test all AI features with sample data
- Review code architecture and patterns
- Use as reference while building your own implementation

### Bonus: LoRA Training Data Exploration
Navigate back to the repository root to explore:
- **LoRA Fine-Tuning Training Data/**: Example datasets for training custom LoRA adapters
- Use these patterns to create your own domain-specific training data

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