using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeNursingRoundPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        public long PracticeDivisionUnitDest = 0;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private SurgicalConciergeNursingRoundPageViewModel viewModel;

        public SurgicalConciergeNursingRoundPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;
            PracticeDivisionUnitDest = practiceDivisionUnit;
            BindingContext = viewModel = new SurgicalConciergeNursingRoundPageViewModel();
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

            viewModel.LoadSurgicalConciergeNursingRound();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadSurgicalConciergeNursingRoundCommand.Execute(this);
        }

        public async Task CreateForm()
        {
            await Task.Yield();
            StackLayout NursingRoundStackViewInner = new StackLayout { Padding = new Thickness(0) };
            Editor editor = new Editor { IsVisible = false, HeightRequest = 100 };
            //editor.TextChanged += Editor_TextChanged;
            editor.Completed += Editor_Completed;
            foreach (var item in viewModel.ScgNursingRoundTemplateCategoryApiViewModels)
            {
                if (item.TemplateCategoryId == 1)
                {
                    StackLayout GroupLayout = new StackLayout { Padding = new Thickness(12), BackgroundColor = Color.FromHex("#47BCDC") };
                    GroupLayout.Children.Add
                    (
                        new Label { FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#FFFFFF"), Text = item.TemplateCategoryName }
                    );
                    StackLayout GroupCheckBox = new StackLayout { Padding = new Thickness(12), BackgroundColor = Color.FromHex("#EEF6F2"), ClassId = "GroupCheckBox" };
                    if (item.TemplateCategoryName != "Create own message")
                    {
                        RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();
                        GroupCheckBox.Children.Add(radioButtonGroupView);
                        //int loop = 1;
                        //int count = item.ScgNursingRoundTemplateCategoryDetailApiViewModels.Count();
                        foreach (var item2 in item.ScgNursingRoundTemplateCategoryDetailApiViewModels)
                        {
                            RadioButton radio = new RadioButton
                            {
                                Text = item2.TemplateCategoryDetailText,
                                CircleSize = 32,
                                TextFontSize = 15,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Value = item2.TemplateCategoryDetailText,
                                Spacing = 4,
                                TextColor = Color.FromHex("#222")
                            };
                            radioButtonGroupView.Children.Add(radio);

                            if (item2.TemplateCategoryDetailId == 12)
                            {
                                radioButtonGroupView.Children.Add(editor);
                            }
                            //radio.ClickCommand = new Command((obj) => {
                            //    RadioButtonClickedCommand(item2.TemplateCategoryDetailText, item2, editor, radioButtonGroupView);
                            //});
                            radio.Clicked += (object sender, EventArgs e) => {
                                var btn = (RadioButton)sender;
                                RadioButtonClickedCommand(item2.TemplateCategoryDetailText, item2, editor, radioButtonGroupView);
                            };
                        }
                    }
                    NursingRoundStackViewInner.Children.Add(GroupLayout);
                    NursingRoundStackViewInner.Children.Add(GroupCheckBox);

                }
                else if (item.TemplateCategoryId != 4 && (item.TemplateCategoryId == 2 || item.TemplateCategoryId == 3))
                {
                    StackLayout GroupLayout = new StackLayout { Padding = new Thickness(12), BackgroundColor = Color.FromHex("#47BCDC") };
                    GroupLayout.Children.Add
                    (
                        new Label { FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#FFFFFF"), Text = item.TemplateCategoryName }
                    );
                    StackLayout GroupCheckBox = new StackLayout { Padding = new Thickness(12), BackgroundColor = Color.FromHex("#EEF6F2"), ClassId = "GroupCheckBox" };
                    if (item.TemplateCategoryName != "Create own message")
                    {
                        RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();
                        GroupCheckBox.Children.Add(radioButtonGroupView);
                        //int loop = 1;
                        //int count = item.ScgNursingRoundTemplateCategoryDetailApiViewModels.Count();
                        foreach (var item2 in item.ScgNursingRoundTemplateCategoryDetailApiViewModels)
                        {
                            CheckBox check = new CheckBox
                            {
                                Text = item2.TemplateCategoryDetailText,
                                BoxSizeRequest = 26,
                                TextFontSize = 15,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Spacing = 4,
                                IsChecked = false,
                                TextColor = Color.FromHex("#222")
                            };
                            radioButtonGroupView.Children.Add(check);

                            if (item2.TemplateCategoryDetailId == 12)
                            {
                                radioButtonGroupView.Children.Add(editor);
                            }
                            //radio.ClickCommand = new Command((obj) => {
                            //    RadioButtonClickedCommand(item2.TemplateCategoryDetailText, item2, editor, radioButtonGroupView);
                            //});
                            //radio.Clicked += (object sender, EventArgs e) => {
                            //    var btn = (RadioButton)sender;
                            //    RadioButtonClickedCommand(item2.TemplateCategoryDetailText, item2, editor, radioButtonGroupView);
                            //};
                            check.CheckChanged += (object sender, EventArgs e) => {
                                var btn = (CheckBox)sender;
                                CheckBoxClickedCommand(item2.TemplateCategoryDetailText, item2, editor, radioButtonGroupView, btn);
                            };
                        }
                    }
                    NursingRoundStackViewInner.Children.Add(GroupLayout);
                    NursingRoundStackViewInner.Children.Add(GroupCheckBox);

                }
            }
            NursingRoundStackView.Children.Clear();
            NursingRoundStackView.Children.Add(NursingRoundStackViewInner);

            CountryCodePicker.ItemsSource = viewModel.CountryViewModels;
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
            EditorGmSigneture.Text = "Thank you for choosing" + Environment.NewLine + Environment.NewLine + _iTokenContainer.ApiPracticeName;
            StackLayoutMain.IsVisible = true;
        }

        public void RadioButtonClickedCommand(string value, ScgNursingRoundTemplateCategoryDetailApiViewModel model, Editor editor, RadioButtonGroupView parent)
        {
            if (model.TemplateCategoryDetailId == 12)
            {
                if (model.TemplateCategoryId == 3)
                {
                    editor.IsVisible = true;
                }
                else
                {
                    editor.Text = "";
                    editor.IsVisible = false;
                }
            }

            if (model.TemplateCategoryId == 1)
            {
                SetTextEditorGmChooseSalutation(value);
            }
            //if (model.TemplateCategoryId == 2)
            //{
            //    SetTextEditorGmProgressUpdate(value);
            //}
            //if (model.TemplateCategoryId == 3)
            //{
            //    if (model.TemplateCategoryDetailId == 12)
            //    {
            //        editor.IsVisible = true;
            //        SetTextEditorGmTodaysPlan("");
            //    }
            //    else
            //    {editor.Text = "";
            //    editor.IsVisible = false;
            //        SetTextEditorGmTodaysPlan(value);
            //    }
            //}
        }
        public void CheckBoxClickedCommand(string value, ScgNursingRoundTemplateCategoryDetailApiViewModel model, Editor editor, RadioButtonGroupView parent, CheckBox checkBox)
        {
            if (model.TemplateCategoryDetailId == 12)
            {
                if (model.TemplateCategoryId == 3)
                {
                    editor.IsVisible = true;
                }
                else
                {
                    editor.Text = "";
                    editor.IsVisible = false;
                }
            }

            //if (model.TemplateCategoryId == 1)
            //{
            //    SetTextEditorGmChooseSalutation(value);
            //}
            if (model.TemplateCategoryId == 2)
            {
                SetTextEditorGmProgressUpdateForCheckBox(value, checkBox.IsChecked);
            }
            if (model.TemplateCategoryId == 3)
            {
                if (model.TemplateCategoryDetailId == 12)
                {
                    editor.IsVisible = true;
                    //SetTextEditorGmTodaysPlanForCheckBox("");
                }
                else
                {
                    editor.Text = "";
                    editor.IsVisible = false;
                    SetTextEditorGmTodaysPlanForCheckBox(value, checkBox.IsChecked);
                }
            }
        }
        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null)
            {
                SetTextEditorGmTodaysPlan(e.NewTextValue);
            }
        }
        private void Editor_Completed(object sender, EventArgs e)
        {
            var editor = (Editor)sender;
            if (editor != null && !string.IsNullOrEmpty(editor.Text?.Trim().ToString()))
            {
                SetTextEditorGmTodaysPlanForEditorTextBox(editor.Text?.Trim().ToString());
            }
        }
        private void FloorPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null)
            {
                var countryCode = CountryCodePicker.SelectedItem == null ? "" : CountryCodePicker.SelectedItem.ToString();
                var fullPhoneNumber = countryCode + " " + FloorPhoneNumber.Text;
                if (FloorPhoneNumber.Text != "")
                {
                    SetTextEditorGmFloorPhoneNumber(fullPhoneNumber);
                }
                else
                {
                    SetTextEditorGmFloorPhoneNumber("");
                }
            }
            else
            {
                SetTextEditorGmFloorPhoneNumber("");
            }

        }

        public void SetTextEditorGmChooseSalutation(string text)
        {
            if (text == "")
            {
                EditorGmChooseSalutation.Text = "";
                StackLayoutEditorGmChooseSalutation.IsVisible = false;
            }
            else
            {
                EditorGmChooseSalutation.Text = text + ",";
                StackLayoutEditorGmChooseSalutation.IsVisible = true;
            }
        }
        public void SetTextEditorGmProgressUpdate(string text)
        {
            if (text == "")
            {
                EditorGmProgressUpdate.Text = "";
                StackLayoutEditorGmProgressUpdate.IsVisible = false;
            }
            else
            {
                EditorGmProgressUpdate.Text = text + ".";
                StackLayoutEditorGmProgressUpdate.IsVisible = true;
            }
        }
        public void SetTextEditorGmTodaysPlan(string text)
        {
            if (text == "")
            {
                EditorGmTodaysPlan.Text = "";
                StackLayoutEditorGmTodaysPlan.IsVisible = false;
            }
            else
            {
                EditorGmTodaysPlan.Text = text + ".";
                StackLayoutEditorGmTodaysPlan.IsVisible = true;
            }
        }
        public void SetTextEditorGmProgressUpdateForCheckBox(string text, bool isChecked)
        {
            string preText = EditorGmProgressUpdate.Text?.Trim().ToString();

            if (isChecked)
            {
                string curText = string.IsNullOrEmpty(preText) ? (text + ".") : (preText + " " + text + ".");
                text = curText;
                EditorGmProgressUpdate.Text = text;
            }
            else
            {
                string curText = string.IsNullOrEmpty(preText) ? "" : (preText.Replace((text + "."), ""));
                EditorGmProgressUpdate.Text = curText;
            }

            if (string.IsNullOrEmpty(EditorGmProgressUpdate.Text?.Trim().ToString()))
            {
                StackLayoutEditorGmProgressUpdate.IsVisible = false;
            }
            else
            {
                StackLayoutEditorGmProgressUpdate.IsVisible = true;
            }
        }
        public void SetTextEditorGmTodaysPlanForCheckBox(string text, bool isChecked)
        {
            string preText = EditorGmTodaysPlan.Text?.Trim().ToString();

            if (isChecked)
            {
                string curText = string.IsNullOrEmpty(preText) ? (text + ".") : (preText + " " + text + ".");
                text = curText;
                EditorGmTodaysPlan.Text = text;
            }
            else
            {
                string curText = string.IsNullOrEmpty(preText) ? "" : (preText.Replace((text + "."), ""));
                EditorGmTodaysPlan.Text = curText;
            }

            if (string.IsNullOrEmpty(EditorGmTodaysPlan.Text?.Trim().ToString()))
            {
                StackLayoutEditorGmTodaysPlan.IsVisible = false;
            }
            else
            {
                StackLayoutEditorGmTodaysPlan.IsVisible = true;
            }
        }
        public void SetTextEditorGmTodaysPlanForEditorTextBox(string text)
        {
            string preText = EditorGmTodaysPlan.Text?.Trim().ToString();

            if (!string.IsNullOrEmpty(text))
            {
                string curText = string.IsNullOrEmpty(preText) ? (text + ".") : (preText + " " + text + ".");
                text = curText;
                EditorGmTodaysPlan.Text = text;
            }

            if (string.IsNullOrEmpty(EditorGmTodaysPlan.Text?.Trim().ToString()))
            {
                StackLayoutEditorGmTodaysPlan.IsVisible = false;
            }
            else
            {
                StackLayoutEditorGmTodaysPlan.IsVisible = true;
            }
        }
        public void SetTextEditorGmFloorPhoneNumber(string text)
        {
            if (text == "")
            {
                StackLayoutGmFloorPhoneNumber.IsVisible = false;
                EditorGmFloorPhoneNumber.Text = "";
            }
            else
            {
                StackLayoutGmFloorPhoneNumber.IsVisible = true;
                EditorGmFloorPhoneNumber.Text = "Please call the floor with any questions regarding his care at " + text + ".";
            }
        }
        public void SetTextEditorGmSigneture(string text) { EditorGmSigneture.Text = text; }

        private async void ButtonSendMessage_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                var mmsg = EditorGmChooseSalutation.Text
                    + EditorGmProgressUpdate.Text
                    + EditorGmTodaysPlan.Text
                    + FloorPhoneNumber.Text;

                if (!string.IsNullOrEmpty(mmsg.Trim()))
                {
                    SurgicalConciergeNursingRoundViewModel surgicalConciergeNursingRoundViewModel = new SurgicalConciergeNursingRoundViewModel();
                    surgicalConciergeNursingRoundViewModel.SalutationMessage = EditorGmChooseSalutation.Text;
                    surgicalConciergeNursingRoundViewModel.ProgressUpdateMessage = EditorGmProgressUpdate.Text;
                    surgicalConciergeNursingRoundViewModel.TodaysPlanMessage = EditorGmTodaysPlan.Text;
                    surgicalConciergeNursingRoundViewModel.FloorPhone = FloorPhoneNumber.Text;
                    surgicalConciergeNursingRoundViewModel.FloorPhoneCode = CountryCodePicker.SelectedItem == null ? "" : CountryCodePicker.SelectedItem.ToString();
                    surgicalConciergeNursingRoundViewModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId;
                    surgicalConciergeNursingRoundViewModel.PracticeLocationId = _surgicalConciergePatientViewModel.PracticeLocationId;
                    surgicalConciergeNursingRoundViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    viewModel.SurgicalConciergeNursingRoundViewModel = surgicalConciergeNursingRoundViewModel;
                    viewModel.ScgNursingRoundSendNotificationCommand.Execute(null);
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertWarningTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void ButtonReset_Clicked(object sender, EventArgs e)
        {
            var itemOneTop = NursingRoundStackView.Children.Where(x => x.GetType() == typeof(StackLayout)).FirstOrDefault() as StackLayout;
            var itemOne = itemOneTop.Children.Where(x => x.GetType() == typeof(StackLayout) && x.ClassId == "GroupCheckBox").ToList();
            foreach (var item in itemOne)
            {
                var groupOne = item as StackLayout;
                if (groupOne.ClassId == "GroupCheckBox")
                {

                }
                RadioButtonGroupView groupOneRadioButtonGroupView = groupOne.Children.Where(x => x.GetType() == typeof(RadioButtonGroupView)).FirstOrDefault() as RadioButtonGroupView;
                if (groupOneRadioButtonGroupView != null)
                {
                    groupOneRadioButtonGroupView.SelectedItem = null;

                    var checkBoxList = groupOneRadioButtonGroupView.Children.Where(x => x.GetType() == typeof(CheckBox));
                    foreach (var checkBox in checkBoxList)
                    {
                        if (checkBox != null)
                        {
                            var innerCheckBox = (checkBox as CheckBox);
                            innerCheckBox.IsChecked = false;
                        }
                    }

                    var editor = groupOneRadioButtonGroupView.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault();
                    if (editor != null)
                    {
                        var innerEditor = (editor as Editor);
                        innerEditor.Text = "";
                        innerEditor.IsVisible = false;
                    }
                }
            }
            SetTextEditorGmChooseSalutation("");
            SetTextEditorGmProgressUpdate("");
            SetTextEditorGmTodaysPlan("");
            SetTextEditorGmFloorPhoneNumber("");
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
            FloorPhoneNumber.Text = "";
        }

        public async void ShowAlert(string title, string message)
        {
            await DisplayAlert(title, message, AppConstant.DisplayAlertErrorButtonText);
        }
    }
}