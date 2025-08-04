using AIDevGallery.Sample.Utils;
using AIDevGallery.Sample.Services;
using Microsoft.Graphics.Imaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace AIDevGallery.Sample;

public class InspectionReport
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string RichTextContent { get; set; } = "";
    public Dictionary<string, ImageData> EmbeddedImages { get; set; } = new();
}

public class ImageData
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public SoftwareBitmap? Bitmap { get; set; }
    public string? Description { get; set; }
    public string? ExtractedText { get; set; }
    public string FileName { get; set; } = "";
    public bool IsProcessing { get; set; } = false;
}

internal sealed partial class Sample : Microsoft.UI.Xaml.Controls.Page
{
    // AI Services
    private AIImageService? _imageService;
    private AITextService? _textService;
    private AISettingsService? _settingsService;
    
    public ObservableCollection<InspectionReport> Reports { get; set; } = new();
    private InspectionReport? _currentReport;

    public Sample()
    {
        this.InitializeComponent();
        
        // Initialize AI services
        try
        {
            _imageService = new AIImageService();
            _textService = new AITextService();
            _settingsService = new AISettingsService(_imageService, _textService);
        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to initialize AI services");
        }
        
        // Pre-populate with sample reports
        var sampleReports = new[]
        {
            new InspectionReport
            {
                Title = "East Wing Level 3 Inspection",
                CreatedDate = new DateTime(2025, 7, 29),
                RichTextContent = GetSampleReportContent1()
            },
            new InspectionReport
            {
                Title = "Main Building Foundation Assessment",
                CreatedDate = new DateTime(2024, 11, 15),
                RichTextContent = GetSampleReportContent2()
            },
            new InspectionReport
            {
                Title = "North Parking Structure Safety Review",
                CreatedDate = new DateTime(2025, 3, 8),
                RichTextContent = GetSampleReportContent3()
            },
            new InspectionReport
            {
                Title = "Administrative Building HVAC Inspection",
                CreatedDate = new DateTime(2025, 1, 22),
                RichTextContent = GetSampleReportContent4()
            }
        };
        
        foreach (var report in sampleReports)
        {
            Reports.Add(report);
        }
    }

    private string GetSampleReportContent1()
    {
        return @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}
\f0\fs28 Inspection Report\par
\par
Visited the site July 29, 2025, at East Wing, Level 3. Weather was overcast with high humidity. Noted standing water near HVAC unit due to clogged floor drain. Observed exposed wiring in maintenance corridor ceiling. Identified cracked window pane in break room. Fire extinguisher cabinet was blocked by storage bins.\par
\par
\b Proposed Fixes:\b0\par
Clear and flush floor drain to prevent water buildup. Secure and shield exposed wiring per electrical code. Replace damaged window pane with tempered glass. Relocate storage items to maintain unobstructed access to fire safety equipment.\par
\par
\b Next Steps:\b0\par
Assign electrician for wiring correction. Schedule glass replacement. Verify compliance with fire safety clearance guidelines on next visit.\par
}";
    }

    private string GetSampleReportContent2()
    {
        return @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}
\f0\fs28 Foundation Assessment Report\par
\par
Conducted comprehensive foundation inspection on November 15, 2024, for the Main Building structure. Weather conditions were clear and dry, providing optimal visibility for assessment. Initial visual examination revealed several areas of concern requiring immediate attention.\par
\par
\b Structural Findings:\b0\par
Identified hairline cracks along the east foundation wall extending approximately 15 feet in length. Observed minor settling in the southwest corner with a measured displacement of 0.5 inches. Discovered water infiltration marks near the basement entrance, indicating potential drainage issues. Foundation bolts in the north section show signs of corrosion and require replacement. Concrete spalling noted around utility penetrations on the west wall.\par
\par
\b Moisture and Drainage:\b0\par
Detected elevated moisture levels in the basement storage area using digital moisture meter readings above 18%. Existing French drain system appears to be partially clogged based on water accumulation patterns. Recommended installation of additional waterproofing membrane along affected areas. Grading around building perimeter needs adjustment to improve water runoff.\par
\par
\b Recommended Actions:\b0\par
Engage structural engineer for detailed crack analysis and repair specifications. Schedule foundation bolt replacement before winter season. Install improved drainage system with upgraded sump pump capacity. Apply waterproof sealant to all identified penetration points. Monitor crack progression with quarterly measurements for next 12 months.\par
\par
\b Priority Timeline:\b0\par
High priority items to be completed within 30 days. Medium priority maintenance scheduled for spring 2025. Long-term monitoring program to be established immediately.\par
}";
    }

    private string GetSampleReportContent3()
    {
        return @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}
