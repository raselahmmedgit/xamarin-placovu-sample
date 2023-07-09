using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OntrackHealthApp.Droid.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(OntrackSpeechRecognizer))]
namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class OntrackSpeechRecognizer : IOntrackSpeechRecognizer
    {
        public async void Speak(string speech)
        {

        }

        public async void StartSpeechRecognize()
        {
            AndroidSpeechToText.SpeechListenerStatus = true;
            //SpeechRecognizerHelper.Speak(AppConstants.LeonardoWelcomeCommand);
            SpeechRecognizerHelper.PlayAudio(Resource.Raw.Welcome);
            await Task.Delay(7000);

            AndroidSpeechToText androidSpeechToText = new AndroidSpeechToText();
            await androidSpeechToText.SpeechToTextAsync();
        }

        public async void StopSpeechRecognize()
        {
            if(AndroidSpeechToText.SpeechListenerStatus)
            {
                AndroidSpeechToText.SpeechListenerStatus = false;
                await Task.Delay(1000);
                SpeechRecognizerHelper.PlayAudio(Resource.Raw.GoodBye);
                //SpeechRecognizerHelper.Speak(AppConstants.LeonardoGoodByeCommand);
            }
        }
    }
}