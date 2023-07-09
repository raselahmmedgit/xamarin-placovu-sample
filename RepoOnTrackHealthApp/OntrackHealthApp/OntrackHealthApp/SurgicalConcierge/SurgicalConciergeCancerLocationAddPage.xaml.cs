using OntrackHealthApp.SurgicalConcierge.Model;
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
    public partial class SurgicalConciergeCancerLocationAddPage : CustomModalContentPage
    {
        private CancerLocationViewModel cancerLocation;

        public SurgicalConciergeCancerLocationAddPage(CancerLocationViewModel cancerLocation)
        {
            InitializeComponent();
            if (cancerLocation.Involved.HasValue && cancerLocation.Involved.Value > 0)
            {
                cancerLocation.LocationValue = cancerLocation.Involved.ToString();
            }
            this.cancerLocation = cancerLocation;
            this.BindingContext = cancerLocation;
            AddRecordButton.Text = cancerLocation.IsChecked ? "Update Record" : "Add Record";
        }

        public void OnCloseModalButton_Clicked(object sender, EventArgs e)
        {
            CloseModal();
        }
        public void OnAddRecordButton_Clicked(object sender, EventArgs e)
        {
            cancerLocation.Involved = string.IsNullOrEmpty(InvolvedTextBox.Text) ? (decimal?)null : decimal.Parse(InvolvedTextBox.Text);
            cancerLocation.IsChecked = cancerLocation.Involved.HasValue;
            CloseModal();
        }
        public void OnDeleteButton_Clicked(object sender, EventArgs e)
        {
            cancerLocation.Involved = null;
            cancerLocation.IsChecked = false;
            InvolvedTextBox.Text = string.Empty;
            CloseModal();
        }
        private async void CloseModal()
        {
            await Navigation.PopModalAsync();
            MessagingCenter.Send<SurgicalConciergeCancerLocationAddPage>(this, "UpdateCancerLocation");
        }

    }
}