
using Foundation;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(OntrackHealthApp.iOS.LatestVersion))]
namespace OntrackHealthApp.iOS
{
    /// <summary>
    /// <see cref="ILatestVersion"/> Implementation for Android.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LatestVersion : ILatestVersion
    {
        public static ITokenContainer _iTokenContainer;
        //private readonly IReleaseHistoryClient _iReleaseHistoryClient;

        string _bundleName => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName").ToString();
        string _bundleIdentifier => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleIdentifier").ToString();
        string _bundleVersion => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        string _bundleVersionCode => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();

        public LatestVersion() {
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            //_iReleaseHistoryClient = new ReleaseHistoryClient(apiClient);
        }

        /// <inheritdoc />
        public string InstalledVersionNumber
        {
            get => _bundleVersionCode;
        }

        public int InstalledVersionCode
        {
            get => _bundleVersion.ToInt();
        }

        public Task OpenAppInStore()
        {
            string appName = _iTokenContainer.AppStoreAppName;
            return OpenAppInStore(appName);
        }

        /// <inheritdoc />
        public Task OpenAppInStore(string appName)
        {
            string url = _iTokenContainer.AppStoreAppUrl;

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            try
            {
                appName = appName.MakeSafeForAppStoreShortLinkUrl();

                #if __IOS__
                UIKit.UIApplication.SharedApplication.OpenUrl(new NSUrl($"http://appstore.com/{appName}"));
                #elif __MACOS__
                AppKit.NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl($"http://appstore.com/mac/{appName}"));
                #endif

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                throw e;
                //throw new LatestVersionException($"Unable to open {appName} in App Store.", e);
            }
        }

    }
}