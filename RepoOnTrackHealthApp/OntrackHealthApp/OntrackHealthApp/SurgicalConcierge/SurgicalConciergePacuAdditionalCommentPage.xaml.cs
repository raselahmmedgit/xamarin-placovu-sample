using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
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
    public partial class SurgicalConciergePacuAdditionalCommentPage : CustomModalContentPage
    {
        Button ButtonSource;

        public SurgicalConciergePacuAdditionalCommentPage(string practiceName, Button btnSource)
        {
            InitializeComponent();
            lblPracticeTitle.Text = practiceName;
            ButtonSource = btnSource;
            AdditionalComment.Text = ButtonSource.ClassId;
        }

        private async void ModalClose_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddComment_ClickedAsync(object sender, EventArgs e)
        {
            ButtonSource.ClassId = AdditionalComment.Text;
            await Navigation.PopModalAsync();
        }

    }
}