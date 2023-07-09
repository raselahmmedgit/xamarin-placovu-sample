using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalPacuFreeFormCommentPage : CustomModalContentPage
    {

        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergePacuPageViewModel _surgicalConciergePacuPageViewModel;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }

        public ProfessionalPacuFreeFormCommentPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            lblPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            FreeFormComment.Text = "";
            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;
            _surgicalConciergePacuPageViewModel = new SurgicalConciergePacuPageViewModel();
        }

        private async void ModalCloseButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddFreeFormCommentButton_ClickedAsync(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {

                if (FreeFormComment.Text == "")
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
                else
                {
                    SurgicalConciergePacuCommentParamViewModel paramModel = new SurgicalConciergePacuCommentParamViewModel();
                    paramModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
                    paramModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    paramModel.ProcedureId = _surgicalConciergePatientViewModel.ProcedureId.ToLong();

                    string scgPacuCommentText = FreeFormComment.Text;
                    paramModel.ScgPacuCommentText = scgPacuCommentText;

                    var result = await _surgicalConciergePacuPageViewModel.ScgPacuAdditionalCommentSendAsync(paramModel);
                    if (result)
                    {
                        //await DisplayAlert(AppConstant.ToastMessageTitle, AppConstant.PacuSuccessMessage, AppConstant.DisplayAlertErrorButtonText);
                        UtilHelper.ShowToastMessage(AppConstant.PacuSuccessMessage);

                        var submitButton = sender as Button;

                        submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                        submitButton.Text = "Delivered";
                        submitButton.IsEnabled = false;

                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.PacuErrorMessage);
                    }
                }
            }

        }
    }
}