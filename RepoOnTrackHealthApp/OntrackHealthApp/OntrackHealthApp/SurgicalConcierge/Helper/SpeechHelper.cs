
using OntrackHealthApp.ProfessionalProfile;
using OntrackHealthApp.SurgicalConcierge.Interfaces;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    public class SpeechHelper
    {
        public static List<SurgicalConciergeStageViewModel> SurgicalConciergeStageList = new List<SurgicalConciergeStageViewModel>();
        public static List<LeonardoSpeechText> LeonardoSpeechTextList = new List<LeonardoSpeechText>();

        public static SurgicalConciergeDetail speechHelperconciergeDetail;
        //public static ProfessionalOperatingPage speechHelperOperatingPage;
        public static Button CommentButton;
        public static void StartSpeechRecognize(Button CommentBtn)
        {
            CommentButton = CommentBtn;
            DependencyService.Get<IOntrackSpeechRecognizer>().StartSpeechRecognize();
        }
        public static void StopSpeechRecognize()
        {
            DependencyService.Get<IOntrackSpeechRecognizer>().StopSpeechRecognize();
        }
        public static void Speak(string speech)
        {
            DependencyService.Get<IOntrackSpeechRecognizer>().Speak(speech);
        }
        public async static Task<int> ExecuteSpeechCommand(string command)
        {
            String successResult = null;
            if (command == null || command.Length == 0)
                return LeonardoCommandStatus.CommandNotFound;

            SurgicalConciergeStageViewModel selectedStage = LeonardoCommandProcessorer.ProcessCommand(command);
            if (selectedStage != null)
            {
                if (!selectedStage.HasStarted)
                {
                    successResult = await speechHelperconciergeDetail.ExecuteSurgicalStageStart(selectedStage);
                    if (successResult != null && successResult.ToLower().Contains(AppConstants.StageSelectedText))
                    {
                        //successResult = successResult.Replace(AppConstants.StageSelectedText, "");
                        //successResult = selectedStage.StageName + successResult;
                        return LeonardoCommandStatus.StageAlreadyEnded;
                    }
                    if (CommentButton.IsVisible == false)
                    {
                        CommentButton.IsVisible = true;
                    }
                    return LeonardoCommandStatus.StageStarted;
                }
                else
                {
                    successResult = await speechHelperconciergeDetail.ExcuteSurgicalStageEnd(selectedStage);
                    if (successResult != null && successResult.ToLower().Contains(AppConstants.StageSelectedText))
                    {
                        //successResult = successResult.Replace(AppConstants.StageSelectedText, "");
                        //successResult = selectedStage.StageName + successResult;
                        return LeonardoCommandStatus.StageAlreadyEnded;
                    }
                    return LeonardoCommandStatus.StageEnded;
                }
            }

            int result = LeonardoCommandProcessorer.CheckLeonardoCommonSpeech(command);
            //if(successResult == null)
            //{
            //    successResult = AppConstants.LeonardoSpeechNotRecognized;
            //}
            return result;

        }
    }
}
