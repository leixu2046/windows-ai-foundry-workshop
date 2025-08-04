# Module 4: Standard Ticket Generation with Phi Silica

## Overview
This module implements structured ticket generation using the Windows AI Foundry AI APIs standard language model (Phi Silica). Enterprise developers will create automated JSON-formatted inspection tickets with standardized fields and professional formatting for business integration.

## Implementation Objectives
- Master prompt engineering for consistent structured business outputs
- Deploy JSON-formatted AI response systems for enterprise integration
- Implement standard LanguageModel for scalable text generation services
- Build robust JSON parsing and validation for production systems
- Create professional ticket formatting suitable for business workflows

## Prerequisites
- Completed Module 3: Text Summarization
- Working with `demo-app/demo-app-start/` folder for hands-on implementation
- Proficiency with JSON format, parsing, and enterprise data interchange
- Advanced prompt engineering and optimization techniques
- Expert-level C# string manipulation and data processing

**💡 Tip**: Reference the complete implementation in `demo-app/demo-app-final/Sample.xaml.cs` (GenerateTicketStandardBtn_Click method) for the finished code.

## Step 1: Understanding Structured Text Generation

### 1.1 Standard vs. Specialized Models
**TextSummarizer** (Lab 3):
- Pre-built, optimized for summarization
- Fixed behavior and output format
- Easy to use but limited customization

**LanguageModel** (This Lab):
- Flexible, prompt-driven text generation
- Custom prompts and output formats
- More control but requires prompt engineering

### 1.2 JSON Ticket Structure
Our target ticket format:
```json
{
    "date": "2025-01-22",
    "location": "Administrative Building HVAC System",
    "issues": [
        "Central boiler operating below efficiency specifications",
        "Ductwork requires cleaning before cooling season",
        "Several dampers stuck in partially closed position"
    ],
    "recommendations": [
        "Schedule boiler efficiency testing and adjustment",
        "Coordinate ductwork cleaning project",
        "Repair stuck dampers within two weeks"
    ]
}
```

## Step 2: Implement Standard Text Generation

### 2.1 Implement the Generation Method
In `AITextService.cs`, implement the `ProcessTextGenerationAsync()` method:

```csharp
public async Task<string> ProcessTextGenerationAsync(string prompt)
{
    // Check prerequisites
    if (!_isTextGenerationAvailable || !_isTextGenerationEnabled || _languageModel == null)
    {
        return "Text generation not available or disabled.";
    }

    // Validate input
    if (string.IsNullOrWhiteSpace(prompt))
    {
        return "No prompt provided for text generation.";
    }

    try
    {
        // Generate response using the language model
        var result = await _languageModel.GenerateResponseAsync(prompt);
        
        // Return the raw text (will be processed by caller)
        return result.Text;
    }
    catch (Exception ex)
    {
        HandleException(ex, "generate text");
        return "Failed to generate content. Please try again.";
    }
}
```

**Key Learning Points:**
- Uses the same `_languageModel` as summarization
- Takes raw prompts (gives full control over AI behavior)
- Returns raw responses (caller handles formatting)

### 2.2 Update Service Initialization
Ensure your main `InitializeAsync()` method includes text generation:

```csharp
public override async Task<bool> InitializeAsync()
{
    // Initialize the core language model
    _isTextGenerationAvailable = await InitializeTextGenerationAsync();
    
    // Initialize specialized services that depend on the language model
    _isTextSummarizationAvailable = InitializeTextSummarizationFeature();
    
    // TODO: LoRA initialization will be added in next lab
    
    // Service is available if any feature works
    _isAvailable = _isTextGenerationAvailable || _isTextSummarizationAvailable;
    return _isAvailable;
}
```

## Step 3: Design the Ticket Generation Prompt

### 3.1 Understanding Prompt Engineering
Effective prompts for structured output need:

1. **Clear Instructions**: What the AI should do
2. **Output Format**: Exact structure expected
3. **Context**: Domain-specific information
4. **Examples**: Show desired behavior (optional)
5. **Constraints**: What to avoid or handle

### 3.2 Examine the System Prompt
In `Sample.xaml.cs`, locate the `GenerateTicketStandardBtn_Click` method and study the prompt:

