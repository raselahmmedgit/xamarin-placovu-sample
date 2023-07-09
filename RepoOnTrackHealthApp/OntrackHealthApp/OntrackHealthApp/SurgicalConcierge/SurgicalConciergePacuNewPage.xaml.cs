using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePacuNewPage : ContentPage
    {
        SurgicalConciergePacuQuestionViewModel surgicalConciergePacuQuestionViewModel;
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergePacuPageViewModel viewModel;
        private SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel { get; set; }
        private SurgicalConciergeRestApiService surgicalConciergeRestApiService;


        public SurgicalConciergePacuNewPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                //Title = _iTokenContainer.ApiPracticeName;

                if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
                {
                    Title = _iTokenContainer.ApiPracticeLocationName;
                }
                else
                {
                    Title = _iTokenContainer.ApiPracticeName;
                }

                SurgicalConciergePatientViewModel = surgicalConciergeViewModel;
                BindingContext = viewModel = new SurgicalConciergePacuPageViewModel();

                viewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
                viewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();

                ShowToolbar();

                PatientFullName.Text = "Patient : " + surgicalConciergeViewModel.PatientFullName;
                ProcedureName.Text = "Procedure : " + surgicalConciergeViewModel.ProcedureName;
                ProfessionalName.Text = "Professional : " + surgicalConciergeViewModel.ProfessionalName;
                surgicalConciergePacuQuestionViewModel = new SurgicalConciergePacuQuestionViewModel();

                surgicalConciergeRestApiService = new SurgicalConciergeRestApiService();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }
        private void ShowToolbar()
        {
            var toolbarItemToBeRemoved = ToolbarItems.FirstOrDefault(c => c.Text.Equals("Recipient"));
            if (toolbarItemToBeRemoved != null)
                ToolbarItems.Remove(toolbarItemToBeRemoved);

            if (Device.RuntimePlatform == Device.iOS)
            {
                // move layout under the status bar
                this.Padding = new Thickness(0, 20, 0, 0);

                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(SurgicalConciergePatientViewModel, (long)PracticeDivisionUnit.PACU));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }
            else
            {
                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(SurgicalConciergePatientViewModel, (long)PracticeDivisionUnit.PACU));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }

        }

        private void RadioButtonGroupViewOne_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "4")
            {
                EntyOne.IsVisible = true;
            }
            else
            {
                EntyOne.IsVisible = false;
            }
            //ShowConditionalBox();
        }
        private void RadioButtonGroupViewTwo_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "7")
            {
                EntyTwo.IsVisible = true;
            }
            else
            {
                EntyTwo.IsVisible = false;
            }
            ShowConditionalBox();
        }
        private void RadioButtonGroupViewThree_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "10")
            {
                EntyThree.IsVisible = true;
            }
            else
            {
                EntyThree.IsVisible = false;
            }
            ShowConditionalBox();
        }
        private void RadioButtonGroupViewFour_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "13")
            {
                EntyFour.IsVisible = true;
            }
            else
            {
                EntyFour.IsVisible = false;
            }
            ShowConditionalBox();
        }
        private void RadioButtonGroupViewFive_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "16")
            {
                EntyFive.IsVisible = true;
            }
            else
            {
                EntyFive.IsVisible = false;
            }
            ShowConditionalBox();
        }
        private void RadioButtonGroupViewSix_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            if (selectedItem != null && selectedItem.ToString() == "19")
            {
                EntySix.IsVisible = true;
            }
            else
            {
                EntySix.IsVisible = false;
            }

            ShowConditionalBox();
        }
        private void RadioButtonGroupViewSeven_SelectedItemChanged(object sender, EventArgs e)
        {
            var sadioButtonGroupView = sender as RadioButtonGroupView;
            var selectedItem = sadioButtonGroupView.SelectedItem;
            //if (selectedItem != null && selectedItem.ToString() == "22")
            //{
            //    //EntySeven.IsVisible = true;
            //}
            //else
            //{
            //    EntySix.IsVisible = false;
            //}
            if (selectedItem != null && selectedItem.ToString() == "20")
            {
                EntyRoom.IsVisible = true;
            }
            else
            {
                EntyRoom.IsVisible = false;
            }
            ShowConditionalBox();
        }
        private bool ShowConditionalBox()
        {
            if (RadioButtonGroupViewOne.SelectedItem?.ToString() == "1"
                 && RadioButtonGroupViewTwo.SelectedItem?.ToString() == "5"
                  && RadioButtonGroupViewThree.SelectedItem?.ToString() == "8"
                   && RadioButtonGroupViewFour.SelectedItem?.ToString() == "11"
                    && RadioButtonGroupViewFive.SelectedItem?.ToString() == "14")
            {
                //StackLayoutSix.IsVisible = true;             

            }

            if (RadioButtonGroupViewSix.SelectedItem?.ToString() == "17")
            {
                //StackLayoutSeven.IsVisible = true;
            }
            else
            {
                //StackLayoutSeven.IsVisible = false;
            }
            //return StackLayoutSix.IsVisible;
            return false;
        }

        private void ClearRadioButtonGroupView()
        {
            RadioButtonGroupViewOne.SelectedItem = null;
            RadioButtonGroupViewTwo.SelectedItem = null;
            RadioButtonGroupViewThree.SelectedItem = null;
            RadioButtonGroupViewFour.SelectedItem = null;
            RadioButtonGroupViewFive.SelectedItem = null;
            RadioButtonGroupViewSix.SelectedItem = null;
            RadioButtonGroupViewSeven.SelectedItem = null;

            chkStackLayoutScgPacuQuestionFamilyType.IsChecked = false;
            chkStackLayoutScgPacuQuestionTransferToFloorType.IsChecked = false;

            EntyOne.IsVisible = false;
            EntyOne.Text = null;

            EntyTwo.IsVisible = false;
            EntyTwo.Text = null;

            EntyThree.IsVisible = false;
            EntyThree.Text = null;

            EntyFour.IsVisible = false;
            EntyFour.Text = null;

            EntyFive.IsVisible = false;
            EntyFive.Text = null;

            EntySix.IsVisible = false;
            EntySix.Text = null;

            EntyRoom.IsVisible = false;
            EntyRoom.Text = null;

            StackLayoutScgPacuQuestionFamilyTypeDetail.IsVisible = false;

            StackLayoutScgPacuQuestionTransferToFloorTypeDetail.IsVisible = false;
        }

        private async void btnContinueToProgram_Clicked(object sender, EventArgs e)
        {
            ApiExecutionResult result = new ApiExecutionResult() { Success = false };
            surgicalConciergePacuQuestionViewModel = new SurgicalConciergePacuQuestionViewModel();
            surgicalConciergePacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
            surgicalConciergePacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
            surgicalConciergePacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;

            using (UserDialogs.Instance.Loading(""))
            {
                if (chkStackLayoutScgPacuQuestionFamilyType.IsChecked)
                {
                    if (RadioButtonGroupViewOne.SelectedItem == null
                     && RadioButtonGroupViewTwo.SelectedItem == null
                      && RadioButtonGroupViewThree.SelectedItem == null
                       && RadioButtonGroupViewFour.SelectedItem == null
                        && RadioButtonGroupViewFive.SelectedItem == null)
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NullReferenceExceptionError);
                        return;
                    }
                }

                //One
                ScgPacuQuestionViewModel scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;

                scgPacuQuestionViewModel.ScgPacuQuestionId = 1;
                if (RadioButtonGroupViewOne.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewOne.SelectedItem.ToString();
                    if (selecteditem == "4")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntyOne.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                //Two
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 2;
                if (RadioButtonGroupViewTwo.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewTwo.SelectedItem.ToString();
                    if (selecteditem == "7")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntyTwo.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                //Three
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 3;
                if (RadioButtonGroupViewThree.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewThree.SelectedItem.ToString();
                    if (selecteditem == "10")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntyThree.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                //Four
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 4;
                if (RadioButtonGroupViewFour.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewFour.SelectedItem.ToString();
                    if (selecteditem == "13")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntyFour.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                //Five
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 5;
                if (RadioButtonGroupViewFive.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewFive.SelectedItem.ToString();
                    if (selecteditem == "16")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntyFive.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                //Six
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 6;
                if (RadioButtonGroupViewSix.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewSix.SelectedItem.ToString();
                    if (selecteditem == "19")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailTypeMessageText = EntySix.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);


                //Seven
                scgPacuQuestionViewModel = new ScgPacuQuestionViewModel();
                scgPacuQuestionViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
                scgPacuQuestionViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId;
                scgPacuQuestionViewModel.PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId;
                scgPacuQuestionViewModel.ScgPacuQuestionId = 7;
                if (RadioButtonGroupViewSeven.SelectedItem != null)
                {
                    var selecteditem = RadioButtonGroupViewSeven.SelectedItem.ToString();
                    if (selecteditem == "20")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailRoomNo = EntyRoom.Text;
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else if (selecteditem == "22")
                    {
                        //scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailRoomNo = EntySeven.Text;
                        //scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                    else if (selecteditem == "21")
                    {
                        scgPacuQuestionViewModel.SelectedScgPacuQuestionDetailId = selecteditem.ToInt();
                    }
                }
                surgicalConciergePacuQuestionViewModel.ScgPacuQuestionViewModels.Add(scgPacuQuestionViewModel);

                surgicalConciergeRestApiService = new SurgicalConciergeRestApiService();
                result = await surgicalConciergeRestApiService.ScgPacuCommentSendAjax(surgicalConciergePacuQuestionViewModel);
            }
            if (result.Success)
            {
                UtilHelper.ShowToastMessage(AppConstant.PacuSuccessMessage);
                ClearRadioButtonGroupView();
            }
        }

        private void chkStackLayoutScgPacuQuestionFamilyType_CheckChanged(object sender, EventArgs e)
        {
            var chkStackLayoutScgPacuQuestionFamilyType = sender as CheckBox;

            if (chkStackLayoutScgPacuQuestionFamilyType.IsChecked)
            {
                StackLayoutScgPacuQuestionFamilyTypeDetail.IsVisible = true;
            }
            else
            {
                StackLayoutScgPacuQuestionFamilyTypeDetail.IsVisible = false;
            }
        }

        private void chkStackLayoutScgPacuQuestionTransferToFloorType_CheckChanged(object sender, EventArgs e)
        {
            var chkStackLayoutScgPacuQuestionTransferToFloorType = sender as CheckBox;

            if (chkStackLayoutScgPacuQuestionTransferToFloorType.IsChecked)
            {
                StackLayoutScgPacuQuestionTransferToFloorTypeDetail.IsVisible = true;
            }
            else
            {
                StackLayoutScgPacuQuestionTransferToFloorTypeDetail.IsVisible = false;
            }
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            ClearRadioButtonGroupView();
        }
    }
}