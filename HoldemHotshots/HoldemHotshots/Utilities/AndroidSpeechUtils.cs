
using System;
using Android.Runtime;
using Android.Speech.Tts;
using Android.App;

namespace HoldemHotshots.Utilities
{
    class AndroidSpeechUtils
    {
        TextToSpeech speaker;

        public AndroidSpeechUtils()
        {
                speaker = new TextToSpeech(Application.Context, null);
        }

        public void Speak(string message)
        {
            speaker.Speak(message, QueueMode.Flush, null, null);
        }
    }
}