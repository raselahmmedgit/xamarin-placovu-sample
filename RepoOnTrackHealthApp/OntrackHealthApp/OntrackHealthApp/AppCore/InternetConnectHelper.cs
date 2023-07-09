using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public static class InternetConnectHelper
    {
        public static bool CheckConnection()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CrossConnectivity.Dispose();
            }
        }

        public static bool DoIHaveInternet()
        {
            if (!CrossConnectivity.IsSupported)
                return true;

            //Do this only if you need to and aren't listening to any other events as they will not fire.
            var connectivity = CrossConnectivity.Current;

            try
            {
                return connectivity.IsConnected;
            }
            finally
            {
                CrossConnectivity.Dispose();
            }

        }
    }
}
