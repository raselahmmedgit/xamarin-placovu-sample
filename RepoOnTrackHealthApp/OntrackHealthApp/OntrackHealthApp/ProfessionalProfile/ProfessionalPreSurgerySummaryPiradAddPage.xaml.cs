using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalPreSurgerySummaryPiradAddPage : CustomModalContentPage
    {
        PatientPreSurgerySummaryViewModel _patientPreSurgerySummaryViewModel;
        PatientPreSurgerySummaryPiradViewModel _patientPreSurgerySummaryPiradViewModel;
        public ProfessionalPreSurgerySummaryPiradAddPage(PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryViewModel = patientPreSurgerySummaryViewModel;
        }
        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Location_Clicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
            {
                var buttonList = LocationStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#000");
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#000");
            }
        }

        private void Zone_Clicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
            {
                var buttonList = ZoneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#000");
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#000");
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
                        btn.TextColor = Color.FromHex("#000");
                    }
                }
                else if (selectedButton.ClassId.Contains("Two"))
                {
                    var buttonList = gradeTwoStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                    foreach (var item in buttonList)
                    {
                        Button btn = item as Button;
                        btn.BackgroundColor = Color.FromHex("#FFF");
                        btn.TextColor = Color.FromHex("#000");
                    }
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#000");
            }
            var classId = selectedButton.ClassId;

        }
        private async void OnSavePiradButton_Clicked(object sender, EventArgs e)
        {
            _patientPreSurgerySummaryPiradViewModel = new PatientPreSurgerySummaryPiradViewModel();
            _patientPreSurgerySummaryPiradViewModel.PiradScore = string.IsNullOrEmpty(ScoreTextBox.Text) ? (decimal?)null : decimal.Parse(ScoreTextBox.Text);
            _patientPreSurgerySummaryPiradViewModel.PiradVolume = string.IsNullOrEmpty(VolumeTextBox.Text) ? (decimal?)null : decimal.Parse(VolumeTextBox.Text);

            var locationButtonList = LocationStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in locationButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryPiradViewModel.LesionLocation = btn.Text;
                    break;
                }
            }
            var zoneButtonList = ZoneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in zoneButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryPiradViewModel.LesionZone = btn.Text;
                    break;
                }
            }

            var gradeOneButtonList = gradeOneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeOneButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryPiradViewModel.LesionGradeOne = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text);
                    break;
                }
            }
            var gradeTwoButtonList = gradeTwoStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeTwoButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryPiradViewModel.LesionGradeTwo = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text);
                    break;
                }
            }

            var capsularInvolvementButtonList = CapsularInvolvementStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in capsularInvolvementButtonList)
            {
                Button btn = item as Button;
                if (btn.BackgroundColor == Color.FromHex("#610094"))
                {
                    _patientPreSurgerySummaryPiradViewModel.CapsularInvolvement = btn.Text;
                    break;
                }
            }
            _patientPreSurgerySummaryViewModel.PatientPreSurgerySummaryPiradViewModels.Add(_patientPreSurgerySummaryPiradViewModel);

            await Navigation.PopModalAsync();

            MessagingCenter.Send<ProfessionalPreSurgerySummaryPiradAddPage>(this, "AddPiradLesion");
        }
        private void CapsularInvolvement_Clicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
            {
                var buttonList = CapsularInvolvementStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                foreach (var item in buttonList)
                {
                    Button btn = item as Button;
                    btn.BackgroundColor = Color.FromHex("#FFF");
                    btn.TextColor = Color.FromHex("#000");
                }
                selectedButton.BackgroundColor = Color.FromHex("#610094");
                selectedButton.TextColor = Color.FromHex("#FFF");
            }
            else
            {
                selectedButton.BackgroundColor = Color.FromHex("#FFF");
                selectedButton.TextColor = Color.FromHex("#000");
            }
        }
    }
}