using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class UserPracticeResponse : ApiResponse<UserPracticeViewModel>
    {
    }

    public class UserPracticeListResponse : ApiListResponse<List<UserPracticeViewModel>>
    {
    }
}
