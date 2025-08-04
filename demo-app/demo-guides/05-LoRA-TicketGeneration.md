# Lab Guide 5: LoRA-Enhanced Ticket Generation

## Overview
In this final lab, you'll implement LoRA (Low-Rank Adaptation) enhanced ticket generation. LoRA allows you to use specialized, fine-tuned models that have been trained for specific tasks, providing more accurate and consistent outputs compared to general-purpose language models.

## Learning Objectives
- Understand LoRA (Low-Rank Adaptation) technology and its benefits
- Implement LanguageModelExperimental for advanced AI features
- Load and use custom LoRA adapters for specialized tasks
- Compare standard vs. LoRA model performance
- Handle the different API patterns for experimental features

## Prerequisites
- Completed Lab 4: Standard Ticket Generation
- Understanding of machine learning model fine-tuning concepts
- Knowledge of file system operations in C#
- Familiarity with experimental API patterns

## Step 1: Understanding LoRA Technology

### 1.1 What is LoRA?
**LoRA (Low-Rank Adaptation)** is a technique for efficiently fine-tuning large language models:

- **Base Model**: General-purpose language model (like Phi Silica)
- **LoRA Adapter**: Small additional parameters (~10-100MB) trained for specific tasks
- **Combined Model**: Base model + adapter = specialized behavior

**Benefits:**
- **Specialized Performance**: Better accuracy for specific domains
- **Efficiency**: Small adapter files vs. full model retraining
- **Flexibility**: Multiple adapters can be swapped for different tasks
- **Cost-Effective**: Less computational resources than full fine-tuning

### 1.2 LoRA in Windows AI Platform
Windows AI Platform supports LoRA through:
- `LanguageModelExperimental`: Extended API for advanced features
- `LowRankAdaptation`: Adapter loading and management
- System prompt handling via contexts (different from standard model)

### 1.3 API Differences: Standard vs. Experimental

**Standard LanguageModel:**
```csharp
// Simple, combined prompt approach
var prompt = $"System: {systemPrompt}\n\nUser: {userContent}";
var result = await _languageModel.GenerateResponseAsync(prompt);
```

**Experimental LanguageModel:**
```csharp
// Separate system context and user content
var context = _languageModel.CreateContext(systemPrompt);
var options = new LanguageModelOptionsExperimental { LoraAdapter = _loraAdapter };
var result = await _languageModelExperimental.GenerateResponseAsync(context, userContent, options);
```

## Step 2: Examine the LoRA Adapter File

### 2.1 Locate the Adapter
In your project, examine `Assets/lora_adapter.safetensors`:
- **File Format**: SafeTensors (secure tensor storage format)
- **Size**: Typically 10-200MB (much smaller than full models)
- **Purpose**: Construction inspection-specific fine-tuning

### 2.2 Understanding SafeTensors
SafeTensors is a secure format for storing ML model weights:
- **Security**: Prevents code injection attacks
- **Efficiency**: Fast loading and memory mapping
- **Compatibility**: Supported by most ML frameworks

*Note: In a real lab environment, you would train this adapter on construction inspection data. For this lab, we assume it's provided.*

## Step 3: Implement LoRA Initialization

### 3.1 Add Required Using Statements
Ensure these imports in `AITextService.cs`:

```csharp
using Microsoft.Windows.AI.Text.Experimental;
using System.IO;
```

### 3.2 Implement LoRA Adapter Initialization
Locate and implement the `InitializeLoraAdapterFeature()` method:

```csharp
private bool InitializeLoraAdapterFeature()
{
    try
    {
        // LoRA requires a working base language model
        if (_languageModel == null)
        {
            ShowError("Cannot initialize LoRA: Base language model not available");
            return false;
        }

        // Create experimental language model session
        _languageModelExperimental = new LanguageModelExperimental(_languageModel);

        // Define the path to the LoRA adapter file
        string adapterFilePath = Path.Combine(
            AppContext.BaseDirectory, 
            "Assets", 
            "lora_adapter.safetensors"
        );

        // Check if the adapter file exists
        if (!File.Exists(adapterFilePath))
        {
            ShowError($"LoRA adapter file not found at: {adapterFilePath}");
            return false;
        }

        // Load the LoRA adapter
        _loraAdapter = _languageModelExperimental.LoadAdapter(adapterFilePath);
        
        if (_loraAdapter == null)
        {
            ShowError("Failed to load LoRA adapter from file");
            return false;
        }

        return true;
    }
    catch (Exception ex)
    {
        HandleException(ex, "initialize LoRA adapter");
        return false;
    }
}
```

