using OntrackHealthApp.ApiService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OntrackHealthApp.AppCore
{
    /// <summary>
    /// LatestVersion plugin
    /// </summary>
    public interface ILatestVersion
    {
        /// <summary>
        /// Gets the version number of the current app's installed version.
        /// </summary>
        /// <value>The current app's installed version number.</value>
        string InstalledVersionNumber { get; }

        /// <summary>
        /// InstalledVersionCode
        /// </summary>
        int InstalledVersionCode { get; }

        /// <summary>
        /// Opens the current app in the public store.
        /// </summary>
        Task OpenAppInStore();
    }
}
