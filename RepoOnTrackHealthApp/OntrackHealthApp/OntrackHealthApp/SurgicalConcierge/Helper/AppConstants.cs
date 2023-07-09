using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    public class AppConstants
    {
        public static string EmailRequiredMessage = "Email is required";
        public static string PhoneNumberRequiredMessage = "Phone Number is required";
        public static string EmailErrorMessage = "Please enter valid email address";
        public static string PhoneNumberErrorMessage = "Please enter valid phone Number";
        public static string EmailAddressOrPhoneNumberRequired = "Please enter valid email or valid phone Number";
        public static string AtLeastOneErrorMessage = "At least one recipient is required";
        public static string LeonardoText = "leonardo";
        public static string LeonardoWelcomeCommand = "Leonardo ready to execute command. Please start to speak after beep.";
        public static string LeonardoGoodByeCommand = "Leonardo will sleep now. Have a nice day.";
        public static string LeonardoNotAnyStageCommand = "I am not working on any stage now";
        public static string LeonardoSpeechNotRecognized = "Speech cannot be recognized. Please try again";
        public static string StageSelectedText = "selected stage already ended";
        public static string LeonardoIsNotSupported = "Leonardo is not supported on your device. we are working on it";
        public static string GoogleHomePackageName = "com.google.android.googlequicksearchbox";


        public static string PickerDefaultText = "Select...";
        public static int PickerDefaultIndex = -1;


        public static List<long> DivisionExcludedFromMobileViewAdmin = new List<long> { 2, 4, 5, 6 };
    }

    public class LeonardoCommandStatus
    {
        public const int CommandNotFound = 0;
        public const int StageStarted = 101;
        public const int StageEnded = 102;
        public const int StageAlreadyEnded = 103;
        public const int StageNotRecognized = 104;
        public const int NotWorkingAnyStage = 105;
        public const int PatientOperationRoom = 1;
        public const int AnesthesiaStarted = 2;
        public const int ProcedureStarted = 3;
        public const int RoboticPortion = 4;
        public const int NerveSparing = 5;
        public const int ProstateRemoved = 6;
        public const int BladderUrethra = 7;
        public const int RobotUndocked = 8;
        public const int ProcedureCompleted = 9;
        public const int PatientLeavingRomm = 10;
        public const int OutOfRoom = 11;
        public const int KidneyAndTumorIsolated = 12;
        public const int KidneyTumorRomved = 13;
        public const int SettingKidneyRemoval = 14;
        public const int KidneyRomved = 15;
        public const int IdentifyingUreteralNarrowing = 16;
        public const int ReconnectPelvisAndUreter = 17;

    }

    public enum PracticeDivisionId
    {
        None = 0,
        Urology = 1,
        PatientInfo = 2,
        Gynaecology = 3,
        PatientRegistration = 4,
        SurgicalConciergeTracker = 5,
        PatientReportedOutcome = 6,
        GeneralSurgery = 7,
        ColoRectal = 8
    }

    public enum PracticeDivisionUnit
    {
        OperatingRoom = 1,
        PACU = 2,
        Floor = 3,
        Pathology = 4,
        NursePatientInfo = 6,
        Discharge = 5,
        PreSurgerySummary = 11,
        PatientReportedOutcome = 101,
        Programs = 102,
        NursingRounds = 13,
        ComparativeAnalysis = 104,
        Billing = 103,
        GenericSurvey = 105
    }

    public enum ScgStageIdEnum
    {
        PatientIsInOperatingRoom = 1,
        ProcedureIsCompleted = 9,
        PatientIsLeavingTheRoom = 10
    }

    public enum DefaultEstimatedTime
    {
        OperatingRoom = 125,
        PACU = 90
    }

    public class PracticeDivision
    {
        public static long Urology = 1;
        public static long PatientInfo = 2;
        public static long Gynaecology = 3;
        public static long PatientRegistration = 4;
        public static long SurgicalConciergeTracker = 5;
        public static long PatientReportedOutcome = 6;

        public const long UrologyId = 1;
        public const long PatientInfoId = 2;
        public const long GynaecologyId = 3;
        public const long PatientRegistrationId = 4;
        public const long SurgicalConciergeTrackerId = 5;
        public const long PatientReportedOutcomeId = 6;
        public const long GeneralSurgeryId = 7;
        public const long ColoRectalId = 8;

        public static List<long> OperatingRoomIds = new List<long>() { 1, 6, 14, 23 };
        public static List<long> PACUIds = new List<long>() { 2, 7, 15, 24 };
        public static List<long> FloorIds = new List<long>() { 3, 8, 16, 25 };
        public static List<long> PathologyIds = new List<long>() { 4, 9, 17, 26 };
        public static List<long> DischargeIds = new List<long>() { 5, 10, 18, 27 };
        public static List<long> PreSurgerySummaryIds = new List<long>() { 11, 12, 19, 28 };
        public static List<long> NursingRoundIds = new List<long>() { 13, 20, 21, 29, 22 };

        public const long OperatingRoomId = 1;
        public const long PACUId = 2;
        public const long FloorId = 3;
        public const long PathologyId = 4;
        public const long NursePatientInfoId = 6;
        public const long DischargeId = 5;
        public const long PreSurgerySummaryId = 11;
        public const long NursingRoundId = 13;

        public static string GetPracticeDivisionImage(long divisionid)
        {
            string practiceDivisionImage;
            switch (divisionid)
            {
                case UrologyId:
                    practiceDivisionImage = "division_urology.png";
                    break;
                case PatientRegistrationId:
                    practiceDivisionImage = "division_patient_registration_new.png";
                    break;
                case PatientInfoId:
                    practiceDivisionImage = "division_patient_info.png";
                    break;
                case GeneralSurgeryId:
                    practiceDivisionImage = "division_patient_info.png";
                    break;
                case GynaecologyId:
                    practiceDivisionImage = "division_gynecology.png";
                    break;
                case ColoRectalId:
                    practiceDivisionImage = "division_colorectal.png";
                    break;
                default:
                    practiceDivisionImage = "";
                    break;
            }
            return practiceDivisionImage;
        }


        public static string GetPracticeDivisionImageBackgroundColor(long divisionid)
        {
            string imageBackgroundColor;

            switch (divisionid)
            {
                case UrologyId:
                    imageBackgroundColor = "#0d376a";
                    break;
                case PatientRegistrationId:
                    imageBackgroundColor = "#00a89d";
                    break;
                case PatientInfoId:
                    imageBackgroundColor = "#e0457b";
                    break;
                case GeneralSurgeryId:
                    imageBackgroundColor = "#e0457b";
                    break;
                case GynaecologyId:
                    imageBackgroundColor = "#ee3190";
                    break;
                default:
                    imageBackgroundColor = "";
                    break;
            }
            return imageBackgroundColor;
        }

        //public static string getPracticeDivisionUnitImage(long divisionUnitId)
        //{
        //    string divisionUnitImage;

        //    switch (divisionUnitId)
        //    {
        //        case OperatingRoomId:
        //            divisionUnitImage = "division_unit_operation.png";
        //            break;
        //        case PACUId:
        //            divisionUnitImage = "division_unit_pacu.png";
        //            break;
        //        case FloorId:
        //            divisionUnitImage = "division_unit_floor.png";
        //            break;
        //        case PathologyId:
        //            divisionUnitImage = "division_unit_pathology.png";
        //            break;
        //        case PreSurgerySummaryId:
        //            //divisionUnitImage = "division_unit_pre_surgery.png";
        //            divisionUnitImage = "division_unit_pre_surgery_summary.png";
        //            break;
        //        case NursingRoundId:
        //            divisionUnitImage = "division_unit_customize_update.png";
        //            break;
        //        case DischargeId:
        //            divisionUnitImage = "division_unit_discharge.png";
        //            break;
        //        default:
        //            divisionUnitImage = "";
        //            break;
        //    }
        //    return divisionUnitImage;

        //}

        public static string getPracticeDivisionUnitImage(long divisionUnitId)
        {
            string divisionUnitImage;

            if (OperatingRoomIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_operation.png";
            }
            else if (PACUIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_pacu.png";
            }
            else if (FloorIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_floor.png";
            }
            else if (PathologyIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_pathology.png";
            }
            else if (PreSurgerySummaryIds.Contains(divisionUnitId))
            {
                //divisionUnitImage = "division_unit_pre_surgery.png";
                divisionUnitImage = "division_unit_pre_surgery_summary.png";
            }
            else if (NursingRoundIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_customize_update.png";
            }
            else if (DischargeIds.Contains(divisionUnitId))
            {
                divisionUnitImage = "division_unit_discharge.png";
            }
            else
            {
                divisionUnitImage = string.Empty;
            }
            return divisionUnitImage;
        }

        //public static string getPracticeDivisionUnitImageBackgroundColor(long divisionUnitId)
        //{
        //    string divisionUnitImageBackgroundColor;

        //    switch (divisionUnitId)
        //    {
        //        case OperatingRoomId:
        //            divisionUnitImageBackgroundColor = "#0075b9";
        //            break;
        //        case PACUId:
        //            divisionUnitImageBackgroundColor = "#1db000";
        //            break;
        //        case FloorId:
        //            divisionUnitImageBackgroundColor = "#98185e";
        //            break;
        //        case PathologyId:
        //            divisionUnitImageBackgroundColor = "#b41800";
        //            break;
        //        case PreSurgerySummaryId:
        //            divisionUnitImageBackgroundColor = "#5e5e5e";
        //            break;
        //        case NursingRoundId:
        //            divisionUnitImageBackgroundColor = "#553393";
        //            break;
        //        case DischargeId:
        //            divisionUnitImageBackgroundColor = "#ff9300";
        //            break;
        //        default:
        //            divisionUnitImageBackgroundColor = "";
        //            break;
        //    }
        //    return divisionUnitImageBackgroundColor;
        //}

        public static string getPracticeDivisionUnitImageBackgroundColor(long divisionUnitId)
        {
            string divisionUnitImageBackgroundColor;

            if (OperatingRoomIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#0075b9";
            }
            else if (PACUIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#1db000";
            }
            else if (FloorIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#98185e";
            }
            else if (PathologyIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#b41800";

            }
            else if (PreSurgerySummaryIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#5e5e5e";

            }
            else if (NursingRoundIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#553393";
            }
            else if (DischargeIds.Contains(divisionUnitId))
            {
                divisionUnitImageBackgroundColor = "#ff9300";
            }
            else
            {
                divisionUnitImageBackgroundColor = string.Empty;
            }
            return divisionUnitImageBackgroundColor;
        }

        public static long GetPracticeDivisionIdByPracticeDivisionUnit(long practiceDivisionUnitId)
        {
            if (GetUrologyDivisionUnitList().Contains(practiceDivisionUnitId))
                return Urology;
            else if (GetGynecologyDivisionUnitList().Contains(practiceDivisionUnitId))
                return Gynaecology;
            return 0;

        }

        private static List<long> GetUrologyDivisionUnitList()
        {
            return new List<long> { 1, 2, 3, 4, 5, 11, 13 };
        }

        private static List<long> GetGynecologyDivisionUnitList()
        {
            return new List<long> { 6, 7, 8, 9, 10, 12 };
        }
    }

    public class PracticeDivisionUnitId
    {
        public static List<long> GetRegularDivisionUnitId()
        {
            return new List<long>() { 1, 2, 6, 7 };
        }
        public static List<long> GetPastWeekDivisionUnitId()
        {
            return new List<long>() { 4, 5, 9, 10 };
        }
        public static List<long> GetOperatingRoomDivisionUnitId()
        {
            return new List<long>() { 1, 6, 14, 23 };
        }
        public static List<long> GetPacuDivisionUnitId()
        {
            return new List<long> { 2, 7, 15, 24 };
        }
        public static List<long> GetPathologyDivisionUnitId()
        {
            return new List<long> { 4, 9, 17, 26 };
        }
        public static List<long> GetDischargeDivisionUnitId()
        {
            return new List<long> { 5, 10, 18, 27};
        }
        public static List<long> GetPreSurgerySummaryDivisionUnitId()
        {
            return new List<long> { 11, 12, 19, 28 };
        }
        public static List<long> GetScgNursingRoundDivisionUnitId()
        {
            return new List<long>() { 13, 20, 21, 29 };
        }
        public static List<long> GetFloorDivisionUnitId()
        {
            return new List<long>() { 3, 8, 16, 25 };
        }
    }
    public class PracticeDivisionBoxColor
    {
        public const string Urology = "#F8BA00";
        public const string PatientInfo = "#61D836";
        public const string Gynecology = "#004D80";
        public const string PatientRegistration = "#00A0FF";

    }
    public class PracticeDivisionUnitBoxColor
    {
        public const string OperatingRoom = "#F8BA00";
        public const string Pacu = "#61D836";
        public const string Floor = "#004D80";
        public const string Pathology = "#EF5FA7";

        public const string Search = "#61D836";
        public const string Discharge = "#98185e";

        public static string GetPracticeDivisionUnitBoxColor(string divisionUnitStyle)
        {
            string boxStyle = "";
            switch (divisionUnitStyle)
            {
                case "operation-room-unit-box":
                    boxStyle = OperatingRoom;
                    break;
                case "pacu-unit-box":
                    boxStyle = Pacu;
                    break;
                case "floor-unit-box":
                    boxStyle = Floor;
                    break;
                case "pathology-unit-box":
                    boxStyle = Pathology;
                    break;
            }
            return boxStyle;
        }

    }

    public class CancerLocationTemplateTypeId
    {
        public const string Twelve = "12";
        public const string Six = "6";
        public const string Two = "2";
    }


}
