using Microsoft.Windows.AI;
using Microsoft.Windows.AI.Text;
using Microsoft.Windows.AI.Text.Experimental;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AIDevGallery.Sample.Services
{
    /// <summary>
    /// Service for AI text processing - generation, summarization, and LoRA
    /// </summary>
    public class AITextService : AIServiceBase
    {
        private LanguageModel? _languageModel;
        private TextSummarizer? _textSummarizer;
        private LanguageModelExperimental? _languageModelExperimental;
        private LowRankAdaptation? _loraAdapter;

        private bool _isTextGenerationAvailable = false;
        private bool _isTextSummarizationAvailable = false;
        private bool _isLoraAdapterAvailable = false;

        private bool _isTextGenerationEnabled = true;
        private bool _isTextSummarizationEnabled = true;
        private bool _isLoraAdapterEnabled = true;

        public bool IsTextGenerationAvailable => _isTextGenerationAvailable;
        public bool IsTextSummarizationAvailable => _isTextSummarizationAvailable;
        public bool IsLoraAdapterAvailable => _isLoraAdapterAvailable;

        public bool IsTextGenerationEnabled
        {
            get => _isTextGenerationEnabled;
            set => _isTextGenerationEnabled = value;
        }

        public bool IsTextSummarizationEnabled
        {
            get => _isTextSummarizationEnabled;
            set => _isTextSummarizationEnabled = value;
        }

        public bool IsLoraAdapterEnabled
        {
            get => _isLoraAdapterEnabled;
            set => _isLoraAdapterEnabled = value;
        }

        public override async Task<bool> InitializeAsync()
        {
            _isTextGenerationAvailable = await InitializeTextGenerationAsync();
            _isTextSummarizationAvailable = InitializeTextSummarizationFeature();
            _isLoraAdapterAvailable = InitializeLoraAdapterFeature();
            _isAvailable = _isTextGenerationAvailable || _isTextSummarizationAvailable || _isLoraAdapterAvailable;
            return _isAvailable;
        }

        private async Task<bool> InitializeTextGenerationAsync()
        {
            try
            {
                var readyState = LanguageModel.GetReadyState();
                if (readyState == AIFeatureReadyState.NotReady)
                {
                    var operation = await LanguageModel.EnsureReadyAsync();
                    if (operation.Status != AIFeatureReadyResultState.Success)
                    {
                        return false;
                    }
                }
                else if (readyState != AIFeatureReadyState.Ready)
                {
                    return false;
                }

                _languageModel = await LanguageModel.CreateAsync();
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "initialize text generation model");
                return false;
            }
        }

        private bool InitializeTextSummarizationFeature()
        {
            try
            {
                if (_languageModel != null)
                {
                    _textSummarizer = new TextSummarizer(_languageModel);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex, "initialize Text Summarizer");
                return false;
            }
        }

        private bool InitializeLoraAdapterFeature()
        {
            try
            {
                if (_languageModel != null)
                {
                    // Create experimental language model session
                    _languageModelExperimental = new LanguageModelExperimental(_languageModel);

                    // Load LoRA adapter from the specified path
                    string adapterFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "lora_adapter.safetensors");
                    if (File.Exists(adapterFilePath))
                    {
                        _loraAdapter = _languageModelExperimental.LoadAdapter(adapterFilePath);
                        return true;
                    }
                    else
                    {
                        ShowError($"LoRA adapter file not found at: {adapterFilePath}");
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex, "initialize LoRA adapter");
                return false;
            }
        }

        public async Task<string> ProcessTextSummarizationAsync(string content)
        {
            if (!_isTextSummarizationAvailable || !_isTextSummarizationEnabled || _textSummarizer == null)
            {
                return "Text summarization not available or disabled.";
            }

            try
            {
                var result = await _textSummarizer.SummarizeAsync(content);
                return $"📋 Report Summary:\n\n{result.Text}";
            }
            catch (Exception ex)
            {
                HandleException(ex, "summarize text");
                return "Failed to summarize content.";
            }
        }

        public async Task<string> ProcessTextGenerationAsync(string prompt)
        {
            if (!_isTextGenerationAvailable || !_isTextGenerationEnabled || _languageModel == null)
            {
                return "Text generation not available or disabled.";
            }

            try
            {
                var result = await _languageModel.GenerateResponseAsync(prompt);
                return result.Text;
            }
            catch (Exception ex)
            {
                HandleException(ex, "generate text");
                return "Failed to generate content.";
            }
        }

        public async Task<string> ProcessLoraTextGenerationAsync(string systemPrompt, string userContent)
        {
            if (!_isLoraAdapterAvailable || !_isLoraAdapterEnabled || _languageModelExperimental == null || _loraAdapter == null)
            {
                return "LoRA adapter not available or disabled.";
            }

            try
            {
                var context = _languageModel?.CreateContext(systemPrompt);
                var options = new LanguageModelOptionsExperimental
                {
                    LoraAdapter = _loraAdapter
                };

                var result = await _languageModelExperimental.GenerateResponseAsync(context, userContent, options);
                return result.Text;
            }
            catch (Exception ex)
            {
                HandleException(ex, "generate text with LoRA");
                return "Failed to generate content with LoRA.";
            }
        }
    }
}