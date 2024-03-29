﻿using System;

namespace OntrackHealthApp.AppCore
{
    internal class LatestVersionException : Exception
    {
        public LatestVersionException(string message)
            : base(message)
        {
        }

        public LatestVersionException(Exception innerException)
            : base("", innerException)
        {
        }

        public LatestVersionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
