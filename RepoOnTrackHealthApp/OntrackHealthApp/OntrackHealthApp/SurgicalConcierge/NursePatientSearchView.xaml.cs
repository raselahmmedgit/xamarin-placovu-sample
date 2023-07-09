using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NursePatientSearchView : CustomModalContentPage
    {

        NursePatientInfoPatientView _nursePatientInfoPatientView;
        NursePatientInfoPatientViewPageNew _nursePatientInfoPatientViewPageNew;

        public NursePatientSearchView()
        {
            InitializeComponent();
        }
        public NursePatientSearchView(NursePatientInfoPatientViewPageNew nursePatientInfoPatientViewPageNew)
        {
            InitializeComponent();

            this._nursePatientInfoPatientViewPageNew = nursePatientInfoPatientViewPageNew;
            SurgeryDate.SetDate(_nursePatientInfoPatientViewPageNew.SurgeryDate);
            PatientName.SetText(_nursePatientInfoPatientViewPageNew.PatientName);
            ProfessionalName.SetText(_nursePatientInfoPatientViewPageNew.ProfessionalName);

            if (nursePatientInfoPatientViewPageNew.SelectedDate != null)
            {
                SurgeryDate.Date = (DateTime)nursePatientInfoPatientViewPageNew.SelectedDate;
            }
        }
        public NursePatientSearchView(NursePatientInfoPatientView nursePatientInfoPatientView)
        {
            this._nursePatientInfoPatientView = nursePatientInfoPatientView;
            InitializeComponent();

            if (nursePatientInfoPatientView.SelectedDate != null)
            {
                SurgeryDate.Date = (DateTime)nursePatientInfoPatientView.SelectedDate;
            }
        }

        private async void PatientSearchSumbitButtonClicked(object sender, EventArgs e)
        {
            await App.ShowUserDialogDelayAsync();
            _nursePatientInfoPatientViewPageNew.PracticeName = string.Empty;
            _nursePatientInfoPatientViewPageNew.ProfessionalName = ProfessionalName.Text?.Trim();
            _nursePatientInfoPatientViewPageNew.PatientName = PatientName.Text?.Trim();
            _nursePatientInfoPatientViewPageNew.PatientEmail = string.Empty;
            _nursePatientInfoPatientViewPageNew.DateofBirth = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientPhoneCode = string.Empty;
            _nursePatientInfoPatientViewPageNew.PatientPhone = string.Empty;
            _nursePatientInfoPatientViewPageNew.SurgeryDate = SurgeryDate.Date;
            _nursePatientInfoPatientViewPageNew.PastDay = null;
            _nursePatientInfoPatientViewPageNew.ReLoadData();
            await Navigation.PopModalAsync();           
        }

        private async void PatientSuearchCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}