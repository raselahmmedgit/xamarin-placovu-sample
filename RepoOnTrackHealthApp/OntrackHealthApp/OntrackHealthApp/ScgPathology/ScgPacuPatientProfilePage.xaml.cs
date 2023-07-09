using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ScgPathology
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScgPacuPatientProfilePage : ContentPage
	{

        SurgicalConciergePatientProfilePageViewModel viewmodel;
        private readonly ITokenContainer _iTokenContainer;

        public ScgPacuPatientProfilePage()
		{
			InitializeComponent ();
            BindingContext = viewmodel = new SurgicalConciergePatientProfilePageViewModel();
            Title = viewmodel.PageTitle;
            _iTokenContainer = new TokenContainer();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewmodel.LoadPatientProfileWithProfessionalProceduresCommand.Execute(null);
            var ccc = viewmodel.PatientProfileWithProfessionalProcedures;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as PatientProfileWithProfessionalProcedureView;
            if (item == null)
                return;

            await Navigation.PushAsync(new SurgicalConciergePacuRecipientPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private async void btnAddNew_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                

            }
            catch
            {
                await DisplayAlert("Warning!", AppConstant.DisplayAlertErrorMessage, AppConstant.MessageOkButtonText);
            }
        }
                       
    }
}