using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using OntrackHealthApp.UserControls;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeDischargePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeDischargePageViewModel _surgicalConciergeDischargePageViewModel;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        public ObservableCollection<ScgProstatectomyViewModel> _scgProstatectomyViewModelList { get; set; }
        public IEnumerable<long> _scgProfessionalProcedureProstectomyIdList { set; get; }

        public SurgicalConciergeDischargePage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
                BindingContext = _surgicalConciergeDischargePageViewModel = new SurgicalConciergeDischargePageViewModel();

                _surgicalConciergeDischargePageViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
                _surgicalConciergeDischargePageViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();

                ShowToolbar();

                PatientFullName.Text = "Patient : " + surgicalConciergeViewModel.PatientFullName;
                ProcedureName.Text = "Procedure : " + surgicalConciergeViewModel.ProcedureName;
                ProfessionalName.Text = "Professional : " + surgicalConciergeViewModel.ProfessionalName;

                LoadData();

            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadData()
        {
            await _surgicalConciergeDischargePageViewModel.ExecuteLoadSurgicalConciergeDischargeAsync();
            var data = _surgicalConciergeDischargePageViewModel.SurgicalConciergeDischargeCommentViewModels;
            _scgProstatectomyViewModelList = _surgicalConciergeDischargePageViewModel.ScgProstatectomyViewModels;
            _scgProfessionalProcedureProstectomyIdList = _surgicalConciergeDischargePageViewModel.ScgProfessionalProcedureProstectomyIdList;
            CreateForm(data);
        }
        private void CreateForm(ObservableCollection<ScgDischargeCommentViewModel> data)
        {
            StackLayout MainContainerInner = new StackLayout { BackgroundColor = Color.Transparent };
            foreach (var item in data)
            {
                Frame outerContent = new Frame
                {
                    BorderColor = Color.FromHex("#004D80"),
                    CornerRadius = 6,
                    BackgroundColor = Color.FromHex("#e8edf1"),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                StackLayout firstContent = new StackLayout
                {
                    Padding = new Thickness(12, 0, 12, 12),
                    BackgroundColor = Color.Transparent,
                };
                StackLayout secondContent = new StackLayout
                {
                    Padding = new Thickness(12),
                    BackgroundColor = Color.Transparent,
                };

                Label lbl = new Label
                {
                    Text = item.ScgDischargeCommentText,
                    TextColor = Color.FromHex("#000"),
                    FontSize = 20,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                firstContent.Children.Add(lbl);
                StackLayout secontContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Button submitButton = new Button
                {
                    HeightRequest = 46,
                    CornerRadius = 23,
                    BackgroundColor = Color.FromHex("#004D80"),
                    TextColor = Color.FromHex("#FFF"),
                    FontSize = 16,
                    Text = "Select",
                    Margin = new Thickness(0, 5),
                    WidthRequest = 120,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                submitButton.Clicked += async (sender, args) => await TemplatedCommentButton_ClickedAsync(sender, args, lbl, submitButton, item.ScgDischargeCommentId);

                Grid grdLayout = new Grid();
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.Children.Add(submitButton, 0, 0);
                secontContent.Children.Add(grdLayout);

                StackLayout thirdContent = new StackLayout { };
                thirdContent.Children.Add(firstContent);
                thirdContent.Children.Add(secontContent);
                outerContent.Content = thirdContent;
                MainContainerInner.Children.Add(outerContent);
            }
            MainContainer.Children.Clear();
            MainContainer.Children.Add(MainContainerInner);
            CreateFormAddFreeText();
        }

        private void CreateFormAddFreeText()
        {
            StackLayout MainContainerInner = new StackLayout { BackgroundColor = Color.Transparent };
            Frame outerContent = new Frame
            {
                BorderColor = Color.FromHex("#004D80"),
                CornerRadius = 6,
                BackgroundColor = Color.FromHex("#e8edf1"),
                Margin = new Thickness(0, 0, 0, 10)
            };

            StackLayout firstContent = new StackLayout
            {
                Padding = new Thickness(12, 0, 12, 12),
                BackgroundColor = Color.Transparent,
            };
            Label lbl = new Label
            {
                Text = "Type Message",
                TextColor = Color.FromHex("#000"),
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            firstContent.Children.Add(lbl);

            StackLayout secondContent = new StackLayout
            {
                Padding = new Thickness(12),
                BackgroundColor = Color.Transparent,
            };
            MtiEditor editor = new MtiEditor
            {
                Text = "",
                FontSize = 20,
                HeightRequest = 100,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            secondContent.Children.Add(editor);

            Button submitButton = new Button
            {
                HeightRequest = 48,
                CornerRadius = 24,
                BackgroundColor = Color.FromHex("#004D80"),
                TextColor = Color.FromHex("#FFF"),
                FontSize = 18,
                Text = "Select",
                Margin = new Thickness(0, 5),
                WidthRequest = 120,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            submitButton.Clicked += async (sender, args) => await BtnFreeText_ClickedAsync(sender, args, editor, outerContent);

            Grid grdLayout = new Grid();
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.Children.Add(submitButton, 1, 0);

            StackLayout thirdContent = new StackLayout
            {
                BackgroundColor = Color.Transparent
            };

            thirdContent.Children.Add(firstContent);
            thirdContent.Children.Add(secondContent);
            thirdContent.Children.Add(grdLayout);

            outerContent.Content = thirdContent;
            MainContainerInner.Children.Add(outerContent);
            MainContainer.Children.Add(MainContainerInner);
        }

        private void CreateFormCheckbox(ObservableCollection<ScgProstatectomyViewModel> model)
        {
            RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();
            if (model != null && model.Count > 0)
            {
                foreach (var item in model)
                {
                    CheckBox check = new CheckBox
                    {
                        Text = item.ProstatectomyName,
                        BoxSizeRequest = 30,
                        TextFontSize = 20,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 10,
                        IsChecked = item.IsActive,
                        TextColor = Color.FromHex("#000000"),
                        ClassId = item.ProstatectomyId.ToString()
                    };
                    radioButtonGroupView.Children.Add(check);
                    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#004D80"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                    radioButtonGroupView.Children.Add(bxCheck);
                }
                MainContainerCheckBox.BackgroundColor = Color.FromHex("#e8edf1");
                MainContainerCheckBox.Padding = new Thickness(12);
                MainContainerCheckBox.Children.Add(radioButtonGroupView);
                MainContainerCheckBox.IsVisible = true;
            }
        }

        protected async Task AdditionalCommentButton_ClickedAsync(object sender, EventArgs args, Button btnSource)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushModalAsync(new SurgicalConciergeDischargeAdditionalCommentPage(_iTokenContainer.ApiPracticeName, btnSource));
            }
        }

        protected async Task TemplatedCommentButton_ClickedAsync(object sender, EventArgs args, Label template, Button btnSource, Guid scgDischargeCommentId)
        {
            ScgDischargeCommentViewModel scgDischargeCommentViewModel = new ScgDischargeCommentViewModel();
            scgDischargeCommentViewModel.ScgDischargeCommentId = scgDischargeCommentId;
            scgDischargeCommentViewModel.ScgDischargeCommentText = template.Text;
            scgDischargeCommentViewModel.ScgDischargeAdditionalComment = btnSource.ClassId;
            scgDischargeCommentViewModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString();
            scgDischargeCommentViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
            await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel));

            //await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel, _scgProstatectomyViewModelList, scgDischargeCommentViewModel, _scgProfessionalProcedureProstectomyIdList));
        }

        protected async Task BtnFreeText_ClickedAsync(object sender, EventArgs e, MtiEditor editor, Frame outerContent)
        {
            if (editor.Text == "")
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }
            else
            {
                ScgDischargeCommentViewModel scgDischargeCommentViewModel = new ScgDischargeCommentViewModel();
                scgDischargeCommentViewModel.ScgDischargeAdditionalComment = editor.Text;
                scgDischargeCommentViewModel.ScgDischargeCommentText = editor.Text;
                scgDischargeCommentViewModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString();
                scgDischargeCommentViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                using (UserDialogs.Instance.Loading(""))
                //await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel, _scgProstatectomyViewModelList, scgDischargeCommentViewModel, _scgProfessionalProcedureProstectomyIdList));
                {
                    await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel));
                }

            }
        }

        private string[] GetSelectedCheckboxValue()
        {
            List<string> check = new List<string>();
            RadioButtonGroupView view = MainContainerCheckBox.Children.FirstOrDefault(x => x.GetType() == typeof(RadioButtonGroupView)) as RadioButtonGroupView;
            foreach (var item in view.Children.Where(x => x.GetType() == typeof(CheckBox)))
            {
                CheckBox checkbox = item as CheckBox;
                if (checkbox.IsChecked)
                {
                    check.Add(checkbox.ClassId);
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
    }
}