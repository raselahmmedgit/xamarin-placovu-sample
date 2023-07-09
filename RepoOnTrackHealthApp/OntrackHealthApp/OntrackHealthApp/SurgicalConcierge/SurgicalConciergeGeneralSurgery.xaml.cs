using System;
using System.Collections.Generic;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeGeneralSurgery : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        public SurgicalConciergeGeneralSurgery()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            App.HideUserDialogAsync();
        }
    }
}