\f0\fs28 Parking Structure Safety Inspection\par
\par
Performed comprehensive safety evaluation of North Parking Structure on March 8, 2025. Weather conditions included light rain with temperatures around 45°F. The five-level concrete structure serves approximately 800 vehicles daily and requires regular safety compliance monitoring.\par
\par
\b Structural Elements:\b0\par
Examined all concrete support columns and found minor surface cracking on levels 2 and 4, primarily due to freeze-thaw cycles. Pre-stressed concrete beams show normal wear patterns with no immediate structural concerns. Expansion joints require resealing on the top deck to prevent water infiltration. Stairwell handrails are secure but need repainting for corrosion protection. Concrete surface shows typical carbonation patterns within acceptable limits.\par
\par
\b Safety Systems:\b0\par
Emergency lighting system tested successfully on all levels with battery backup functioning properly. Fire suppression system operational but requires quarterly maintenance as scheduled. Exit signage is clearly visible and illuminated. Vehicle barriers at deck edges are structurally sound but show paint deterioration. Pedestrian walkways are clearly marked and in good condition.\par
\par
\b Drainage and Utilities:\b0\par
Roof drainage system partially blocked with debris causing water pooling in northwest corner. Electrical conduits are properly secured with no exposed wiring observed. LED lighting conversion completed on levels 1-3, remaining levels scheduled for upgrade. HVAC ventilation fans operational but filters require replacement. Utility access panels are locked and properly labeled.\par
\par
\b Traffic and Access:\b0\par
Vehicular access gates function smoothly with updated electronic systems. Parking space striping is in excellent condition following recent repainting. Speed bumps and directional signage clearly visible. Disabled parking spaces properly marked and accessible. Entry/exit ramps have appropriate non-slip surface treatment.\par
\par
\b Immediate Actions Required:\b0\par
Clear roof drains and install debris guards. Schedule handrail repainting for next maintenance cycle. Seal expansion joints before next winter season. Replace HVAC filters and schedule routine maintenance. Update emergency contact information on all posted signs.\par
\par
\b Long-term Recommendations:\b0\par
Complete LED lighting upgrade on remaining levels by summer 2025. Develop five-year concrete restoration plan. Implement quarterly safety system testing schedule. Consider installing EV charging stations on level 1.\par
}";
    }

    private string GetSampleReportContent4()
    {
        return @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}