```csharp
// The system prompt defines the AI's behavior and output format
var systemPrompt = "The input is a construction inspection note with possibly some descriptions of photos taken on site, return a json with date, location, issues, and recommendations. If certain information is missing, do not make it up, just fill in: To-Be-Specified.";

// For standard models, combine system and user content
var combinedPrompt = $"System: {systemPrompt}\n\nUser: {reportContent}";

// Generate the response
var result = await (_textService?.ProcessTextGenerationAsync(combinedPrompt) ?? Task.FromResult("Text service not available"));
```

**Key Design Decisions:**
- **JSON Output**: Ensures structured, parseable results
- **Specific Fields**: Date, location, issues, recommendations
- **Missing Data Handling**: "To-Be-Specified" instead of hallucination
- **Domain Context**: "Construction inspection note" sets the context

### 3.3 Prompt Engineering Best Practices

**Good Prompt Characteristics:**
```csharp
// ✅ GOOD: Specific, structured, with clear expectations
var goodPrompt = @"
System: You are an AI assistant specializing in construction inspection reports. 
Analyze the following inspection data and create a structured ticket.

Output Format: Return valid JSON with these exact fields:
- date: Inspection date (YYYY-MM-DD format)
- location: Specific location inspected
- issues: Array of identified problems
- recommendations: Array of suggested actions

Rules:
1. Use only information from the input - do not invent details
2. If information is missing, use 'To-Be-Specified'
3. Keep issues and recommendations concise but specific
4. Ensure JSON is valid and properly formatted

Input Data: {reportContent}";
```

**Poor Prompt Examples:**
```csharp
// ❌ BAD: Too vague, no structure guidance
var badPrompt1 = "Make a ticket from this report: {reportContent}";

// ❌ BAD: No output format specified
var badPrompt2 = "Analyze this inspection and tell me what's wrong: {reportContent}";

// ❌ BAD: Encourages hallucination
var badPrompt3 = "Create a detailed inspection ticket with all necessary information: {reportContent}";
```

## Step 4: Implement JSON Parsing and Formatting

### 4.1 Examine the JSON Parser
In `Sample.xaml.cs`, study the `FormatTicketFromJson()` method:

```csharp
private string FormatTicketFromJson(string jsonResponse)
{
    try
    {
        using var document = JsonDocument.Parse(jsonResponse);
        var root = document.RootElement;

        var ticket = "🏗️ CONSTRUCTION INSPECTION REPORT\n";
        ticket += "═══════════════════════════════════════\n\n";

        // Extract date field
        if (root.TryGetProperty("date", out var date))
        {
            ticket += $"📅 DATE:\n{date.GetString()}\n\n";
        }

        // Extract location field
        if (root.TryGetProperty("location", out var location))
        {
            ticket += $"📍 LOCATION:\n{location.GetString()}\n\n";
        }

        // Extract issues (handle both array and string formats)
        if (root.TryGetProperty("issues", out var issues))
        {
            ticket += "⚠️ ISSUES IDENTIFIED:\n";
            if (issues.ValueKind == JsonValueKind.Array)
            {
                foreach (var issue in issues.EnumerateArray())
                {
                    ticket += $"• {issue.GetString()}\n";
                }
            }
            else
            {
                ticket += $"{issues.GetString()}\n";
            }
            ticket += "\n";
        }

        // Extract recommendations (handle both array and string formats)
        if (root.TryGetProperty("recommendations", out var recommendations))
        {
            ticket += "🔧 RECOMMENDATIONS:\n";
            if (recommendations.ValueKind == JsonValueKind.Array)
            {
                foreach (var recommendation in recommendations.EnumerateArray())
                {
                    ticket += $"• {recommendation.GetString()}\n";
                }
            }
            else
            {
                ticket += $"{recommendations.GetString()}\n";
            }
            ticket += "\n";
        }

        // Add metadata
        ticket += "═══════════════════════════════════════\n";
        ticket += $"Report Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
        ticket += "Report ID: IR-" + DateTime.Now.ToString("yyyy") + "-" + new Random().Next(1000, 9999);

        return ticket;
    }
    catch (JsonException)
    {
        // Fallback for invalid JSON
        return $"🏗️ Construction Inspection Report (Raw JSON):\n\n{jsonResponse}";
    }
    catch (Exception ex)
    {
        // Fallback for other errors
        return $"🏗️ Inspection Report Generation Error:\n\n{ex.Message}\n\nRaw Response:\n{jsonResponse}";
    }
}
```

