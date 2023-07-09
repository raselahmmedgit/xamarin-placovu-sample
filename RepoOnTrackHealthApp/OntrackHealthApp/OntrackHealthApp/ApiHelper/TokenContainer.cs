using System;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiHelper
{
    public class TokenContainer : ITokenContainer
    {
        private const string ApiTokenKey = "ApiToken";

        public object ApiToken
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiTokenKey) ? Application.Current.Properties[ApiTokenKey] : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiTokenKey))
                    {
                        Application.Current.Properties[ApiTokenKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public bool IsApiToken()
        {
            return this.ApiToken == null ? false : true;
        }

        public void ClearApiToken()
        {
            Application.Current.Properties.Clear();
            Application.Current.SavePropertiesAsync();
        }

        #region User Identity

        private const string ApiUserIdKey = "ApiUserId";
        private const string ApiUserRoleKey = "ApiUserRole";
        private const string ApiUserNameDisplayKey = "ApiUserNameDisplay";
        private const string ApiUserEmailKey = "ApiUserEmail";
        private const string ApiPracticeProfileIdKey = "ApiPracticeProfileId";
        private const string ApiPatientProfileIdKey = "ApiPatientProfileId";
        private const string ApiIsSystemAdminKey = "ApiIsSystemAdmin";
        private const string ApiIsAdminKey = "ApiIsAdmin";
        private const string ApiPracticeNameKey = "ApiPracticeName";
        private const string ApiPracticeLocationNameKey = "ApiPracticeLocationName";
        private const string ApiProcedureNameKey = "ApiProcedureName";

        private const string ApiMobileNotificationIntervalMinuteKey = "ApiMobileNotificationIntervalMinute";
        private const string ApiMobileNotificationElapsedMinuteKey = "ApiMobileNotificationElapsedMinute";

        private const string CurrentProcedureNameKey = "CurrentProcedureName";
        private const string CurrentProcedureIdKey = "CurrentProcedureId";

        private const string CurrentPatientProcedureDetailIdKey = "CurrentPatientProcedureDetailId";
        private const string HasSendToastNotificationKey = "HasSendToastNotification";
        private const string HasStartByClickToastNotificationKey = "HasStartByClickToastNotification";

        private const string ApiProfessionalProfileIdKey = "ApiProfessionalProfileId";
        private const string ApiProfessionalProfileNameKey = "ApiProfessionalProfileName";

        private const string ApiIsAgreePatientMonitoringConsentKey = "ApiIsAgreePatientMonitoringConsent";
        private const string ApiIsDoNotAgreePatientMonitoringConsentKey = "ApiIsDoNotAgreePatientMonitoringConsent";

        public string ApiUserId
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiUserIdKey) ? Application.Current.Properties[ApiUserIdKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiUserIdKey))
                    {
                        Application.Current.Properties[ApiUserIdKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiUserRole
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiUserRoleKey) ? Application.Current.Properties[ApiUserRoleKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiUserRoleKey))
                    {
                        Application.Current.Properties[ApiUserRoleKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiUserNameDisplay
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiUserNameDisplayKey) ? Application.Current.Properties[ApiUserNameDisplayKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiUserNameDisplayKey))
                    {
                        Application.Current.Properties[ApiUserNameDisplayKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiUserEmail
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiUserEmailKey) ? Application.Current.Properties[ApiUserEmailKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiUserEmailKey))
                    {
                        Application.Current.Properties[ApiUserEmailKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public long? ApiPracticeProfileId
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(ApiPracticeProfileIdKey))
                {
                    return Convert.ToInt64(Application.Current.Properties[ApiPracticeProfileIdKey].ToString());
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiPracticeProfileIdKey))
                    {
                        Application.Current.Properties[ApiPracticeProfileIdKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public long? ApiPatientProfileId
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(ApiPatientProfileIdKey))
                {
                    return Convert.ToInt64(Application.Current.Properties[ApiPatientProfileIdKey]?.ToString());
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiPatientProfileIdKey))
                    {
                        Application.Current.Properties[ApiPatientProfileIdKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public bool ApiIsSystemAdmin
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiIsSystemAdminKey) ? Convert.ToBoolean(Application.Current.Properties[ApiIsSystemAdminKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiIsSystemAdminKey))
                    {
                        Application.Current.Properties[ApiIsSystemAdminKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public bool ApiIsAdmin
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiIsAdminKey) ? Convert.ToBoolean(Application.Current.Properties[ApiIsAdminKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[ApiIsAdminKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string ApiPracticeName
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiPracticeNameKey) ? Convert.ToString(Application.Current.Properties[ApiPracticeNameKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiPracticeNameKey))
                    {
                        Application.Current.Properties[ApiPracticeNameKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiPracticeLocationName
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiPracticeLocationNameKey) ? Convert.ToString(Application.Current.Properties[ApiPracticeLocationNameKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiPracticeLocationNameKey))
                    {
                        Application.Current.Properties[ApiPracticeLocationNameKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiProcedureName
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiProcedureNameKey) ? Convert.ToString(Application.Current.Properties[ApiProcedureNameKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiProcedureNameKey))
                    {
                        Application.Current.Properties[ApiProcedureNameKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiMobileNotificationIntervalMinute
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiMobileNotificationIntervalMinuteKey) ? Convert.ToString(Application.Current.Properties[ApiMobileNotificationIntervalMinuteKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiMobileNotificationIntervalMinuteKey))
                    {
                        Application.Current.Properties[ApiMobileNotificationIntervalMinuteKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiMobileNotificationElapsedMinute
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiMobileNotificationElapsedMinuteKey) ? Convert.ToString(Application.Current.Properties[ApiMobileNotificationElapsedMinuteKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiMobileNotificationElapsedMinuteKey))
                    {
                        Application.Current.Properties[ApiMobileNotificationElapsedMinuteKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public long CurrentProcedureId
        {
            get
            {
                return Application.Current.Properties.ContainsKey(CurrentProcedureIdKey) ? Convert.ToInt64(Application.Current.Properties[CurrentProcedureIdKey].ToString()) : 0;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[CurrentProcedureIdKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string CurrentProcedureName
        {
            get
            {
                return Application.Current.Properties.ContainsKey(CurrentProcedureNameKey) ? Convert.ToString(Application.Current.Properties[CurrentProcedureNameKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[CurrentProcedureNameKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }
        
        public string CurrentPatientProcedureDetailId
        {
            get
            {
                return Application.Current.Properties.ContainsKey(CurrentPatientProcedureDetailIdKey) ? Convert.ToString(Application.Current.Properties[CurrentPatientProcedureDetailIdKey].ToString()) : string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[CurrentPatientProcedureDetailIdKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public bool HasSendToastNotification
        {
            get
            {
                return Application.Current.Properties.ContainsKey(HasSendToastNotificationKey) ? Convert.ToBoolean(Application.Current.Properties[HasSendToastNotificationKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[HasSendToastNotificationKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public bool HasStartByClickToastNotification
        {
            get
            {
                return Application.Current.Properties.ContainsKey(HasStartByClickToastNotificationKey) ? Convert.ToBoolean(Application.Current.Properties[HasStartByClickToastNotificationKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[HasStartByClickToastNotificationKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public long? ApiProfessionalProfileId
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(ApiProfessionalProfileIdKey))
                {
                    if (Application.Current.Properties[ApiProfessionalProfileIdKey] != null)
                    {
                        return Convert.ToInt64(Application.Current.Properties[ApiProfessionalProfileIdKey].ToString());
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiProfessionalProfileIdKey))
                    {
                        Application.Current.Properties[ApiProfessionalProfileIdKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public string ApiProfessionalProfileName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(ApiProfessionalProfileNameKey))
                {
                    if (Application.Current.Properties[ApiProfessionalProfileNameKey] != null)
                    {
                        return Application.Current.Properties[ApiProfessionalProfileNameKey].ToString();
                    }
                    return string.Empty;
                }
                return string.Empty;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiProfessionalProfileNameKey))
                    {
                        Application.Current.Properties[ApiProfessionalProfileNameKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public bool ApiIsAgreePatientMonitoringConsent
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiIsAgreePatientMonitoringConsentKey) ? Convert.ToBoolean(Application.Current.Properties[ApiIsAgreePatientMonitoringConsentKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[ApiIsAgreePatientMonitoringConsentKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public bool ApiIsDoNotAgreePatientMonitoringConsent
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiIsDoNotAgreePatientMonitoringConsentKey) ? Convert.ToBoolean(Application.Current.Properties[ApiIsDoNotAgreePatientMonitoringConsentKey].ToString()) : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[ApiIsDoNotAgreePatientMonitoringConsentKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public UserIdentityModel GetUserIdentityModel()
        {
            UserIdentityModel model = new UserIdentityModel
            {
                UserId = this.ApiUserId,
                RoleId = this.ApiUserRole,
                UserNameDisplay = this.ApiUserNameDisplay,
                UserEmail = this.ApiUserEmail,
                PracticeProfileId = this.ApiPracticeProfileId,
                PatientProfileId = this.ApiPatientProfileId,
                IsSystemAdmin = this.ApiIsSystemAdmin,
                //IsAdmin = this.ApiIsAdmin,
                PracticeName = this.ApiPracticeName,
                PracticeLocationName = this.ApiPracticeLocationName,
                ProcedureName = this.ApiProcedureName,

                ProfessionalProfileId = this.ApiProfessionalProfileId,
                ProfessionalProfileName = this.ApiProfessionalProfileName
            };

            //if (this.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
            //    || this.ApiUserRole == RoleIdConstants.OperatingRoomNurse
            //    || this.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            //{
            //    model.PracticeName = model.PracticeLocationName;
            //}

            return model;
        }

        public void SetUserIdentityModel(UserIdentityModel model)
        {
            this.ApiUserId = model.UserId;
            this.ApiUserRole = model.RoleId;
            this.ApiUserNameDisplay = model.UserNameDisplay;
            this.ApiUserEmail = model.UserEmail;
            this.ApiPracticeProfileId = model.PracticeProfileId;
            this.ApiPatientProfileId = model.PatientProfileId;
            this.ApiIsSystemAdmin = model.IsSystemAdmin;
            //this.ApiIsAdmin = model.IsAdmin;
            this.ApiPracticeName = model.PracticeName;
            this.ApiPracticeLocationName = model.PracticeLocationName;
            this.ApiProcedureName = model.ProcedureName;

            this.ApiProfessionalProfileId = model.ProfessionalProfileId;
            this.ApiProfessionalProfileName = model.ProfessionalProfileName;

            //if (this.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
            //    || this.ApiUserRole == RoleIdConstants.OperatingRoomNurse
            //    || this.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            //{
            //    this.ApiPracticeName = this.ApiPracticeLocationName;
            //}
        }

        public ApplicationSettingModel GetApplicationSettingModel()
        {
            ApplicationSettingModel model = new ApplicationSettingModel
            {
                MobileNotificationIntervalMinute = this.ApiMobileNotificationIntervalMinute,
                MobileNotificationElapsedMinute = this.ApiMobileNotificationElapsedMinute
            };

            return model;
        }

        public void SetApplicationSettingModel(ApplicationSettingModel model)
        {
            this.ApiMobileNotificationIntervalMinute = model.MobileNotificationIntervalMinute;
            this.ApiMobileNotificationElapsedMinute = model.MobileNotificationElapsedMinute;
        }

        #endregion


        #region Version
        public bool IsUsingLatestVersion
        {
            get
            {
                return Application.Current.Properties.ContainsKey("IsUsingLatestVersion") ? (bool)Application.Current.Properties["IsUsingLatestVersion"] : false;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties["IsUsingLatestVersion"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string GooglePlayStoreAppUrl
        {
            get
            {
                return Application.Current.Properties.ContainsKey("GooglePlayStoreAppUrl") ? (string)Application.Current.Properties["GooglePlayStoreAppUrl"] : "";
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties["GooglePlayStoreAppUrl"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string AppStoreAppUrl
        {
            get
            {
                return Application.Current.Properties.ContainsKey("AppStoreAppUrl") ? (string)Application.Current.Properties["AppStoreAppUrl"] : "";
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties["AppStoreAppUrl"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string AppStoreAppName
        {
            get
            {
                return Application.Current.Properties.ContainsKey("AppStoreAppName") ? (string)Application.Current.Properties["AppStoreAppName"] : "";
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties["AppStoreAppName"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string AndroidVersionCode
        {
            get
            {
                return Application.Current.Properties.ContainsKey("AndroidVersionCode") ? (string)Application.Current.Properties["AndroidVersionCode"] : "1.0.0";
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties["AndroidVersionCode"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }
        #endregion

    }
}
