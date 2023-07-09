using OntrackHealthApp.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class ResetPasswordViewModel : ApiModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public string UserId { get; set; }

        public string ReturnUrl { get; set; }

        public string TokenType { get; set; }

        public string OldPassword { get; set; }

        public bool IsResetPasswordFirst { get; set; }
    }
}
