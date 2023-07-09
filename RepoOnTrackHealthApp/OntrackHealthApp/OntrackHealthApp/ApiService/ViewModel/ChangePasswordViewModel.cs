﻿using OntrackHealthApp.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ChangePasswordViewModel : ApiModel
    {
        public string Email { get; set; }

        public string UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

    }
}
