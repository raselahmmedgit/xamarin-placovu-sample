using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergePacuQuestionViewModel
    {
        public SurgicalConciergePacuQuestionViewModel()
        {
            ScgPacuQuestionViewModels = new List<ScgPacuQuestionViewModel>();
        }
        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string ProfilePictureId { get; set; }

        public string BirthMonth { get; set; }

        public string BirthDay { get; set; }

        public string BirthYear { get; set; }

        public string DateOfBirthStr { get; set; }

        public int PatientActiveProcedureCount { get; set; }

        public string ProfessionalName { get; set; }

        public string ProcedureName { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public string SurgeryTime { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public virtual IList<ScgPacuQuestionViewModel> ScgPacuQuestionViewModels { get; set; }

    }
    public class ScgPacuQuestionViewModel
    {
        public long ScgPacuQuestionId { get; set; }
        public string ScgPacuQuestionText { get; set; }
        public long PatientProfileId { get; set; }
        public long PracticeProfileId { get; set; }
        public Guid? PatientProcedureDetailId { get; set; }
        public bool IsSelected { set; get; }
        public long SelectedScgPacuQuestionDetailId { get; set; }
        public string SelectedScgPacuQuestionDetailTypeMessageText { get; set; }
        public string SelectedScgPacuQuestionDetailRoomNo { get; set; }
        public IList<ScgPacuQuestionDetailViewModel> ScgPacuQuestionDetailViewModels { set; get; }
    }

    public class ScgPacuQuestionDetailViewModel
    {
        public long ScgPacuQuestionDetailId { get; set; }
        public string ScgPacuQuestionDetailText { get; set; }
        public bool IsRoomNo { get; set; }
        public bool IsTypeMessage { get; set; }
        public long ScgPacuQuestionId { get; set; }
        public long? NextScgPacuQuestionId { get; set; }
        public ScgPacuQuestionViewModel ScgPacuQuestionViewModel { get; set; }
        public bool IsSelected { set; get; }
        public string ScgPacuQuestionDetailTypeMessageText { get; set; }
        public string ScgPacuQuestionDetailRoomNo { get; set; }
        public bool? IsYesAnswer { get; set; }
    }
}
