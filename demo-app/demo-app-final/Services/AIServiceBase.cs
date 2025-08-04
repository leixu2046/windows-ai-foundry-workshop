using System;
using System.Threading.Tasks;

namespace AIDevGallery.Sample.Services
{
    /// <summary>
    /// Base interface for all AI services
    /// </summary>
    public interface IAIService
    {
        bool IsAvailable { get; }
        bool IsEnabled { get; set; }
        Task<bool> InitializeAsync();
    }

    /// <summary>
    /// Common base class for AI services
    /// </summary>
    public abstract class AIServiceBase : IAIService
    {
        protected bool _isAvailable = false;
        protected bool _isEnabled = true;

        public bool IsAvailable => _isAvailable;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        public abstract Task<bool> InitializeAsync();

        protected bool IsFeatureReady => _isAvailable && _isEnabled;

        protected void HandleException(Exception ex, string operation)
        {
            App.Window?.ShowException(ex, $"Failed to {operation}");
        }

        protected void ShowError(string message)
        {
            App.Window?.ShowException(null, message);
        }
    }
}