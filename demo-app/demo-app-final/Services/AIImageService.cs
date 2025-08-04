using Microsoft.Graphics.Imaging;
using Microsoft.Windows.AI;
using Microsoft.Windows.AI.ContentSafety;
using Microsoft.Windows.AI.Imaging;
using Microsoft.Windows.AI.Text;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AIDevGallery.Sample.Services
{
    /// <summary>
    /// Service for AI image processing - OCR and image description
    /// </summary>
    public class AIImageService : AIServiceBase
    {
        private ImageDescriptionGenerator? _imageDescriptor;
        private TextRecognizer? _textRecognizer;
        private bool _isImageDescriptionAvailable = false;
        private bool _isOcrAvailable = false;
        private bool _isImageDescriptionEnabled = true;
        private bool _isOcrEnabled = true;

        public bool IsImageDescriptionAvailable => _isImageDescriptionAvailable;
        public bool IsOcrAvailable => _isOcrAvailable;

        public bool IsImageDescriptionEnabled
        {
            get => _isImageDescriptionEnabled;
            set => _isImageDescriptionEnabled = value;
        }

        public bool IsOcrEnabled
        {
            get => _isOcrEnabled;
            set => _isOcrEnabled = value;
        }

        public override async Task<bool> InitializeAsync()
        {
            _isImageDescriptionAvailable = await InitializeImageDescriptionAsync();
            _isOcrAvailable = await InitializeOcrAsync();
            _isAvailable = _isImageDescriptionAvailable || _isOcrAvailable;
            return _isAvailable;
        }

        private async Task<bool> InitializeImageDescriptionAsync()
        {
            try
            {
                var readyState = ImageDescriptionGenerator.GetReadyState();
                if (readyState is AIFeatureReadyState.Ready or AIFeatureReadyState.NotReady)
                {
                    if (readyState == AIFeatureReadyState.NotReady)
                    {
                        var operation = await ImageDescriptionGenerator.EnsureReadyAsync();
                        if (operation.Status != AIFeatureReadyResultState.Success)
                        {
                            ShowError("Image Description is not available");
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    var msg = readyState == AIFeatureReadyState.DisabledByUser
                        ? "Disabled by user."
                        : "Not supported on this system.";
                    ShowError($"Image Description is not available: {msg}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "initialize Image Description");
                return false;
            }
        }

        private async Task<bool> InitializeOcrAsync()
        {
            try
            {
                var readyState = TextRecognizer.GetReadyState();
                if (readyState == AIFeatureReadyState.NotReady)
                {
                    var operation = await TextRecognizer.EnsureReadyAsync();
                    if (operation.Status != AIFeatureReadyResultState.Success)
                    {
                        ShowError($"Failed to ensure OCR model is ready: {operation.ExtendedError?.Message}");
                        return false;
                    }
                }
                else if (readyState != AIFeatureReadyState.Ready)
                {
                    var msg = readyState == AIFeatureReadyState.DisabledByUser
                        ? "Disabled by user."
                        : "Not supported on this system.";
                    ShowError($"OCR Text Recognition is not available: {msg}");
                    return false;
                }

                _textRecognizer = await TextRecognizer.CreateAsync();
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "initialize OCR");
                return false;
            }
        }

        public async Task ProcessImageOcrAsync(ImageData imageData)
        {
            if (!_isOcrAvailable || imageData.Bitmap == null || _textRecognizer == null)
            {
                imageData.ExtractedText = "OCR not available";
                return;
            }

            if (!_isOcrEnabled)
            {
                imageData.ExtractedText = "Feature disabled";
                return;
            }

            try
            {
                var imageBuffer = ImageBuffer.CreateForSoftwareBitmap(imageData.Bitmap);
                var recognizedText = _textRecognizer.RecognizeTextFromImage(imageBuffer);

                if (recognizedText?.Lines != null)
                {
                    var extractedText = new StringBuilder();
                    foreach (var line in recognizedText.Lines)
                    {
                        if (line != null && !string.IsNullOrWhiteSpace(line.Text))
                        {
                            extractedText.AppendLine(line.Text);
                        }
                    }

                    var result = extractedText.ToString().Trim();
                    imageData.ExtractedText = string.IsNullOrWhiteSpace(result) ? "No text detected" : result;
                }
                else
                {
                    imageData.ExtractedText = "No text detected";
                }
            }
            catch (Exception ex)
            {
                imageData.ExtractedText = $"OCR failed: {ex.Message}";
                HandleException(ex, "extract text from image");
            }
        }

        public async Task ProcessImageDescriptionAsync(ImageData imageData)
        {
            if (!_isImageDescriptionAvailable || imageData.Bitmap == null)
            {
                imageData.Description = "Image description not available";
                imageData.IsProcessing = false;
                return;
            }

            if (!_isImageDescriptionEnabled)
            {
                imageData.Description = "Feature disabled";
                imageData.IsProcessing = false;
                return;
            }

            try
            {
                using var bitmapBuffer = ImageBuffer.CreateForSoftwareBitmap(imageData.Bitmap);
                _imageDescriptor ??= await ImageDescriptionGenerator.CreateAsync();
                var describeTask = _imageDescriptor.DescribeAsync(bitmapBuffer, ImageDescriptionKind.BriefDescription, new ContentFilterOptions());

                if (describeTask != null)
                {
                    var response = await describeTask;
                    imageData.Description = response.Description;
                    imageData.IsProcessing = false;
                }
            }
            catch (Exception ex)
            {
                imageData.Description = "Failed to generate description ❌";
                imageData.IsProcessing = false;
                HandleException(ex, "generate image description");
            }
        }

        public bool ShouldProcessImages()
        {
            return (_isOcrAvailable && _isOcrEnabled) || (_isImageDescriptionAvailable && _isImageDescriptionEnabled);
        }
    }
}