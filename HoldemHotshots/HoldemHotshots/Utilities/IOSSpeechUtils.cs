#if __IOS__
using AVFoundation;

namespace HoldemHotshots.Utilities
{
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