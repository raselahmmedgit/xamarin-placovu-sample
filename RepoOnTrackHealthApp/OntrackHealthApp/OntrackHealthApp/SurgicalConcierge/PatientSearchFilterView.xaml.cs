using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
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
    public partial class PatientSearchFilterView : CustomModalContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeSidebarSearchViewModel SurgicalConciergeSidebarSearchViewModel { get; set; }

        private string SelectedPracticeProfile = string.Empty;
        private string SelectedProfessionalProfile = string.Empty;
        private string SelectedProcedure = string.Empty;
        private string SelectedPracticeLocation = string.Empty;

        SurgicalConciergePatientView _surgicalConciergePatientView;
        public PatientSearchFilterView(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();

                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

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

                this._surgicalConciergePatientView = surgicalConciergePatientView;

                SelectedPracticeProfile = _surgicalConciergePatientView.SelectedPracticeProfile;
                SelectedProfessionalProfile = _surgicalConciergePatientView.SelectedProfessionalProfile;
                SelectedProcedure = _surgicalConciergePatientView.SelectedProcedure;
                SelectedPracticeLocation = _surgicalConciergePatientView.SelectedPracticeLocation;
                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        SurgicalConciergePatientPage _surgicalConciergePatientPage;
        public PatientSearchFilterView(SurgicalConciergePatientPage surgicalConciergePatientPage)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

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

                this._surgicalConciergePatientPage = surgicalConciergePatientPage;

                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }

        }

        private async void LoadDataAsyc()
        {
            App.ShowUserDialog();
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    var restApiService = new SurgicalConciergeRestApiService();
                    var _surgicalConciergeSidebarSearchViewModels = await restApiService.GetSurgicalConciergeSidebarSearchViewModels();

                    if (_surgicalConciergeSidebarSearchViewModels != null)
                    {
                        SurgicalConciergeSidebarSearchViewModel = _surgicalConciergeSidebarSearchViewModels;
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
                    }

                    CreateForm();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private void CreateForm()
        {
            StackLayoutMainBlock.Children.Clear();

            var surgicalConciergeSidebarSearchViewModel = SurgicalConciergeSidebarSearchViewModel;
            if (surgicalConciergeSidebarSearchViewModel != null)
            {
                //Practice
                var practiceProfileCheckBoxListViews = surgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews;
                if (practiceProfileCheckBoxListViews != null)
                {
                    if (!string.IsNullOrEmpty(SelectedPracticeProfile))
                    {
                        var practiceProfiles = SelectedPracticeProfile.Split(',').ToList().ConvertAll(s => long.Parse(s));
                        practiceProfileCheckBoxListViews.Where(w => practiceProfiles.Contains(w.PracticeProfileId)).ToList().ForEach(c =>
                        {
                            if (c.IsChecked == false)
                            {
                                c.IsChecked = true;
                            }
                        });
                    }

                    #region Practice

                    int loopOne = 0;

                    StackLayout practiceStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    #region Header

                    StackLayout practiceHeaderStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 5)
                    };

                    HtmlLabel labelHeaderTitle = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = "Practice",
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    practiceHeaderStackLayout.Children.Add(labelHeaderTitle);

                    practiceStackLayout.Children.Add(practiceHeaderStackLayout);

                    #endregion

                    #region Body

                    StackLayout practiceBodyStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(15, 10),
                        BackgroundColor = Color.FromHex("#d6f7fe")
                    };

                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                    foreach (var item in practiceProfileCheckBoxListViews)
                    {
                        string classId = item.PracticeProfileId.ToString();

                        //Checkbox
                        classId = "PracticeProfileId_" + classId;
                        CheckBox check = new CheckBox
                        {
                            Text = item.PracticeName,
                            BoxSizeRequest = 24,
                            TextFontSize = 19,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ClassId = classId,
                            Spacing = 5,
                            IsChecked = item.IsChecked,
                            CheckChangedCommand = new Command((tag) => {
                                CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                            }),
                            FontFamily = "Fonts/georgia.ttf#georgia",
                            TextColor = Color.FromHex("#222"),
                            Padding = new Thickness(10, 0)
                        };

                        radioButtonGroupView.Children.Add(check);
                        BoxView bxCheck = new BoxView
                        {
                            BackgroundColor = Color.FromHex("#d6f7fe"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        radioButtonGroupView.Children.Add(bxCheck);

                        loopOne += 1;
                    }//

                    practiceBodyStackLayout.Children.Add(radioButtonGroupView);

                    practiceStackLayout.Children.Add(practiceBodyStackLayout);

                    #endregion

                    StackLayoutMainBlock.Children.Add(practiceStackLayout);

                    #endregion
                }

                //Professional
                var professionalProfileCheckBoxListViews = surgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews;
                if (professionalProfileCheckBoxListViews != null)
                {
                    if (!string.IsNullOrEmpty(SelectedProfessionalProfile))
                    {
                        var professionalProfiles = SelectedProfessionalProfile.Split(',').ToList().ConvertAll(s => long.Parse(s));
                        professionalProfileCheckBoxListViews.Where(w => professionalProfiles.Contains(w.ProfessionalProfileId)).ToList().ForEach(c =>
                        {
                            if (c.IsChecked == false)
                            {
                                c.IsChecked = true;
                            }
                        });
                    }

                    #region Professional

                    int loopOne = 0;

                    StackLayout professionalStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    #region Header

                    StackLayout professionalHeaderStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 5)
                    };

                    HtmlLabel labelHeaderTitle = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = "Professional",
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    professionalHeaderStackLayout.Children.Add(labelHeaderTitle);

                    professionalStackLayout.Children.Add(professionalHeaderStackLayout);

                    #endregion

                    #region Body

                    StackLayout professionalBodyStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(15, 10),
                        BackgroundColor = Color.FromHex("#d6f7fe")
                    };

                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                    foreach (var item in professionalProfileCheckBoxListViews)
                    {
                        string classId = item.ProfessionalProfileId.ToString();

                        //Checkbox
                        classId = "ProfessionalProfileId_" + classId;
                        CheckBox check = new CheckBox
                        {
                            Text = item.ProfessionalProfileName,
                            BoxSizeRequest = 24,
                            TextFontSize = 19,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ClassId = classId,
                            Spacing = 5,
                            IsChecked = item.IsChecked,
                            CheckChangedCommand = new Command((tag) => {
                                CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                            }),
                            FontFamily = "Fonts/georgia.ttf#georgia",
                            TextColor = Color.FromHex("#222"),
                            Padding = new Thickness(10, 0)
                        };

                        radioButtonGroupView.Children.Add(check);
                        BoxView bxCheck = new BoxView
                        {
                            BackgroundColor = Color.FromHex("#d6f7fe"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        radioButtonGroupView.Children.Add(bxCheck);

                        loopOne += 1;
                    }//

                    professionalBodyStackLayout.Children.Add(radioButtonGroupView);

                    professionalStackLayout.Children.Add(professionalBodyStackLayout);

                    #endregion

                    StackLayoutMainBlock.Children.Add(professionalStackLayout);

                    #endregion
                }

                //Procedure
                var procedureCheckBoxListViews = surgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews;
                if (procedureCheckBoxListViews != null)
                {
                    if (!string.IsNullOrEmpty(SelectedProcedure))
                    {
                        var procedureIds = SelectedProcedure.Split(',').ToList().ConvertAll(s => long.Parse(s));
                        procedureCheckBoxListViews.Where(w => procedureIds.Contains(w.ProcedureId)).ToList().ForEach(c =>
                        {
                            if (c.IsChecked == false)
                            {
                                c.IsChecked = true;
                            }
                        });
                    }

                    #region Procedure

                    int loopOne = 0;

                    StackLayout procedureStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    #region Header

                    StackLayout procedureHeaderStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 5)
                    };

                    HtmlLabel labelHeaderTitle = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = "Procedure",
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    procedureHeaderStackLayout.Children.Add(labelHeaderTitle);

                    procedureStackLayout.Children.Add(procedureHeaderStackLayout);

                    #endregion

                    #region Body

                    StackLayout procedureBodyStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(15, 10),
                        BackgroundColor = Color.FromHex("#d6f7fe")
                    };

                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                    foreach (var item in procedureCheckBoxListViews)
                    {
                        string classId = item.ProcedureId.ToString();

                        //Checkbox
                        classId = "ProcedureId_" + classId;
                        CheckBox check = new CheckBox
                        {
                            Text = item.ProcedureName,
                            BoxSizeRequest = 24,
                            TextFontSize = 19,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ClassId = classId,
                            Spacing = 5,
                            IsChecked = item.IsChecked,
                            CheckChangedCommand = new Command((tag) => {
                                CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                            }),
                            FontFamily = "Fonts/georgia.ttf#georgia",
                            TextColor = Color.FromHex("#222"),
                            Padding = new Thickness(10, 0)
                        };

                        radioButtonGroupView.Children.Add(check);
                        BoxView bxCheck = new BoxView
                        {
                            BackgroundColor = Color.FromHex("#d6f7fe"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        radioButtonGroupView.Children.Add(bxCheck);

                        loopOne += 1;
                    }//

                    procedureBodyStackLayout.Children.Add(radioButtonGroupView);

                    procedureStackLayout.Children.Add(procedureBodyStackLayout);

                    #endregion

                    StackLayoutMainBlock.Children.Add(procedureStackLayout);

                    #endregion
                }

                //Location
                var locationCheckBoxListViewModels = surgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews;
                if (locationCheckBoxListViewModels != null)
                {
                    if (!string.IsNullOrEmpty(SelectedPracticeLocation))
                    {
                        var practiceLocationIds = SelectedPracticeLocation.Split(',').ToList().ConvertAll(s => Guid.Parse(s));
                        locationCheckBoxListViewModels.Where(w => practiceLocationIds.Contains(w.PracticeLocationId)).ToList().ForEach(c =>
                        {
                            if (c.IsChecked == false)
                            {
                                c.IsChecked = true;
                            }
                        });
                    }
                    #region Location

                    int loopOne = 0;

                    StackLayout locationStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    #region Header

                    StackLayout locationHeaderStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 5)
                    };

                    HtmlLabel labelHeaderTitle = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = "Hospital",
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    locationHeaderStackLayout.Children.Add(labelHeaderTitle);

                    locationStackLayout.Children.Add(locationHeaderStackLayout);

                    #endregion

                    #region Body

                    StackLayout locationBodyStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(15, 10),
                        BackgroundColor = Color.FromHex("#d6f7fe")
                    };

                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                    foreach (var item in locationCheckBoxListViewModels)
                    {
                        string classId = item.PracticeLocationId.ToString();

                        //Checkbox
                        classId = "PracticeLocationId_" + classId;
                        CheckBox check = new CheckBox
                        {
                            Text = item.LocationName,
                            BoxSizeRequest = 24,
                            TextFontSize = 19,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ClassId = classId,
                            Spacing = 5,
                            IsChecked = item.IsChecked,
                            CheckChangedCommand = new Command((tag) => {
                                CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                            }),
                            FontFamily = "Fonts/georgia.ttf#georgia",
                            TextColor = Color.FromHex("#222"),
                            Padding = new Thickness(10, 0)
                        };

                        radioButtonGroupView.Children.Add(check);
                        BoxView bxCheck = new BoxView
                        {
                            BackgroundColor = Color.FromHex("#d6f7fe"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        radioButtonGroupView.Children.Add(bxCheck);

                        loopOne += 1;
                    }//

                    locationBodyStackLayout.Children.Add(radioButtonGroupView);

                    locationStackLayout.Children.Add(locationBodyStackLayout);

                    #endregion

                    StackLayoutMainBlock.Children.Add(locationStackLayout);

                    #endregion
                }
            }
            else
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
            }

            App.HideUserDialog();
        }

        private void CheckBoxCheckChangedCommand(object value, RadioButtonGroupView parent)
        {
            var classIdArr = value.ToString().Split('_').ToArray();

            if (classIdArr != null)
            {
                string id = classIdArr[0].ToString();
                string idValue = classIdArr[1].ToString();

                if (id == "PracticeProfileId")
                {
                    //if (string.IsNullOrEmpty(SelectedPracticeProfile)) {
                    //    SelectedPracticeProfile = idValue;
                    //}
                    //else {
                    //    SelectedPracticeProfile = "," + idValue;
                    //}

                    SurgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews.Where(s => s.PracticeProfileId == Convert.ToInt64(idValue)).ToList().ForEach(c =>
                    {
                        if (c.IsChecked == true)
                        {
                            c.IsChecked = false;
                        }
                        else
                        {
                            c.IsChecked = true;
                        }
                    });

                }
                else if (id == "ProfessionalProfileId")
                {
                    //if (string.IsNullOrEmpty(SelectedProfessionalProfile))
                    //{
                    //    SelectedProfessionalProfile = idValue;
                    //}
                    //else
                    //{
                    //    SelectedProfessionalProfile = "," + idValue;
                    //}

                    SurgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews.Where(s => s.ProfessionalProfileId == Convert.ToInt64(idValue)).ToList().ForEach(c =>
                    {
                        if (c.IsChecked == true)
                        {
                            c.IsChecked = false;
                        }
                        else
                        {
                            c.IsChecked = true;
                        }
                    });
                }
                else if (id == "ProcedureId")
                {
                    //if (string.IsNullOrEmpty(SelectedProcedure))
                    //{
                    //    SelectedProcedure = idValue;
                    //}
                    //else
                    //{
                    //    SelectedProcedure = "," + idValue;
                    //}

                    SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.Where(s => s.ProcedureId == Convert.ToInt64(idValue)).ToList().ForEach(c =>
                    {
                        if (c.IsChecked == true)
                        {
                            c.IsChecked = false;
                        }
                        else
                        {
                            c.IsChecked = true;
                        }
                    });
                }
                else if (id == "PracticeLocationId")
                {
                    SurgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews.Where(s => s.PracticeLocationId == (idValue).ToGuid()).ToList().ForEach(c =>
                    {
                        if (c.IsChecked == true)
                        {
                            c.IsChecked = false;
                        }
                        else
                        {
                            c.IsChecked = true;
                        }
                    });
                }

            }// Data

            //ReLoadData();

        }

        private async void ReLoadData()
        {
            if (SurgicalConciergeSidebarSearchViewModel != null)
            {
                if (SurgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews != null)
                {
                    var practiceProfileCheckBoxListViews = SurgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.PracticeProfileId.ToString()).ToArray();
                    SelectedPracticeProfile = string.Join(",", practiceProfileCheckBoxListViews);
                }

                if (SurgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews != null)
                {
                    var professionalProfileCheckBoxListViews = SurgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.ProfessionalProfileId.ToString()).ToArray();
                    SelectedProfessionalProfile = string.Join(",", professionalProfileCheckBoxListViews);
                }

                if (SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews != null)
                {
                    var procedureCheckBoxListViews = SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.ProcedureId.ToString()).ToArray();
                    SelectedProcedure = string.Join(",", procedureCheckBoxListViews);
                }
                if (SurgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews != null)
                {
                    var locationCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.PracticeLocationId.ToString()).ToArray();
                    SelectedPracticeLocation = string.Join(",", locationCheckBoxListViewModels);
                }
            }

            _surgicalConciergePatientView.PracticeName = string.Empty;
            _surgicalConciergePatientView.ProfessionalName = string.Empty;
            _surgicalConciergePatientView.PatientName = string.Empty;
            _surgicalConciergePatientView.PatientEmail = string.Empty;
            _surgicalConciergePatientView.DateofBirth = string.Empty;
            _surgicalConciergePatientView.PatientPhoneCode = string.Empty;
            _surgicalConciergePatientView.PatientPhone = string.Empty;
            _surgicalConciergePatientView.SurgeryDate = _surgicalConciergePatientView.SurgeryDate;
            _surgicalConciergePatientView.PastDay = _surgicalConciergePatientView.SelectedDate ?? DateTime.UtcNow;

            _surgicalConciergePatientView.SelectedPracticeProfile = SelectedPracticeProfile;
            _surgicalConciergePatientView.SelectedProfessionalProfile = SelectedProfessionalProfile;
            _surgicalConciergePatientView.SelectedProcedure = SelectedProcedure;
            _surgicalConciergePatientView.SelectedPracticeLocation = SelectedPracticeLocation;
            await Navigation.PopModalAsync();
            _surgicalConciergePatientView.ReLoadData();
        }

        private void PatientSearchFilterSumbitButtonClicked(object sender, EventArgs e)
        {
            ReLoadData();
        }

        private async void PatientSearchFilterCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                base.OnAppearing();
            }
        }
    }
}