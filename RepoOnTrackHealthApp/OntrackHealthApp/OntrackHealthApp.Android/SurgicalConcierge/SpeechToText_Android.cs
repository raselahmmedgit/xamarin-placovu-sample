using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using OntrackHealthApp.Droid.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Interfaces;
using Xamarin.Forms;
using static OntrackHealthApp.AppCore.Enums;

[assembly: Dependency(typeof(SpeechToText_Android))]
namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class SpeechToText_Android
    {

    }
    //public class SpeechToText_Android : ISpeechToText
    //{
    //    public static AutoResetEvent autoEvent = new AutoResetEvent(false);
    //    public static string SpeechText;
    //    const int VOICE = 10;

    //    public async Task<string> SpeechToTextAsync()
    //    {
    //        var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Leonardo ready to execute command.");
    //        // Speech Recognization will be over if silence for 1.5s
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, (int)SpeechToText.CompleteSilence);

    //        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, (int)SpeechToText.CompleteSilence);
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, (int)SpeechToText.CompleteSilence);
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
    //        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
    //        voiceIntent.SetFlags(ActivityFlags.NoHistory);

    //        SpeechText = "";
    //        autoEvent.Reset();
    //        try
    //        {
    //            ((Activity)Forms.Context).StartActivityForResult(voiceIntent, VOICE);
    //        }
    //        catch (ActivityNotFoundException e)
    //        {
    //            Toast.MakeText(
    //                (Activity)Forms.Context,
    //                "Oops! your phone is not supported speech recognization",
    //                ToastLength.Short
    //                ).Show();
    //        }

    //        await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 0, 15)); });
    //        return SpeechText;
    //    }

    //}
}
