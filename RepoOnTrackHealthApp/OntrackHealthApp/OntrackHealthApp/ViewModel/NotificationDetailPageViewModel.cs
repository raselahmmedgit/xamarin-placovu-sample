using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using OntrackHealthApp.Model;
using OntrackHealthApp.ApiService.ViewModel;
using System.Threading.Tasks;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.AppCore;

namespace OntrackHealthApp.ViewModel
{

    public class NotificationDetailPageViewModel : ViewModelBase
    {
        #region Property

        string __notificationTitle = "";
        string __notificationDetailHeader = "";
        string __notificationDetail = "";
        string __notificationDetailCustom = "";

        public string NotificationTitle
        {
            set { SetProperty(ref __notificationTitle, value); }
            get
            {
                return __notificationTitle;
            }
        }
        public string NotificationDetailHeader
        {
            set { SetProperty(ref __notificationDetailHeader, value); }
            get
            {
                return __notificationDetailHeader;
            }
        }
        public string NotificationDetail
        {
            set { SetProperty(ref __notificationDetail, value); }
            get
            {
                return __notificationDetail;
            }
        }
        public string NotificationDetailCustom
        {
            set
            {
                var str = "<html>"
                    + " <head>"
                    + " <meta charset=\"utf-8\" />"
                    + " <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">"
                    + " <style type=\"text/css\">"
                    + " @font-face {"
                    + " font-family: georgia;"
                    + " src: url('Fonts/georgia.ttf');"
                    + " } body{font-family: georgia; font-size:16px;padding:0; margin:0; width: 100%; height: 100%;}"
                    + " * {box-sizing: border-box;-moz-box-sizing: border-box;-webkit-box-sizing: border-box;}"
                    //+ " ul { margin:0px; margin-bottom: 10px; padding:50px; margin-left:15px; display:block;} li{list-style: disc;margin-bottom: 10px; display:block;}"
                    //+ " ul li ul{ margin:0px; margin-bottom: 10px; padding:50px; margin-left:30px; display:block;}"
                    + " h2{margin:0px; padding:0px; font-size:18px;margin-bottom:15px;}"
                    + " h3{margin:0px; padding:0px; font-size:17px;margin-bottom:15px;}"
                    + " h4{margin:0px; padding:0px; font-size:16px;margin-bottom:15px;}"
                    + " .row {margin-right:0px; margin-left:0px;} .row:after{clear: both;} .col-md-12{float: left; width: 100%; position: relative; min-height: 1px;padding-right: 15px;padding-left: 15px}"
                    + " table{border-collapse: collapse;}"
                    + " th{border-bottom:1px solid #777;border-top:1px solid #777;vertical-align: middle;padding-top:6px; padding-bottom:6px;fornt-weight:bold;}"
                    + " th:first-child{padding-right:4px} th:last-child{padding-left:4px}"
                    + " td{border-bottom:1px solid #777;vertical-align: middle;padding-top:6px; padding-bottom:6px;}"
                    + " td:first-child{padding-right:4px} td:last-child{padding-left:4px}"
                    + " .img-responsive{display:block; max-width: 100%; height: auto; vertical-align: middle; border: 0px;}"
                    + " </style></head><body>"
                    + NotificationDetail
                    + " </body></html>";
                str = str.Replace("<br/>", "");
                str = str.Replace("<br />", "");
                str = str.Replace("<p>&nbsp;</p>", "");
                SetProperty(ref __notificationDetailCustom, str);
            }
            get
            {
                return __notificationDetailCustom;
            }
        }

        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;
        public PSProcedureNotificationDetail PSProcedureNotificationDetail { get; set; }
        public long NotificationDetailId { get; set; }
        public Command LoadPatientNotificationDetailCommand { get; set; }

        #endregion

        public NotificationDetailPageViewModel()
        {
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
            PSProcedureNotificationDetail = new PSProcedureNotificationDetail();
            LoadPatientNotificationDetailCommand = new Command(async () => { await ExecuteLoadPatientNotificationDetailCommandAsync(); });
        }

        public async Task ExecuteLoadPatientNotificationDetailCommandAsync()
        {
            if (IsBusy) { return; }
            App.ShowUserDialogAsync();
            try
            {
                IsBusy = true;

                var response = await _iScheduleClient.GetScheduleDetailContent(NotificationDetailId);

                if (response.StatusIsSuccessful)
                {
                    PSProcedureNotificationDetail = response.Data;
                    NotificationDetailHeader = PSProcedureNotificationDetail.NotificationDetailHeader;
                    NotificationDetail = PSProcedureNotificationDetail.NotificationDetail;
                    NotificationDetailCustom = PSProcedureNotificationDetail.NotificationDetail;
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }

            }
            catch (Exception)
            {
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
                App.HideUserDialogAsync();
            }
            finally
            {
                IsBusy = false;
                App.HideUserDialogAsync();
            }
        }
    }
}
