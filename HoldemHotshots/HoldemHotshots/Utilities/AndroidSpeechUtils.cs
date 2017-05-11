#if __ANDROID__
using Android.Speech.Tts;
using Android.App;
using Android.OS;

namespace HoldemHotshots.Utilities
{
///<summary>
///Performs speech operations on Android devices
///</summary>
    class AndroidSpeechUtils
    {
        TextToSpeech speaker;

        public AndroidSpeechUtils()
        {
            speaker = new TextToSpeech(Application.Context, null);
        }

        public void Speak(string message)
        {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                speaker.Speak(message, QueueMode.Flush, null, null);
            else
#pragma warning disable
                speaker.Speak(message, QueueMode.Flush, null);
#pragma warning restore
        }
    }
}
#endif