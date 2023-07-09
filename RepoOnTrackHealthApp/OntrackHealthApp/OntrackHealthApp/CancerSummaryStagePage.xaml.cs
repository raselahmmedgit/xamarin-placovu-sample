using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CancerSummaryStagePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }


        public CancerSummaryStagePage(PatientProstateCancerSummary patientProstateCancerSummary)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            PatientProstateCancerSummary = patientProstateCancerSummary;

        }

        private void BindForm()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                string str = "<p>To understand the stages of prostate cancer, it’s important to understand the anatomy involved. The specimens removed during surgery include the prostate, seminal vesicles and in most cases the pelvic lymph nodes.</p>";
                str = "<div style='font-size:22px'>" + str + "</div>";
                LblCancerSummeryStageOne.Text = str;
                str = "<p>Staging is a way of describing where the cancer is located, if or where it has spread, and whether it is affecting other parts of the body.</p>"
                        + "<p>There are 2 types of staging for prostate cancer:</p>"
                        + "<ul>"
                        + "<li>Clinical staging. This is based on the results of rectal exam, PSA testing, and volume of cancer. This stage is used before treatment has begun.</li>"
                        + "<li>Pathologic staging. This is based on information found during surgery, plus the pathology, of the prostate tissue removed during surgery. This stage will be highlighted below.</li>"
                        + "</ul>"
                        + "<p>The most common staging classification is called TNM</p>"
                        + "<ul>"
                        + "<li><strong>T</strong> describes how much of the prostate is involved and whether the cancer has grown into nearby tissue. (You will see pT, meaning pathologic stage.)</li>"
                        + "<li><strong>N</strong> describes if there is lymph node involvement.</li>"
                        + "<li><strong>M</strong> describes if there is metastasis (the spread of the cancer to other parts of the body, beyond the lymph nodes).</li>"
                        + "</ul>";
                str = "<div style='font-size:22px;padding-left:10px'>" + str + "</div>";
                // CancerSummeryStageTwoWebView.Source = new HtmlWebViewSource { Html = str };
                LblCancerSummeryStageTwo.Text = str;
                string strResult = "";
                if (PatientProstateCancerSummary != null)
                {
                    strResult = "<p style='font-size:22; text-align:center'><span><strong>Your Stage is</strong></span>"
                           + "<span style=\"padding-left:10px\"> <strong>" + (PatientProstateCancerSummary.StageScore != null ? PatientProstateCancerSummary.StageScore : "N/A") + "</strong> </span>"
                           + "<span style=\"padding-left:10px\"> <strong>" + (PatientProstateCancerSummary.NodeStatus != null ? PatientProstateCancerSummary.NodeStatusN : "N/A") + "</strong> </span>"
                           + "<span style=\"padding-left:10px\"> <strong>" + (PatientProstateCancerSummary.MetastasisN != null ? PatientProstateCancerSummary.MetastasisN : "N/A") + "</strong> </span></p>";
                }
                else
                {
                    strResult = "<p style='font-size:22; text-align:center'><span><strong>Your Stage is</strong></span>"
                           + "<span style=\"padding-left:10px\"> <strong>" +  "N/A" + "</strong> </span>";
                }

                LblCancerSummeryStageTen.Text = strResult;
                string strElaven = "<strong>T</strong> describes the extent of cancer in the prostate and surrounding tissue. T stage is broken down into 3 categories T2 (a,b,c), T3 (a,b) and T4"
                                + "<ul><li><strong>T2</strong> cancers are contained within the prostate.</li></ul>";
                LblCancerSummeryStageElaven.Text = strElaven;

                string strT3 = "<ul><li>" +
                    "<strong>T3</strong> cancers extend thru the prostate capsule (T3a) or into the seminal vesicles (T3b)" +
                    "<ul><li>" +
                    "<p><strong>T3a: </strong>The cancer extends into the tissue around the prostate.</p>" +
                    "</li></ul>";

                LblCancerSummeryStageThree.Text = strT3;

                string strT3b = "<ul style=\"list-style-type: none\" ><li><ul><li>" +
                    "<strong>T3b: </strong>The cancer extends into the seminal vesicles." +
                    "</li></ul></li></ul>";
                LblCancerSummeryStageThreeB.Text = strT3b;

                string strT4 = "<ul><li><p><strong>T4</strong> cancer extends into structures around the prostate, like the bladder and rectum.</p></li></ul>";

                LblCancerSummeryStageFour.Text = strT4;

                string strN = "<p><strong>N</strong> describes if cancer has spread to the lymph nodes.</p><ul><li><p><strong>N0: </strong> No lymph node involvement</p></li><li><p><strong>N1: </strong> Lymph node involvement</p></li></ul>";
                LblCancerSummeryStageEight.Text = strN;

                string strM = "<p><strong>M</strong> delineates if there is metastatic disease</p><ul><li><p><strong>M0: </strong> No metastasis</p></li><li><p><strong>M1: </strong> Metastasis</p></li></ul><p>An example of prostate cancer staging would be “pT2N0M0” This means the cancer is contained within the prostate and has not spread outside the prostate.​</p>";

                LblCancerSummeryStageNine.Text = strM;
            }

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindForm();
        }
    }
}