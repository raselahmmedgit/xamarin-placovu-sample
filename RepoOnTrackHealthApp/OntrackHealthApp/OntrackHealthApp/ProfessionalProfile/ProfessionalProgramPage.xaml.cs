using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        public ProfessionalProgramPage ()
		{
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadDataAsync()
        {
            try
            {
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await App.ShowUserDialogDelayAsync();

                    ProgramProcedureViewModel programProcedureViewModel = await professionalProfileRestApiService.ProfessionalProgram();

                    if (programProcedureViewModel != null)
                    {
                        if (programProcedureViewModel.ProfessionalProcedureViewModels != null)
                        {
                            BuildProfessionalProcedure(programProcedureViewModel.ProfessionalProcedureViewModels);
                        }

                        if (programProcedureViewModel.Medications != null)
                        {
                            BuildMedication(programProcedureViewModel.Medications);
                        }

                        if (programProcedureViewModel.SurgicalConceirgeProcedures != null)
                        {
                            BuildSurgicalConceirgeProcedure(programProcedureViewModel.SurgicalConceirgeProcedures);
                        }

                        App.HideUserDialogAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        private void BuildProfessionalProcedure(List<ProfessionalProcedureViewModel> professionalProcedureViewModels)
        {
            ProfessionalProcedureStackLayout.Children.Clear();

            if (professionalProcedureViewModels.Count() == 0)
            {
                #region No Data Found

                ////No Data Found
                //StackLayout noDataFoundStackLayout = new StackLayout
                //{
                //    Padding = new Thickness(0, 0, 0, 0),
                //    Margin = new Thickness(0, 0, 0, 0),
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    VerticalOptions = LayoutOptions.FillAndExpand
                //};

                //Label noDataFoundLabel = new Label
                //{
                //    Text = "No record(s) found.",
                //    TextColor = Color.FromHex("#000"),
                //    FontSize = 16,
                //    HorizontalTextAlignment = TextAlignment.Start,
                //    HorizontalOptions = LayoutOptions.StartAndExpand,
                //    VerticalOptions = LayoutOptions.Center
                //};
                //noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                ////No Data Found

                //ProfessionalProcedureStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                StackLayout procedureStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                foreach (var procedureGroupByType in professionalProcedureViewModels.GroupBy(g => g.ProcedureTypeName))
                {
                    #region Title

                    //Procedure Title
                    StackLayout titleStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 10),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    string titleName = (procedureGroupByType.Key + " (press icon to visualize)");

                    Label titleLabel = new Label
                    {
                        Text = titleName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 16,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    titleStackLayout.Children.Add(titleLabel);
                    //Procedure Title

                    StackLayout titleLineStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 10),
                        BackgroundColor = Color.FromHex("#002E5D"),
                        HeightRequest = 5,
                        Spacing = 0
                    };

                    #endregion

                    #region Header

                    StackLayout procedureHeaderStackLayout = new StackLayout
                    {
                        BackgroundColor = Color.FromHex("#D7D7D7"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    //Procedure List Header
                    Grid headerGrid = new Grid();
                    headerGrid.Margin = new Thickness(0, 0, 0, 0);
                    headerGrid.Padding = new Thickness(0, 0, 0, 0);
                    headerGrid.ColumnSpacing = 0;
                    headerGrid.RowSpacing = 0;
                    headerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                    headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                    headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

                    StackLayout headerProcedureStackLayout = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        Spacing = 0
                    };
                    Label headerProcedureLabel = new Label
                    {
                        Text = "Procedure",
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    headerProcedureStackLayout.Children.Add(headerProcedureLabel);
                    headerGrid.Children.Add(headerProcedureStackLayout, 0, 0); //c,r

                    StackLayout headerNotificationStackLayout = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        Spacing = 0
                    };
                    Label headerNotificationLabel = new Label
                    {
                        Text = "Notification",
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    headerNotificationStackLayout.Children.Add(headerNotificationLabel);
                    headerGrid.Children.Add(headerNotificationStackLayout, 1, 0); //c,r

                    StackLayout headerResourceStackLayout = new StackLayout
                    {
                        Padding = new Thickness(15, 0, 0, 0),
                        Spacing = 0
                    };
                    Label headerResourceLabel = new Label
                    {
                        Text = "Resource",
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    headerResourceStackLayout.Children.Add(headerResourceLabel);
                    headerGrid.Children.Add(headerResourceStackLayout, 2, 0); //c,r

                    procedureHeaderStackLayout.Children.Add(headerGrid);
                    //Procedure List Header

                    #endregion

                    StackLayout procedureItemStackLayout = new StackLayout
                    {
                        BackgroundColor = Color.FromHex("#ffffff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 0)
                    };

                    foreach (var item in procedureGroupByType.ToList().Where(m => m.IsDeleted == false))
                    {
                        #region Item

                        Grid procedureGrid = new Grid();
                        procedureGrid.Margin = new Thickness(0, 0, 0, 0);
                        procedureGrid.Padding = new Thickness(0, 0, 0, 0);
                        procedureGrid.ColumnSpacing = 0;
                        procedureGrid.RowSpacing = 0;
                        procedureGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                        procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                        procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

                        //Procedure List Item
                        StackLayout itemProcedureStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };

                        long procedureId = item.ProcedureId;
                        string procedureName = item.ProcedureName;

                        Label itemProcedureLabel = new Label
                        {
                            Text = procedureName,
                            TextColor = Color.FromHex("#000"),
                            FontSize = 14,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        };
                        itemProcedureStackLayout.Children.Add(itemProcedureLabel);
                        procedureGrid.Children.Add(itemProcedureStackLayout, 0, 0);

                        StackLayout itemNotificationStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };
                        ImageButton itemNotificationImageButton = new ImageButton
                        {
                            Source = "patientreportedoutcome/circle_gray_light.png",
                            Aspect = Aspect.AspectFit,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Padding = new Thickness(0, 0, 0, 0),
                            HeightRequest = 25
                        };
                        itemNotificationImageButton.Clicked += async (sender, args) => await BtnProfessionalProcedureNotification_ClickedAsync(sender, args, procedureId, procedureName);
                        itemNotificationStackLayout.Children.Add(itemNotificationImageButton);
                        procedureGrid.Children.Add(itemNotificationStackLayout, 1, 0);

                        StackLayout itemResourceStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };
                        ImageButton itemResourceImageButton = new ImageButton
                        {
                            Source = "patientreportedoutcome/circle_gray_light.png",
                            Aspect = Aspect.AspectFit,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Padding = new Thickness(0, 0, 0, 0),
                            HeightRequest = 25
                        };
                        itemResourceImageButton.Clicked += async (sender, args) => await BtnProfessionalProcedureResource_ClickedAsync(sender, args, procedureId, procedureName);
                        itemResourceStackLayout.Children.Add(itemResourceImageButton);
                        procedureGrid.Children.Add(itemResourceStackLayout, 2, 0);

                        procedureItemStackLayout.Children.Add(procedureGrid);
                        //Procedure List Item

                        #endregion
                    }

                    procedureStackLayout.Children.Add(titleStackLayout);
                    procedureStackLayout.Children.Add(titleLineStackLayout);
                    procedureStackLayout.Children.Add(procedureHeaderStackLayout);
                    procedureStackLayout.Children.Add(procedureItemStackLayout);
                }

                ProfessionalProcedureStackLayout.Children.Add(procedureStackLayout);

                #endregion
            }

        }

        protected async Task BtnProfessionalProcedureNotification_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                await Navigation.PushAsync(new ProfessionalProgramNotificationListPage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        protected async Task BtnProfessionalProcedureResource_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                await App.ShowUserDialogDelayAsync();
                await Navigation.PushAsync(new ProfessionalResourcePage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        private void BuildMedication(List<ProProcedureViewModel> medications)
        {
            MedicationStackLayout.Children.Clear();

            if (medications.Count() == 0)
            {
                #region No Data Found

                ////No Data Found
                //StackLayout noDataFoundStackLayout = new StackLayout
                //{
                //    Padding = new Thickness(0, 0, 0, 0),
                //    Margin = new Thickness(0, 0, 0, 0),
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    VerticalOptions = LayoutOptions.FillAndExpand
                //};

                //Label noDataFoundLabel = new Label
                //{
                //    Text = "No record(s) found.",
                //    TextColor = Color.FromHex("#000"),
                //    FontSize = 16,
                //    HorizontalTextAlignment = TextAlignment.Start,
                //    HorizontalOptions = LayoutOptions.StartAndExpand,
                //    VerticalOptions = LayoutOptions.Center
                //};
                //noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                ////No Data Found

                //MedicationStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                #region Title

                //Procedure Title
                StackLayout titleStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 10),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                string titleName = "Medication (press icon to visualize)";

                Label titleLabel = new Label
                {
                    Text = titleName,
                    TextColor = Color.FromHex("#000"),
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center
                };
                titleStackLayout.Children.Add(titleLabel);
                //Procedure Title

                StackLayout titleLineStackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 10),
                    BackgroundColor = Color.FromHex("#002E5D"),
                    HeightRequest = 5,
                    Spacing = 0
                };

                #endregion

                #region Header

                StackLayout procedureHeaderStackLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#D7D7D7"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                //Procedure List Header
                Grid headerGrid = new Grid();
                headerGrid.Margin = new Thickness(0, 0, 0, 0);
                headerGrid.Padding = new Thickness(0, 0, 0, 0);
                headerGrid.ColumnSpacing = 0;
                headerGrid.RowSpacing = 0;
                headerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

                StackLayout headerProcedureStackLayout = new StackLayout
                {
                    Padding = new Thickness(5, 0, 0, 0),
                    Spacing = 0
                };
                Label headerProcedureLabel = new Label
                {
                    Text = "Procedure",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center
                };
                headerProcedureStackLayout.Children.Add(headerProcedureLabel);
                headerGrid.Children.Add(headerProcedureStackLayout, 0, 0); //c,r

                StackLayout headerNotificationStackLayout = new StackLayout
                {
                    Padding = new Thickness(5, 0, 0, 0),
                    Spacing = 0
                };
                Label headerNotificationLabel = new Label
                {
                    Text = "Notification",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center
                };
                headerNotificationStackLayout.Children.Add(headerNotificationLabel);
                headerGrid.Children.Add(headerNotificationStackLayout, 1, 0); //c,r

                StackLayout headerResourceStackLayout = new StackLayout
                {
                    Padding = new Thickness(15, 0, 0, 0),
                    Spacing = 0
                };
                Label headerResourceLabel = new Label
                {
                    Text = "Resource",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center
                };
                headerResourceStackLayout.Children.Add(headerResourceLabel);
                headerGrid.Children.Add(headerResourceStackLayout, 2, 0); //c,r

                procedureHeaderStackLayout.Children.Add(headerGrid);
                //Procedure List Header

                #endregion

                StackLayout procedureItemStackLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#ffffff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                foreach (var item in medications.ToList().Where(m => m.IsDeleted == false))
                {
                    #region Item

                    Grid procedureGrid = new Grid();
                    procedureGrid.Margin = new Thickness(0, 0, 0, 0);
                    procedureGrid.Padding = new Thickness(0, 0, 0, 0);
                    procedureGrid.ColumnSpacing = 0;
                    procedureGrid.RowSpacing = 0;
                    procedureGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

                    //Procedure List Item
                    StackLayout itemProcedureStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 5, 0, 5),
                        Spacing = 0
                    };

                    long procedureId = item.ProcedureId;
                    string procedureName = item.ProcedureName;

                    Label itemProcedureLabel = new Label
                    {
                        Text = procedureName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    itemProcedureStackLayout.Children.Add(itemProcedureLabel);
                    procedureGrid.Children.Add(itemProcedureStackLayout, 0, 0);

                    StackLayout itemNotificationStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        Spacing = 0
                    };
                    ImageButton itemNotificationImageButton = new ImageButton
                    {
                        Source = "patientreportedoutcome/circle_gray_light.png",
                        Aspect = Aspect.AspectFit,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0, 0, 0, 0),
                        HeightRequest = 25
                    };
                    itemNotificationImageButton.Clicked += async (sender, args) => await BtnMedicationNotification_ClickedAsync(sender, args, procedureId, procedureName);
                    itemNotificationStackLayout.Children.Add(itemNotificationImageButton);
                    procedureGrid.Children.Add(itemNotificationStackLayout, 1, 0);

                    StackLayout itemResourceStackLayout = new StackLayout
                    {
                        Margin = new Thickness(0, 0, 0, 0),
                        Padding = new Thickness(0, 0, 0, 0),
                        Spacing = 0
                    };
                    ImageButton itemResourceImageButton = new ImageButton
                    {
                        Source = "patientreportedoutcome/circle_gray_light.png",
                        Aspect = Aspect.AspectFit,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0, 0, 0, 0),
                        HeightRequest = 25
                    };
                    itemResourceImageButton.Clicked += async (sender, args) => await BtnMedicationResource_ClickedAsync(sender, args, procedureId, procedureName);
                    itemResourceStackLayout.Children.Add(itemResourceImageButton);
                    procedureGrid.Children.Add(itemResourceStackLayout, 2, 0);

                    procedureItemStackLayout.Children.Add(procedureGrid);
                    //Procedure List Item

                    #endregion
                }

                MedicationStackLayout.Children.Add(titleStackLayout);
                MedicationStackLayout.Children.Add(titleLineStackLayout);
                MedicationStackLayout.Children.Add(procedureHeaderStackLayout);
                MedicationStackLayout.Children.Add(procedureItemStackLayout);

                #endregion
            }
        }

        protected async Task BtnMedicationNotification_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                await App.ShowUserDialogDelayAsync();
                await Navigation.PushAsync(new ProfessionalProgramNotificationListPage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        protected async Task BtnMedicationResource_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                await App.ShowUserDialogDelayAsync();
                await Navigation.PushAsync(new ProfessionalResourcePage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        private void BuildSurgicalConceirgeProcedure(List<ProProcedureViewModel> surgicalConceirgeProcedures)
        {
            ORProgramContentStackLayout.Children.Clear();

            if (surgicalConceirgeProcedures.Count() == 0)
            {
                #region No Data Found

                ////No Data Found
                //StackLayout noDataFoundStackLayout = new StackLayout
                //{
                //    Padding = new Thickness(0, 0, 0, 0),
                //    Margin = new Thickness(0, 0, 0, 0),
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    VerticalOptions = LayoutOptions.FillAndExpand
                //};

                //Label noDataFoundLabel = new Label
                //{
                //    Text = "No record(s) found.",
                //    TextColor = Color.FromHex("#000"),
                //    FontSize = 16,
                //    HorizontalTextAlignment = TextAlignment.Start,
                //    HorizontalOptions = LayoutOptions.StartAndExpand,
                //    VerticalOptions = LayoutOptions.Center
                //};
                //noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                ////No Data Found

                //ORProgramContentStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                StackLayout procedureItemStackLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#ffffff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                foreach (var item in surgicalConceirgeProcedures.ToList().Where(m => m.IsDeleted == false))
                {
                    #region Item

                    Grid procedureGrid = new Grid();
                    procedureGrid.Margin = new Thickness(0, 0, 0, 0);
                    procedureGrid.Padding = new Thickness(0, 0, 0, 0);
                    procedureGrid.ColumnSpacing = 0;
                    procedureGrid.RowSpacing = 0;
                    procedureGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                    procedureGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

                    //Procedure List Item
                    StackLayout itemProcedureStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 5, 0, 5),
                        Spacing = 0
                    };

                    long procedureId = item.ProcedureId;
                    string procedureName = item.ProcedureName;

                    Label itemProcedureLabel = new Label
                    {
                        Text = procedureName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    itemProcedureStackLayout.Children.Add(itemProcedureLabel);
                    procedureGrid.Children.Add(itemProcedureStackLayout, 0, 0);

                    StackLayout itemStageStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 10, 0),
                        Spacing = 0
                    };
                    ImageButton itemStageImageButton = new ImageButton
                    {
                        Source = "patientreportedoutcome/circle_gray_light.png",
                        Aspect = Aspect.AspectFit,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0, 0, 0, 0),
                        HeightRequest = 25
                    };
                    itemStageImageButton.Clicked += async (sender, args) => await BtnORProgramStage_ClickedAsync(sender, args, procedureId, procedureName);
                    itemStageStackLayout.Children.Add(itemStageImageButton);
                    procedureGrid.Children.Add(itemStageStackLayout, 1, 0);

                    StackLayout itemResourceStackLayout = new StackLayout
                    {
                        Margin = new Thickness(0, 0, 0, 0),
                        Padding = new Thickness(15, 0, 0, 0),
                        Spacing = 0
                    };
                    ImageButton itemResourceImageButton = new ImageButton
                    {
                        Source = "patientreportedoutcome/circle_gray_light.png",
                        Aspect = Aspect.AspectFit,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0, 0, 0, 0),
                        HeightRequest = 25
                    };
                    itemResourceImageButton.Clicked += async (sender, args) => await BtnORProgramResource_ClickedAsync(sender, args, procedureId, procedureName);
                    itemResourceStackLayout.Children.Add(itemResourceImageButton);
                    procedureGrid.Children.Add(itemResourceStackLayout, 2, 0);

                    procedureItemStackLayout.Children.Add(procedureGrid);
                    //Procedure List Item

                    #endregion
                }

                ORProgramContentStackLayout.Children.Add(procedureItemStackLayout);

                #endregion
            }

            SurgicalConciergeStackLayout.IsVisible = true;
        }

        protected async Task BtnORProgramStage_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                App.HideUserDialogAsync();
                await Navigation.PushAsync(new ProfessionalProgramOperatingStagePage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        protected async Task BtnORProgramResource_ClickedAsync(object sender, EventArgs e, long procedureId, string procedureName)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                await App.ShowUserDialogDelayAsync();
                await Navigation.PushAsync(new ProfessionalProgramOperatingResourcePage(procedureId, procedureName));
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            Title = _iTokenContainer.ApiPracticeName;
        }

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                HomeButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {

            App.ShowUserDialogAsync();
            App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion

        #region OR Programs
        private async void ButtonORProgram_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    if (ORProgramContentStackLayout.IsVisible == true) {
                        ORProgramContentStackLayout.IsVisible = false;
                        ORProgramImage.Source = "img_down_arrow.png";
                    }
                    else {
                        ORProgramContentStackLayout.IsVisible = true;
                        ORProgramImage.Source = "img_up_arrow.png";
                    }

                    App.HideUserDialogAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        #endregion

        #region PACU
        private async void ImageButtonPACU_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    await Navigation.PushAsync(new ProfessionalProgramPacuPage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void ButtonPACU_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    await Navigation.PushAsync(new ProfessionalProgramPacuPage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        #endregion

        #region Floor
        private async void ImageButtonFloor_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    //await Navigation.PushAsync(new ProfessionalProgramFloorPage());
                    await Navigation.PushAsync(new ProfessionalProgramNursingRoundPage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void ButtonFloor_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    //await Navigation.PushAsync(new ProfessionalProgramFloorPage());
                    await Navigation.PushAsync(new ProfessionalProgramNursingRoundPage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        #endregion

        #region Discharge
        private async void ImageButtonDischarge_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    await Navigation.PushAsync(new ProfessionalProgramDischargePage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void ButtonDischarge_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();

                try
                {
                    await Navigation.PushAsync(new ProfessionalProgramDischargePage());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    App.HideUserDialogAsync();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        #endregion
    }
}