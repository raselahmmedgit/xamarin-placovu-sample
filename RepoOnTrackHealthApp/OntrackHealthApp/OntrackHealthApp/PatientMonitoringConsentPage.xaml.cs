using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientMonitoringConsentPage : CustomContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        public ContentPage CallBackPage { get; set; }
        private readonly AdministrationPatientProfileRestApiService _restApiService;

        public PatientMonitoringConsentPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();

                _restApiService = new AdministrationPatientProfileRestApiService();

                var patientMonitoringConsentWebViewHtmlWebViewSource = new HtmlWebViewSource
                {
                    Html = PatientMonitoringConsentHtml
                };
                patientMonitoringConsentWebViewHtmlWebView.Source = patientMonitoringConsentWebViewHtmlWebViewSource;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private string PatientMonitoringConsentHtml
        {
            get
            {
                var str = "<html>"
                    + " <head><style type=\"text/css\">"
                    + " @font-face {"
                    + " font-family: georgia;"
                    + " src: url('Fonts/georgia.ttf');"
                    + " } body{font-family: georgia; font-size:16px;padding:0; margin:0;}"
                    + " * {box-sizing: border-box;-moz-box-sizing: border-box;-webkit-box-sizing: border-box;}"
                    + " ul { margin:0px; margin-bottom: 10px; padding:50px; margin-left:15px; display:block;} li{list-style: disc;margin-bottom: 10px; display:block;}"
                    + " ul li ul{ margin:0px; margin-bottom: 10px; padding:50px; margin-left:30px; display:block;}"
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
                    + " <div>"
                    + " <h2 style=\"font-size:18px\">Remote Patient Monitoring Consent</h2>"
                    + " </div>"
                    + " <div>"
                    + " <p>PURPOSE: The purpose of this form is to obtain your consent to participate in remote patient monitoring (RPM). This involves the use of electronic communication to enable you to share information with your health care provider. The information may be used for diagnosis, therapy, follow-up, and/or education.</p>"
                    + " <p>MEDICAL INFORMATION AND RECORDS: All existing laws regarding your access to medical information and copies of your medical records apply to this program.</p>"
                    + " <p>CONFIDENTIALITY: Reasonable and appropriate efforts have been made to eliminate any confidentiality risks associated with the use of this program. All existing confidentiality protections under federal and state laws apply to information collected in the use of this program.</p>"
                    + " <p>RIGHTS: You may withhold or withdraw consent to use of this program at any time, without affecting your right to future care or treatment, or risking the loss or withdrawal of any benefits to which you would otherwise be entitled.</p>"
                    + " <p>RISKS, CONSEQUENCES, AND BENEFITS: You agree that you have been advised of the potential risks, consequences, and benefits of this program. Potential risks include, but are not limited to:</p>"
                    + " </div>"
                    + " <div>"
                    + " <ul>"
                    + " <li>In rare cases, transmitted information may not be sufficient to allow for appropriate medical decision-making.</li>"
                    + " <li>Delays in medical decision-making could occur due to deficiencies or failures of the equipment.</li>"
                    + " <li>In very rare instances security protocols may fail, causing a privacy breach of personal medical information.</li>"
                    + " </ul>"
                    + " </div>"
                    + " <div>"
                    + " <p>I consent to participate in the OnTrack Health™ remote patient monitoring program.</p>"
                    + " </div>"
                    + " </body></html>";

                return str;
            }
        }

        private async void PatientMonitoringConsentAgreeButtonClicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }

            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    bool result = await _restApiService.UpdateMonitoringConsent(_iTokenContainer.ApiUserId, true);
                    if (result == true)
                    {
                        _iTokenContainer.ApiIsAgreePatientMonitoringConsent = true;
                        _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = false;
                        CloseModal();
                    }
                    else
                    {
                        _iTokenContainer.ApiIsAgreePatientMonitoringConsent = false;
                        _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = true;
                        await DisplayAlert(AppConstant.MonitoringConsentTitle, AppConstant.MonitoringConsentDisagreeMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void PatientMonitoringConsentCancelButtonClicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }

            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    bool result = await _restApiService.UpdateMonitoringConsent(_iTokenContainer.ApiUserId, false);
                    if (result == true)
                    {
                        _iTokenContainer.ApiIsAgreePatientMonitoringConsent = false;
                        _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = true;
                        CloseModal();
                    }
                    else
                    {
                        _iTokenContainer.ApiIsAgreePatientMonitoringConsent = false;
                        _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = true;
                        await DisplayAlert(AppConstant.MonitoringConsentTitle, AppConstant.MonitoringConsentDisagreeMessage, AppConstant.DisplayAlertErrorButtonText);
                        //Thread.CurrentThread.Abort();
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }

        }

        private void CloseModal()
        {
            App.Instance.ClearNavigationAndGoToMenuPage();
        }

    }
}