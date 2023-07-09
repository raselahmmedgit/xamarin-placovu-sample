using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class SchedulePageResponse : ApiResponse<PatientScheduleHomePageViewModel>
    {
    }

    public class PatientSchedulesUpToDateApiResponse : ApiListResponse<List<PatientNotificationShortViewFromApi>>
    {
    }

    public class SchedulePageNResponse : ApiResponse<PatientSurveyPatientNotificationDetailViewModel>
    {
    }

    public class NotificationDetailPageResponse : ApiResponse<PSProcedureNotificationDetail>
    {
    }

    public class ScheduleDetailWithSurveyPageResponse : ApiResponse<PatientSurveyPatientNotificationDetailViewModel>
    {
    }
}
