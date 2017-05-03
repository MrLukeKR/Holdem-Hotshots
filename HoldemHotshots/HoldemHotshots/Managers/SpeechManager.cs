using HoldemHotshots.Utilities;

namespace HoldemHotshots.Managers
{     
    /// <summary>
    /// Handles speech (TTS) on both operating systems
    /// </summary>
    static class SpeechManager
    {
#if __ANDROID__
        static readonly AndroidSpeechUtils speaker = new AndroidSpeechUtils();
#elif __IOS__
        static readonly IOSSpeechUtils speaker = new IOSSpeechUtils();
#endif

        /// <summary>
        /// Makes the TTS engine say a given message
        /// </summary>
        /// <param name="message">Message to read out</param>
        public static void Speak(string message)
        {
            speaker.Speak(message);
        }
    }
}