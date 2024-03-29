﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidToast))]
namespace OntrackHealthApp.Droid
{
    public class AndroidToast : IToast
    {
        public void SetNotificationSettings()
        {
            ITokenContainer tokenContainer = new TokenContainer();
            Droid.NotificationService.apiToken = tokenContainer.ApiToken != null ? tokenContainer.ApiToken.ToString() : "";
            Droid.NotificationService.isUserLoggedIn = true;
        }

        public void SetSettingsForUserLogout()
        {
            Droid.NotificationService.isUserLoggedIn = false;
        }

        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public void ShowNotification (string title, string message, string notificationId, string patientProcedureDetailId)
        {
            //await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new PhysicianPage());
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            Bundle bundle = new Bundle();
            bundle.PutString("notificationId", notificationId);
            bundle.PutString("patientProcedureDetailId", patientProcedureDetailId);
            intent.PutExtra("notification", bundle);
            //intent.SetComponent(new ComponentName(Android.App.Application.Context,typeof(MainActivity)));
            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):


            const int pendingIntentId = 10;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(((Activity)Forms.Context), pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            string CHANNEL_ID = "Ontrackheath_channel_01";// The id of the channel. 
            ICharSequence name = new Java.Lang.String("Ontrackheath");// The user-visible name of the channel.

            // Instantiate the builder and set notification elements, including pending intent:
            Notification.Builder builder = new Notification.Builder(((Activity)Forms.Context))
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetDefaults(NotificationDefaults.Sound)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.icon);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                builder.SetChannelId(CHANNEL_ID);
            }
            // Build the notification:
            Notification notification = builder.Build();


            // Get the notification manager:
            NotificationManager notificationManager = (NotificationManager)((Activity)Forms.Context).GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel mChannel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }
 
            // Publish the notification:
            const int notificationManagerNotifyId = 0;
            notificationManager.Notify(notificationManagerNotifyId, notification);
        }
    }
}