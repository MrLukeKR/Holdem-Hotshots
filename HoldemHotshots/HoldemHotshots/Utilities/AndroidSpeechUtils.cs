#if __ANDROID__
using System;
using Android.Runtime;
using Android.Speech.Tts;
using Android.App;

namespace HoldemHotshots.Utilities
{
    class AndroidSpeechUtils
    {
        TextToSpeech speaker = new TextToSpeech(Application.Context, null);

        public AndroidSpeechUtils()
        {
        }

        public void Speak(string message)
        {
            speaker.Speak(message, QueueMode.Add, null, null);
        }
    }
}
#endif