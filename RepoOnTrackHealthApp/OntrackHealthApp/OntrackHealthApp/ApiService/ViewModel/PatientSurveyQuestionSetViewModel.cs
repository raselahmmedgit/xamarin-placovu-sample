using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientSurveyQuestionSetViewModel
    {
        public PatientSurveyQuestionSetViewModel()
        {
            this.PatientSurveyQuestionViewModels = new List<PatientSurveyQuestionViewModel>();
        }

        public long? PatientProfileId { get; set; }

        public long NotificationId { get; set; }

        public long PatientNotificationDetailId { get; set; }

        public long? PracticeProfileId { get; set; }

        public long? ProcedureId { get; set; }

        public long? ProfessionalProfileId { get; set; }

        public long? SurveyQuestionSetId { get; set; }

        public string SurveyQuestionSetName { get; set; }

        public string SurveyQuestionSetNamePatient { get; set; }

        public string SurveyQuestionSetHeader { get; set; }

        public int? SetDisplayOrder { get; set; }

        public bool IsLastQuestionSet { get; set; }

        public bool IsLastNotification { get; set; }

        public bool HasQuestions { get; set; }

        public int NotificationOrder { set; get; }

        public Guid? PatientProcedureDetailId { get; set; }

        public List<PatientSurveyQuestionViewModel> PatientSurveyQuestionViewModels { get; set; }

        public string SurveyQuestionSetHeaderCustom
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
                    + SurveyQuestionSetHeader
                    + " </body></html>";
                str = str.Replace("<br/>", "");
                str = str.Replace("<br />", "");
                return str.Replace("<p>&nbsp;</p>", "");
            }
        }

    }
}
