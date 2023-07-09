using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.UserControls;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationListPageN : ContentPage
    {
        bool CalendarCreated = false;
        private List<CalendarMonth> CalendarMonths { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private NotificationListPageViewModelN _notificationListPageViewModel;
        private readonly IProcedureClient _iProcedureClient;

        public NotificationListPageN()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            //Subtitle = _iTokenContainer.CurrentProcedureName;

            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            CalendarCreated = false;

            BindingContext = _notificationListPageViewModel = new NotificationListPageViewModelN();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _notificationListPageViewModel.ExecuteLoadPatientNotificationCommandAsync();
            CreateCalendar();
        }

        //private async void ShowMoreButton_Clicked(object sender, EventArgs e)
        //{
        //    if (InternetConnectHelper.CheckConnection())
        //    {
        //        var showMoreButton = sender as Button;
        //        var notificationId = showMoreButton.ClassId.ToLong();
        //        var notification = _notificationListPageViewModel.PatientNotificationShortViews.FirstOrDefault(x => x.NotificationId == notificationId);

        //        if (notification != null && notification.NotificationId == Enums.SurgeryDatePatientNotificationEnum.NotificationId.ToInt()) {
        //            await Navigation.PushAsync(new NotificationSurgeryDatePage(notification.NotificationId));
        //        }
        //        else {
        //            await Navigation.PushAsync(new NotificationPageN(notification.NotificationId));
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }
        //}

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    HomeButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainPatientPage());
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
        }

        private async void OnResourceButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ResourceButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ResourceButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                ResourcePage resourcePage = new ResourcePage();
                await resourcePage.LoadDataAsync();
                await Navigation.PushAsync(resourcePage);
            }
        }

        private async void OnScheduleButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ScheduleButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ScheduleButton()
        {
            //using (UserDialogs.Instance.Loading(""))
            //{
            //    //NotificationListPageViewModel model = new NotificationListPageViewModel();
            //    //await model.ExecuteLoadCommandAsync();
            //    //var notificationListPage = new NotificationListPage(model);

            //    //await Navigation.PushAsync(notificationListPage);

            //    //Navigation.InsertPageBefore(new MainPatientPage(), this);
            //    //await Navigation.PopToRootAsync();

            //    //MenuPatientPage menuPatientPage = new MenuPatientPage();
            //    //menuPatientPage.Detail = new NavigationPage(notificationListPage);

            //}

            //App.ShowUserDialogAsync();
            await Navigation.PushAsync(new NotificationListPageN());
        }

        private async Task<AppMessage> IsCurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }
            else
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            using (UserDialogs.Instance.Loading(""))
            {
                if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
                {
                    return appMessage = await CurrentActiveProcedureWiseButtonShowHideAsync();
                }
                else
                {
                    return appMessage = await CurrentPatientProcedureDetailWiseButtonShowHideAsync();
                }
            }
        }

        private async Task<AppMessage> CurrentActiveProcedureWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
            if (responseCurrentActiveProcedure.StatusIsSuccessful)
            {
                var data = responseCurrentActiveProcedure.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetailWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentPatientProcedureDetail = await _iProcedureClient.GetPatientProcedureDetail();
            if (responseCurrentPatientProcedureDetail.StatusIsSuccessful)
            {
                var data = responseCurrentPatientProcedureDetail.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        #endregion

        private void NotificationListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var patientNotificationShortView = (PatientNotificationShortView)e.SelectedItem;

                if (NotificationListView.SelectedItem != null)
                {
                    NotificationListView.SelectedItem = null;

                    if (InternetConnectHelper.CheckConnection())
                    {
                        var notificationId = patientNotificationShortView.NotificationId.ToLong();
                        var notification = _notificationListPageViewModel.PatientNotificationShortViews.FirstOrDefault(x => x.NotificationId == notificationId);

                        if (notification != null && notification.NotificationId == Enums.SurgeryDatePatientNotificationEnum.NotificationId.ToInt())
                        {
                            Navigation.PushAsync(new NotificationSurgeryDatePage(notification.NotificationId, notification.NotificationDay, notification.NotificationMonthName, notification.NotificationTitle));
                        }
                        else
                        {
                            Navigation.PushAsync(new NotificationPageN(notification.NotificationId, notification.NotificationDay, notification.NotificationMonthName, notification.NotificationTitle));
                        }

                    }
                    else
                    {
                        DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                    }

                }
            }
            catch (Exception)
            {
                DisplayAlert(string.Empty, AppConstant.ApplicationExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void BtnCalendar_Clicked(object sender, EventArgs e)
        {
            var obj = (CalendarRoundButton)sender;
            if (InternetConnectHelper.CheckConnection())
            {
                if (obj.EventId == Enums.SurgeryDatePatientNotificationEnum.NotificationId.ToInt())
                {
                    Navigation.PushAsync(new NotificationSurgeryDatePage(obj.EventId, obj.EventDay, obj.EventMonthName, obj.EventTitle));
                }
                else
                {
                    Navigation.PushAsync(new NotificationPageN(obj.EventId, obj.EventDay, obj.EventMonthName, obj.EventTitle));
                }
            }
            else
            {
                DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        #region Calendar

        private double yPosition = 6;
        private class CalendarMonth
        {
            public int Id { get; set; }
            public long EventId { get; set; }
            public int Day { get; set; }
            public int DayOfWeek { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public string MonthName { get; set; }
            public string EventTitle { get; set; }
            public int DaysInMonth { get; set; }
            public bool IsEnabeled { get; set; }
            public bool IsPrimary { get; set; }
            public bool IsSecondary { get; set; }
            public object EventDetailItem { get; set; }
        }
        private void CreateCalendar()
        {
            try
            {            
                if (CalendarCreated == true) { return; }
                CalendarCreated = true;
                var calendar = _notificationListPageViewModel.CalendarMonthModels;
                int monthCount = calendar.Count();
                int currentMonth = DateTime.UtcNow.Month;
                int currentYear = DateTime.UtcNow.Year;

                foreach (var item in calendar)
                {
                    int startDay = 1;
                    int startMonth = item.Month;
                    int startYear = item.Year;
                    var dt = new DateTime(startYear, startMonth, startDay);
                    var title = dt.ToString("MMMM yyyy");
                
                    InitializeCalendar(item);

                    StackLayout stkTitle = new StackLayout { Padding = 8, VerticalOptions = LayoutOptions.Start, Spacing = 0, Margin = new Thickness(0, 0, 0, 10) };
                    Label lblMonth = new Label();
                    lblMonth.FontSize = 20;
                    lblMonth.VerticalOptions = LayoutOptions.Center;
                    lblMonth.HorizontalOptions = LayoutOptions.Center;
                    lblMonth.Text = title;
                    stkTitle.BackgroundColor = Color.FromHex("#f4f4f4");
                    stkTitle.Margin = new Thickness(0, 0, 0, 0);
                    stkTitle.Children.Add(lblMonth);

                    StackLayout stkWeekTitle = new StackLayout { Padding = new Thickness(2, 10), Spacing = 0 };
                    CreateMonthWeekTitle(stkWeekTitle);

                    StackLayout stkBody = new StackLayout { Padding = 0, Spacing = 0 };
                    CreateMonthBody(0, 0, stkBody);

                    StackLayout mainLayout = new StackLayout { Padding = 0, VerticalOptions = LayoutOptions.Start, Spacing = 0, Margin = new Thickness(6, 6, 6, 30), BackgroundColor = Color.FromHex("#f7f7f7") };
                    mainLayout.Children.Add(stkTitle);
                    mainLayout.Children.Add(stkWeekTitle);
                    mainLayout.Children.Add(stkBody);
                    MainCalendarView.Children.Add(mainLayout);
                    if (startMonth == currentMonth  && startYear == currentYear)
                    {
                        mainLayout.PropertyChanged += MainLayout_PropertyChanged;
                    }
                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        private void CreateMonthWeekTitle(StackLayout stkWeekTitle)
        {
            string[] titletext = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
            Grid grd = new Grid { HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0, RowSpacing = 0, VerticalOptions = LayoutOptions.Start };
            grd.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (int loop = 0; loop < 7; loop++)
            {
                Label lbl = new Label();
                lbl.HorizontalOptions = LayoutOptions.Center;
                lbl.VerticalOptions = LayoutOptions.Center;
                lbl.HorizontalTextAlignment = TextAlignment.Center;
                lbl.Text = titletext[loop];
                lbl.FontSize = 18;
                lbl.TextColor = Color.Black;
                grd.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grd.Children.Add(lbl, loop, 0);
            }
            stkWeekTitle.Children.Add(grd);
        }
        private void CreateMonthBody(int row, int start, StackLayout contain)
        {
            
            int cols = 7;
            var cals = CalendarMonths.Where(x => x.Id > start).Take(cols);
            int loop = 0;
            Grid grd = new Grid { HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0, RowSpacing = 20, VerticalOptions = LayoutOptions.Start };
            grd.RowDefinitions.Add(new RowDefinition { Height = 52 });
            for (int i = 0; i < 7; i++)
            {
                grd.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }
            foreach (var item in cals)
            {
                CalendarRoundButton btn = new CalendarRoundButton();
                btn.Text = item.Day.ToString();
                btn.HorizontalOptions = LayoutOptions.Center;
                btn.VerticalOptions = LayoutOptions.CenterAndExpand;
                btn.FontSize = 18;

                btn.EventId = item.EventId;
                btn.EventMonthName = item.MonthName;
                btn.EventDay = item.Day;
                btn.EventTitle = item.EventTitle;
                btn.EventMonthName = item.MonthName;

                Grid grdSun = new Grid { HorizontalOptions = LayoutOptions.Center, ColumnSpacing = 0, RowSpacing = 0, VerticalOptions = LayoutOptions.Center };
                if (item.IsEnabeled)
                {
                    if (item.IsSecondary)
                    {
                        btn.BackgroundColor = Color.FromHex("#f8ba00");
                        btn.TextColor = Color.White;
                        btn.Clicked += BtnCalendar_Clicked;                        
                        btn.Margin = new Thickness(4);
                        Image imgSun = new Image { Source = "sun_yellow.png", Aspect = Aspect.AspectFit };
                        grdSun.Children.Add(imgSun, 0, 0);
                        grdSun.Children.Add(btn, 0, 0);
                        grd.Children.Add(grdSun, loop, 0);
                    }
                    else if (item.IsPrimary)
                    {
                        btn.BackgroundColor = Color.FromHex("#00a89c");
                        btn.TextColor = Color.White;
                        btn.Clicked += BtnCalendar_Clicked;
                        btn.Margin = new Thickness(4);
                        Image imgSun = new Image { Source = "rounded_list_icon.png", Aspect = Aspect.AspectFit };
                        grdSun.Children.Add(imgSun, 0, 0);
                        grdSun.Children.Add(btn, 0, 0);
                        grd.Children.Add(grdSun, loop, 0);                        
                    }
                    else
                    {
                        btn.BackgroundColor = Color.FromHex("#f7f7f7");
                        btn.TextColor = Color.Black;
                        grd.Children.Add(btn, loop, 0);
                    }
                }
                else
                {
                    btn.BackgroundColor = Color.Transparent;
                    btn.TextColor = Color.LightGray;
                    grd.Children.Add(btn, loop, 0);
                }
                start++;
                loop++;
            }
            contain.Children.Add(grd);
            if (row < 5)
            {
                CreateMonthBody(row + 1, start, contain);
            }
        }
        private void InitializeCalendar(CalendarMonthModel calendarMonthModel)
        {
            int startDay = 1;
            int startMonth = calendarMonthModel.Month;
            int startYear = calendarMonthModel.Year;
            
            var dt = new DateTime(startYear, startMonth, startDay);
            int dw = (int)dt.DayOfWeek;
            int endDay = DateTime.DaysInMonth(startYear, startMonth);

            string monthName = dt.ToString("MMM").ToUpper();

            CalendarMonths = new List<CalendarMonth>();
            
            var dtNext = dt;
            var dtPre = dt;
            int loop = 1;

            for (int i = 0; i < dw; i++)
            {
                CalendarMonth cv = new CalendarMonth();
                dtPre = dt.AddDays(-(dw - i));
                cv.Day = dtPre.Day;
                cv.Month = dtPre.Month;
                cv.Year = dtPre.Year;
                cv.DayOfWeek = (int)dtPre.DayOfWeek;
                cv.IsEnabeled = false;
                cv.Id = loop;
                CalendarMonths.Add(cv);
                loop++;
            }

            for (int i = 1; i <= endDay; i++)
            {
                CalendarMonth cv = new CalendarMonth();
                dtNext = dt.AddDays(i);
                cv.Day = i;
                cv.Month = startMonth;
                cv.Year = startYear;
                cv.DayOfWeek = (int)dtNext.DayOfWeek;
                cv.IsEnabeled = true;
                cv.Id = loop;
                cv.EventId = 0;
                cv.MonthName = monthName;
                var primary = calendarMonthModel.PrimaryEvents.OrderByDescending(x=> x.OrderBy).FirstOrDefault(x => x.EventDay == i);
                var secondary = calendarMonthModel.SecondaryEvents.OrderByDescending(x => x.OrderBy).FirstOrDefault(x => x.EventDay == i);
                //Demo Data
                if (primary != null)
                {
                    cv.IsPrimary = true;
                    cv.EventId = primary.EventId;
                    cv.EventTitle = primary.EventTitle;                    
                }
                if (secondary != null)
                {
                    cv.IsSecondary = true;
                    cv.EventId = secondary.EventId;
                    cv.EventTitle = secondary.EventTitle;
                }

                CalendarMonths.Add(cv);
                // End
                loop++;
            }

            for (int i = endDay; i <= (42 - dw); i++)
            {
                CalendarMonth cv = new CalendarMonth();
                dtNext = dt.AddDays(i);
                cv.Day = dtNext.Day;
                cv.Month = dtNext.Month;
                cv.Year = dtNext.Year;
                cv.DayOfWeek = (int)dtNext.DayOfWeek;
                cv.IsEnabeled = false;
                cv.Id = loop;
                CalendarMonths.Add(cv);
                loop++;
            }

        }
        private async void MainLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var obj = (sender as StackLayout);
            if (e.PropertyName == "X")
            {
                //xPosition = obj.X;
            }
            if (e.PropertyName == "Y")
            {
                yPosition = obj.Y - 6;
                await Task.Delay(10);
                await MainScrollView.ScrollToAsync(0, yPosition, true);               
            }
        }
        #endregion

        private void BtnListView_Clicked(object sender, EventArgs e)
        {
            BtnListView.TextColor = Color.FromHex("#F7A50F");
            BtnCalendarView.TextColor = Color.FromHex("#000000");
            MainScrollView.IsVisible = false;
            NotificationListView.IsVisible = true;
        }

        private void BtnCalendarView_Clicked(object sender, EventArgs e)
        {
            App.ShowUserDialogAsync();
            BtnListView.TextColor = Color.FromHex("#000000");
            BtnCalendarView.TextColor = Color.FromHex("#F7A50F");
            CreateCalendar();
            NotificationListView.IsVisible = false;
            MainScrollView.IsVisible = true;                     
            App.HideUserDialogAsync();
        }
    }
}