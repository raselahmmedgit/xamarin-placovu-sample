using System;
using System.Linq;
using System.Text.RegularExpressions;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using LabelHtml.Forms.Plugin.iOS;
using Newtonsoft.Json;
using ObjCRuntime;
using OntrackHealthApp.AppCore;
using Plugin.Toasts;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OntrackHealthApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

           
            global::Xamarin.Forms.Forms.Init();

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init();            

            Xamarin.FormsGoogleMaps.Init(AppConstant.GoogleServerKey);

            //ImageCircleRenderer.Init();
            ImageCircleRenderer.Init();

            HtmlLabelRenderer.Initialize();

            LoadApplication(new App());

            NotificationService.SetInitialSettings();
            //Pull Notification While App is in foreground
            //NotificationService.StartPullingNotification();

            //Pull Notification While app is in background
            //SetMinimumBackgroundFetchInterval();

            //after iOS 10
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter center = UNUserNotificationCenter.Current;

                center.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (bool arg1, NSError arg2) =>
                {

                    // remote notification registration
                    InvokeOnMainThread(() =>
                    {
                        var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
                        //local notification registration
                        UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

                        UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                        UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
                        UIApplication.SharedApplication.RegisterForRemoteNotifications();
                    });


                });

                center.Delegate = new NotificationDelegate();
            }

            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {

                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
                //local notification registration
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

                // remote notification registration
                InvokeOnMainThread(() =>
                {
                    UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                    UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                });

            }

            //UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            //UINavigationBar.Appearance.ShadowImage = new UIImage();
            //UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            //UINavigationBar.Appearance.TintColor = UIColor.White;
            //UINavigationBar.Appearance.BarTintColor = UIColor.Red;
            //UINavigationBar.Appearance.Translucent = true;

            var attribute = new UITextAttributes();
            attribute.TextColor = UIColor.Clear;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Normal);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Highlighted);

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {


            //// Get current device token
            //var DeviceToken = deviceToken.Description;
            //if (!string.IsNullOrWhiteSpace(DeviceToken))
            //{
            //    DeviceToken = DeviceToken.Trim('<').Trim('>');
            //    DeviceToken = Regex.Replace(DeviceToken, @"\s", "");
            //}

            byte[] bytes = deviceToken.ToArray<byte>();
            string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
            var DeviceToken = string.Join(string.Empty, hexArray);
            
            // Get previous device token
            var oldDeviceToken = Preferences.Get(AppConstant.PatientProfileDeviceTokenKey, null);

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                Preferences.Set(AppConstant.IsPatientProfileDeviceTokenChangedKey, true);
            }
            else
            {
                Preferences.Set(AppConstant.IsPatientProfileDeviceTokenChangedKey, false);
            }            
            // Save new device token
            Preferences.Set(AppConstant.PatientProfileDeviceTokenKey, DeviceToken);
            
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,Action<UIBackgroundFetchResult> completionHandler)
        {
            string key = AppConstant.RemoteNotificationDataKey;
            var data = (userInfo[new NSString(key)] as NSString).ToString();
            NotificationInfo notificationInfo = JsonConvert.DeserializeObject<NotificationInfo>(data);

            if (notificationInfo.NotificationId != 0 && notificationInfo.PatientProcedureDetailId != null)
            {
                //var currentApp = Xamarin.Forms.Application.Current;
                //currentApp.MainPage = new Xamarin.Forms.NavigationPage(new NotificationListPage(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId));
                App.Instance.MainPage = new MenuPage(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId);
                //((MasterDetailPage)Xamarin.Forms.Application.Current.MainPage).Detail.Navigation.PushAsync(new NotificationPageN(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId));
            }
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            var userInfo = notification.UserInfo;
            string key = AppConstant.LocalNotificationDataKey; ;
            var data = (userInfo[new NSString(key)] as NSString).ToString();
            NotificationInfo notificationInfo = JsonConvert.DeserializeObject<NotificationInfo>(data);

            if (notificationInfo.NotificationId != 0 && notificationInfo.PatientProcedureDetailId != null)
            {
                //var currentApp = Xamarin.Forms.Application.Current;
                //currentApp.MainPage = new Xamarin.Forms.NavigationPage(new NotificationListPage(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId));
                 App.Instance.MainPage = new MenuPage(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId);
                //((MasterDetailPage)Xamarin.Forms.Application.Current.MainPage).Detail.Navigation.PushAsync(new NotificationPageN(notificationInfo.NotificationId.ToString(), notificationInfo.PatientProcedureDetailId));
            }
        }
               
               

        private void SetMinimumBackgroundFetchInterval()
        {
            // Minimum number of seconds between a background refresh
            // 2 minutes = 2 * 60 = 120 seconds
            double MINIMUM_BACKGROUND_FETCH_INTERVAL = 120;
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);            
        }
                
        public override void PerformFetch(UIApplication application,Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                //NotificationService.GetLatestNotification();
                base.PerformFetch(application, completionHandler);
                
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate(UIApplication application)
        {
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    return UIInterfaceOrientationMask.Portrait;
                case TargetIdiom.Tablet:
                    return UIInterfaceOrientationMask.Portrait;
                default:
                    return UIInterfaceOrientationMask.Portrait;
            }
        }
    }
    
}
