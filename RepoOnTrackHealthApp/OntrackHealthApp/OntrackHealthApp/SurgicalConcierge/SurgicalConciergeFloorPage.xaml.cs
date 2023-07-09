using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
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
    public partial class SurgicalConciergeFloorPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeFloorPageViewModel _surgicalConciergeFloorPageViewModel;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        public ObservableCollection<ScgProstatectomyViewModel> _scgProstatectomyViewModelList { get; set; }

        public SurgicalConciergeFloorPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
                BindingContext = _surgicalConciergeFloorPageViewModel = new SurgicalConciergeFloorPageViewModel();

                _surgicalConciergeFloorPageViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
                _surgicalConciergeFloorPageViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();

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
            await _surgicalConciergeFloorPageViewModel.ExecuteLoadSurgicalConciergeFloorAsync();
            var data = _surgicalConciergeFloorPageViewModel.SurgicalConciergeFloorCommentViewModels;
            _scgProstatectomyViewModelList = _surgicalConciergeFloorPageViewModel.ScgProstatectomyViewModels;
            CreateFormCheckbox(_scgProstatectomyViewModelList);
            CreateForm(data);
        }

        private void CreateForm(ObservableCollection<ScgFloorCommentViewModel> data)
        {
            MainContainer.Children.Clear();

            foreach (var item in data)
            {

                StackLayout firstContent = new StackLayout
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Color.FromHex("#e8edf1")
                };
                StackLayout secontContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label lbl = new Label
                {
                    Text = item.ScgFloorCommentText,
                    TextColor = Color.FromHex("#000"),
                    FontSize = 20,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                secontContent.Children.Add(lbl);

                StackLayout thirdContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    MinimumWidthRequest = 100
                };
                Button submitButton = new Button
                {
                    HeightRequest = 44,
                    CornerRadius = 10,
                    BackgroundColor = Color.FromHex("#004D80"),
                    TextColor = Color.FromHex("#FFF"),
                    FontSize = 18,
                    Text = "Send",
                    Margin = new Thickness(0, 5),
                    WidthRequest = 100
                };

                submitButton.Clicked += async (sender, args) => await btnTemplateText_ClickedAsync(sender, args, item.ScgFloorCommentId, item.ScgFloorCommentText, firstContent, lbl);

                thirdContent.Children.Add(submitButton);

                Grid grdLayout = new Grid();
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.Children.Add(thirdContent, 1, 0);

                firstContent.Children.Add(secontContent);
                firstContent.Children.Add(grdLayout);

                StackLayout outerContent = new StackLayout
                {
                    Padding = new Thickness(2),
                    Margin = new Thickness(6, 4),
                    BackgroundColor = Color.FromHex("#004D80")
                };
                outerContent.Children.Add(firstContent);
                MainContainer.Children.Add(outerContent);
            }
            CreateFormAddFreeText();
        }

        private void CreateFormAddFreeText()
        {

            StackLayout firstContent = new StackLayout
            {
                Padding = new Thickness(10),
                BackgroundColor = Color.FromHex("#e8edf1")
            };
            StackLayout secontContent = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Label lbl = new Label
            {
                Text = "Type Message",
                TextColor = Color.FromHex("#000"),
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            secontContent.Children.Add(lbl);
            Editor editor = new Editor
            {
                Text = "",
                FontSize = 20,
                BackgroundColor = Color.FromHex("#FFFFFF"),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            secontContent.Children.Add(editor);

            StackLayout thirdContent = new StackLayout
            {
                BackgroundColor = Color.Transparent
            };
            Button submitButton = new Button
            {
                HeightRequest = 44,
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#004D80"),
                TextColor = Color.FromHex("#FFF"),
                FontSize = 18,
                Text = "Send",
                Margin = new Thickness(0, 5),
                WidthRequest = 100
            };
            submitButton.Clicked += async (sender, args) => await btnFreeText_ClickedAsync(sender, args, editor, firstContent);

            thirdContent.Children.Add(submitButton);

            Grid grdLayout = new Grid();
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            grdLayout.Children.Add(thirdContent, 1, 0);

            firstContent.Children.Add(secontContent);
            firstContent.Children.Add(grdLayout);

            StackLayout outerContent = new StackLayout
            {
                Padding = new Thickness(2),
                BackgroundColor = Color.FromHex("#004D80")
            };
            outerContent.Children.Add(firstContent);
            MainContainer.Children.Add(outerContent);
        }

        private void CreateFormCheckbox(ObservableCollection<ScgProstatectomyViewModel> model)
        {
            RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

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
        }

        protected async Task btnFreeText_ClickedAsync(object sender, EventArgs e, Editor editor, StackLayout firstContent)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (editor.Text == "")
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
                else
                {
                    ScgFloorCommentViewModel dataSendModel = new ScgFloorCommentViewModel();
                    dataSendModel.ScgFloorAdditionalComment = editor.Text?.Trim();
                    dataSendModel.ScgFloorCommentText = editor.Text?.Trim();
                    dataSendModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString();
                    dataSendModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    dataSendModel.ProstatectomyIds = GetSelectedCheckboxValue();
                    var result = await _surgicalConciergeFloorPageViewModel.ScgFloorCommentSendAsync(dataSendModel);
                    if (result)
                    {
                        //await DisplayAlert(AppConstant.ToastMessageTitle, AppConstant.FloorSuccessMessage, AppConstant.DisplayAlertErrorButtonText);
                        UtilHelper.ShowToastMessage(AppConstant.FloorSuccessMessage);

                        var submitButton = sender as Button;

                        submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                        submitButton.TextColor = Color.FromHex("#000");
                        submitButton.Text = "Delivered";
                        submitButton.IsEnabled = false;

                        firstContent.BackgroundColor = Color.FromHex("#004c7e");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.FloorErrorMessage);
                    }

                }
            }

        }

        protected async Task btnTemplateText_ClickedAsync(object sender, EventArgs e, Guid scgFloorCommentId, string template, StackLayout firstContent, Label label)
        {
            using (UserDialogs.Instance.Loading(""))
            {

                if (template == "")
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
                else
                {
                    ScgFloorCommentViewModel dataSendModel = new ScgFloorCommentViewModel();
                    dataSendModel.ScgFloorCommentId = scgFloorCommentId;
                    dataSendModel.ScgFloorCommentText = template;
                    dataSendModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString();
                    dataSendModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    dataSendModel.ProstatectomyIds = GetSelectedCheckboxValue();
                    var result = await _surgicalConciergeFloorPageViewModel.ScgFloorCommentSendAsync(dataSendModel);
                    if (result)
                    {
                        //await DisplayAlert(AppConstant.ToastMessageTitle, AppConstant.FloorSuccessMessage, AppConstant.DisplayAlertErrorButtonText);
                        UtilHelper.ShowToastMessage(AppConstant.FloorSuccessMessage);

                        var submitButton = sender as Button;

                        submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                        submitButton.Text = "Delivered";
                        submitButton.IsEnabled = false;

                        firstContent.BackgroundColor = Color.FromHex("#004c7e");

                        label.TextColor = Color.FromHex("#FFF");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.FloorErrorMessage);
                    }
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
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Floor));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }
            else
            {
                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Floor));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }

        }


    }
}