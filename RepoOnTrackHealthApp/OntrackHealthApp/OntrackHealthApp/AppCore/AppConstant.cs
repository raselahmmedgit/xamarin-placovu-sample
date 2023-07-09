using System.Collections.Generic;

namespace OntrackHealthApp.AppCore
{
    public static class AppConstant
    {
        public const string BaseAddress = "https://ontrack-health.com/";
        public const string BaseApiDirectory = "webapi/v4/";

        internal static string GoogleServerKey = "AIzaSyCxepLrdfCXLYnSJIgDAETnqwEH4WZCu1o";
        internal static string GeoCodingUrl = "https://maps.googleapis.com/maps/api/geocode/json?{0}&key=" + GoogleServerKey + "";

        public static string DbExceptionError = "Database is currently experiencing problems:";
        public static string UpdateExceptionError = "Datebase is currently updating problem.";
        public static string EntityExceptionError = "Entity is problem.";
        public static string NullReferenceExceptionError = "There are one or more required fields that are missing.";
        public static string ApplicationExceptionError = "Exception in application:";

        public static string NotFound = "Data not found.";
        public static string NullError = "Requested object could not found.";
        public static string ErrorCommon = "Oops! Exception in application.";
        public static string ModelValidError = "Form is not valid.";
        public static string NoInternetConnectMessage = "No internet connect please try again.";
        public static string NoProcedureMessage = "You do not have any procedure scheduled.";
        public static string NoRecipientMessage = "You do not have any recipient(s).";
        public static string NoPatientContactMessage = "patient email or phone needed to continue.";
        public static string NoPatientContactOrRecipientMessage = "Patient email or at least one recipient is required to continue.";

        public static string Error400 = "Oops! - Exception in application.";
        public static string Error401 = "Oops! - Exception in application.";
        public static string Error403 = "Oops! - Exception in application.";
        public static string Error404 = "Oops! - Exception in application.";
        public static string Error405 = "Oops! - Exception in application.";
        public static string Error406 = "Oops! - Exception in application.";
        public static string Error408 = "Oops! - Exception in application.";
        public static string Error412 = "Oops! - Exception in application.";
        public static string Error500 = "Oops! - Exception in application.";
        public static string Error501 = "Oops! - Exception in application.";
        public static string Error502 = "Oops! - Exception in application.";

        public static string SaveSuccessMessage = "Data has been saved successfully.";
        public static string SaveInformationMessage = "Data has not been saved.";
        public static string UpdateSuccessMessage = "Data has been updated successfully.";
        public static string UpdateInformationMessage = "Data has not been updated.";
        public static string DeleteSuccessMessage = "Data has been deleted successfully.";
        public static string DeleteInformationMessage = "Data has not been deleted.";

        public static string DisplayAlertErrorTitle = "Error!";
        public static string DisplayAlertTitle = "Message!";
        public static string DisplayAlertWarningTitle = "Warning!";
        public static string DisplayAlertErrorMessage = "Oops! Exception in application.";
        public static string DisplayAlertGeocoderErrorMessage = "Oops! Geocoder returns no results.";
        public static string DisplayAlertErrorButtonText = "Ok";

        public static string PacuSuccessMessage = "Pacu comment has been sent successfully.";
        public static string PacuErrorMessage = "Pacu comment has not been sent successfully.";
        public static string FloorSuccessMessage = "Floor comment has been sent successfully.";
        public static string FloorErrorMessage = "Floor comment has not been sent successfully.";
        public static string PatientEmailAddressRequired = "Patient Email Address Required.";
        public static string NotificationSuccessMessage = "Notification has been sent successfully.";
        public static string NotificationFailedMessage = "Notification has not been sent successfully.";
        public static string DischargeSuccessMessage = "Discharge comment has been sent successfully.";
        public static string DischargeErrorMessage = "Discharge comment has not been sent successfully.";

        public static string PatientDeviceTokenSaveSuccessMessage = "Patient device token has been saved successfully.";
        public static string PatientDeviceTokenSaveErrorMessage = "Patient device token save failed.";
        public static string PatientProfileDeviceTokenKey = "PatientProfileDeviceToken";
        public static string IsPatientProfileDeviceTokenChangedKey = "IsPatientProfileDeviceTokenChanged";
        public static string RemoteNotificationDataKey = "notificationInfo";
        public static string LocalNotificationDataKey = "notificationInfo";
        public static string notificationIdKey = "notificationId";
        public static string patientNotificationDetailIdKey = "patientNotificationDetailId";


        public static string ToastMessageTitle = "Message!";
        public static string ToastMessageButtonText = "Ok";
        public static string MessageOkButtonText = "Ok";

        public static int NotificationManagerNotifyId = 0;
        public static string DischargeNotificationInfo = "Discharge Info";
        public static int MaximumDischargeItemSelectLimit = 5;
        public static string MaximumDischargeItemSelectMessage = "You have already selected maximum number of items!";

        public static string iOSName5 = "iPhone 5";
        public static string iOSName5s = "iPhone 5s";
        public static string iOSName6 = "iPhone 6";
        public static string iOSName6s = "iPhone 6s";
        public static string iOSName7 = "iPhone 7";
        public static string iOSName7s = "iPhone 7s";
        public static string iOSName8 = "iPhone 8";
        public static string iOSName8s = "iPhone 8s";

        public static string MonitoringConsentDisagreeMessage = "You have opted out of the OnTrack program.";
        public static string MonitoringConsentTitle = "Remote Patient Monitoring Consent";

    }

    public static class RoleIdConstants
    {

        public static string SystemAdmin = "SystemAdmin";

        public static string PracticeAdmin = "PracticeAdmin";

        public static string Professional = "Professional";

        public static string Patient = "Patient";

        public static string Scheduler = "Scheduler";

        public static string OperatingRoomPersonnel = "OperatingRoomPersonnel";

        public static string OperatingRoomNurse = "OperatingRoomNurse";

        public static string OperatingRoomMD = "OperatingRoomMD";

        public static string PhysicianAssistant = "PhysicianAssistant";
    }

    public static class PatientReportedOutComeReportTypeName
    {
        public static string AllRegisteredPatient = "All Patients";
        public static string ProgramStarted = "Program Started";
        public static string SurveyCompleted = "Survey Completed";
        public static string SurveyPending = "Survey Pending";
        public static string SurveyVisited = "Survey Visited";
        public static string SurveyCritical = "Critical Result(s)";
        public static string SearchPatient = "Search Patient";

        public static string AllRegisteredPatientIcon = "mob_patient.png";
        public static string ProgramStartedIcon = "patient_outcome_circle_green_dark_ok.png";
        public static string SurveyCompletedIcon = "mob_surveycompleted.png";
        public static string SurveyPendingIcon = "mob_surveypending.png";
        public static string SurveyVisitedIcon = "patient_outcome_circle_patient.png";
        public static string SurveyCriticalIcon = "mob_red_flag.png";
        public static string SearchPatientIcon = "patient_outcome_search_icon.png";

        public static string AllRegisteredPatientBgColor = "#60D836";
        public static string ProgramStartedBgColor = "#D7D7D7";
        public static string SurveyCompletedBgColor = "#60D836";
        public static string SurveyPendingBgColor = "#F8BA00";
        public static string SurveyVisitedBgColor = "#D7D7D7";
        public static string SurveyCriticalBgColor = "#ED220B";
        public static string SearchPatientBgColor = "#D7D7D7";

    }
}