\f0\fs28 HVAC System Inspection Report\par
\par
Conducted detailed HVAC assessment of Administrative Building on January 22, 2025. Building houses 150 office staff across three floors with mixed-use conference and training facilities. Outside temperature was 28°F with clear skies, providing ideal conditions for heating system evaluation under load.\par
\par
\b Main Heating System:\b0\par
Central boiler unit operating at 85% efficiency, slightly below manufacturer specifications of 90%. Primary heat exchanger shows normal wear patterns but requires annual cleaning scheduled for spring shutdown. Circulation pumps functioning properly with recent bearing replacements. Expansion tank pressure within normal range at 12 PSI. Natural gas supply lines inspected with no leaks detected using electronic gas detector.\par
\par
\b Air Distribution Network:\b0\par
Ductwork inspection revealed accumulated dust buildup requiring professional cleaning before cooling season. Several dampers on second floor stuck in partially closed position affecting airflow balance. Main air handler filters replaced during inspection - previous filters were 85% loaded. Return air grilles clean and unobstructed throughout building. Fresh air intake operating correctly with automated damper controls responding properly.\par
\par
\b Climate Control Systems:\b0\par
Programmable thermostats calibrated and updated with new schedule reflecting current occupancy patterns. Zone control valves operating smoothly with recent actuator maintenance. Building automation system (BAS) online and recording data correctly. Temperature sensors verified accurate within ±1°F tolerance. Humidity control systems maintaining 45-55% relative humidity as specified.\par
\par
\b Safety and Code Compliance:\b0\par
Emergency shutoff switches properly labeled and accessible. Carbon monoxide detectors tested and functioning with fresh batteries installed. Combustion air supply adequate with properly sized intake vents. Flue gas venting system clear with annual cleaning completed. Pressure relief valves tested and certified within inspection period.\par
\par
\b Energy Efficiency Opportunities:\b0\par
Recommended installation of variable frequency drives on circulation pumps to reduce energy consumption by estimated 15%. Suggested upgrading pneumatic controls to electronic for improved precision and efficiency. Window sealing improvements could reduce heating load by 8-12%. LED lighting upgrades in mechanical rooms completed, reducing heat load on cooling system.\par
\par
\b Maintenance Schedule Updates:\b0\par
Filter replacement every 60 days during heating season, 45 days during cooling season. Annual boiler tune-up scheduled for April 2025. Ductwork cleaning recommended for May 2025 before cooling season startup. Quarterly BAS system updates and calibration checks. Monthly inspection of all mechanical room equipment.\par
\par
\b Critical Action Items:\b0\par
Repair stuck dampers on second floor within two weeks. Schedule boiler efficiency testing and adjustment. Update BAS software to latest version. Train maintenance staff on new filter replacement procedures. Coordinate with facilities team for ductwork cleaning project scheduling.\par
}";
    }

    protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        await InitializeAI();
        App.Window?.ModelLoaded();
    }

    private async Task InitializeAI()
    {
        // Initialize all AI services
        if (_settingsService != null)
        {
            await _settingsService.InitializeAllServicesAsync();
            
            // Update UI based on feature availability
            UpdateFeatureToggleStates();
            UpdateButtonStates();
        }
    }

    private void NewReportBtn_Click(object sender, RoutedEventArgs e)
    {
        var newReport = new InspectionReport
        {
            Title = $"Report {Reports.Count + 1}",
            CreatedDate = DateTime.Now
        };
        
        Reports.Add(newReport);
        ReportsListView.SelectedItem = newReport;
        SelectReport(newReport);
    }

    private void ReportsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.FirstOrDefault() is InspectionReport selectedReport)
        {
            SelectReport(selectedReport);
        }
    }

    private async void SelectReport(InspectionReport report)
    {
        // Save current report content if any
        if (_currentReport != null)
        {
            await SaveCurrentReportContent();
        }

        _currentReport = report;
        CurrentReportTitle.Text = report.Title;
        
        // Enable controls
        AddImageBtn.IsEnabled = true;
        DocumentEditor.IsEnabled = true;
        QueryTextBox.IsEnabled = true;
        SummarizeReportBtn.IsEnabled = true;
        
        // Update AI context
        AiContextText.Text = $"Discussing: {report.Title}";
        QueryResponseTxt.Visibility = Visibility.Collapsed;
        QueryTextBox.Text = "";
        
        // Update button states based on feature availability and user settings
        UpdateButtonStates();
        
        // Load document content
        await LoadReportContent();
    }

    private Task SaveCurrentReportContent()
    {
        if (_currentReport == null) return Task.CompletedTask;

        // Get RTF content from RichEditBox
        DocumentEditor.Document.GetText(Microsoft.UI.Text.TextGetOptions.FormatRtf, out string rtfContent);
        _currentReport.RichTextContent = rtfContent;
        return Task.CompletedTask;
    }

    private Task LoadReportContent()
    {
        if (_currentReport == null) return Task.CompletedTask;

        // Load RTF content into RichEditBox
        if (!string.IsNullOrEmpty(_currentReport.RichTextContent))
        {
            DocumentEditor.Document.SetText(Microsoft.UI.Text.TextSetOptions.FormatRtf, _currentReport.RichTextContent);
        }
        else
        {
            DocumentEditor.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, "");
        }
        return Task.CompletedTask;
    }

    private async void DeleteReport_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem && menuItem.Tag is InspectionReport reportToDelete)
        {
            var dialog = new ContentDialog
            {
                Title = "Delete Report",
                Content = $"Are you sure you want to delete '{reportToDelete.Title}'? This action cannot be undone.",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel",
                XamlRoot = this.XamlRoot,
                PrimaryButtonStyle = (Style)App.Current.Resources["AccentButtonStyle"]
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Reports.Remove(reportToDelete);
                
                // If this was the current report, clear the document
                if (_currentReport == reportToDelete)
                {
                    _currentReport = null;
                    CurrentReportTitle.Text = "Select or create a report";
                    AddImageBtn.IsEnabled = false;
                    DocumentEditor.IsEnabled = false;
                    QueryTextBox.IsEnabled = false;
                    SummarizeReportBtn.IsEnabled = false;
                    GenerateTicketStandardBtn.IsEnabled = false;
                    GenerateTicketBtn.IsEnabled = false;
                    AiContextText.Text = "Select a report to activate AI assistance";
                    QueryResponseTxt.Visibility = Visibility.Collapsed;
                    DocumentEditor.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, "");
                }
            }
        }
    }

    private async void AddImageBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReport == null) return;

        // Temporarily disable the button to prevent multiple operations
        AddImageBtn.IsEnabled = false;

        try
        {
            var window = new Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var picker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.ViewMode = PickerViewMode.Thumbnail;

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using var stream = await file.OpenReadAsync();
                await InsertImageInlineAsync(stream, file.Name);
            }
        }
        finally
        {
            // Re-enable the button
            AddImageBtn.IsEnabled = true;
        }
    }

    private async Task InsertImageInlineAsync(IRandomAccessStream stream, string fileName)
    {
        if (_currentReport == null) return;

        try
        {
            var decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap inputBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            if (inputBitmap == null) return;

            // Create image data for storage
            var imageData = new ImageData
            {
                Bitmap = inputBitmap,
                FileName = fileName,
                IsProcessing = true
            };

            // Store the image data
            _currentReport.EmbeddedImages[imageData.Id] = imageData;

            // Get current selection in RichEditBox
            var selection = DocumentEditor.Document.Selection;
            
            // Convert bitmap to stream for RichEditBox
            using var memoryStream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, memoryStream);
            encoder.SetSoftwareBitmap(inputBitmap);
            await encoder.FlushAsync();

            // Insert image only
            selection.InsertImage(400, 300, 0, Microsoft.UI.Text.VerticalCharacterAlignment.Bottom, imageData.Id, memoryStream);
            
            // Show processing overlay
            ProcessingOverlay.Visibility = Visibility.Visible;

            // Generate description and extract text using AI (this will add all results when ready)
            _ = ProcessImageAsync(imageData, fileName);

        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to insert image");
        }
    }



    private async Task ProcessImageAsync(ImageData imageData, string fileName)
    {
        if (imageData.Bitmap == null) return;

        // Run both OCR and image description in parallel (if available)
        var tasks = new List<Task>();
        
        if (_imageService?.IsOcrAvailable == true && _imageService.IsOcrEnabled)
        {
            tasks.Add(_imageService.ProcessImageOcrAsync(imageData));
        }
        
        if (_imageService?.IsImageDescriptionAvailable == true && _imageService.IsImageDescriptionEnabled)
        {
            tasks.Add(_imageService.ProcessImageDescriptionAsync(imageData));
        }

        // Wait for all available tasks to complete
        if (tasks.Any())
        {
            await Task.WhenAll(tasks);
        }

        // Update the document with all results
        DispatcherQueue?.TryEnqueue(() =>
        {
            UpdateImageResults(fileName, imageData.Description, imageData.ExtractedText);
            ProcessingOverlay.Visibility = Visibility.Collapsed;
        });
    }


    private void UpdateImageResults(string fileName, string? description, string? extractedText)
    {
        try
        {
            // Insert title, description, and extracted text together after the image
            var selection = DocumentEditor.Document.Selection;
            selection.Collapse(false); // Move to end of document
            
            var resultText = $"\n\nInline Image: \"{fileName}\"\n";
            
            // Format OCR result - always wrap in quotes unless it's a status message
            if (string.IsNullOrWhiteSpace(extractedText) || extractedText == "No text detected")
            {
                resultText += $"Text Extracted from Image: {extractedText ?? "No text detected"}\n";
            }
            else
            {
                resultText += $"Text Extracted from Image: \"{extractedText}\"\n";
            }
            
            resultText += $"AI Generated Description: {description ?? "Failed to generate description"}\n\n";
            
            selection.TypeText(resultText);
        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to update image results");
        }
    }


    private async void GenerateTicketStandardBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReport == null)
        {
            App.Window?.ShowException(null, "Please select a report first");
            return;
        }

        QueryLoader.Visibility = Visibility.Visible;
        QueryStopBtn.Visibility = Visibility.Visible;
        GenerateTicketStandardBtn.IsEnabled = false;
        QueryResponseTxt.Text = string.Empty;
        QueryResponseTxt.Visibility = Visibility.Collapsed;

        try
        {
            // Extract full report content for ticket generation
            var reportContent = ExtractFullReportContent();
            
            if (string.IsNullOrWhiteSpace(reportContent))
            {
                QueryResponseTxt.Text = "No content found in the report to generate a ticket.";
                QueryResponseTxt.Visibility = Visibility.Visible;
                return;
            }

            // Use the same system prompt as LoRA version, but combine with user content for standard model
            var systemPrompt = "The input is a construction inspection note with possibly some descriptions of photos taken on site, return a json with date, location, issues, and recommendations. If certain information is missing, do not make it up, just fill in: To-Be-Specified.";
            var combinedPrompt = $"System: {systemPrompt}\n\nUser: {reportContent}";
            
            var result = await (_textService?.ProcessTextGenerationAsync(combinedPrompt) ?? Task.FromResult("Text service not available"));
            
            // Display both raw JSON and formatted ticket
            var rawOutput = $"RAW STANDARD OUTPUT:\n{new string('═', 50)}\n{result}\n{new string('═', 50)}\n\n";
            var formattedTicket = FormatTicketFromJson(result);
            QueryResponseTxt.Text = rawOutput + formattedTicket;
            QueryResponseTxt.Visibility = Visibility.Visible;
        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to generate ticket");
        }
        finally
        {
            QueryLoader.Visibility = Visibility.Collapsed;
            QueryStopBtn.Visibility = Visibility.Collapsed;
            GenerateTicketStandardBtn.IsEnabled = true;
        }
    }


    private void QueryStopBtn_Click(object sender, RoutedEventArgs e)
    {
        QueryLoader.Visibility = Visibility.Collapsed;
        QueryStopBtn.Visibility = Visibility.Collapsed;
        SummarizeReportBtn.IsEnabled = true;
        GenerateTicketStandardBtn.IsEnabled = true;
    }


    private async void SummarizeReportBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReport == null)
        {
            App.Window?.ShowException(null, "Please select a report first");
            return;
        }

        QueryLoader.Visibility = Visibility.Visible;
        QueryStopBtn.Visibility = Visibility.Visible;
        SummarizeReportBtn.IsEnabled = false;
        QueryResponseTxt.Text = string.Empty;
        QueryResponseTxt.Visibility = Visibility.Collapsed;

        try
        {
            // Extract full report content for summarization
            var reportContent = ExtractFullReportContent();
            
            if (string.IsNullOrWhiteSpace(reportContent))
            {
                QueryResponseTxt.Text = "No content found in the report to summarize.";
                QueryResponseTxt.Visibility = Visibility.Visible;
                return;
            }

            var result = await (_textService?.ProcessTextSummarizationAsync(reportContent) ?? Task.FromResult("Text service not available"));
            QueryResponseTxt.Text = result;
            QueryResponseTxt.Visibility = Visibility.Visible;
        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to summarize report");
        }
        finally
        {
            QueryLoader.Visibility = Visibility.Collapsed;
            QueryStopBtn.Visibility = Visibility.Collapsed;
            SummarizeReportBtn.IsEnabled = true;
        }
    }

    private string ExtractFullReportContent()
    {
        if (_currentReport == null) return "";

        var content = $"Inspection Report: {_currentReport.Title}\nCreated: {_currentReport.CreatedDate:yyyy-MM-dd HH:mm}\n\n";
        
        // Get plain text content from the document
        DocumentEditor.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string plainText);
        if (!string.IsNullOrWhiteSpace(plainText))
        {
            content += $"Document Content:\n{plainText}\n\n";
        }

        // Add structured data from embedded images
        foreach (var imageData in _currentReport.EmbeddedImages.Values)
        {
            content += $"Image: {imageData.FileName}\n";
            
            if (!string.IsNullOrEmpty(imageData.Description))
            {
                content += $"Description: {imageData.Description}\n";
            }
            
            if (!string.IsNullOrEmpty(imageData.ExtractedText) && imageData.ExtractedText != "No text detected")
            {
                content += $"Extracted Text: {imageData.ExtractedText}\n";
            }
            
            content += "\n";
        }

        return content.Trim();
    }


    private async void GenerateTicketBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReport == null)
        {
            App.Window?.ShowException(null, "Please select a report first");
            return;
        }

        QueryLoader.Visibility = Visibility.Visible;
        QueryStopBtn.Visibility = Visibility.Visible;
        GenerateTicketBtn.IsEnabled = false;
        QueryResponseTxt.Text = string.Empty;
        QueryResponseTxt.Visibility = Visibility.Collapsed;

        try
        {
            // Extract full report content for ticket generation
            var reportContent = ExtractFullReportContent();
            
            if (string.IsNullOrWhiteSpace(reportContent))
            {
                QueryResponseTxt.Text = "No content found in the report to generate a ticket.";
                QueryResponseTxt.Visibility = Visibility.Visible;
                return;
            }

            var systemPrompt = "The input is a construction inspection note with possibly some descriptions of photos taken on site, return a json with date, location, issues, and recommendations. If certain information is missing, do not make it up, just fill in: To-Be-Specified.";
            var result = await (_textService?.ProcessLoraTextGenerationAsync(systemPrompt, reportContent) ?? Task.FromResult("Text service not available"));
            
            // Display both raw JSON and formatted ticket
            var rawOutput = $"RAW LoRA OUTPUT:\n{new string('═', 50)}\n{result}\n{new string('═', 50)}\n\n";
            var formattedTicket = FormatTicketFromJson(result);
            QueryResponseTxt.Text = rawOutput + formattedTicket;
            QueryResponseTxt.Visibility = Visibility.Visible;
        }
        catch (Exception ex)
        {
            App.Window?.ShowException(ex, "Failed to generate ticket with LoRA");
        }
        finally
        {
            QueryLoader.Visibility = Visibility.Collapsed;
            QueryStopBtn.Visibility = Visibility.Collapsed;
            GenerateTicketBtn.IsEnabled = true;
        }
    }

    private string FormatTicketFromJson(string jsonResponse)
    {
        try
        {
            using var document = JsonDocument.Parse(jsonResponse);
            var root = document.RootElement;

            var ticket = "🏗️ CONSTRUCTION INSPECTION REPORT\n";
            ticket += "═══════════════════════════════════════\n\n";

            if (root.TryGetProperty("date", out var date))
            {
                ticket += $"📅 DATE:\n{date.GetString()}\n\n";
            }

            if (root.TryGetProperty("location", out var location))
            {
                ticket += $"📍 LOCATION:\n{location.GetString()}\n\n";
            }

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

            ticket += "═══════════════════════════════════════\n";
            ticket += $"Report Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
            ticket += "Report ID: IR-" + DateTime.Now.ToString("yyyy") + "-" + new Random().Next(1000, 9999);

            return ticket;
        }
        catch (JsonException)
        {
            return $"🏗️ Construction Inspection Report (Raw JSON):\n\n{jsonResponse}";
        }
        catch (Exception ex)
        {
            return $"🏗️ Inspection Report Generation Error:\n\n{ex.Message}\n\nRaw Response:\n{jsonResponse}";
        }
    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        await ShowSearchNotImplementedDialog();
    }

    private async void GlobalSearchBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            await ShowSearchNotImplementedDialog();
        }
    }

    private async Task ShowSearchNotImplementedDialog()
    {
        var dialog = new ContentDialog
        {
            Title = "Search Feature",
            Content = "Search functionality is not yet implemented. This feature will allow you to search across all reports, content, and metadata.",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };

        await dialog.ShowAsync();
    }

    private async void RenameReport_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem && menuItem.Tag is InspectionReport reportToRename)
        {
            await ShowRenameDialog(reportToRename);
        }
    }

    private async void ReportTitle_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.Tag is InspectionReport reportToRename)
        {
            await ShowRenameDialog(reportToRename);
        }
    }

    private async Task ShowRenameDialog(InspectionReport report)
    {
        var textBox = new TextBox
        {
            Text = report.Title,
            SelectionStart = 0,
            SelectionLength = report.Title.Length
        };

        var dialog = new ContentDialog
        {
            Title = "Rename Report",
            Content = textBox,
            PrimaryButtonText = "Rename",
            SecondaryButtonText = "Cancel",
            XamlRoot = this.XamlRoot,
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(textBox.Text))
        {
            report.Title = textBox.Text.Trim();
            
            // Update the current report title if this is the selected report
            if (_currentReport == report)
            {
                CurrentReportTitle.Text = report.Title;
            }
        }
    }

    #region Settings UI Management

    private void SettingsBtn_Click(object sender, RoutedEventArgs e)
    {
        DocumentView.Visibility = Visibility.Collapsed;
        SettingsView.Visibility = Visibility.Visible;
    }

    private void BackToDocumentBtn_Click(object sender, RoutedEventArgs e)
    {
        SettingsView.Visibility = Visibility.Collapsed;
        DocumentView.Visibility = Visibility.Visible;
    }

    private void UpdateFeatureToggleStates()
    {
        if (_settingsService == null) return;

        // Update toggle states using settings service
        var (ocrOn, ocrEnabled) = _settingsService.GetOcrToggleState();
        OcrToggle.IsOn = ocrOn;
        OcrToggle.IsEnabled = ocrEnabled;

        var (imageDescOn, imageDescEnabled) = _settingsService.GetImageDescriptionToggleState();
        ImageDescriptionToggle.IsOn = imageDescOn;
        ImageDescriptionToggle.IsEnabled = imageDescEnabled;

        var (summaryOn, summaryEnabled) = _settingsService.GetTextSummarizationToggleState();
        TextSummarizationToggle.IsOn = summaryOn;
        TextSummarizationToggle.IsEnabled = summaryEnabled;

        var (textGenOn, textGenEnabled) = _settingsService.GetTextGenerationToggleState();
        TicketGenerationToggle.IsOn = textGenOn;
        TicketGenerationToggle.IsEnabled = textGenEnabled;

        var (loraOn, loraEnabled) = _settingsService.GetLoraToggleState();
        LoraTicketGenerationToggle.IsOn = loraOn;
        LoraTicketGenerationToggle.IsEnabled = loraEnabled;
    }

    private void UpdateButtonStates()
    {
        if (_currentReport == null || _settingsService == null) return;

        // Use settings service for button state logic
        bool imageFeatureEnabled = _settingsService.ShouldEnableImageButton();
        AddImageBtn.IsEnabled = imageFeatureEnabled;

        bool summarizeEnabled = _settingsService.ShouldEnableSummarizeButton();
        SummarizeReportBtn.IsEnabled = summarizeEnabled;

        bool ticketStandardEnabled = _settingsService.ShouldEnableStandardTicketButton();
        GenerateTicketStandardBtn.IsEnabled = ticketStandardEnabled;

        bool ticketLoraEnabled = _settingsService.ShouldEnableLoraTicketButton();
        GenerateTicketBtn.IsEnabled = ticketLoraEnabled;

        // Update visual states (opacity)
        AddImageBtn.Opacity = imageFeatureEnabled ? 1.0 : 0.5;
        SummarizeReportBtn.Opacity = summarizeEnabled ? 1.0 : 0.5;
        GenerateTicketStandardBtn.Opacity = ticketStandardEnabled ? 1.0 : 0.5;
        GenerateTicketBtn.Opacity = ticketLoraEnabled ? 1.0 : 0.5;
    }

    private void OcrToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggle && _settingsService != null)
        {
            _settingsService.IsOcrEnabled = toggle.IsOn && _settingsService.IsOcrAvailable;
            UpdateButtonStates();
        }
    }

    private void ImageDescriptionToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggle && _settingsService != null)
        {
            _settingsService.IsImageDescriptionEnabled = toggle.IsOn && _settingsService.IsImageDescriptionAvailable;
            UpdateButtonStates();
        }
    }

    private void TextSummarizationToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggle && _settingsService != null)
        {
            _settingsService.IsTextSummarizationEnabled = toggle.IsOn && _settingsService.IsTextSummarizationAvailable;
            UpdateButtonStates();
        }
    }

    private void TicketGenerationToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggle && _settingsService != null)
        {
            _settingsService.IsTextGenerationEnabled = toggle.IsOn && _settingsService.IsTextGenerationAvailable;
            UpdateButtonStates();
        }
    }

    private void LoraTicketGenerationToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggle && _settingsService != null)
        {
            _settingsService.IsLoraAdapterEnabled = toggle.IsOn && _settingsService.IsLoraAdapterAvailable;
            UpdateButtonStates();
        }
    }

    #endregion
}