using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.AppCore
{
    public static class ToastHelper
    {
        public static void ShowToastMessage(string message)
        {
            DependencyService.Get<IToast>().Show(message);
        }

        public static void ShowPushNotification(string title, string description, string notificationId, string patientProcedureDetailId)
        {
            DependencyService.Get<IToast>().ShowNotification(title, description, notificationId, patientProcedureDetailId);

            //var iToastNotificator = DependencyService.Get<IToastNotificator>();
            //IDictionary<string, string> customArgs = new Dictionary<string, string>();
            //var notificationOptions = new NotificationOptions
            //{

            //    Title = title,
            //    Description = description,
            //    IsClickable = true,
            //    CustomArgs = customArgs,
            //    AndroidOptions = new AndroidOptions
            //    {
            //        ForceOpenAppOnNotificationTap = true,

            //    }

            //};

            //INotificationResult iNotificationResult = await iToastNotificator.Notify(notificationOptions);
        }

    }
}
