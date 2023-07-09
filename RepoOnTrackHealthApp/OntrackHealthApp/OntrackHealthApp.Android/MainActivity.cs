using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.Toasts;
using Android.Content;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using OntrackHealthApp.Droid.AndroidHelper;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;


namespace OntrackHealthApp.Droid
{
    [Activity(Label = "OnTrackHealth", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);

            //Map
            global::Xamarin.FormsGoogleMaps.Init(this, bundle);


            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this);

            UserDialogs.Init(this);
            //SurgicalConcierge.AndroidHelper.PermissionCheck(this);
            LoadApplication(new App());

            Intent intent = this.Intent;
            Bundle notificationBundle = intent.GetBundleExtra("notification");

            // only show when app first start
            //if (notificationBundle == null)
            //{
            //    Util.StartPowerSaverIntent(this);
            //}

            //start service to keep app run always
            Droid.NotificationService.baseUrl = AppConstant.BaseAddress;
            Droid.NotificationService.IntervaInlMinute = (long)App._notificationIntervalMinute;
            ITokenContainer tokenContainer = new TokenContainer();
            Droid.NotificationService.apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
            Intent serviceIntent = new Intent(this, typeof(BackgroudService));

            StartService(serviceIntent);

            if (tokenContainer.ApiToken != null)
            {
                // open notification schecule through notifiaiton
                if (notificationBundle != null)
                {
                    string notificationId = notificationBundle.GetString("notificationId");

                    string patientProcedureDetailId = notificationBundle.GetString("patientProcedureDetailId");

                    if (notificationId != null && patientProcedureDetailId != null)
                    {
                        Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NotificationListPage(notificationId, patientProcedureDetailId));
                    }
                }
            }
                
        }


        protected override void OnStart()
        {
            base.OnStart();

            string permission = Manifest.Permission.AccessFineLocation;

            if (ContextCompat.CheckSelfPermission(this, permission) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permission Granted!!!");
            }
        }
    }
}

