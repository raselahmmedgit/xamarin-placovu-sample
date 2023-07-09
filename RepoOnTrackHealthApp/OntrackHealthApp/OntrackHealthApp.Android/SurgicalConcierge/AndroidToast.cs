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
using OntrackHealthApp.Droid.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidToast))]
namespace OntrackHealthApp.Droid.SurgicalConcierge
{
    public class AndroidToast : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}