**Key Learning Points:**
- LoRA requires an existing base language model
- `LanguageModelExperimental` wraps the standard model
- Adapter files must exist and be accessible
- Loading can fail due to file corruption or compatibility issues

### 3.3 Update Main Initialization
Update the `InitializeAsync()` method to include LoRA:

```csharp
public override async Task<bool> InitializeAsync()
{
    // Initialize the base language model first
    _isTextGenerationAvailable = await InitializeTextGenerationAsync();
    
    // Initialize summarization (depends on language model)
    _isTextSummarizationAvailable = InitializeTextSummarizationFeature();
    
    // Initialize LoRA adapter (depends on language model)
    _isLoraAdapterAvailable = InitializeLoraAdapterFeature();
    
    // Service is available if any feature works
    _isAvailable = _isTextGenerationAvailable || _isTextSummarizationAvailable || _isLoraAdapterAvailable;
    return _isAvailable;
}
```

## Step 4: Implement LoRA Text Generation

### 4.1 Implement the LoRA Processing Method
Implement the `ProcessLoraTextGenerationAsync()` method:

```csharp
public async Task<string> ProcessLoraTextGenerationAsync(string systemPrompt, string userContent)
{
    // Check prerequisites
    if (!_isLoraAdapterAvailable || !_isLoraAdapterEnabled || 
        _languageModelExperimental == null || _loraAdapter == null)
    {
        return "LoRA adapter not available or disabled.";
    }

    // Validate inputs
    if (string.IsNullOrWhiteSpace(systemPrompt))
    {
        return "System prompt is required for LoRA generation.";
    }

    if (string.IsNullOrWhiteSpace(userContent))
    {
        return "User content is required for LoRA generation.";
    }

    try
    {
        // Create context with system prompt (different from standard model!)
        var context = _languageModel?.CreateContext(systemPrompt);
        
        if (context == null)
        {
            return "Failed to create language model context.";
        }

        // Configure experimental options with LoRA adapter
        var options = new LanguageModelOptionsExperimental
        {
            LoraAdapter = _loraAdapter
        };

        // Generate response using experimental API
        var result = await _languageModelExperimental.GenerateResponseAsync(context, userContent, options);
        
        return result.Text;
    }
    catch (Exception ex)
    {
        HandleException(ex, "generate text with LoRA");
        return "Failed to generate content with LoRA adapter.";
    }
}
```

**Key Learning Points:**
- LoRA uses separate system prompt and user content
- `CreateContext()` prepares the system prompt for processing
- `LanguageModelOptionsExperimental` carries the adapter reference
- The API pattern is different from standard text generation

### 4.2 Understanding Context vs. Combined Prompts

**Why Different API Patterns?**

**Standard Model**: Treats everything as one continuous text
```
"System: You are a helpful assistant.\n\nUser: What is the weather?"
```

**Experimental Model**: Separates system behavior from user query
```
Context: "You are a helpful assistant." (defines AI behavior)
User Input: "What is the weather?" (actual query)
```

This separation allows LoRA adapters to modify system behavior without affecting user input processing.

## Step 5: Test Your LoRA Implementation

### 5.1 Build and Run
1. Press **F5** to build and run the application
2. Select a report with substantial content
3. Click **"Generate Ticket (LoRA)"** button (right-most button)

### 5.2 Compare Standard vs. LoRA Output

**Test the Same Report with Both Models:**

1. Click **"Generate Ticket (Standard)"** first
2. Note the output structure, language, and detail level
3. Click **"Generate Ticket (LoRA)"** on the same report
4. Compare the differences

**Expected Differences:**
- **LoRA**: More consistent JSON structure
- **LoRA**: Better construction terminology
- **LoRA**: More accurate issue categorization
- **LoRA**: Improved recommendation specificity

