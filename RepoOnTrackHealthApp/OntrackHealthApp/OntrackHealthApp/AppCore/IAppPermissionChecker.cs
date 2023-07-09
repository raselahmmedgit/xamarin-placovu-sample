using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public interface IAppPermissionChecker
    {
        void CheckMicrophonePermission();
        void CheakPowerSaverPermission();
    }
}
