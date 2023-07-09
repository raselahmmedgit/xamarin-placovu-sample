using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Util;
using Android.Widget;
using OntrackHealthApp.Droid.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidSpeechToText))]
namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class AndroidSpeechToText:Java.Lang.Object,IRecognitionListener
    {
        //public static AutoResetEvent autoEvent = new AutoResetEvent(false);
        private static SpeechRecognizer mSpeechRecognizer;
        public static bool SpeechListenerStatus = false;
        private Intent mSpeechRecognizerIntent;
        public IntPtr Handle => IntPtr.Zero;
        public static string speechToText;
        public static bool executeCommandStatus = false;
        

        public AndroidSpeechToText(IntPtr handle, JniHandleOwnership transfer):base(handle,transfer)
        {
            
        }
        public AndroidSpeechToText()
        {
            InitializeSpeechRecognizer();
        }
        public void InitializeSpeechRecognizer()
        {
            if (SpeechRecognizer.IsRecognitionAvailable((Activity)Forms.Context))
            {
                if (mSpeechRecognizer == null)
                {
                    mSpeechRecognizer = SpeechRecognizer.CreateSpeechRecognizer((Activity)Forms.Context);
                    mSpeechRecognizer.SetRecognitionListener(this);
                }
                else
                {
                    mSpeechRecognizer.StopListening();
                }
            }
        }

        public async Task SpeechToTextAsync()
        {
            if (!SpeechListenerStatus)
                return;
            if (executeCommandStatus)
                return;

            try
            {
                
                mSpeechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 5000);
                mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 5000);
                mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
                mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, Java.Util.Locale.Default);
                
                //autoEvent.Reset();
                mSpeechRecognizer.StartListening(mSpeechRecognizerIntent);
                //await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 0, 15)); });                
            }

            catch (Exception ex)
            {
                Toast.MakeText(
                    (Activity)Forms.Context,
                    "Oops! your phone is not supported speech recognization",
                    ToastLength.Short
                    ).Show();
            }
            

        }


        private async void ExecuteVoiceCommand(string speechToText)
        {
            if (!SpeechListenerStatus)
                return;

            if (!string.IsNullOrEmpty(speechToText))
            {
                executeCommandStatus = true;
                //string successResult = await SpeechHelper.ExecuteSpeechCommand(speechToText);
                //SpeechRecognizerHelper.Speak(successResult);

                int successResult = await SpeechHelper.ExecuteSpeechCommand(speechToText);
                int audioId = SpeechRecognizerHelper.GetMappedAudio(successResult);
                SpeechRecognizerHelper.PlayAudio(audioId);
                await Task.Delay(6000);
                executeCommandStatus = false;
                await SpeechToTextAsync();
            }
            else
            {
                await SpeechToTextAsync();
            }

        }

        #region recognize listener
        public void OnBeginningOfSpeech()
        {

        }

        public void OnBufferReceived(byte[] buffer)
        {

        }

        public void OnEndOfSpeech()
        {

        }

        public async void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            Log.Info("Error: ", error.ToString());
            await Task.Delay(6000);
            if (error != SpeechRecognizerError.RecognizerBusy || error!=SpeechRecognizerError.Client)
            {
                await SpeechToTextAsync();
            }
            else
            {
                // mSpeechRecognizer = null;
            }
        }

        public void OnEvent(int eventType, Bundle @params)
        {

        }

        public void OnPartialResults(Bundle partialResults)
        {

        }

        public void OnReadyForSpeech(Bundle @params)
        {

        }

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);

            if (matches != null)
            {
                speechToText = matches[0];
                ExecuteVoiceCommand(speechToText);
            }
        }

        public void OnRmsChanged(float rmsdB)
        {

        }

        #endregion

        #region dispose

        public void Dispose()
        {

        }
        #endregion



    }
}