using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public class Enums
    {
        public enum SearchCrieteria
        {
            pageSize = 8,
            DefaultPageSize = 8

        }
        public enum SpeechToText
        {
            CompleteSilence = 5000
        }
        public enum ScgPacuCommentEnum
        {
            ScgPacuComment1 = 1,
            ScgPacuComment2 = 2,
            ScgPacuComment3 = 3
        }
        public enum ScgFloorCommentEnum
        {
            ScgFloorComment1 = 1,
            ScgFloorComment2 = 2,
            ScgFloorComment3 = 3
        }
        public enum ProcedureIdEnum
        {
            Robotic = 2,
            RoboticPartialNephrectomy = 15,
            RoboticRadicalNephrectomy = 25,
            PastMedicalMaleSurvey = 47,
            PastMedicalFemaleSurvey = 48
        }
        public enum ProcedureReportUrl
        {
            [Description("RoboticPartialNephrectomyReport/PartialNephrectomyGraphReport")]
            RoboticPartialNephrectomyTabular = 915,
            [Description("RoboticRadicalNephrectomyReport/RadicalNephrectomyGraphReport")]
            RoboticRadicalNephrectomyTabular = 925,
            [Description("RoboticPartialNephrectomyReport/RiskFactorGraphReport")]
            RoboticPartialNephrectomy = 15,
            [Description("RoboticRadicalNephrectomyReport/RiskFactorGraphReport")]
            RoboticRadicalNephrectomy = 25,
        }
        public enum NotificationIdEnum
        {
            PastMedicalMaleSurvey = 577,
            PastMedicalFemaleSurvey = 578
        }
        public enum SurgeryDatePatientNotificationEnum
        {
            NotificationId = 999999
        }
        public enum SurveyQuestionSetIdForEnum
        {
            IPSS = 8,
            BSS = 8,
            IIEF = 12,
            PAD = 7,
            UQOL = 9,
            SignAndSymptoms = 19,
            AcutePain = 21,
            Travel = 1,
            Regret = 16,
            SexuallyActive = 28,
            Readmission = 22,
            OutsideCare = 23,
            BowelRecovery = 20,
            AntibioticSurveyQuestion = 2,
            HealthCareWorkerSurveyQuestion = 4,
            EDAssistance = 10,

            PastMedicalSurveyGeneralInformation = 75,
            PastMedicalSurveyAllergies = 76,
            PastMedicalSurveyMedications = 77,
            PastMedicalSurveyPastSurgicalHistory = 78,
            PastMedicalSurveyMedicalAndFamilyHistory = 79,
            PastMedicalSurveySocialHistory = 80,
            PastMedicalSurveyUDI6 = 81,
            PastMedicalSurveyIIQ7 = 82

        }
        public enum SurveyQuestionDetailDefaultValueTypeForEnum
        {
            [Description("Boolean")]
            Boolean = 1,
            [Description("Number")]
            Number = 2,
            [Description("String")]
            String = 3,
            [Description("Button")]
            Button = 4
        }
        public enum SurveyQuestionIdForEnum
        {
            Regret = 65,
            SexuallyActive = 102,
            SexuallyActivePostOp = 45,
            UQOL = 43,
            Pain = 84,
            Fever = 71,
            Fever_101 = 72,
            PainPills = 85,
            Pads = 94,
            Hospitalized = 21,
            AdditionalCareYesOrNo = 19,
            AdditionalCare = 20,
            OutsideCare = 89,
            OutsideCareYesOrNo = 88,
            Readmission = 86,
            Travel = 1,
            Antibiotic = 2,
            HealthCare = 3,
            Biopsy = 4,
            None = 0,
            ErectionalStatusPreOp = 105,
            ErectionalStatusPreOpExcluding = 304,
            ErectionalStatusPostOp = 47,
            ErectionalStatusPostOpExcluding = 138,

            PastMedicalSurveyGeneralInformationHeight = 371,
            PastMedicalSurveyGeneralInformationWeight = 372,
            PastMedicalSurveyGeneralInformationPneumoniaVaccine = 383,
            PastMedicalSurveyGeneralInformationPneumoniaVaccineYear = 384,
            PastMedicalSurveyGeneralInformationColorectalExaminationYear = 386,
            UsedRestRoomIn24Hour = 469
        }
        public enum SurveyQuestionTypeEnum
        {
            Yes_No = 1,
            Multiple_Option = 2,
            Text = 3,
            Multiple_Checkbox = 4,
            Date = 5,
            [Description("Email")]
            Email = 6,
            [Description("Phone")]
            Phone = 7,
            [Description("Text Encrypt")]
            TextEncrypt = 8
        }

        public enum PatientReportedOutComeReportType
        {
            AllRegisteredPatient = 1,
            ProgramStarted = 2,
            FlaggedResult = 3,
            PastMonthCompletedSurvey = 4,
            PastMonthPendingSurvey = 5,
            SearchPatientDirectory = 6
        }
              

        public enum NotificationTypeEnum
        {
            Registration = 1,
            PreProcedure = 2,
            PostProcedure = 3,
        }
       
        public enum PracticeDivision
        {
            None = 0,
            Urology = 1,
            PatientInfo = 2,
            Gynaecology = 3,
            PatientRegistration = 4,
            GeneralSurgery = 7
        }

        public enum NerveSparingEnum
        {
            ProcedureId = 39,
            NotificationId = 561,
            NotificationOrder = 1,
            SurveyQuestionId = 318,
            SurveyQuestionSetId = 67,
            StageId = 5
        }

        public enum ProfessionPracticeDivision
        {
            Outcomes = 1,
            Hospital = 2,
            NotesOrPathology = 3,
            OthersDivisionId = 4
        }

        public enum PatientPageItemBackgroundColorEnum
        {
            [Description("#00a2ff")]
            BackgroundColorOne = 0,
            [Description("#FF9300")]
            BackgroundColorTwo = 1,
            [Description("#89F952")]
            BackgroundColorThree = 2,
            [Description("#FF42A1")]
            BackgroundColorFour = 3,
            [Description("#CCCCFF")]
            BackgroundColorFive = 4
        }

        public enum AttendeeProfileTypeEnum
        {
            [Description("Patient")]
            Patient = 0,
            [Description("Others")]
            Others = 9
        }
    }
}
