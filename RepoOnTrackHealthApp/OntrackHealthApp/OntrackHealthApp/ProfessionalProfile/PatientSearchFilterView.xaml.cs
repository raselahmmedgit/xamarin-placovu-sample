﻿using Acr.UserDialogs;
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

namespace OntrackHealthApp.ProfessionalProfile
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

        ProfessionalPatientPage _professionalPatientPage;
        public PatientSearchFilterView(ProfessionalPatientPage professionalPatientPage)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                this._professionalPatientPage = professionalPatientPage;

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

                    App.HideUserDialog();
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
                var practiceProfileCheckBoxListViewModels = surgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews;
                if (practiceProfileCheckBoxListViewModels != null)
                {
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

                    foreach (var item in practiceProfileCheckBoxListViewModels)
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
                var professionalProfileCheckBoxListViewModels = surgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews;
                if (professionalProfileCheckBoxListViewModels != null)
                {
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

                    foreach (var item in professionalProfileCheckBoxListViewModels)
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
                var procedureCheckBoxListViewModels = surgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews;
                if (procedureCheckBoxListViewModels != null)
                {
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

                    foreach (var item in procedureCheckBoxListViewModels)
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
                    //if (string.IsNullOrEmpty(SelectedProcedure))
                    //{
                    //    SelectedProcedure = idValue;
                    //}
                    //else
                    //{
                    //    SelectedProcedure = "," + idValue;
                    //}

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
                    var practiceProfileCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.PracticeProfileCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.PracticeProfileId.ToString()).ToArray();
                    SelectedPracticeProfile = string.Join(",", practiceProfileCheckBoxListViewModels);
                }

                if (SurgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews != null)
                {
                    var professionalProfileCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.ProfessionalProfileCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.ProfessionalProfileId.ToString()).ToArray();
                    SelectedProfessionalProfile = string.Join(",", professionalProfileCheckBoxListViewModels);
                }

                if (SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews != null)
                {
                    var procedureCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.ProcedureId.ToString()).ToArray();
                    SelectedProcedure = string.Join(",", procedureCheckBoxListViewModels);
                }

                if (SurgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews != null)
                {
                    var locationCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.PracticeLocationCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.PracticeLocationId.ToString()).ToArray();
                    SelectedPracticeLocation = string.Join(",", locationCheckBoxListViewModels);
                }
            }

            _professionalPatientPage.PracticeName = string.Empty;
            _professionalPatientPage.ProfessionalName = string.Empty;
            _professionalPatientPage.PatientName = string.Empty;
            _professionalPatientPage.PatientEmail = string.Empty;
            _professionalPatientPage.DateofBirth = string.Empty;
            _professionalPatientPage.PatientPhoneCode = string.Empty;
            _professionalPatientPage.PatientPhone = string.Empty;
            _professionalPatientPage.SurgeryDate = _professionalPatientPage.SurgeryDate;
            _professionalPatientPage.PastDay = _professionalPatientPage.SelectedDate ?? DateTime.UtcNow;

            _professionalPatientPage.SelectedPracticeProfile = SelectedPracticeProfile;
            _professionalPatientPage.SelectedProfessionalProfile = SelectedProfessionalProfile;
            _professionalPatientPage.SelectedProcedure = SelectedProcedure;
            _professionalPatientPage.SelectedPracticeLocation = SelectedPracticeLocation;

            await Navigation.PopModalAsync();
            _professionalPatientPage.ReLoadData();
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