using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramOperatingStageContentPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private ScgStageViewModel ScgStageViewModel { get; set; }

        private string _procedureName { get; set; }

        public ProfessionalProgramOperatingStageContentPage(string procedureName, ScgStageViewModel scgStageViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;
                ProcedureName.Text = procedureName;

                _procedureName = procedureName;
                ScgStageViewModel = scgStageViewModel;

                BuildSurgicalConciergeStageContent(scgStageViewModel);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
        }

        private void BuildSurgicalConciergeStageContent(ScgStageViewModel scgStageViewModel)
        {
            SurgicalConciergeStageContentProcedureNameLabel.Text = "The Stages of " + _procedureName;
            SurgicalConciergeStageContentOnlineTitleLabel.Text = scgStageViewModel.ScgStageOnlineTitle;
            SurgicalConciergeStageContentVideoWebView.Source = new HtmlWebViewSource { Html = scgStageViewModel.ScgProcedureStageVideoUrlCustom };
            SurgicalConciergeStageContentDescriptionWebView.Source = new HtmlWebViewSource { Html = scgStageViewModel.ScgProcedureStageDescriptionCustom };
            SurgicalConciergeStageContentStackLayout.IsVisible = true;
        }

    }
}