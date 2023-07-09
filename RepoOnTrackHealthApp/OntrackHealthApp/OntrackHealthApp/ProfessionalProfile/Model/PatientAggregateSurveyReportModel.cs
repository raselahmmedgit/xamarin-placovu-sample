﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class PatientAggregateSurveyReportModel
    {
        public bool IsShowPatientEmailTemplate { get; set; }
        public PatientAggregateSurveyReportModel()
        {
            List<PatientAggregateSurveyReportView> PatientAggregateSurveyReportViewList = new List<PatientAggregateSurveyReportView>();
        }
        public PatientProfileViewModelForAggregateSurvey PatientProfileViewModel { get; set; }
        public string PatientAge { get; set; }
        public PatientProcedureDetailViewModelForAggregateSurvey PatientProcedureDetailViewModel { get; set; }
        public List<PatientAggregateSurveyReportView> PatientAggregateSurveyReportViewList { get; set; }
        public long isActiveNotificationId { get; set; }
        public bool IsOneWeekNotification { get; set; }
    }

    public class PatientProfileViewModelForAggregateSurvey
    {
        public long PatientProfileId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthStr { get; set; }
        public string PreferredName { get; set; }
    }

    public class PatientProcedureDetailViewModelForAggregateSurvey
    {
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
    }

    public class PatientAggregateSurveyReportView
    {
        public long NotificationId { get; set; }
    }
}