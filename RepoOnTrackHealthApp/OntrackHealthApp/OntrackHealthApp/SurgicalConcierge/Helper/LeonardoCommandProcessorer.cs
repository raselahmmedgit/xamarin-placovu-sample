using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    public class LeonardoCommandProcessorer
    {
        public static SurgicalConciergeStageViewModel ProcessCommand(string command)
        {
            SurgicalConciergeStageViewModel surgicalConciergeStageViewModel = null;
            try
            {
                if (command == null || !command.ToLower().Contains(AppConstants.LeonardoText))
                    return null;
                command = command.ToLower();
                command = command.Replace(AppConstants.LeonardoText, "");
                char[] delimiters = new char[] { ' ' };
                string[] commandWord = command.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                int noOfWord = commandWord.Length;
                List<List<long>> stageIdsList = new List<List<long>>();
                for (int i = 0; i < noOfWord; i++)
                {
                    List<long> stageIds = SpeechHelper.SurgicalConciergeStageList.Where(c => c.StageName.ToLower().Contains(commandWord[i])).Select(d => d.StageId).ToList();
                    if (stageIds.Count() > 0)
                        stageIdsList.Add(stageIds);
                }
                List<long> globalIds = new List<long>();
                int stageIdsListSize = stageIdsList.Count();

                if (stageIdsListSize < (noOfWord * 60) * 1.0 / 100)
                    return null;


                for (int i = 0; i < stageIdsListSize; i++)
                {
                    globalIds.AddRange(stageIdsList[i]);
                }
                var max = 0;
                long stageId = 0;
                for (int i = 0; i < globalIds.Count(); i++)
                {
                    int count = 0;
                    for (int j = 0; j < stageIdsListSize; j++)
                    {
                        if (stageIdsList[j].Contains(globalIds[i]))
                            count++;
                    }
                    if (max < count)
                    {
                        stageId = globalIds[i];
                        max = count;
                    }
                }
                surgicalConciergeStageViewModel = SpeechHelper.SurgicalConciergeStageList.Where(c => c.StageId == stageId).FirstOrDefault();
                int orginalStageWordNo = surgicalConciergeStageViewModel.StageName.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                if (max < (orginalStageWordNo * 60) * 1.0 / 100)
                    return null;
                return surgicalConciergeStageViewModel;
            }
            catch (Exception) { }
            return null;
        }

        internal static int CheckLeonardoCommonSpeech(string command)
        {
            try
            {
                if (command == null || !command.ToLower().Contains(AppConstants.LeonardoText))
                    return LeonardoCommandStatus.CommandNotFound;
                command = command.ToLower();
                command = command.Replace(AppConstants.LeonardoText, "");
                char[] delimiters = new char[] { ' ' };
                string[] commandWord = command.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                int noOfWord = commandWord.Length;
                List<List<long>> stageIdsList = new List<List<long>>();
                for (int i = 0; i < noOfWord; i++)
                {
                    List<long> stageIds = SpeechHelper.LeonardoSpeechTextList.Where(c => c.SpeechText.ToLower().Contains(commandWord[i])).Select(d => d.SpeechTextId).ToList();
                    if (stageIds.Count() > 0)
                        stageIdsList.Add(stageIds);
                }
                List<long> globalIds = new List<long>();
                int stageIdsListSize = stageIdsList.Count();

                if (stageIdsListSize < (noOfWord * 60) * 1.0 / 100)
                    return LeonardoCommandStatus.CommandNotFound;


                for (int i = 0; i < stageIdsListSize; i++)
                {
                    globalIds.AddRange(stageIdsList[i]);
                }
                var max = 0;
                long stageId = 0;
                for (int i = 0; i < globalIds.Count(); i++)
                {
                    int count = 0;
                    for (int j = 0; j < stageIdsListSize; j++)
                    {
                        if (stageIdsList[j].Contains(globalIds[i]))
                            count++;
                    }
                    if (max < count)
                    {
                        stageId = globalIds[i];
                        max = count;
                    }
                }
                LeonardoSpeechText leonardoSpeechText = SpeechHelper.LeonardoSpeechTextList.Where(c => c.SpeechTextId == stageId).FirstOrDefault();
                int orginalStageWordNo = leonardoSpeechText.SpeechText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                if (max < (orginalStageWordNo * 60) * 1.0 / 100)
                    return LeonardoCommandStatus.CommandNotFound;
                long selectedStageId = SpeechHelper.SurgicalConciergeStageList.Where(c=>c.HasStarted == true && c.HasEnded == false).Select(d=>d.StageId).FirstOrDefault();
                if(selectedStageId != 0)
                {
                    return (int)selectedStageId;
                }
                else
                {
                    return LeonardoCommandStatus.NotWorkingAnyStage;
                }
            }
            catch (Exception) { }
            return LeonardoCommandStatus.CommandNotFound;
        }
    }
}
