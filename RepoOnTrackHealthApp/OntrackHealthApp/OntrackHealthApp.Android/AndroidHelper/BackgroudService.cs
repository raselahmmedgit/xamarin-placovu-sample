using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using Xamarin.Forms;

namespace OntrackHealthApp.Droid.AndroidHelper
{
    [Service]
    public class BackgroudService : Service
    {
        NotificationService alarm = new NotificationService();
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            //PullNotificationService pullNotificationService = new PullNotificationService();
            //pullNotificationService.StartTimer();
            //private Stopwatch _stopWatch = new Stopwatch(); 
            //TokenContainer tokenContainer = new TokenContainer();
            //string apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
            alarm.SetAlarm(Android.App.Application.Context, intent);

            return StartCommandResult.Sticky;
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            try
            {
                ITokenContainer tokenContainer = new TokenContainer();
                Droid.NotificationService.apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
                Droid.NotificationService.baseUrl = AppConstant.BaseAddress;
                Droid.NotificationService.IntervaInlMinute = (long)App._notificationIntervalMinute;

                Intent intent = new Intent(this, typeof(BackgroudService));

                StartService(intent);

                base.OnTaskRemoved(rootIntent);
            }
            catch (Exception)
            {

            }
        }
    }
}