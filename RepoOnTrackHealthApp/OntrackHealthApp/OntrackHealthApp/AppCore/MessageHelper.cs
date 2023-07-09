using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public enum AppMessageType
    {
        None = 1,
        Success = 2,
        Error = 3,
        Information = 4,
        Warning = 5,
        LoginRequired = 6
    }

    public class AppMessage
    {
        public AppMessage()
        {
            MessageType = AppMessageType.None;
        }
        public AppMessageType MessageType { get; set; }
        public string Message { get; set; }
        public int State { get; set; }

    }

    public static class SetAppMessage
    {
        public static AppMessage SetSuccessMessage(string message = "Success !")
        {
            return new AppMessage { MessageType = AppMessageType.Success, Message = message, State = 0 };
        }
        public static AppMessage SetErrorMessage(string message = "Error !")
        {
            return new AppMessage { MessageType = AppMessageType.Error, Message = message, State = 0 };
        }
        public static AppMessage SetInformationMessage(string message = "Information !")
        {
            return new AppMessage { MessageType = AppMessageType.Information, Message = message, State = 0 };
        }
        public static AppMessage SetWarningMessage(string message = "Warning !")
        {
            return new AppMessage { MessageType = AppMessageType.Warning, Message = message, State = 0 };
        }
        public static AppMessage SetLoginRequiredMessage(string message = "Login Required !")
        {
            return new AppMessage { MessageType = AppMessageType.LoginRequired, Message = message, State = 0 };
        }

    }
}
