using OntrackHealthApp.Droid;
using OntrackHealthApp.iOS;
using System;

namespace OntrackHealthApp.AppCore
{
    public class CrossLatestVersion
    {
        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

        static Lazy<ILatestVersion> _impl = new Lazy<ILatestVersion>(() => CreateLatestVersionImplementation(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        static ILatestVersion CreateLatestVersionImplementation()
        {
            #if NETSTANDARD2_0
            return null;
            #else
            return new LatestVersion();
            #endif
        }

        /// <summary>
        /// Checks if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => _impl.Value != null;

        /// <summary>
        /// Gets the current LatestVersion Plugin implementation.
        /// </summary>
        public static ILatestVersion Current
        {
            get
            {
                if (_impl.Value == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }

                return _impl.Value;
            }
        }
    }
}
