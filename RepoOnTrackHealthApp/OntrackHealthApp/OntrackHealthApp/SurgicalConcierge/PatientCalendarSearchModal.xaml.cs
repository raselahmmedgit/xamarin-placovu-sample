using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientCalendarSearchModal : CustomModalContentPage
    {
        DateTime _searchDateTime;
        DateTime _surgeryDateTime;
        SurgicalConciergePatientView _surgicalConciergePatientView;
        SurgicalConciergePatientPage _surgicalConciergePatientPage;
        NursePatientInfoPatientViewPageNew _nursePatientInfoPatientViewPageNew;

        bool IsSurgicalConciergePatientView = false;
        bool IsSurgicalConciergePatientPage = false;
        bool IsNursePatientInfoPatientViewPageNew = false;

        public PatientCalendarSearchModal()
        {
            InitializeComponent();
            _searchDateTime = DateTime.UtcNow;
            BindDateList(_searchDateTime);
        }

        public PatientCalendarSearchModal(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            InitializeComponent();
            _surgicalConciergePatientView = surgicalConciergePatientView;
            IsSurgicalConciergePatientView = true;
            IsNursePatientInfoPatientViewPageNew = false;

            if (surgicalConciergePatientView.SelectedDate != null)
            {
                _surgeryDateTime = (DateTime)surgicalConciergePatientView.SelectedDate;
                _searchDateTime = _surgeryDateTime;
            }
            else
            {
                _searchDateTime = DateTime.UtcNow;
            }

            BindDateList(_searchDateTime);
        }

        public PatientCalendarSearchModal(SurgicalConciergePatientPage surgicalConciergePatientPage)
        {
            InitializeComponent();
            _surgicalConciergePatientPage= surgicalConciergePatientPage;
            IsSurgicalConciergePatientPage = true;
            IsNursePatientInfoPatientViewPageNew = false;

            if (surgicalConciergePatientPage.SelectedDate != null)
            {
                _surgeryDateTime = (DateTime)surgicalConciergePatientPage.SelectedDate;
                _searchDateTime = _surgeryDateTime;
            }
            else
            {
                _searchDateTime = DateTime.UtcNow;
            }

            BindDateList(_searchDateTime);
        }

        public PatientCalendarSearchModal(NursePatientInfoPatientViewPageNew nursePatientInfoPatientViewPageNew)
        {
            InitializeComponent();
            _nursePatientInfoPatientViewPageNew = nursePatientInfoPatientViewPageNew;
            IsSurgicalConciergePatientView = false;
            IsNursePatientInfoPatientViewPageNew = true;

            if (nursePatientInfoPatientViewPageNew.SelectedDate != null)
            {
                _surgeryDateTime = (DateTime)nursePatientInfoPatientViewPageNew.SelectedDate;
                _searchDateTime = _surgeryDateTime;
            }
            else
            {
                _searchDateTime = DateTime.UtcNow;
            }

            BindDateList(_searchDateTime);

        }

        private void BindDateList(DateTime searchDate)
        {
            try
            {
                _searchDateTime = searchDate;

                DateTime todayDateTime = DateTime.UtcNow;
                int todayMonth = todayDateTime.Month;
                int todayDay = todayDateTime.Day;

                CalendarDateListView.Children.Clear();
                DateTime[] weekDays = new DateTime[7];
                DateTime[] lastWeekDays = new DateTime[7];

                int i = 0;
                for (i = 0; i < 7; i++)
                {
                    DateTime currentDate = searchDate.AddDays(i);
                    lastWeekDays[i] = currentDate;
                    if (currentDate.DayOfWeek.ToString().Substring(0, 2).ToLower().Equals("sa"))
                    {
                        break;
                    }
                }
                int dayCount = 0;
                for (int j = i + 1; j < 7; j++)
                {
                    weekDays[dayCount++] = searchDate.AddDays(j - 7);
                }
                for (int j = 0; dayCount < 7; j++)
                {
                    weekDays[dayCount++] = lastWeekDays[j];
                }

                string monthName = searchDate.ToString("MMMM");
                string lastWeekFirstDayMonthName = weekDays[0].ToString("MMMM");
                string lastWeekLastDayMonthName = weekDays[6].ToString("MMMM");
                if (lastWeekFirstDayMonthName == lastWeekLastDayMonthName)
                {
                    monthName = weekDays[6].ToString("MMMM");
                }
                else
                {
                    monthName = (lastWeekFirstDayMonthName + " / " + lastWeekLastDayMonthName);
                }

                MonthName.Text = monthName;

                for (i = 0; i < 7; i++)
                {
                    DateTime currentWeekDate = weekDays[i];
                    StackLayout dateTimeLayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    Label nameLabel = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Color.FromHex("#FFF"),
                        Text = currentWeekDate.DayOfWeek.ToString().Substring(0, 3)
                    };

                    int cornerRadius = 0;
                    string bgColor = string.Empty;
                    string txtColor = string.Empty;

                    if (_surgeryDateTime != default(DateTime))
                    {
                        cornerRadius = currentWeekDate.Day == _surgeryDateTime.Day ? 20 : 0;
                        bgColor = currentWeekDate.Day == _surgeryDateTime.Day ? "#eee" : "#46b8da";
                        txtColor = currentWeekDate.Day == _surgeryDateTime.Day ? "#000" : "#FFF";
                    }
                    else
                    {
                        cornerRadius = currentWeekDate.Day == todayDateTime.Day ? 20 : 0;
                        bgColor = currentWeekDate.Day == todayDateTime.Day ? "#eee" : "#46b8da";
                        txtColor = currentWeekDate.Day == todayDateTime.Day ? "#000" : "#FFF";
                    }

                    Color backgroundColor = Color.FromHex(bgColor);
                    Color textColor = Color.FromHex(txtColor);

                    Button dateLabel = new Button()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        CornerRadius = cornerRadius,
                        BackgroundColor = backgroundColor,
                        TextColor = textColor,
                        Text = currentWeekDate.Day.ToString("00"),
                        WidthRequest = 40,
                        HeightRequest = 40,
                    };

                    if (IsSurgicalConciergePatientView == true && IsNursePatientInfoPatientViewPageNew == false)
                    {
                        dateLabel.Clicked += async (sender, args) => await PatientSearchDateSelected(sender, args, currentWeekDate);
                    }
                    else if (IsSurgicalConciergePatientView == false && IsNursePatientInfoPatientViewPageNew == true)
                    {
                        dateLabel.Clicked += async (sender, args) => await NursePatientSearchDateSelected(sender, args, currentWeekDate);
                    }
                    else if (IsSurgicalConciergePatientPage == true && IsNursePatientInfoPatientViewPageNew == false)
                    {
                        dateLabel.Clicked += async (sender, args) => await PatientSearchDateSelected(sender, args, currentWeekDate);
                    }
                    else if (IsSurgicalConciergePatientPage == false && IsNursePatientInfoPatientViewPageNew == true)
                    {
                        dateLabel.Clicked += async (sender, args) => await NursePatientSearchDateSelected(sender, args, currentWeekDate);
                    }

                    dateTimeLayout.Children.Add(nameLabel);
                    dateTimeLayout.Children.Add(dateLabel);
                    CalendarDateListView.Children.Add(dateTimeLayout);
                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async Task PatientSearchDateSelected(object sender, EventArgs e, DateTime selectedDate)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                btn.BackgroundColor = Color.FromHex("#eee");
                btn.TextColor = Color.FromHex("#000");
                btn.CornerRadius = 50;
            }

            _surgicalConciergePatientView.PracticeName = string.Empty;
            _surgicalConciergePatientView.ProfessionalName = string.Empty;
            _surgicalConciergePatientView.PatientName = string.Empty;
            _surgicalConciergePatientView.PatientEmail = string.Empty;
            _surgicalConciergePatientView.DateofBirth = string.Empty;
            _surgicalConciergePatientView.PatientPhoneCode = string.Empty;
            _surgicalConciergePatientView.PatientPhone = string.Empty;
            _surgicalConciergePatientView.SurgeryDate = null;
            _surgicalConciergePatientView.SelectedDate = selectedDate;
            _surgicalConciergePatientView.PastDay = selectedDate;
            await Navigation.PopModalAsync();
            _surgicalConciergePatientView.ReLoadData();
        }

        private async Task NursePatientSearchDateSelected(object sender, EventArgs e, DateTime selectedDate)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                btn.BackgroundColor = Color.FromHex("#eee");
                btn.TextColor = Color.FromHex("#000");
                btn.CornerRadius = 50;
            }
            _nursePatientInfoPatientViewPageNew.PracticeName = string.Empty;
            _nursePatientInfoPatientViewPageNew.ProfessionalName = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientName = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientEmail = string.Empty;
            _nursePatientInfoPatientViewPageNew.DateofBirth = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientPhoneCode = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientPhone = string.Empty;
            _nursePatientInfoPatientViewPageNew.SurgeryDate = null;
            _nursePatientInfoPatientViewPageNew.SelectedDate = selectedDate;
            _nursePatientInfoPatientViewPageNew.PastDay = selectedDate;
            await Navigation.PopModalAsync();
            _nursePatientInfoPatientViewPageNew.ReLoadData();
        }

        private void PatientSearchCalendarPrevButton_Clicked(object sender, EventArgs e)
        {
            BindDateList(_searchDateTime.AddDays(-7));
        }

        private void PatientSearchCalendarNextButton_Clicked(object sender, EventArgs e)
        {
            BindDateList(_searchDateTime.AddDays(+7));
        }

        private async void PatientCalendarSearchCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}