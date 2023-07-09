using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Droid.AndroidHelper;
using OntrackHealthApp.Droid.SurgicalConcierge;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppPermissionChecker))]
namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class AppPermissionChecker : IAppPermissionChecker
    {
        public void CheakPowerSaverPermission()
        {
            AndroidHelper.PermissionCheck();
        }

        public void CheckMicrophonePermission()
        {
            Util.StartPowerSaverIntent();
        }
    }
}