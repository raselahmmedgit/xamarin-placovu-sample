using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using OntrackHealthApp.UserControls;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramDischargePage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeDischargePageViewModel _surgicalConciergeDischargePageViewModel;
        public ObservableCollection<ScgProstatectomyViewModel> _scgProstatectomyViewModelList { get; set; }
        public List<ScgProstatectomyViewModel> _scgProstatectomyViewModelTmpList { get; set; }
        private ScgDischargeCommentViewModel _scgDischargeCommentViewModel { get; set; }
        private IEnumerable<long> _scgProfessionalProcedureProstectomyIdList { set; get; }

        private int dischargeButtonCounter = 0;

        public ProfessionalProgramDischargePage()
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            
            _scgDischargeCommentViewModel = new ScgDischargeCommentViewModel();

            BindingContext = _surgicalConciergeDischargePageViewModel = new SurgicalConciergeDischargePageViewModel(true);

            _surgicalConciergeDischargePageViewModel.AfterLoadSurgicalConciergeDischargeData += _surgicalConciergeDischargePageViewModel_AfterLoadSurgicalConciergeDischargeData;

        }

        private void LoadData()
        {
            App.ShowUserDialogAsync();
            _surgicalConciergeDischargePageViewModel.LoadSurgicalConciergeDischargeCommandForProgram.Execute(null);

        }

        private void CreateFormCheckbox(List<ScgProstatectomyViewModel> model)
        {

            var data = model.Skip(0).Take(3).ToList();

            int loop_one = 0;

            Grid grdLayout = new Grid();
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.HorizontalOptions = LayoutOptions.FillAndExpand;


            foreach (var item in data)
            {
                Button btnImageLink = new Button
                {
                    //Text = item.ProstatectomyName,
                    HeightRequest = 110,
                    BackgroundColor = Color.FromHex("#c4d8f9"),
                    Image = "scg_dischrarge/" + item.ResourceIconUrl.Replace("~/content/images/scg_prostatectomy_icon/", "").Replace("-", "_"),
                    ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Top, 10),
                    FontSize = 12,
                    TextColor = Color.Black,
                    CornerRadius = 4,
                    ClassId = item.ProstatectomyId.ToString(),
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("#000000"),
                    Margin = new Thickness(0, 0, 0, 5),
                };

                if (_scgProfessionalProcedureProstectomyIdList != null && _scgProfessionalProcedureProstectomyIdList.Any() && _scgProfessionalProcedureProstectomyIdList.Contains(item.ProstatectomyId))
                {
                    btnImageLink.BackgroundColor = Color.FromHex("#f0b400");
                    btnImageLink.TextColor = Color.FromHex("#FFFFFF");
                    dischargeButtonCounter++;
                }

                grdLayout.Children.Add(btnImageLink, loop_one, 0);

                model.Remove(item);
                loop_one++;
            }

            //StackLayout sl2 = new StackLayout();
            MainContainer2.Children.Add(grdLayout);

            if (model.Count > 0)
            {
                CreateFormCheckbox(model);
            }
        }

        private async void BtnImageLink_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackgroundColor == Color.FromHex("#f0b400"))
            {
                btn.BackgroundColor = Color.FromHex("#c4d8f9");
                btn.TextColor = Color.FromHex("#000000");
                dischargeButtonCounter--;
            }
            else
            {
                if (dischargeButtonCounter == AppConstant.MaximumDischargeItemSelectLimit)
                {
                    await DisplayAlert("Message", AppConstant.MaximumDischargeItemSelectMessage, "Ok");
                    return;
                }
                else
                {
                    btn.BackgroundColor = Color.FromHex("#f0b400");
                    btn.TextColor = Color.FromHex("#FFFFFF");
                    dischargeButtonCounter++;
                }
            }
        }

        private string[] GetSelectedCheckboxValue()
        {
            List<string> check = new List<string>();
            var view = MainContainer2.Children.Where(x => x.GetType() == typeof(Grid)).ToList();
            foreach (var item in view)
            {
                var grdLayout = item as Grid;
                var buttons = grdLayout.Children.Where(x => x.GetType() == typeof(Button));

                foreach (var btn in buttons)
                {
                    var btnView = btn as Button;
                    if (btnView.BackgroundColor == Color.FromHex("#f0b400"))
                    {
                        check.Add(btnView.ClassId);
                    }
                }
            }
            return check.ToArray();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            LoadData();
        }

        private void _surgicalConciergeDischargePageViewModel_AfterLoadSurgicalConciergeDischargeData(object sender, EventArgs e)
        {
            _scgProstatectomyViewModelList = _surgicalConciergeDischargePageViewModel.ScgProstatectomyViewModels;
            _scgProfessionalProcedureProstectomyIdList = _surgicalConciergeDischargePageViewModel.ScgProfessionalProcedureProstectomyIdList;
            _scgProstatectomyViewModelTmpList = _scgProstatectomyViewModelList.ToList();
            CreateFormCheckbox(_scgProstatectomyViewModelTmpList);
            App.HideUserDialogAsync();
        }
    }
}