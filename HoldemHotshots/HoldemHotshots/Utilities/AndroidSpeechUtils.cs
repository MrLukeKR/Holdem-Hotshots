using System;
using Android.Runtime;
using Android.Speech.Tts;
using Android.App;

namespace HoldemHotshots.Utilities
{
    class AndroidSpeechUtils : TextToSpeech.IOnInitListener
    {
        public AndroidSpeechUtils()
        {

        }

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Speak(string message)
        {
            var speaker = new TextToSpeech(Application.Context, this);

            speaker.Speak(message, QueueMode.Flush, null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            throw new NotImplementedException();
        }
    }
}