**Key Learning Points:**
- `JsonDocument.Parse()` handles JSON parsing safely
- `TryGetProperty()` handles missing fields gracefully
- Support both array and string formats (AI sometimes varies output)
- Always include raw output for debugging
- Professional formatting with Unicode icons

### 4.2 Understanding Flexible JSON Parsing
AI models don't always return perfectly consistent JSON. Handle variations:

```csharp
// AI might return issues as an array:
"issues": ["Problem 1", "Problem 2", "Problem 3"]

// Or as a single string:
"issues": "Multiple problems identified including ventilation and structural concerns"

// Parser handles both cases:
if (issues.ValueKind == JsonValueKind.Array)
{
    // Handle array format
    foreach (var issue in issues.EnumerateArray())
    {
        ticket += $"• {issue.GetString()}\n";
    }
}
else
{
    // Handle string format
    ticket += $"{issues.GetString()}\n";
}
```

## Step 5: Test Your Implementation

### 5.1 Build and Run
1. Press **F5** to build and run the application
2. Select a report with substantial content
3. Add images and ensure OCR/description worked (from Labs 2-3)
4. Click **"Generate Ticket (Standard)"** button

### 5.2 Test Different Scenarios

**Test Case 1: Complete Report**
- Use pre-populated sample report
- Add 2-3 images with good OCR results
- Expected: Well-structured JSON ticket with all fields

**Test Case 2: Minimal Data**
- Create report with basic title and minimal content
- Expected: JSON with "To-Be-Specified" for missing information

**Test Case 3: Image-Heavy Report**
- Report with mostly images and OCR text
- Expected: Issues and recommendations based on image analysis

### 5.3 Analyze the Raw Output
The application shows both raw AI output and formatted ticket:

```
RAW STANDARD OUTPUT:
══════════════════════════════════════════════════
{
  "date": "2025-01-22",
  "location": "Administrative Building HVAC System",
  "issues": [
    "Central boiler operating at 85% efficiency below specifications",
    "Ductwork shows accumulated dust buildup",
    "Several dampers on second floor stuck in partially closed position"
  ],
  "recommendations": [
    "Schedule boiler efficiency testing and adjustment",
    "Coordinate professional ductwork cleaning before cooling season",
    "Repair stuck dampers within two weeks"
  ]
}
══════════════════════════════════════════════════

🏗️ CONSTRUCTION INSPECTION REPORT
═══════════════════════════════════════

📅 DATE:
2025-01-22

📍 LOCATION:
Administrative Building HVAC System

⚠️ ISSUES IDENTIFIED:
• Central boiler operating at 85% efficiency below specifications
• Ductwork shows accumulated dust buildup
• Several dampers on second floor stuck in partially closed position

🔧 RECOMMENDATIONS:
• Schedule boiler efficiency testing and adjustment
• Coordinate professional ductwork cleaning before cooling season
• Repair stuck dampers within two weeks

═══════════════════════════════════════
Report Generated: 2025-08-03 14:30:22
Report ID: IR-2025-7839
```

## Step 6: Understanding AI Model Behavior

### 6.1 Standard Model Characteristics
The Windows AI APIs standard language model (Phi Silica):
- **Size**: Optimized for local processing (~3GB)
- **Speed**: Fast inference (2-5 seconds for typical prompts)
- **Capabilities**: General text understanding and generation
- **Limitations**: May struggle with very complex reasoning

### 6.2 Prompt Response Patterns
**Consistent Responses:**
- Usually follows JSON format when requested
- Good at extracting obvious information
- Respects "don't make up information" instructions

**Variable Behaviors:**
- Array vs. string output for lists
- Occasional formatting variations
- Different levels of detail based on input complexity

### 6.3 Quality Factors
Output quality depends on:
- **Input Quality**: Well-structured reports produce better tickets
- **Prompt Clarity**: Specific instructions yield consistent results
- **Content Length**: 500-2000 words work best
- **Domain Familiarity**: Construction terminology is well-understood

## Step 7: Advanced Prompt Engineering (Optional)

### 7.1 Enhanced System Prompt
For better results, consider a more detailed system prompt:

