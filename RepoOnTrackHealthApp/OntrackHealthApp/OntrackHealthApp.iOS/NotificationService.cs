using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;

namespace OntrackHealthApp.iOS
{
    public class NotificationService
    {
        public static string apiToken = "";
        public static string baseUrl = "";
        public static long IntervaInlMinute = 2; //default 2 minute
        public static bool isUserLoggedIn = false;
        public static ApplicationApiService _applicationApiService;

        private static HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient();
            //httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            return httpClient;
        }

        public static void StartPullingNotification()
        {   
            NSTimer timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(IntervaInlMinute*60), delegate { GetLatestNotification(); });
            timer.Fire();
        }

        public static void SetInitialSettings()
        {
            baseUrl = AppConstant.BaseAddress;
            IntervaInlMinute = (long)App._notificationIntervalMinute;
            _applicationApiService = new ApplicationApiService();
            ITokenContainer tokenContainer = new TokenContainer();
            apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
        }

        public static async Task<bool> RegisterPushNotificationDeviceToken(long patientProfileId)
        {
            bool isRegistrationForPushNotificationDeviceTokenSucccess = false;
            try
            {
                var patientProfileDeviceToken = new PatientProfileDeviceToken
                {
                    CreatedOn = DateTime.Now,
                    PatientProfileId = patientProfileId,
                    DeviceToken = Preferences.Get(AppConstant.PatientProfileDeviceTokenKey, null),
                    IsExpired = false
                };                
                //ToastHelper.ShowToastMessage("IsPatientProfileDeviceTokenChanged: " + IsPatientProfileDeviceTokenChanged);
                if (patientProfileId > 0 && !string.IsNullOrEmpty(patientProfileDeviceToken.DeviceToken))
                {
                    isRegistrationForPushNotificationDeviceTokenSucccess = await _applicationApiService.RegisterPushNotificationDeviceToken(patientProfileDeviceToken);
                }
                return isRegistrationForPushNotificationDeviceTokenSucccess;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> MarkedPatientProfileDeviceTokenAsExpired(long patientProfileId)
        {
            bool isMarkedDeviceTokenAsExpiredSucccess = false;
            try
            {
                var patientProfileDeviceToken = new PatientProfileDeviceToken
                {
                    CreatedOn = DateTime.Now,
                    PatientProfileId = patientProfileId,
                    DeviceToken = Preferences.Get(AppConstant.PatientProfileDeviceTokenKey, null),
                    IsExpired = true
                };
                isMarkedDeviceTokenAsExpiredSucccess = await _applicationApiService.MarkedPatientProfileDeviceTokenAsExpired(patientProfileDeviceToken);
                return isMarkedDeviceTokenAsExpiredSucccess;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static void RegisterNotification(PatientEmailNotificationHistory notificationData)
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;

            //creat a UNMutableNotificationContent which contains your notification content
            UNMutableNotificationContent notificationContent = new UNMutableNotificationContent();

            notificationContent.Title = notificationData.MobileTemplateTitle;
            notificationContent.Body = notificationData.MobileTemplateDetail;

            notificationContent.Sound = UNNotificationSound.Default;

            NotificationInfo notificationInfo = new NotificationInfo
            {
                NotificationId = notificationData.NotificationId,
                PatientProcedureDetailId = notificationData.PatientProcedureDetailId.ToString()
            };

            notificationContent.UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(JsonConvert.SerializeObject(notificationInfo)), new NSString(AppConstant.RemoteNotificationDataKey));

            UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(10,false);

            UNNotificationRequest request = UNNotificationRequest.FromIdentifier("FiveSecond", notificationContent, trigger);


            center.AddNotificationRequest(request, (NSError obj) =>
            {



            });

        }

        static void BuildBannerNotification(PatientEmailNotificationHistory notificationData)
        {
            var content = new UNMutableNotificationContent();
            content.Title = notificationData.MobileTemplateTitle;
            content.Subtitle = notificationData.MobileTemplateDetail;
            //content.Body = "This is the message body of the notification.";
            content.Badge = 1;

            NotificationInfo notificationInfo = new NotificationInfo
            {
                NotificationId = notificationData.NotificationId,
                PatientProcedureDetailId = notificationData.PatientProcedureDetailId.ToString()
            };

            content.UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(JsonConvert.SerializeObject(notificationInfo)), new NSString(AppConstant.LocalNotificationDataKey));

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(10, false);

            var requestID = "sampleRequest-"+notificationData.NotificationId;
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
                if (err != null)
                {
                    Console.WriteLine("Error while generating banner...");
                }
            });
            Console.WriteLine("banner created...");
        }

        public async static Task<HttpResponseMessage> GetFormEncodedContent(string apiTokenKey, string baseUrl)
        {
            try
            {
                string restUrl = "api/PatientProfile/CurrentNotification";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }

                //Android.Widget.Toast.MakeText(Android.App.Application.Context,"success: "+response.StatusCode+" "+apiTokenKey, ToastLength.Short).Show();
            }
            catch (Exception)
            {}

            return null;
        }

        public async static void GetLatestNotification()
        {
            if (!isUserLoggedIn)
                return;

            PatientEmailNotificationHistory patientEmailNotificationHistory = null;
            HttpResponseMessage response = await GetFormEncodedContent(apiToken, baseUrl);

            if (response == null)
            {
                return;
            }

            var result = await response.Content.ReadAsStringAsync();
            if (result != null)
            {
                patientEmailNotificationHistory = JsonConvert.DeserializeObject<PatientEmailNotificationHistory>(result);
            }
            if (patientEmailNotificationHistory == null || string.IsNullOrEmpty(patientEmailNotificationHistory.MobileTemplateTitle) || string.IsNullOrEmpty(patientEmailNotificationHistory.MobileTemplateDetail))
            {
                return;
            }

            //BuildBannerNotification(patientEmailNotificationHistory);
            RegisterNotification(patientEmailNotificationHistory);

        }

    }

    partial class NotificationInfo
    {
        public long NotificationId { get; set; }
        public string PatientProcedureDetailId { get; set; }
    }

    partial class PatientEmailNotificationHistory
    {
        public long PatientEmailNotificationHistoryId { get; set; }

        public long PatientNotificationDetailId { get; set; }

        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public string PatientProfileName { get; set; }

        public string PracticeProfileName { get; set; }

        public string ProfessionalProfileName { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public long NotificationId { get; set; }

        public string NotificationTitle { get; set; }

        public DateTime? NotificationDate { get; set; }

        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public Guid? PatientEmailNotificationHistoryKey { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string SmsTemplateId { get; set; }

        public string SmsTemplateTypeId { get; set; }

        public string SmsTemplateTitle { get; set; }

        public string SmsTemplateDetail { get; set; }

        public string MobileTemplateTitle { get; set; }

        public string MobileTemplateDetail { get; set; }

    }
}