using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OntrackHealthApp.SurgicalConcierge.Interfaces
{
    public interface IOntrackSpeechRecognizer
    {
        void StartSpeechRecognize();
        void StopSpeechRecognize();
        void Speak(string speech);
    }
}