```csharp
var enhancedSystemPrompt = @"
You are a professional construction inspector AI assistant. Analyze inspection reports and create structured tickets.

CONTEXT: Construction inspections document building conditions, safety issues, and maintenance needs.

OUTPUT FORMAT: Return valid JSON with exactly these fields:
{
  ""date"": ""YYYY-MM-DD"" or ""To-Be-Specified"",
  ""location"": ""Specific building/area location"" or ""To-Be-Specified"",
  ""issues"": [""Issue 1"", ""Issue 2"", ...] or ""To-Be-Specified"",
  ""recommendations"": [""Action 1"", ""Action 2"", ...] or ""To-Be-Specified""
}

RULES:
1. Extract only factual information from the input
2. Issues should be specific problems, not general observations
3. Recommendations should be actionable next steps
4. Use ""To-Be-Specified"" if information is unclear or missing
5. Maintain professional construction terminology
6. Keep issues and recommendations concise but informative

INPUT DATA:";
```

### 7.2 Input Preprocessing
Enhance input content for better AI understanding:

```csharp
private string PrepareContentForTicketGeneration(string rawContent)
{
    var prepared = "=== INSPECTION REPORT DATA ===\n\n";
    
    // Add structure markers
    prepared += rawContent.Replace("Inline Image:", "\n[IMAGE ANALYSIS]");
    prepared += rawContent.Replace("Text Extracted from Image:", "\n[OCR RESULT]");
    prepared += rawContent.Replace("AI Generated Description:", "\n[IMAGE DESCRIPTION]");
    
    // Add context hints
    prepared += "\n\n=== ANALYSIS REQUEST ===\n";
    prepared += "Focus on: structural issues, safety concerns, maintenance needs, code violations\n";
    prepared += "Prioritize: immediate safety risks, regulatory compliance, preventive measures\n";
    
    return prepared;
}
```

## Common Issues and Solutions

### Issue: Invalid JSON responses
**Causes:**
- AI model returns malformed JSON
- Extra text before/after JSON
- AI interprets prompt differently

**Solutions:**
- Improve prompt specificity
- Add JSON validation examples
- Implement robust error handling
- Show raw output for debugging

### Issue: Missing or incomplete information
**Causes:**
- Input content lacks sufficient detail
- AI model conservative about extracting info
- Prompt doesn't emphasize thoroughness

**Solutions:**
- Improve input content quality
- Add more specific extraction instructions
- Use few-shot examples in prompts

### Issue: Inconsistent output format
**Causes:**
- AI varies between array and string formats
- Different levels of detail across runs
- Prompt ambiguity

**Solutions:**
- Make format requirements more explicit
- Handle multiple output formats in parser
- Add consistency examples to prompt

### Issue: Slow performance
**Causes:**
- Large input content
- Complex prompts
- Model initialization time

**Solutions:**
- Limit input content size
- Optimize prompt length
- Show progress indicators
- Cache model between uses

## Performance Optimization

### 6.1 Input Size Management
```csharp
private string OptimizeInputSize(string content)
{
    // Limit content to improve performance
    const int maxLength = 8000; // ~2000 tokens
    
    if (content.Length > maxLength)
    {
        // Keep beginning and end, summarize middle
        var start = content.Substring(0, maxLength / 3);
        var end = content.Substring(content.Length - maxLength / 3);
        return $"{start}\n\n[CONTENT TRUNCATED FOR PROCESSING]\n\n{end}";
    }
    
    return content;
}
```

### 6.2 Response Caching
For repeated similar requests, consider caching:

```csharp
private readonly Dictionary<string, string> _responseCache = new();

private async Task<string> GetCachedResponseAsync(string prompt)
{
    var hash = prompt.GetHashCode().ToString();
    
    if (_responseCache.TryGetValue(hash, out var cached))
    {
        return cached;
    }
    
    var result = await ProcessTextGenerationAsync(prompt);
    _responseCache[hash] = result;
    
    return result;
}
```

## Verification Checklist
Before proceeding to the next lab:
- [ ] Standard text generation initializes successfully
- [ ] Ticket generation produces valid JSON output
- [ ] JSON parser handles both array and string formats
- [ ] Formatted ticket displays professionally
- [ ] Raw output is visible for debugging
- [ ] Error handling works for various failure modes
- [ ] Performance is acceptable for typical reports
- [ ] "To-Be-Specified" appears for missing information

## Next Steps
Once standard ticket generation is working:
- **Lab Guide 5**: LoRA-Enhanced Ticket Generation
- Learn about fine-tuned models and adapter loading
- Implement specialized model behavior
- Compare standard vs. LoRA model outputs

---
**✅ Module Complete!** You've successfully implemented structured ticket generation using advanced prompt engineering and Windows AI APIs standard language model.