using Plugin.InputKit.Shared.Controls;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.AppCore;
using Acr.UserDialogs;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalCommentView : CustomModalContentPage
    {
        SurgicalConciergeStageCommentViewModel _surgicalConciergeStageCommentViewModel;
        string selectedCommentTextPrev = "";
        int selectedCommentIdPrev = -1;

        public SurgicalCommentView(SurgicalConciergeStageCommentViewModel surgicalConciergeStageCommentViewModel)
        {
            InitializeComponent();
            BindingContext = surgicalConciergeStageCommentViewModel;
            this._surgicalConciergeStageCommentViewModel = surgicalConciergeStageCommentViewModel;
            LoadSurgicalConciergeStageCommentRadioValue(surgicalConciergeStageCommentViewModel);

        }

        private async void ModalCloseButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();

        }

        private async void PostSurgicalComment(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading())
            {
                try
                {
                    var button = sender as Button;
                    var surgicalConciergeStageCommentViewModel = this.BindingContext as SurgicalConciergeStageCommentViewModel;
                    var commentText = AdditionalComment.Text?.Trim();
                    //UtilHelper.ShowLoader(loaderView);
                    ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                    apiExecutionResult = await restApiService.PostSurgicalConciergeStageComment(surgicalConciergeStageCommentViewModel.ScgStageProfessionalProcedureId, surgicalConciergeStageCommentViewModel.PatientProfileId, surgicalConciergeStageCommentViewModel.PatientProcedureDetailId.ToString(), commentText);
                    //UtilHelper.HideLoader(loaderView);
                    if (apiExecutionResult.Success)
                    {
                        UtilHelper.ShowToastMessage("Comment posted sucessfully");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Comment post failed");
                    }

                    await Navigation.PopModalAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private void LoadSurgicalConciergeStageCommentRadioValue(SurgicalConciergeStageCommentViewModel surgicalConciergeStageCommentViewModel)
        {
            var commentList = surgicalConciergeStageCommentViewModel.ScgStageCommentList.Select(c => c.ScgStageCommentText).ToArray();
            //pickerAnswer.ItemsSource = commentList;
            //pickerAnswer.CheckedChanged += pickerAnswer_CheckedChanged;
            BuildRadioButtons(commentList);
        }

        private void pickerAnswer_CheckedChanged(object sender, int e)
        {
            try
            {
                var selectedComment = sender as CustomRadioButton;

                if (selectedComment == null || selectedComment.Id == this.selectedCommentIdPrev)
                {
                    return;
                }
                if (selectedComment.Checked)
                {
                    this.selectedCommentIdPrev = selectedComment.Id;
                    AdditionalComment.Text += " " + selectedComment.Text;
                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }




        }

        private void radioComment_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedComment = sender as RadioButton;

                if (selectedComment == null || selectedComment.Text.Equals(this.selectedCommentTextPrev))
                {
                    return;
                }
                this.selectedCommentTextPrev = selectedComment.Text?.Trim();
                AdditionalComment.Text += " " + selectedComment.Text?.Trim();
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        private void BuildRadioButtons(string[] commentList)
        {
            RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();
            foreach (var comment in commentList)
            {
                RadioButton com = new RadioButton();
                com.Text = comment;
                com.TextFontSize = 16;
                com.Color = Color.FromHex("#0F4563");
                com.Clicked += radioComment_CheckChanged;
                radioButtonGroupView.Children.Add(com);
                BoxView box = new BoxView { HeightRequest = 1, BackgroundColor = Color.FromHex("#FFFFFF") };
                radioButtonGroupView.Children.Add(box);
            }

            radioComment.Children.Add(radioButtonGroupView);

        }
    }
}