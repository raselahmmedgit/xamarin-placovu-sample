using Foundation;
using GlobalToast;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(OntrackHealthApp.iOS.iOSToast))]
namespace OntrackHealthApp.iOS
{
    public class iOSToast : IToast
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void SetNotificationSettings()
        {
            ITokenContainer tokenContainer = new TokenContainer();
            iOS.NotificationService.apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
            iOS.NotificationService.isUserLoggedIn = true;
        }

        public void SetSettingsForUserLogout()
        {
            iOS.NotificationService.isUserLoggedIn = false;
        }

        public void Show(string message)
        {
            //alertDelay = NSTimer.CreateScheduledTimer(SHORT_DELAY, (obj) =>
            //{
            //    dismissMessage();
            //});
            //alert = UIAlertController.Create(AppConstant.ToastMessageTitle, message, UIAlertControllerStyle.Alert);
            //UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            Toast.MakeToast(message).Show();
        }

        public void ShowNotification(string title, string message, string notificationId, string patientProcedureDetailId)
        {
            alertDelay = NSTimer.CreateScheduledTimer(SHORT_DELAY, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }


    }
}