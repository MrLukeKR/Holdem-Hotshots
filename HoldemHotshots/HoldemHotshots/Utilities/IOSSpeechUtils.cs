#if __IOS__
using AVFoundation;

namespace HoldemHotshots.Utilities
{
    /// <summary>
    /// Performs speech operations on iOS devices
    /// </summary>
    class IOSSpeechUtils
    {
        AVSpeechSynthesizer speechSynthesizer = new AVSpeechSynthesizer();

        public void Speak(string message)
        {
            var speechUtterance = new AVSpeechUtterance(message);

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
}
#endif