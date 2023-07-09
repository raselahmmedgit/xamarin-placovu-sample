using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
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
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePreSurgerySummaryPage : ContentPage
    {
        private SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel;
        private readonly ITokenContainer _iTokenContainer;
        private PatientPreSurgerySummaryViewModel _patientPreSurgerySummaryViewModel;
        private SurgicalConciergeRestApiService _restApiService;

        public SurgicalConciergePreSurgerySummaryPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel)
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
            PopulateSummaryPage();
        }

        private void PopulateSummaryPage()
        {
            PreopPsa.Text = _patientPreSurgerySummaryViewModel.DisplayPreopPsa;
            IntIndexErectileFunction5.Text = _patientPreSurgerySummaryViewModel.DisplayIntIndexErectileFunction5;
            IntProstateSymptomScore.Text = _patientPreSurgerySummaryViewModel.DisplayIntProstateSymptomScore;
            GleasonScore.Text = _patientPreSurgerySummaryViewModel.DisplayGleasonScore;
            StageScore.Text = _patientPreSurgerySummaryViewModel.DisplayStageScore;
            Volume.Text = _patientPreSurgerySummaryViewModel.DisplayVolume;
            Note.Text = _patientPreSurgerySummaryViewModel.Notes;
            NoteStackLayout.IsVisible = _patientPreSurgerySummaryViewModel.HasNotes;

            if (_patientPreSurgerySummaryViewModel != null)
            {
                if (_patientPreSurgerySummaryViewModel.CancerLocationTemplateType == int.Parse(CancerLocationTemplateTypeId.Twelve))
                {
                    GenerateCancerLocationViewForTwelveItems();
                }
                else if (_patientPreSurgerySummaryViewModel.CancerLocationTemplateType == int.Parse(CancerLocationTemplateTypeId.Six))
                {
                    GenerateCancerLocationViewForSixItems();
                }
                else if (_patientPreSurgerySummaryViewModel.CancerLocationTemplateType == int.Parse(CancerLocationTemplateTypeId.Two))
                {
                    GenerateCancerLocationViewForTwoItems();
                }
                PopulatePiradLesionData();
            }
        }

        private void PopulatePiradLesionData()
        {
            piradLesionListGrid.Children.Clear();
            int columnDefinationCount = 0;
            if (_patientPreSurgerySummaryViewModel != null && _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels != null)
            {
                columnDefinationCount = _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Count(c => c.IsDeleted == false);
                _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels = _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Where(c => c.IsDeleted == false).ToList();
            }
            if (columnDefinationCount > 0)
            {
                piradLesionListStackLayout.IsVisible = true;

                string[] piradLabels = { "Score", "Location", "Zone", "Grade", "Capsule", "Volume(cc)" };
                //var rowDefinations = new RowDefinitionCollection
                //{
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //    new RowDefinition{Height = 40},
                //};
                //piradLesionListGrid.RowDefinitions = rowDefinations;

                //var columnDefinations = new ColumnDefinitionCollection
                //{
                //    new ColumnDefinition{ Width = 90 }
                //};

                //for (int i = 0; i < columnDefinationCount; i++)
                //{
                //    columnDefinations.Add(new ColumnDefinition { Width = 70 });
                //}

                //piradLesionListGrid.ColumnDefinitions = columnDefinations;

                //for (int labelCount = 0; labelCount < piradLabels.Length; labelCount++)
                //{
                //    Label piradLabel = new Label
                //    {
                //        Text = piradLabels[labelCount],
                //        TextColor = Color.FromHex("#000"),
                //        FontSize = 16,
                //        HorizontalTextAlignment = TextAlignment.Start,
                //        VerticalTextAlignment = TextAlignment.Center,
                //    };
                //    piradLesionListGrid.Children.Add(piradLabel, 0, labelCount + 1);
                //}

                //for (int row = 0; row < columnDefinationCount; row++)
                //{
                //    Color evenRowColor = Color.FromHex("#FFFFFF");
                //    Color oddRowColor = Color.FromHex("#FFFFFF");
                //    Color textColor = Color.FromHex("#000");
                //    Color borderColor = Color.FromHex("#54F854");
                //    int borderWidth = 1;
                //    double fontSize = 16;
                //    int cornerRadius = 10;

                //    var singlePiradItem = _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels[row];
                //    if (singlePiradItem != null)
                //    {
                //        if (singlePiradItem.IsDeleted)
                //            continue;

                //        int columnCount = row + 1;

                //        Label slNoLabel = new Label
                //        {
                //            Text = "#" + columnCount,
                //            TextColor = Color.FromHex("#000"),
                //            FontSize = 16,
                //            HorizontalTextAlignment = TextAlignment.Start,
                //            VerticalTextAlignment = TextAlignment.Center,
                //        };
                //        piradLesionListGrid.Children.Add(slNoLabel, columnCount, 0);

                //        Button scoreButton = new Button
                //        {
                //            Text = singlePiradItem.PiradScore == null ? "N/A" : decimal.Round((decimal)singlePiradItem.PiradScore, 2).ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = oddRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };
                //        piradLesionListGrid.Children.Add(scoreButton, columnCount, 1);

                //        Button locationButton = new Button
                //        {
                //            Text = singlePiradItem.LesionLocation == null ? "N/A" : singlePiradItem.LesionLocation.ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = evenRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };

                //        piradLesionListGrid.Children.Add(locationButton, columnCount, 2);

                //        Button zoneButton = new Button
                //        {
                //            Text = singlePiradItem.LesionZone == null ? "N/A" : singlePiradItem.LesionZone.ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = oddRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };
                //        piradLesionListGrid.Children.Add(zoneButton, columnCount, 3);

                //        Button gradeButton = new Button
                //        {
                //            Text = singlePiradItem.DisplayLesionGrade == null ? "N/A" : singlePiradItem.DisplayLesionGrade.ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = evenRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };
                //        piradLesionListGrid.Children.Add(gradeButton, columnCount, 4);

                //        Button capsuleButton = new Button
                //        {
                //            Text = singlePiradItem.CapsularInvolvement == null ? "N/A" : singlePiradItem.CapsularInvolvement.ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = oddRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };
                //        piradLesionListGrid.Children.Add(capsuleButton, columnCount, 5);

                //        Button volumeButton = new Button
                //        {
                //            Text = singlePiradItem.PiradVolume == null ? "N/A" : decimal.Round((decimal)singlePiradItem.PiradVolume, 2).ToString(),
                //            TextColor = textColor,
                //            FontSize = fontSize,
                //            BackgroundColor = evenRowColor,
                //            BorderColor = borderColor,
                //            BorderWidth = borderWidth,
                //            CornerRadius = cornerRadius
                //        };
                //        piradLesionListGrid.Children.Add(volumeButton, columnCount, 6);
                //    }
                //}
                var rowDefinations = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width = 90},
                    new ColumnDefinition{Width = 90},
                    new ColumnDefinition{Width = 90},
                    new ColumnDefinition{Width = 90},
                };
                piradLesionListGrid.ColumnDefinitions = rowDefinations;

                var columnDefinations = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 80 },
                };

                for (int i = 0; i < columnDefinationCount; i++)
                {
                    columnDefinations.Add(new RowDefinition { Height = 40 });
                    columnDefinations.Add(new RowDefinition { Height = 40 });
                    columnDefinations.Add(new RowDefinition { Height = 40 });
                    columnDefinations.Add(new RowDefinition { Height = 80 });
                }

                piradLesionListGrid.RowDefinitions = columnDefinations;

                for (int row = 0; row < columnDefinationCount; row++)
                {
                    Color evenRowColor = Color.FromHex("#FFFFFF");
                    Color oddRowColor = Color.FromHex("#FFFFFF");
                    Color textColor = Color.FromHex("#000");
                    Color borderColor = Color.FromHex("#54F854");
                    int borderWidth = 1;
                    double fontSize = 16;
                    int cornerRadius = 10;

                    for (int labelCount = 0; labelCount < piradLabels.Length; labelCount++)
                    {
                        Label piradLabel = new Label
                        {
                            Text = piradLabels[labelCount],
                            TextColor = Color.FromHex("#000"),
                            FontSize = 16,
                            HorizontalTextAlignment = TextAlignment.End,
                            VerticalTextAlignment = TextAlignment.Center,
                        };
                        //piradLesionListGrid.Children.Add(piradLabel, 0, labelCount + 1);
                        if (labelCount > 2)
                        {
                            piradLesionListGrid.Children.Add(piradLabel, 2, (row * 4) + labelCount - 2);
                        }
                        else
                        {
                            piradLesionListGrid.Children.Add(piradLabel, 0, (row * 4) + labelCount + 1);
                        }
                    }

                    var singlePiradItem = _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels[row];
                    if (singlePiradItem != null)
                    {
                        if (singlePiradItem.IsDeleted)
                            continue;

                        int columnCount = 1;

                        Label slNoLabel = new Label
                        {
                            Text = "Lesion #" + (row + 1),
                            TextColor = Color.FromHex("#000"),
                            BackgroundColor = Color.FromHex("#E4E5FF"),

                            FontSize = 16,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(120, 20, 120, 20),
                        };
                        Grid.SetRow(slNoLabel, row * 4);
                        Grid.SetColumnSpan(slNoLabel, 4);
                        piradLesionListGrid.Children.Add(slNoLabel);

                        Button scoreButton = new Button
                        {
                            Text = singlePiradItem.PiradScore == null ? "N/A" : decimal.Round((decimal)singlePiradItem.PiradScore, 2).ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = oddRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius,
                        };
                        piradLesionListGrid.Children.Add(scoreButton, columnCount, (row * 4) + 1);

                        Button locationButton = new Button
                        {
                            Text = singlePiradItem.LesionLocation == null ? "N/A" : singlePiradItem.LesionLocation.ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = evenRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius
                        };

                        piradLesionListGrid.Children.Add(locationButton, columnCount, (row * 4) + 2);

                        Button zoneButton = new Button
                        {
                            Text = singlePiradItem.LesionZone == null ? "N/A" : singlePiradItem.LesionZone.ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = oddRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius
                        };
                        piradLesionListGrid.Children.Add(zoneButton, columnCount, (row * 4) + 3);

                        Button gradeButton = new Button
                        {
                            Text = singlePiradItem.DisplayLesionGrade == null ? "N/A" : singlePiradItem.DisplayLesionGrade.ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = evenRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius
                        };
                        piradLesionListGrid.Children.Add(gradeButton, columnCount + 2, (row * 4) + 1);

                        Button capsuleButton = new Button
                        {
                            Text = singlePiradItem.CapsularInvolvement == null ? "N/A" : singlePiradItem.CapsularInvolvement.ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = oddRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius
                        };
                        piradLesionListGrid.Children.Add(capsuleButton, columnCount + 2, (row * 4) + 2);

                        Button volumeButton = new Button
                        {
                            Text = singlePiradItem.PiradVolume == null ? "N/A" : decimal.Round((decimal)singlePiradItem.PiradVolume, 2).ToString(),
                            TextColor = textColor,
                            FontSize = fontSize,
                            BackgroundColor = evenRowColor,
                            BorderColor = borderColor,
                            BorderWidth = borderWidth,
                            CornerRadius = cornerRadius,
                        };
                        piradLesionListGrid.Children.Add(volumeButton, columnCount + 2, (row * 4) + 3);
                    }
                }
            }
            else
            {
                piradLesionListStackLayout.IsVisible = false;
            }
        }

        private void GenerateCancerLocationViewForTwoItems()
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }
                else
                {
                    IsiOSName5s = false;
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
                new RowDefinition{Height = 240}
            };
            cancerLocationGridFor16Items.RowDefinitions = rowDefinations;
            var columnDefinations = new ColumnDefinitionCollection
            {
                new ColumnDefinition{Width = (IsiOSName5s == true ? 140 : 160)},
                new ColumnDefinition{Width = (IsiOSName5s == true ? 140 : 160)}
            };
            cancerLocationGridFor16Items.ColumnDefinitions = columnDefinations;
            Label left = new Label
            {
                Text = "Left",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 20 : 24),
            };
            Label right = new Label
            {
                Text = "Right",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 20 : 24),
            };
            cancerLocationGridFor16Items.Children.Add(left, 0, 0);
            cancerLocationGridFor16Items.Children.Add(right, 1, 0);

            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 2;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.IsChecked ? decimal.Round((decimal)cancerLocation.Involved, 0) + "%" : string.Empty,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#FFF"),
                        FontSize = (IsiOSName5s == true ? 16 : 20),
                        BackgroundColor = cancerLocation.IsChecked ? Color.FromHex("#54F854") : Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };

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
        private void GenerateCancerLocationViewForSixItems()
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = DeviceInfo.Name;
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
                new ColumnDefinition{Width = 70}
            };
            cancerLocationGridFor16Items.ColumnDefinitions = columnDefinations;
            Label left = new Label
            {
                Text = "Left",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 16 : 18),
            };
            Label right = new Label
            {
                Text = "Right",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#000"),
                FontAttributes = FontAttributes.Bold,
                FontSize = (IsiOSName5s == true ? 16 : 18),
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
                TranslationY = (IsiOSName5s == true ? 55 : 70)
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
                TranslationY = (IsiOSName5s == true ? 105 : 140)
            };
            cancerLocationGridFor16Items.Children.Add(apexLabel, 2, 1);


            cancerLocationGridFor16Items.Children.Add(left, 0, 0);
            //Grid.SetColumnSpan(left, 2);
            cancerLocationGridFor16Items.Children.Add(right, 1, 0);
            //Grid.SetColumnSpan(right, 2);
            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 2;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.IsChecked ? decimal.Round((decimal)cancerLocation.Involved, 0) + "%" : string.Empty,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#FFF"),
                        FontSize = (IsiOSName5s == true ? 14 : 18),
                        BackgroundColor = cancerLocation.IsChecked ? Color.FromHex("#54F854") : Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };

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
        private void GenerateCancerLocationViewForTwelveItems()
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = DeviceInfo.Name;
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
                TranslationY = (IsiOSName5s == true ? 65 : 80)
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
                TranslationY = (IsiOSName5s == true ? 125 : 160)
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
            int rowCount = 1;
            int columnCount = 0;
            int maxColumnCount = 4;
            if (_patientPreSurgerySummaryViewModel.CancerLocationViewModels != null && _patientPreSurgerySummaryViewModel.CancerLocationViewModels.Any())
            {
                foreach (var cancerLocation in _patientPreSurgerySummaryViewModel.CancerLocationViewModels)
                {
                    Button cancerLocationButton = new Button
                    {
                        Text = cancerLocation.IsChecked ? decimal.Round((decimal)cancerLocation.Involved, 0) + "%" : string.Empty,
                        ClassId = cancerLocation.LocationId.ToString(),
                        TextColor = Color.FromHex("#FFF"),
                        FontSize = (IsiOSName5s == true ? 14 : 18),
                        BackgroundColor = cancerLocation.IsChecked ? Color.FromHex("#54F854") : Color.FromHex("#FFF"),
                        BindingContext = cancerLocation,
                    };
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
        private async void btnEditSummary_ClickedAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryEditPage(SurgicalConciergePatientViewModel, _patientPreSurgerySummaryViewModel));
            }
        }

        private void SubscribeToEvents()
        {
            //MessagingCenter.Unsubscribe<SurgicalConciergeCancerLocationAddPage>(this, "UpdateCancerLocation");
            MessagingCenter.Unsubscribe<SurgicalConciergePreSurgerySummaryPiradEditPage, PatientPreSurgerySummaryViewModel>(this, "UpdatePreSummary");

            MessagingCenter.Subscribe<SurgicalConciergePreSurgerySummaryEditPage, PatientPreSurgerySummaryViewModel>(this, "UpdatePreSummary", (sender, updatedPreSurgerySummary) =>
            {
                _patientPreSurgerySummaryViewModel = updatedPreSurgerySummary;
                PopulateSummaryPage();
            });
        }

        private async void OnXrayButton_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                if (_patientPreSurgerySummaryViewModel == null)
                {
                    UtilHelper.ShowToastMessage(AppConstant.NotFound);
                    return;
                }

                var patientPreSurgerySummaryXrayViewModelList = await _restApiService.GetPreSurgerySummaryXraysData(_patientPreSurgerySummaryViewModel.PreSurgerySummaryId.ToString());
                if (patientPreSurgerySummaryXrayViewModelList != null)
                {
                    await Navigation.PushModalAsync(new SurgicalConciergePreSurgerySummaryXrayPage(patientPreSurgerySummaryXrayViewModelList));
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void OnAssessmentPlanButton_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                if (_patientPreSurgerySummaryViewModel == null)
                {
                    UtilHelper.ShowToastMessage(AppConstant.NotFound);
                    return;
                }

                var patientPreSurgerySummaryAssessmentPlanViewModel = await _restApiService.GetPreSurgerySummaryAssessmentPlanData(_patientPreSurgerySummaryViewModel.PreSurgerySummaryId.ToString());
                if (patientPreSurgerySummaryAssessmentPlanViewModel != null)
                {
                    await Navigation.PushModalAsync(new SurgicalConciergePreSurgerySummaryAssessmentPlanPage(patientPreSurgerySummaryAssessmentPlanViewModel));
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubscribeToEvents();
        }

    }
}