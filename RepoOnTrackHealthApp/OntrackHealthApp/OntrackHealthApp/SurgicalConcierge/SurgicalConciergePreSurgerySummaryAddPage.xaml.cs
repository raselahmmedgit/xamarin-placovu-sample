using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePreSurgerySummaryAddPage : ContentPage
    {
        private SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private Dictionary<string, double> cancerLocationPairs = new Dictionary<string, double>();
        private PatientPreSurgerySummaryViewModel _patientPreSurgerySummaryViewModel;
        private SurgicalConciergeRestApiService _restApiService;

        public SurgicalConciergePreSurgerySummaryAddPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel)
        {
            InitializeComponent();
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
            this.SurgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            PatientFullName.Text = SurgicalConciergePatientViewModel.PatientFullName;
            ProcedureName.Text = SurgicalConciergePatientViewModel.ProcedureName;
            ProfessionalName.Text = SurgicalConciergePatientViewModel.ProfessionalName;
            _patientPreSurgerySummaryViewModel = patientPreSurgerySummaryViewModel;
            _restApiService = new SurgicalConciergeRestApiService();
            using (UserDialogs.Instance.Loading(""))
            {
                _patientPreSurgerySummaryViewModel.CancerLocationTemplateType = int.Parse(CancerLocationTemplateTypeId.Twelve);
                GenerateCancerLocationViewForTwelveItems(int.Parse(CancerLocationTemplateTypeId.Twelve), _patientPreSurgerySummaryViewModel.PreSurgerySummaryId);
            }

        }
        private void GeneratePiradListView(PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel)
        {
            piradListContainerStackLayout.Children.Clear();
            if (patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels == null || patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Count == 0)
            {
                piradListContainerStackLayout.IsVisible = false;
                return;
            }

            StackLayout piradListStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            Grid piradGrid = new Grid
            {
                BackgroundColor = Color.FromHex("#FFF"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 330
            };
            var rowDefinations = new RowDefinitionCollection();
            foreach (var piradItem in patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels)
            {
                rowDefinations.Add(new RowDefinition { Height = GridLength.Auto });
            }

            piradGrid.RowDefinitions = rowDefinations;

            var columnDefinations = new ColumnDefinitionCollection
            {
                new ColumnDefinition{Width=120},
                new ColumnDefinition{Width=GridLength.Star},
                new ColumnDefinition{Width=GridLength.Star}
            };

            piradGrid.ColumnDefinitions = columnDefinations;

            for (int i = 0; i < patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Count; i++)
            {
                var singlePiradItem = patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels[i];
                Label piradLabel = new Label
                {
                    Text = "Pirad " + (i + 1),
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                    TextColor = Color.FromHex("#000")
                };
                piradGrid.Children.Add(piradLabel, 0, i);
                Button editButton = new Button
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Text = "Edit",
                    FontSize = 18,
                    TextColor = Color.FromHex("#FFF"),
                    BackgroundColor = Color.FromHex("#610094"),
                    WidthRequest = 80,
                    HeightRequest = 50
                };
                editButton.Clicked += (sender, arg) => OnPiradEditButtonClicked(sender, arg, singlePiradItem);

                piradGrid.Children.Add(editButton, 1, i);
                Button deleteButton = new Button
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Text = "Delete",
                    FontSize = 18,
                    TextColor = Color.FromHex("#FFF"),
                    BackgroundColor = Color.FromHex("#C83330"),
                    WidthRequest = 100,
                    HeightRequest = 50
                };

                deleteButton.Clicked += (sender, arg) => OnPiradDeleteButtonClicked(sender, arg, singlePiradItem);

                piradGrid.Children.Add(deleteButton, 2, i);
            }
            piradListStackLayout.Children.Add(piradGrid);
            piradListContainerStackLayout.Children.Add(piradListStackLayout);
            piradListContainerStackLayout.IsVisible = true;

        }
        public void OnPiradEditButtonClicked(object sender, EventArgs e, PatientPreSurgerySummaryPiradViewModel piradViewModel)
        {
            Navigation.PushModalAsync(new SurgicalConciergePreSurgerySummaryPiradEditPage(piradViewModel));
        }
        public void OnPiradDeleteButtonClicked(object sender, EventArgs e, PatientPreSurgerySummaryPiradViewModel piradViewModel)
        {
            RemovePiradItem(piradViewModel);
        }

        public async void OnFinishedButton_Clicked(object sender, EventArgs e)
        {

            _patientPreSurgerySummaryViewModel.PreopPsa = string.IsNullOrEmpty(PreopPsaTextBox.Text) ? (decimal?)null : decimal.Parse(PreopPsaTextBox.Text);
            var gradeOneButtonList = gradeOneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeOneButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryViewModel.GleasonScoreOne = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text);
                    break;
                }
            }
            var gradeTwoButtonList = gradeTwoStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeTwoButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryViewModel.GleasonScoreTwo = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text);
                    break;
                }
            }

            var stageButtonList = stageStackLayoutOne.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in stageButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryViewModel.StageScore = btn.Text;
                    break;
                }
            }
            stageButtonList = stageStackLayoutTwo.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in stageButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryViewModel.StageScore = btn.Text;
                    break;
                }
            }
            stageButtonList = stageStackLayoutThree.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in stageButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryViewModel.StageScore = btn.Text;
                    break;
                }
            }

            _patientPreSurgerySummaryViewModel.Volume = string.IsNullOrEmpty(VolumeTextBox.Text) ? (decimal?)null : decimal.Parse(VolumeTextBox.Text);
            _patientPreSurgerySummaryViewModel.IntIndexErectileFunction5 = string.IsNullOrEmpty(IIEF5TextBox.Text) ? (decimal?)null : decimal.Parse(IIEF5TextBox.Text);
            _patientPreSurgerySummaryViewModel.IntProstateSymptomScore = string.IsNullOrEmpty(IPSSTextBox.Text) ? (decimal?)null : decimal.Parse(IPSSTextBox.Text);
            _patientPreSurgerySummaryViewModel.PreSurgerySummaryId = Guid.NewGuid();
            _patientPreSurgerySummaryViewModel.ProcedureId = SurgicalConciergePatientViewModel.ProcedureId ?? 0;
            _patientPreSurgerySummaryViewModel.PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
            _patientPreSurgerySummaryViewModel.PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId;
            _patientPreSurgerySummaryViewModel.PracticeProfieId = SurgicalConciergePatientViewModel.PracticeProfileId;
            _patientPreSurgerySummaryViewModel.Notes = string.IsNullOrEmpty(NoteTextBox.Text) ? null : Convert.ToString(NoteTextBox.Text?.Trim());

            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    ApiExecutionResult result = await SavePreSurgerySummary(_patientPreSurgerySummaryViewModel);
                    if (result.Success)
                    {
                        UtilHelper.ShowToastMessage("Data saved successfully");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Data save failed");
                    }
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        //Change Cancer Location Layout Template - 12/6/2
        public async void OnCancerLocationTemplateTypeButton_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    cancerLocationGridFor16Items.Children.Clear();
                    var selectedButton = sender as Button;
                    if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
                    {
                        var buttonList = cancerLocationTemplateTypeStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                        foreach (var item in buttonList)
                        {
                            Button btn = item as Button;
                            btn.BackgroundColor = Color.FromHex("#FFF");
                            btn.TextColor = Color.FromHex("#007ACC");
                        }
                        selectedButton.BackgroundColor = Color.FromHex("#610094");
                        selectedButton.TextColor = Color.FromHex("#FFF");
                    }
                    else
                    {
                        selectedButton.BackgroundColor = Color.FromHex("#FFF");
                        selectedButton.TextColor = Color.FromHex("#007ACC");
                    }
                    if (selectedButton.ClassId.Contains(CancerLocationTemplateTypeId.Twelve))
                    {
                        _patientPreSurgerySummaryViewModel.CancerLocationTemplateType = int.Parse(CancerLocationTemplateTypeId.Twelve);
                        GenerateCancerLocationViewForTwelveItems(int.Parse(CancerLocationTemplateTypeId.Twelve));
                    }
                    else if (selectedButton.ClassId.Contains(CancerLocationTemplateTypeId.Six))
                    {
                        _patientPreSurgerySummaryViewModel.CancerLocationTemplateType = int.Parse(CancerLocationTemplateTypeId.Six);
                        GenerateCancerLocationViewForSixItems(int.Parse(CancerLocationTemplateTypeId.Six));
                    }
                    else if (selectedButton.ClassId.Contains(CancerLocationTemplateTypeId.Two))
                    {
                        _patientPreSurgerySummaryViewModel.CancerLocationTemplateType = int.Parse(CancerLocationTemplateTypeId.Two);
                        GenerateCancerLocationViewForTwoItems(int.Parse(CancerLocationTemplateTypeId.Two));
                    }
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void Grade_Clicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
            {
                if (selectedButton.ClassId.Contains("One"))
                {
                    var buttonList = gradeOneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                    foreach (var item in buttonList)
                    {
                        Button btn = item as Button;
                        btn.BackgroundColor = Color.FromHex("#FFF");
                        btn.TextColor = Color.FromHex("#007ACC");
                    }
                }
                else if (selectedButton.ClassId.Contains("Two"))
                {
                    var buttonList = gradeTwoStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                    foreach (var item in buttonList)
                    {
                        Button btn = item as Button;
                        btn.BackgroundColor = Color.FromHex("#FFF");
                        btn.TextColor = Color.FromHex("#007ACC");
                    }
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#007ACC");
            }
            var classId = selectedButton.ClassId;

        }
        private void Stage_Clicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
            {
                var buttonList = stageStackLayoutOne.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#007ACC");
                }
                buttonList = stageStackLayoutTwo.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#007ACC");
                }
                buttonList = stageStackLayoutThree.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#007ACC");
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#007ACC");
            }
        }
        private async void OnAddPiradButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SurgicalConciergePreSurgerySummaryPiradAddPage(_patientPreSurgerySummaryViewModel));
        }
        private async void GenerateCancerLocationViewForTwoItems(int cancerLocationTemplateType, Guid? preSurgerySummaryId = null)
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = Xamarin.Essentials.DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }

                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 750)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 960)
                {
                    IsiOSName5s = true;
                }
            }
            #endregion

            cancerLocationGridFor16Items.Children.Clear();
            var rowDefinations = new RowDefinitionCollection
            {
                new RowDefinition{Height = GridLength.Auto},
                new RowDefinition{Height = (IsiOSName5s == true ? 80 : 100)}
            };
            cancerLocationGridFor16Items.RowDefinitions = rowDefinations;
            var columnDefinations = new ColumnDefinitionCollection
            {
                new ColumnDefinition{Width = (IsiOSName5s == true ? 130 : 150)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 130 : 150)}
            };
            cancerLocationGridFor16Items.ColumnDefinitions = columnDefinations;
            Label left = new Label
            {
                Text = "Left",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };
            Label right = new Label
            {
                Text = "Right",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };
            cancerLocationGridFor16Items.Children.Add(left, 0, 0);
            cancerLocationGridFor16Items.Children.Add(right, 1, 0);

            _patientPreSurgerySummaryViewModel.CancerLocationViewModels = await GetAllCancerLocationsByCancerTemplateType(cancerLocationTemplateType, preSurgerySummaryId);
            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 2;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.LocationDispalyText,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#000"),
                        FontSize = (IsiOSName5s == true ? 14 : 18),
                        BackgroundColor = Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };
                    cancerLocationButton.Clicked += (sender, args) => OnCancerLocationButton_Clicked(sender, args, cancerLocation);

                    cancerLocationGridFor16Items.Children.Add(cancerLocationButton, columnCount, rowCount);
                    columnCount++;
                    if (columnCount == maxColumnCount)
                    {
                        columnCount = 0;
                        rowCount++;
                    }

                }
            }



        }
        private async void GenerateCancerLocationViewForSixItems(int cancerLocationTemplateType, Guid? preSurgerySummaryId = null)
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = Xamarin.Essentials.DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }

                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 750)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 960)
                {
                    IsiOSName5s = true;
                }
            }
            #endregion

            cancerLocationGridFor16Items.Children.Clear();
            var rowDefinations = new RowDefinitionCollection
            {
                new RowDefinition{Height = GridLength.Auto},
                new RowDefinition{Height = (IsiOSName5s == true ? 60 : 70)},
                new RowDefinition{Height = (IsiOSName5s == true ? 60 : 70)},
                new RowDefinition{Height = (IsiOSName5s == true ? 60 : 70)}
            };
            cancerLocationGridFor16Items.RowDefinitions = rowDefinations;
            var columnDefinations = new ColumnDefinitionCollection
            {
                new ColumnDefinition{Width = (IsiOSName5s == true ? 120 : 150)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 120 : 150)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 65 : 70)}
            };
            cancerLocationGridFor16Items.ColumnDefinitions = columnDefinations;
            Label left = new Label
            {
                Text = "Left",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };
            Label right = new Label
            {
                Text = "Right",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };

            Label baseLabel = new Label
            {
                Text = "Base",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = 0
            };
            cancerLocationGridFor16Items.Children.Add(baseLabel, 2, 1);
            Label middleLabel = new Label
            {
                Text = "Middle",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = (IsiOSName5s == true ? 65 : 70)
            };
            cancerLocationGridFor16Items.Children.Add(middleLabel, 2, 1);
            Label apexLabel = new Label
            {
                Text = "Apex",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = (IsiOSName5s == true ? 130 : 140)
            };
            cancerLocationGridFor16Items.Children.Add(apexLabel, 2, 1);


            cancerLocationGridFor16Items.Children.Add(left, 0, 0);
            //Grid.SetColumnSpan(left, 2);
            cancerLocationGridFor16Items.Children.Add(right, 1, 0);
            //Grid.SetColumnSpan(right, 2);

            _patientPreSurgerySummaryViewModel.CancerLocationViewModels = await GetAllCancerLocationsByCancerTemplateType(cancerLocationTemplateType, preSurgerySummaryId);
            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 2;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.LocationDispalyText,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#000"),
                        FontSize = (IsiOSName5s == true ? 14 : 18),
                        BackgroundColor = Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };
                    cancerLocationButton.Clicked += (sender, args) => OnCancerLocationButton_Clicked(sender, args, cancerLocation);

                    cancerLocationGridFor16Items.Children.Add(cancerLocationButton, columnCount, rowCount);
                    columnCount++;
                    if (columnCount == maxColumnCount)
                    {
                        columnCount = 0;
                        rowCount++;
                    }

                }
            }



        }
        private async void GenerateCancerLocationViewForTwelveItems(int cancerLocationTemplateType, Guid? preSurgerySummaryId = null)
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = Xamarin.Essentials.DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }

                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 750)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 960)
                {
                    IsiOSName5s = true;
                }
            }
            #endregion

            cancerLocationGridFor16Items.Children.Clear();
            var rowDefinations = new RowDefinitionCollection
            {
                new RowDefinition{Height = GridLength.Auto},
                new RowDefinition{Height = (IsiOSName5s == true ? 55 : 70)},
                new RowDefinition{Height = (IsiOSName5s == true ? 55 : 70)},
                new RowDefinition{Height = (IsiOSName5s == true ? 55 : 70)},
                new RowDefinition{Height = 50}
            };
            cancerLocationGridFor16Items.RowDefinitions = rowDefinations;
            var columnDefinations = new ColumnDefinitionCollection
            {
                new ColumnDefinition{Width = (IsiOSName5s == true ? 55 : 70)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 55 : 70)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 55 : 70)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 55 : 70)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 70 : 80)}
            };
            cancerLocationGridFor16Items.ColumnDefinitions = columnDefinations;
            Label left = new Label
            {
                Text = "Left",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };
            Label right = new Label
            {
                Text = "Right",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
            };

            Label baseLabel = new Label
            {
                Text = "Base",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = 0
            };
            cancerLocationGridFor16Items.Children.Add(baseLabel, 4, 1);
            Label middleLabel = new Label
            {
                Text = "Middle",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = (IsiOSName5s == true ? 62 : 70)
            };
            cancerLocationGridFor16Items.Children.Add(middleLabel, 4, 1);
            Label apexLabel = new Label
            {
                Text = "Apex",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18),
                Rotation = 90,
                TranslationY = (IsiOSName5s == true ? 125 : 140)
            };
            cancerLocationGridFor16Items.Children.Add(apexLabel, 4, 1);

            Label lateralLabelLeft = new Label
            {
                Text = "Lateral",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18)
            };
            Label lateralLabelRight = new Label
            {
                Text = "Lateral",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18)
            };
            cancerLocationGridFor16Items.Children.Add(lateralLabelLeft, 0, 4);
            cancerLocationGridFor16Items.Children.Add(lateralLabelRight, 3, 4);

            Label medialLabel = new Label
            {
                Text = "Medial",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 14 : 18)
            };

            cancerLocationGridFor16Items.Children.Add(medialLabel, 1, 4);
            Grid.SetColumnSpan(medialLabel, 2);


            cancerLocationGridFor16Items.Children.Add(left, 0, 0);
            Grid.SetColumnSpan(left, 2);
            cancerLocationGridFor16Items.Children.Add(right, 2, 0);
            Grid.SetColumnSpan(right, 2);
            _patientPreSurgerySummaryViewModel.CancerLocationViewModels = await GetAllCancerLocationsByCancerTemplateType(cancerLocationTemplateType, preSurgerySummaryId);
            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 4;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.LocationDispalyText,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#000"),
                        FontSize = (IsiOSName5s == true ? 14 : 18),
                        BackgroundColor = Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };
                    cancerLocationButton.Clicked += (sender, args) => OnCancerLocationButton_Clicked(sender, args, cancerLocation);

                    cancerLocationGridFor16Items.Children.Add(cancerLocationButton, columnCount, rowCount);
                    columnCount++;
                    if (columnCount == maxColumnCount)
                    {
                        columnCount = 0;
                        rowCount++;
                    }

                }
            }



        }

        private async void OnCancerLocationButton_Clicked(object sender, EventArgs e, CancerLocationViewModel cancerLocation)
        {
            await Navigation.PushModalAsync(new SurgicalConciergeCancerLocationAddPage(cancerLocation));
        }

        private void SubscribeToEvents()
        {
            MessagingCenter.Unsubscribe<SurgicalConciergeCancerLocationAddPage>(this, "UpdateCancerLocation");
            MessagingCenter.Unsubscribe<SurgicalConciergePreSurgerySummaryPiradAddPage>(this, "AddPiradLesion");
            MessagingCenter.Unsubscribe<SurgicalConciergePreSurgerySummaryPiradEditPage>(this, "UpdatePiradLesion");
            MessagingCenter.Unsubscribe<SurgicalConciergePreSurgerySummaryPiradEditPage, PatientPreSurgerySummaryPiradViewModel>(this, "DeletePiradLesion");

            MessagingCenter.Subscribe<SurgicalConciergeCancerLocationAddPage>(this, "UpdateCancerLocation", (arg) =>
            {
                UpdateCancerLocationMaps();
            });

            MessagingCenter.Subscribe<SurgicalConciergePreSurgerySummaryPiradAddPage>(this, "AddPiradLesion", (arg) =>
            {
                GeneratePiradListView(_patientPreSurgerySummaryViewModel);
            });

            MessagingCenter.Subscribe<SurgicalConciergePreSurgerySummaryPiradEditPage>(this, "UpdatePiradLesion", (arg) =>
            {
                GeneratePiradListView(_patientPreSurgerySummaryViewModel);
            });

            MessagingCenter.Subscribe<SurgicalConciergePreSurgerySummaryPiradEditPage, PatientPreSurgerySummaryPiradViewModel>(this, "DeletePiradLesion", (sender, deletedPirad) =>
            {
                RemovePiradItem(deletedPirad);
            });

        }

        private void RemovePiradItem(PatientPreSurgerySummaryPiradViewModel deletedPirad)
        {
            _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Remove(deletedPirad);
            GeneratePiradListView(_patientPreSurgerySummaryViewModel);
        }

        private void UpdateCancerLocationMaps()
        {
            try
            {
                var buttonList = cancerLocationGridFor16Items.Children.Where(c => c.GetType() == typeof(Button));
                if (buttonList != null)
                {
                    foreach (var item in buttonList)
                    {
                        Button btn = item as Button;
                        var bindedcancerLocation = btn.BindingContext as CancerLocationViewModel;
                        if (bindedcancerLocation != null && bindedcancerLocation.IsChecked)
                        {
                            btn.BackgroundColor = Color.FromHex("#610094");
                            btn.TextColor = Color.FromHex("#FFF");
                        }
                        else
                        {
                            btn.BackgroundColor = Color.FromHex("#FFF");
                            btn.TextColor = Color.FromHex("#007ACC");
                        }

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task<List<CancerLocationViewModel>> GetAllCancerLocationsByCancerTemplateType(int cancerLocationTemplateType, Guid? preSurgerySummaryId = null)
        {
            try
            {
                return await _restApiService.GetPreSurgerySummaryGetCancerLocations(cancerLocationTemplateType, preSurgerySummaryId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ApiExecutionResult> SavePreSurgerySummary(PatientPreSurgerySummaryViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                apiExecutionResult = await _restApiService.SavePreSurgerySummary(model);
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubscribeToEvents();
        }



    }


}