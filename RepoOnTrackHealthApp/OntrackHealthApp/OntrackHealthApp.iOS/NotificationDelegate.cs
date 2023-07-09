using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Foundation;
using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace OntrackHealthApp.iOS
{
    public class NotificationDelegate : UNUserNotificationCenterDelegate
    {
        public NotificationDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            //completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
            completionHandler(UNNotificationPresentationOptions.Alert);
            Console.WriteLine("completionHandler");
            //DisplayNotification(center, notification);
        }

        private void DisplayNotification(UNUserNotificationCenter center, UNNotification notificationResponse)
        {
            UNMutableNotificationContent notificationContent = new UNMutableNotificationContent();

            notificationContent.Title = notificationResponse.Request.Content.Title;
            notificationContent.Body = notificationResponse.Request.Content.Body;
            notificationContent.Sound = UNNotificationSound.Default;

            string remoteNotificationKey = AppConstant.RemoteNotificationDataKey;
            string notificationKey = AppConstant.notificationIdKey;
            string patientProcedureDetailIdKey = AppConstant.patientNotificationDetailIdKey;

            var userInfo = notificationResponse.Request.Content.UserInfo;
            if (userInfo != null && userInfo.ContainsKey(new NSString(remoteNotificationKey)))
            {
                NSDictionary notification = userInfo.ObjectForKey(new NSString(remoteNotificationKey)) as NSDictionary;
                if (notification != null)
                {
                    var notificationId = int.Parse(notification.ValueForKey(new NSString(notificationKey)).ToString());
                    var patientProcedureDetailId = notification.ValueForKey(new NSString(patientProcedureDetailIdKey)).ToString();

                    NotificationInfo notificationInfo = new NotificationInfo
                    {
                        NotificationId = notificationId,
                        PatientProcedureDetailId = patientProcedureDetailId
                    };

                    notificationContent.UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(JsonConvert.SerializeObject(notificationInfo)), new NSString(remoteNotificationKey));

                    UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

                    UNNotificationRequest request = UNNotificationRequest.FromIdentifier("FiveSecond", notificationContent, trigger);

                    center.AddNotificationRequest(request, (NSError obj) =>
                    {

                    });
                }


            }
        }


        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            try
            {
                
                var userInfo = response.Notification.Request.Content.UserInfo;
                string remoteNotificationKey = AppConstant.RemoteNotificationDataKey;
                string notificationKey = AppConstant.notificationIdKey;
                string patientProcedureDetailIdKey = AppConstant.patientNotificationDetailIdKey;
                if (userInfo != null && userInfo.ContainsKey(new NSString(remoteNotificationKey)))
                {
                    NSDictionary notification = userInfo.ObjectForKey(new NSString(remoteNotificationKey)) as NSDictionary;
                    if (notification != null)
                    {
                        var notificationId = int.Parse(notification.ValueForKey(new NSString(notificationKey)).ToString());
                        var patientProcedureDetailId = notification.ValueForKey(new NSString(patientProcedureDetailIdKey)).ToString();

                        if (notificationId != 0 && patientProcedureDetailId != null)
                        {
                            TokenContainer _iTokenContainer = new TokenContainer();
                            if (_iTokenContainer.IsApiToken() && !_iTokenContainer.ApiIsAdmin)
                            {
                                App.ShowUserDialogAsync();
                                ((MasterDetailPage)Xamarin.Forms.Application.Current.MainPage).Detail.Navigation.PushAsync(new NotificationPageN(notificationId.ToString(), patientProcedureDetailId));
                                //App.Instance.MainPage = new MenuPage(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId);
                            }
                            else
                            {
                                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                            }

                        }
                    }
                }
                completionHandler();
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
                completionHandler();
                ToastHelper.ShowToastMessage(AppConstant.DisplayAlertErrorMessage);
            }
        }
    }
}