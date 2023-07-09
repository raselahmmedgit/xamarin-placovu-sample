using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using Xamarin.Forms;

namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class AndroidHelper
    {
        public static void PermissionCheck()
        {
            if ((int)Build.VERSION.SdkInt >= 21)
            {
                if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.RecordAudio) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.RecordAudio }, 1001);
                }
            }
        }

        public static bool IsSpeechRecordingSupported()
        {
            //bool hasRecordingPermission = ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.RecordAudio) != Permission.Granted;
            //return hasRecordingPermission;
            //return SpeechRecognizer.IsRecognitionAvailable((Activity)Forms.Context);
            var appContext =  (Activity)Forms.Context;
            return IsAppInstalled(appContext,AppConstants.GoogleHomePackageName);
        }
        public static bool IsAppInstalled(Context context, string packageName)
        {
            try
            {
                context.PackageManager.GetApplicationInfo(packageName, 0);
                return true;
            }
            catch (PackageManager.NameNotFoundException e)
            {
                return false;
            }
        }

        public static void OpenPlayStoreForGoogleHome()
        {
            var context = (Activity)Forms.Context;
            try
            {                
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + AppConstants.GoogleHomePackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + AppConstants.GoogleHomePackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }
        }

    }
}