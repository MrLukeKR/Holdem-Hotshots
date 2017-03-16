using HoldemHotshots.Utilities;

namespace HoldemHotshots.Managers
{     
    static class SpeechManager
    {
#if __ANDROID__
        static readonly AndroidSpeechUtils speaker = new AndroidSpeechUtils();
#elif __IOS__
        static readonly IOSSpeechUtils speaker = new IOSSpeechUtils();
#endif

        public static void Speak(string message)
        {
            speaker.Speak(message);
        }
    }
}