### 5.3 Analyze Output Patterns

**Standard Model Output Example:**
```json
{
  "date": "2025-01-22",
  "location": "Building HVAC",
  "issues": "Various problems with heating system",
  "recommendations": "Fix the problems and maintain system"
}
```

**LoRA Model Output Example:**
```json
{
  "date": "2025-01-22",
  "location": "Administrative Building HVAC System - Main Mechanical Room",
  "issues": [
    "Central boiler efficiency 85% (below 90% specification)",
    "Ductwork accumulating debris requiring cleaning",
    "Zone dampers #2-4 stuck partially closed (airflow imbalance)"
  ],
  "recommendations": [
    "Schedule boiler tune-up and efficiency testing within 30 days",
    "Contract professional ductwork cleaning before cooling season",
    "Repair/replace actuators on dampers #2-4 immediately"
  ]
}
```

## Step 6: Understanding LoRA Model Behavior

### 6.1 LoRA Advantages in This Domain

**Better Domain Knowledge:**
- Understands construction terminology
- Recognizes safety priorities
- Knows regulatory requirements
- Uses industry-standard language

**Improved Structure:**
- Consistent JSON formatting
- Appropriate field granularity
- Logical issue categorization
- Actionable recommendations

**Enhanced Accuracy:**
- Better extraction of technical details
- Proper prioritization of issues
- More specific location descriptions
- Realistic timeline recommendations

### 6.2 When to Use LoRA vs. Standard

**Use LoRA When:**
- Domain expertise is critical
- Consistent output format is required
- Specialized terminology is important
- High accuracy is essential

**Use Standard Model When:**
- General-purpose tasks
- Quick prototyping
- Simple text generation
- Broad domain coverage needed

### 6.3 LoRA Limitations

**Potential Issues:**
- **Overfitting**: May be too specialized for varied inputs
- **Brittleness**: Sensitive to input format changes
- **Limited Scope**: May struggle with out-of-domain content
- **File Dependency**: Requires adapter file to be present

## Step 7: Advanced LoRA Configuration (Optional)

### 7.1 Dynamic Adapter Loading
For production systems, consider supporting multiple adapters:

```csharp
private readonly Dictionary<string, LowRankAdaptation> _adapterCache = new();

public async Task<LowRankAdaptation?> LoadAdapterAsync(string adapterName)
{
    if (_adapterCache.TryGetValue(adapterName, out var cached))
    {
        return cached;
    }

    var adapterPath = Path.Combine(AppContext.BaseDirectory, "Assets", $"{adapterName}.safetensors");
    
    if (File.Exists(adapterPath) && _languageModelExperimental != null)
    {
        try
        {
            var adapter = _languageModelExperimental.LoadAdapter(adapterPath);
            _adapterCache[adapterName] = adapter;
            return adapter;
        }
        catch (Exception ex)
        {
            HandleException(ex, $"load adapter {adapterName}");
        }
    }
    
    return null;
}
```

### 7.2 Adapter Validation
Verify adapter compatibility before use:

```csharp
private bool ValidateAdapter(string adapterPath)
{
    try
    {
        var fileInfo = new FileInfo(adapterPath);
        
        // Check file size (typical range: 10MB-500MB)
        if (fileInfo.Length < 1_000_000 || fileInfo.Length > 500_000_000)
        {
            return false;
        }
        
        // Check file format (SafeTensors header)
        using var stream = File.OpenRead(adapterPath);
        var header = new byte[8];
        stream.Read(header, 0, 8);
        
        // SafeTensors files start with a specific pattern
        return header[0] == 0x00 && header[1] == 0x00; // Simplified check
    }
    catch
    {
        return false;
    }
}
```

### 7.3 Performance Monitoring
Track LoRA vs. standard model performance:

```csharp
private readonly Dictionary<string, List<TimeSpan>> _performanceMetrics = new();

private async Task<string> ProcessWithMetricsAsync(string method, Func<Task<string>> processor)
{
    var stopwatch = Stopwatch.StartNew();
    var result = await processor();
    stopwatch.Stop();
    
    if (!_performanceMetrics.ContainsKey(method))
    {
        _performanceMetrics[method] = new List<TimeSpan>();
    }
    
    _performanceMetrics[method].Add(stopwatch.Elapsed);
    
    return result;
}
```

