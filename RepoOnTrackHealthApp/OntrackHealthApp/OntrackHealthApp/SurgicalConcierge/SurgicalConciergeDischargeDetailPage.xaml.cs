using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeDischargeDetailPage : ContentPage
    {

        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeDischargePageViewModel _surgicalConciergeDischargePageViewModel;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        public ObservableCollection<ScgProstatectomyViewModel> _scgProstatectomyViewModelList { get; set; }
        public List<ScgProstatectomyViewModel> _scgProstatectomyViewModelTmpList { get; set; }
        private ScgDischargeCommentViewModel _scgDischargeCommentViewModel { get; set; }
        private IEnumerable<long> _scgProfessionalProcedureProstectomyIdList { set; get; }
        private int dischargeButtonCounter = 0;

        //public SurgicalConciergeDischargeDetailPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel, ObservableCollection<ScgProstatectomyViewModel> scgProstatectomyViewModels, ScgDischargeCommentViewModel scgDischargeCommentViewModel, IEnumerable<long> scgProfessionalProcedureProstectomyIdList)
        //{
        //    InitializeComponent();

        //    _iTokenContainer = new TokenContainer();
        //    Title = _iTokenContainer.ApiPracticeName;

        //    _scgProstatectomyViewModelList = scgProstatectomyViewModels;
        //    _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
        //    _scgDischargeCommentViewModel = scgDischargeCommentViewModel;
        //    _scgProfessionalProcedureProstectomyIdList = scgProfessionalProcedureProstectomyIdList;

        //    BindingContext = _surgicalConciergeDischargePageViewModel = new SurgicalConciergeDischargePageViewModel();
        //    _surgicalConciergeDischargePageViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
        //    _surgicalConciergeDischargePageViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();

        //    PatientFullName.Text = "Patient : " + surgicalConciergeViewModel.PatientFullName;
        //    ProcedureName.Text = "Procedure : " + surgicalConciergeViewModel.ProcedureName;
        //    ProfessionalName.Text = "Professional : " + surgicalConciergeViewModel.ProfessionalName;

        //}
        public SurgicalConciergeDischargeDetailPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

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

            _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
            _scgDischargeCommentViewModel = new ScgDischargeCommentViewModel();
            _scgDischargeCommentViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.HasValue ? surgicalConciergeViewModel.PatientProcedureDetailId.Value.ToString() : Guid.Empty.ToString();
            _scgDischargeCommentViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;

            BindingContext = _surgicalConciergeDischargePageViewModel = new SurgicalConciergeDischargePageViewModel();

            _surgicalConciergeDischargePageViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
            _surgicalConciergeDischargePageViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();
            _surgicalConciergeDischargePageViewModel.AfterLoadSurgicalConciergeDischargeData += _surgicalConciergeDischargePageViewModel_AfterLoadSurgicalConciergeDischargeData;

            PatientFullName.Text = surgicalConciergeViewModel.PatientFullName;
            ProcedureName.Text = surgicalConciergeViewModel.ProcedureName;
            ProfessionalName.Text = surgicalConciergeViewModel.ProfessionalName;

        }
        private void LoadData()
        {
            App.ShowUserDialogAsync();
            _surgicalConciergeDischargePageViewModel.LoadSurgicalConciergeDischargeCommand.Execute(null);

        }

        private void CreateFormCheckbox(List<ScgProstatectomyViewModel> model)
        {

            var data = model.Skip(0).Take(3).ToList();

            int loop_one = 0;

            Grid grdLayout = new Grid();
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.HorizontalOptions = LayoutOptions.FillAndExpand;


            foreach (var item in data)
            {
                Button btnImageLink = new Button
                {
                    //Text = item.ProstatectomyName,
                    HeightRequest = 110,
                    BackgroundColor = Color.FromHex("#c4d8f9"),
                    Image = "scg_dischrarge/" + item.ResourceIconUrl.Replace("~/content/images/scg_prostatectomy_icon/", "").Replace("-", "_"),
                    ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Top, 10),
                    FontSize = 12,
                    TextColor = Color.Black,
                    CornerRadius = 4,
                    ClassId = item.ProstatectomyId.ToString(),
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("#000000"),
                    Margin = new Thickness(0, 0, 0, 5),
                };

                if (_scgProfessionalProcedureProstectomyIdList != null && _scgProfessionalProcedureProstectomyIdList.Any() && _scgProfessionalProcedureProstectomyIdList.Contains(item.ProstatectomyId))
                {
                    btnImageLink.BackgroundColor = Color.FromHex("#0433ff");
                    btnImageLink.TextColor = Color.FromHex("#FFFFFF");
                    dischargeButtonCounter++;
                }

                btnImageLink.Clicked += BtnImageLink_Clicked;

                grdLayout.Children.Add(btnImageLink, loop_one, 0);

                model.Remove(item);
                loop_one++;
            }

            //StackLayout sl2 = new StackLayout();
            MainContainer2.Children.Add(grdLayout);

            if (model.Count > 0)
            {
                CreateFormCheckbox(model);
            }
        }

        private async void BtnImageLink_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackgroundColor == Color.FromHex("#0433ff"))
            {
                btn.BackgroundColor = Color.FromHex("#c4d8f9");
                btn.TextColor = Color.FromHex("#000000");
                dischargeButtonCounter--;
            }
            else
            {
                if (dischargeButtonCounter == AppConstant.MaximumDischargeItemSelectLimit)
                {
                    await DisplayAlert("Message", AppConstant.MaximumDischargeItemSelectMessage, "Ok");
                    return;
                }
                else
                {
                    btn.BackgroundColor = Color.FromHex("#0433ff");
                    btn.TextColor = Color.FromHex("#FFFFFF");
                    dischargeButtonCounter++;
                }
            }
        }

        private string[] GetSelectedCheckboxValue()
        {
            List<string> check = new List<string>();
            var view = MainContainer2.Children.Where(x => x.GetType() == typeof(Grid)).ToList();
            foreach (var item in view)
            {
                var grdLayout = item as Grid;
                var buttons = grdLayout.Children.Where(x => x.GetType() == typeof(Button));

                foreach (var btn in buttons)
                {
                    var btnView = btn as Button;
                    if (btnView.BackgroundColor == Color.FromHex("#0433ff"))
                    {
                        check.Add(btnView.ClassId);
                    }
                }
            }
            return check.ToArray();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            LoadData();
            //ShowToolbar();
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
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Discharge));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }
            else
            {
                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Discharge));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }

        }

        private async void SendDischargeComment_ClickedAsync(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    _scgDischargeCommentViewModel.ProstatectomyIds = GetSelectedCheckboxValue();
                    var result = await _surgicalConciergeDischargePageViewModel.ScgDischargeCommentSendAsync(_scgDischargeCommentViewModel);
                    if (result)
                    {
                        UtilHelper.ShowToastMessage(AppConstant.DischargeSuccessMessage);
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.DischargeErrorMessage);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }


        private void _surgicalConciergeDischargePageViewModel_AfterLoadSurgicalConciergeDischargeData(object sender, EventArgs e)
        {
            var data = _surgicalConciergeDischargePageViewModel.SurgicalConciergeDischargeCommentViewModels;
            _scgProstatectomyViewModelList = _surgicalConciergeDischargePageViewModel.ScgProstatectomyViewModels;
            _scgProfessionalProcedureProstectomyIdList = _surgicalConciergeDischargePageViewModel.ScgProfessionalProcedureProstectomyIdList;
            _scgProstatectomyViewModelTmpList = _scgProstatectomyViewModelList.ToList();
            CreateFormCheckbox(_scgProstatectomyViewModelTmpList);
            App.HideUserDialogAsync();
        }

        #region Top Menu Actions

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new AdminChangePassword());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {

                if (_iTokenContainer != null)
                {
                    _iTokenContainer.ClearApiToken();
                }
                DependencyService.Get<IToast>().SetSettingsForUserLogout();
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion

        //private async void SendDischargeComment_ClickedAsync(object sender, EventArgs e)
        //{
        //    using (UserDialogs.Instance.Loading(""))
        //    {
        //        _scgDischargeCommentViewModel.ProstatectomyIds = GetSelectedCheckboxValue();
        //        var result = await _surgicalConciergeDischargePageViewModel.ScgDischargeCommentSendAsync(_scgDischargeCommentViewModel);
        //        if (result)
        //        {
        //            UtilHelper.ShowToastMessage(AppConstant.DischargeSuccessMessage);
        //        }
        //        else
        //        {
        //            UtilHelper.ShowToastMessage(AppConstant.DischargeErrorMessage);
        //        }
        //    }
        //}
    }
}