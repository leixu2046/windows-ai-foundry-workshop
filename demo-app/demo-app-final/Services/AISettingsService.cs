using System;
using System.Threading.Tasks;

namespace AIDevGallery.Sample.Services
{
    /// <summary>
    /// Service for managing AI feature settings and coordination
    /// </summary>
    public class AISettingsService
    {
        private readonly AIImageService _imageService;
        private readonly AITextService _textService;

        public AISettingsService(AIImageService imageService, AITextService textService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _textService = textService ?? throw new ArgumentNullException(nameof(textService));
        }

        // Image service properties
        public bool IsOcrAvailable => _imageService.IsOcrAvailable;
        public bool IsImageDescriptionAvailable => _imageService.IsImageDescriptionAvailable;

        public bool IsOcrEnabled
        {
            get => _imageService.IsOcrEnabled;
            set => _imageService.IsOcrEnabled = value;
        }

        public bool IsImageDescriptionEnabled
        {
            get => _imageService.IsImageDescriptionEnabled;
            set => _imageService.IsImageDescriptionEnabled = value;
        }

        // Text service properties
        public bool IsTextGenerationAvailable => _textService.IsTextGenerationAvailable;
        public bool IsTextSummarizationAvailable => _textService.IsTextSummarizationAvailable;
        public bool IsLoraAdapterAvailable => _textService.IsLoraAdapterAvailable;

        public bool IsTextGenerationEnabled
        {
            get => _textService.IsTextGenerationEnabled;
            set => _textService.IsTextGenerationEnabled = value;
        }

        public bool IsTextSummarizationEnabled
        {
            get => _textService.IsTextSummarizationEnabled;
            set => _textService.IsTextSummarizationEnabled = value;
        }

        public bool IsLoraAdapterEnabled
        {
            get => _textService.IsLoraAdapterEnabled;
            set => _textService.IsLoraAdapterEnabled = value;
        }

        // Combined logic for UI state management
        public bool ShouldEnableImageButton()
        {
            return (_imageService.IsOcrAvailable && _imageService.IsOcrEnabled) || 
                   (_imageService.IsImageDescriptionAvailable && _imageService.IsImageDescriptionEnabled);
        }

        public bool ShouldEnableSummarizeButton()
        {
            return _textService.IsTextSummarizationAvailable && _textService.IsTextSummarizationEnabled;
        }

        public bool ShouldEnableStandardTicketButton()
        {
            return _textService.IsTextGenerationAvailable && _textService.IsTextGenerationEnabled;
        }

        public bool ShouldEnableLoraTicketButton()
        {
            return _textService.IsLoraAdapterAvailable && _textService.IsLoraAdapterEnabled;
        }

        // Feature toggle state for UI
        public (bool isOn, bool isEnabled) GetOcrToggleState()
        {
            return (_imageService.IsOcrAvailable && _imageService.IsOcrEnabled, _imageService.IsOcrAvailable);
        }

        public (bool isOn, bool isEnabled) GetImageDescriptionToggleState()
        {
            return (_imageService.IsImageDescriptionAvailable && _imageService.IsImageDescriptionEnabled, _imageService.IsImageDescriptionAvailable);
        }

        public (bool isOn, bool isEnabled) GetTextSummarizationToggleState()
        {
            return (_textService.IsTextSummarizationAvailable && _textService.IsTextSummarizationEnabled, _textService.IsTextSummarizationAvailable);
        }

        public (bool isOn, bool isEnabled) GetTextGenerationToggleState()
        {
            return (_textService.IsTextGenerationAvailable && _textService.IsTextGenerationEnabled, _textService.IsTextGenerationAvailable);
        }

        public (bool isOn, bool isEnabled) GetLoraToggleState()
        {
            return (_textService.IsLoraAdapterAvailable && _textService.IsLoraAdapterEnabled, _textService.IsLoraAdapterAvailable);
        }

        public async Task InitializeAllServicesAsync()
        {
            await _imageService.InitializeAsync();
            await _textService.InitializeAsync();
        }
    }
}