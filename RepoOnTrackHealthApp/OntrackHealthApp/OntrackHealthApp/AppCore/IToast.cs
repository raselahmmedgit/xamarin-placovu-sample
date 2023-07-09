using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public interface IToast
    {
        void Show(string message);
        void ShowNotification(string title, string message, string notificationId, string patientProcedureDetailId);
        void SetNotificationSettings();
        void SetSettingsForUserLogout();
    }
}
