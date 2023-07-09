using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PatientSearchView : CustomModalContentPage
    {
        ProfessionalPatientPage _professionalPatientPage;

        public PatientSearchView()
        {
            InitializeComponent();
        }

        public PatientSearchView(ProfessionalPatientPage professionalPatientPage)
        {
            InitializeComponent();
            this._professionalPatientPage = professionalPatientPage;
            ProfessionalName.Text = professionalPatientPage.ProfessionalName;
            PatientName.Text = professionalPatientPage.PatientName;
            if (professionalPatientPage.SelectedDate != null)
            {
                SurgeryDate.Date = (DateTime)professionalPatientPage.SelectedDate;
            }
        }

        private async void PatientSearchSumbitButtonClicked(object sender, EventArgs e)
        {
            await App.ShowUserDialogDelayAsync();
            _professionalPatientPage.PracticeName = string.Empty;
            _professionalPatientPage.ProfessionalName = ProfessionalName.Text?.Trim();
            _professionalPatientPage.PatientName = PatientName.Text?.Trim();
            _professionalPatientPage.PatientEmail = string.Empty;
            _professionalPatientPage.DateofBirth = string.Empty;
            _professionalPatientPage.PatientPhoneCode = string.Empty;
            _professionalPatientPage.PatientPhone = string.Empty;
            _professionalPatientPage.SurgeryDate = SurgeryDate.Date;
            _professionalPatientPage.PastDay = null;

            _professionalPatientPage.ReLoadData();

            await Navigation.PopModalAsync();
        }

        private async void PatientSuearchCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}