## Common Issues and Solutions

### Issue: "LoRA adapter file not found"
**Causes:**
- Adapter file missing from Assets folder
- Incorrect file path in code
- Build configuration doesn't copy assets

**Solutions:**
- Verify file exists in `Assets/lora_adapter.safetensors`
- Check file properties: Build Action = "Content", Copy to Output = "Copy always"
- Use absolute paths for debugging

### Issue: "Failed to load LoRA adapter"
**Causes:**
- Corrupted adapter file
- Incompatible adapter version
- Insufficient memory

**Solutions:**
- Re-download or regenerate adapter file
- Check adapter compatibility with current model version
- Close other applications to free memory

### Issue: LoRA output is inconsistent
**Causes:**
- Adapter overfitted to training data
- Input format differs from training examples
- Adapter not well-suited for current task

**Solutions:**
- Improve adapter training with more diverse data
- Standardize input format
- Consider retraining with current use cases

### Issue: LoRA slower than standard model
**Causes:**
- Adapter loading overhead
- Additional processing complexity
- Memory management issues

**Solutions:**
- Cache loaded adapters
- Optimize input preprocessing
- Monitor memory usage patterns

## Performance Comparison

### Typical Performance Metrics:

**Standard Model:**
- **Initialization**: 5-15 seconds
- **Processing**: 2-5 seconds per request
- **Memory**: 500MB-1GB
- **Consistency**: 70-80%

**LoRA Model:**
- **Initialization**: 10-25 seconds (including adapter loading)
- **Processing**: 3-7 seconds per request
- **Memory**: 600MB-1.2GB (base + adapter)
- **Consistency**: 85-95%

**Trade-offs:**
- LoRA: Higher accuracy, slower initialization
- Standard: Faster startup, less specialized output

## Verification Checklist
Before completing the lab series:
- [ ] LoRA adapter initializes successfully
- [ ] LoRA ticket generation produces superior JSON output
- [ ] Performance comparison shows quality improvements
- [ ] Error handling works for missing/corrupted adapters
- [ ] Both standard and LoRA models work independently
- [ ] Settings toggles control both models appropriately
- [ ] Output formatting handles both model types correctly

## Lab Series Completion

### What You've Built
Congratulations! You've implemented a complete AI-powered inspection report application with:

1. **Image Processing**: OCR text extraction and AI-generated descriptions
2. **Text Summarization**: Intelligent report summarization
3. **Standard Ticket Generation**: JSON-formatted inspection tickets
4. **LoRA-Enhanced Generation**: Specialized model for improved accuracy
5. **Settings Management**: User control over AI features
6. **Error Handling**: Robust handling of various failure scenarios

### Architecture Overview
```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   UI Layer      │    │   Service Layer  │    │   AI Platform  │
│                 │    │                  │    │                 │
│ • Sample.xaml   │───▶│ • AIImageService │───▶│ • TextRecognizer│
│ • Settings      │    │ • AITextService  │    │ • ImageDescGen  │
│ • Formatting    │    │ • AISettings     │    │ • LanguageModel │
│                 │    │                 │    │ • LoRA Adapter  │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

### Real-World Applications
This architecture can be extended for:
- **Quality Control**: Manufacturing inspection reports
- **Healthcare**: Medical examination documentation
- **Safety Audits**: Workplace safety inspections
- **Property Management**: Building maintenance reports
- **Compliance**: Regulatory inspection documentation

### Next Steps for Production
To make this production-ready, consider:
- **Data Privacy**: Ensure sensitive data stays on-device
- **Model Updates**: Implement adapter versioning and updates
- **Performance Optimization**: Implement caching and batching
- **User Training**: Provide guidance for optimal AI usage
- **Analytics**: Track AI feature usage and performance
- **Backup Systems**: Handle AI service unavailability gracefully

---
**🎓 Lab Series Complete!** You've successfully built a comprehensive AI-powered application using the Windows AI Platform, demonstrating both standard and advanced (LoRA) AI capabilities for real-world document processing tasks.