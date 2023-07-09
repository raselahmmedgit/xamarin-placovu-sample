using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using OntrackHealthApp.SurgicalConcierge.Helper;
using Xamarin.Essentials;

namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class SpeechRecognizerHelper
    {
        public async static void Speak(string detectedSpeech)
        {

            var locales = await TextToSpeech.GetLocalesAsync();
            foreach (var item in locales)
            {
                Log.Info("Voice", item.Language);
                Log.Info("Name:", item.Name);
            }

            // Grab the first locale
            var locale = locales.FirstOrDefault(c => c.Language.Equals("en"));
            var speechSettings = new SpeakSettings
            {
                Locale = locale,
                Pitch = 0
            };

            await TextToSpeech.SpeakAsync(detectedSpeech, speechSettings).ContinueWith((t) =>
             {
             }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public static void PlayAudio(int fileName)
        {
            try
            {
                string filePath = GenerateFilePath(fileName);
                //await CrossMediaManager.Current.Play(filePath);
                MediaPlayer player = new MediaPlayer();
                player = MediaPlayer.Create(MainActivity.Instance, Android.Net.Uri.Parse(filePath));
                player.Start();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.StackTrace);
            }
        }
        public static string GenerateFilePath(int fileName)
        {
            string filePath = "android.resource://" + Android.App.Application.Context.PackageName + "/" + fileName;
            return filePath;
        }

        public static int GetMappedAudio(int result)
        {
            int audioFileId = 0;
            switch(result) {
                case LeonardoCommandStatus.CommandNotFound : audioFileId = Resource.Raw.SpeechUnrecognized; break;
                case LeonardoCommandStatus.StageStarted : audioFileId = Resource.Raw.StageStarted; break;
                case LeonardoCommandStatus.StageEnded : audioFileId = Resource.Raw.StageEnded; break;
                case LeonardoCommandStatus.StageAlreadyEnded : audioFileId = Resource.Raw.StageAlreadyEnded; break;
                case LeonardoCommandStatus.NotWorkingAnyStage : audioFileId = Resource.Raw.NotWorkingAnyStage; break;
                case LeonardoCommandStatus.PatientOperationRoom : audioFileId = Resource.Raw.PatientOperatingRoom; break;
                case LeonardoCommandStatus.AnesthesiaStarted : audioFileId = Resource.Raw.AnesthesiaStarted; break;
                case LeonardoCommandStatus.ProcedureStarted : audioFileId = Resource.Raw.PrecudureStarted; break;
                case LeonardoCommandStatus.RoboticPortion : audioFileId = Resource.Raw.RoboticPortion; break;
                case LeonardoCommandStatus.NerveSparing : audioFileId = Resource.Raw.NerveSparing; break;
                case LeonardoCommandStatus.ProstateRemoved : audioFileId = Resource.Raw.prostateremoved; break;
                case LeonardoCommandStatus.BladderUrethra : audioFileId = Resource.Raw.BladderUrethra; break;
                case LeonardoCommandStatus.RobotUndocked : audioFileId = Resource.Raw.RobotUndocked; break;
                case LeonardoCommandStatus.ProcedureCompleted : audioFileId = Resource.Raw.ProcedureCompleted; break;
                case LeonardoCommandStatus.PatientLeavingRomm : audioFileId = Resource.Raw.PatientLeavingRoom; break;
                case LeonardoCommandStatus.OutOfRoom : audioFileId = Resource.Raw.OutOfRoom; break;
                case LeonardoCommandStatus.KidneyAndTumorIsolated : audioFileId = Resource.Raw.KidneyAndTumorIsolated; break;
                case LeonardoCommandStatus.KidneyTumorRomved : audioFileId = Resource.Raw.KidneyTumorRemoved; break;
                case LeonardoCommandStatus.SettingKidneyRemoval: audioFileId = Resource.Raw.SettingKidneyRemoval; break;
                case LeonardoCommandStatus.KidneyRomved: audioFileId = Resource.Raw.KidneyRemoved; break;
                case LeonardoCommandStatus.IdentifyingUreteralNarrowing: audioFileId = Resource.Raw.IdentifyingUreteralNarrowing; break;
                case LeonardoCommandStatus.ReconnectPelvisAndUreter: audioFileId = Resource.Raw.ReconnectPelvisAndUreter; break;
                default: audioFileId = Resource.Raw.SpeechUnrecognized; break;
            }
            return audioFileId;
        }
    }
}