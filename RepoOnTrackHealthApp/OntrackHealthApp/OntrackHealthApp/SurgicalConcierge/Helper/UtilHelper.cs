using Plugin.Toasts;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    public class UtilHelper
    {
        public static void ShowToastMessage(string message)
        {
            DependencyService.Get<IToast>().Show(message);
        }

        private async static void ShowPushNotification(string title, string description)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            var options = new NotificationOptions
            {
                Title = title,
                Description = description,
                IsClickable = true,
                AndroidOptions = new AndroidOptions
                {
                    ForceOpenAppOnNotificationTap = true
                }

            };
            INotificationResult tapped = await notificator.Notify(options);

        }
        public static void ShowLoader(StackLayout loaderView)
        {
            loaderView.IsVisible = true;
        }
        public static void HideLoader(StackLayout loaderView)
        {
            loaderView.IsVisible = false;
        }
    }
}
