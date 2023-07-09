using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientSearchView : CustomModalContentPage
    {
        SurgicalConciergePatientView _surgicalConciergePatientView;
        SurgicalConciergePatientPage _surgicalConciergePatientPage;

        public PatientSearchView()
        {
            InitializeComponent();
        }

        public PatientSearchView(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            InitializeComponent();
            this._surgicalConciergePatientView = surgicalConciergePatientView;
            ProfessionalName.Text = surgicalConciergePatientView.ProfessionalName;
            PatientName.Text = surgicalConciergePatientView.PatientName;
            if (surgicalConciergePatientView.SelectedDate != null)
            {
                SurgeryDate.Date = (DateTime)surgicalConciergePatientView.SelectedDate;
            }
        }

        public PatientSearchView(SurgicalConciergePatientPage surgicalConciergePatientPage)
        {
            InitializeComponent();
            this._surgicalConciergePatientPage = surgicalConciergePatientPage;
            ProfessionalName.Text = surgicalConciergePatientPage.ProfessionalName;
            PatientName.Text = surgicalConciergePatientPage.PatientName;
            if (surgicalConciergePatientPage.SelectedDate != null)
            {
                SurgeryDate.Date = (DateTime)surgicalConciergePatientPage.SelectedDate;
            }
        }

        private async void PatientSearchSumbitButtonClicked(object sender, EventArgs e)
        {
            await App.ShowUserDialogDelayAsync();
            _surgicalConciergePatientView.PracticeName = string.Empty;
            _surgicalConciergePatientView.ProfessionalName = ProfessionalName.Text?.Trim();
            _surgicalConciergePatientView.PatientName = PatientName.Text?.Trim();
            _surgicalConciergePatientView.PatientEmail = string.Empty;
            _surgicalConciergePatientView.DateofBirth = string.Empty;
            _surgicalConciergePatientView.PatientPhoneCode = string.Empty;
            _surgicalConciergePatientView.PatientPhone = string.Empty;
            _surgicalConciergePatientView.SurgeryDate = SurgeryDate.Date;
            _surgicalConciergePatientView.PastDay = null;

            _surgicalConciergePatientView.ReLoadData();

            await Navigation.PopModalAsync();            
        }

        private async void PatientSuearchCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}



