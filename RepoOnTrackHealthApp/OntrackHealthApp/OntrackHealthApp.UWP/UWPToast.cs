using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OntrackHealthApp.UWP
{
    public class UWPToast : IToast
    {
        public void SetNotificationSettings()
        {
            throw new NotImplementedException();
        }

        public void SetSettingsForUserLogout()
        {
            throw new NotImplementedException();
        }

        public async void Show(string message)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = AppConstant.ToastMessageTitle,
                Content = message,
                CloseButtonText = AppConstant.ToastMessageButtonText
            };

            ContentDialogResult contentDialogResult = await contentDialog.ShowAsync();
        }

        public void ShowNotification(string title, string message, string notificationId, string patientProcedureDetailId)
        {
            throw new NotImplementedException();
        }
    }
}
