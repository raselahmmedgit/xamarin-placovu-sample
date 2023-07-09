using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class PatientEmailNotificationHistoryResponse : ApiResponse<PatientEmailNotificationHistoryViewModel>
    {
    }

    public class PatientEmailNotificationHistoryListResponse : ApiListResponse<List<PatientEmailNotificationHistoryViewModel>>
    {
    }
}
