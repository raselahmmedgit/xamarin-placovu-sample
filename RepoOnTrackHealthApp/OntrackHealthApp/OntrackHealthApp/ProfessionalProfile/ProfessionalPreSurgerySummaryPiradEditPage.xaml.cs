﻿using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.SurgicalConcierge.Model;
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
    public partial class ProfessionalPreSurgerySummaryPiradEditPage : CustomModalContentPage
    {
        PatientPreSurgerySummaryPiradViewModel _patientPreSurgerySummaryPiradViewModel;
        public ProfessionalPreSurgerySummaryPiradEditPage(PatientPreSurgerySummaryPiradViewModel patientPreSurgerySummaryPiradViewModel)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryPiradViewModel = patientPreSurgerySummaryPiradViewModel;
            PopulatePiradFormPage();
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

        private void PopulatePiradFormPage()
        {
            ScoreTextBox.Text = _patientPreSurgerySummaryPiradViewModel.PiradScore?.ToRound().ToString();
            VolumeTextBox.Text = _patientPreSurgerySummaryPiradViewModel.PiradVolume?.ToRound().ToString();

            var locationButtonList = LocationStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in locationButtonList)
            {
                Button btn = item as Button;
                if (btn.Text == _patientPreSurgerySummaryPiradViewModel.LesionLocation)
                {
                    btn.BackgroundColor = Color.FromHex("#610094");
                    btn.TextColor = Color.FromHex("#FFF");
                    break;
                }
            }

            var zoneButtonList = ZoneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in zoneButtonList)
            {
                Button btn = item as Button;
                if (btn.Text == _patientPreSurgerySummaryPiradViewModel.LesionZone)
                {
                    btn.BackgroundColor = Color.FromHex("#610094");
                    btn.TextColor = Color.FromHex("#FFF");
                    break;
                }
            }
            var gradeOneButtonList = gradeOneStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeOneButtonList)
            {
                Button btn = item as Button;
                if (btn.Text == _patientPreSurgerySummaryPiradViewModel.LesionGradeOne.ToString())
                {
                    btn.BackgroundColor = Color.FromHex("#610094");
                    btn.TextColor = Color.FromHex("#FFF");
                    break;
                }
            }

            var gradeTwoButtonList = gradeTwoStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in gradeTwoButtonList)
            {
                Button btn = item as Button;
                if (btn.Text == _patientPreSurgerySummaryPiradViewModel.LesionGradeTwo.ToString())
                {
                    btn.BackgroundColor = Color.FromHex("#610094");
                    btn.TextColor = Color.FromHex("#FFF");
                    break;
                }
            }

            var capsularInvolvementButtonList = CapsularInvolvementStackLayout.Children.Where(c => c.GetType() == typeof(Button));
            foreach (var item in capsularInvolvementButtonList)
            {
                Button btn = item as Button;
                if (btn.Text == _patientPreSurgerySummaryPiradViewModel.CapsularInvolvement)
                {
                    btn.BackgroundColor = Color.FromHex("#610094");
                    btn.TextColor = Color.FromHex("#FFF");
                    break;
                }
            }



        }
        private async void OnDeletePiradButton_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<ProfessionalPreSurgerySummaryPiradEditPage, PatientPreSurgerySummaryPiradViewModel>(this, "DeletePiradLesion", _patientPreSurgerySummaryPiradViewModel);
            await Navigation.PopModalAsync();
        }

        private async void OnClosePiradModalButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnUpdatePiradButton_Clicked(object sender, EventArgs e)
        {
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
            await Navigation.PopModalAsync();

            MessagingCenter.Send<ProfessionalPreSurgerySummaryPiradEditPage>(this, "UpdatePiradLesion");
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
    }
}