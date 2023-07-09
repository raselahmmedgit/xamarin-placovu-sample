using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConceirgePatientAddNew : CustomModalContentPage
    {
        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;
        private readonly ITokenContainer _iTokenContainer;
        private AdministrationPatientProfileRestApiService adminRestApiService;
        private SurgicalConciergeRestApiService restApiService;
        private long PatientProfileId;
        private long PracticeProfileId;
        private Guid? PatientProcedureDetailId;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel;
        private SurgicalConciergeDetail _surgicalConciergeDetail;
        private bool isSurgicalConciergeDetailPage = false;
        private SurgicalConciergePatientView _surgicalConciergePatientView { get; set; }
        private SurgicalConciergePatientPage _surgicalConciergePatientPage { get; set; }
        public List<CountryViewModel> _countryViewModelList;

        private bool IsAddMode = false;
        private bool IsEditMode = false;

        public SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel;
        public List<PatientAttendeeProfileViewModel> PatientAttendeeProfileViewModelList;
        ObservableCollection<PatientAttendeeProfileViewModel> _patientAttendeeProfileViewModelObservableCollection;

        //SurgicalConciergePatientView

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientView surgicalConciergePatientView, long practiceDivisionUnit)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientView = surgicalConciergePatientView;

            PracticeDivisionUnitDest = practiceDivisionUnit;

            SetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientView surgicalConciergePatientView, long practiceDivision, long practiceDivisionUnit, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientView = surgicalConciergePatientView;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;

            SetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientView surgicalConciergePatientView, long practiceDivision, long practiceDivisionUnit, long patientProfileId, Guid? patientProcedureDetailId, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientView = surgicalConciergePatientView;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;
            PatientProfileId = patientProfileId;
            PatientProcedureDetailId = patientProcedureDetailId.ToGuid();

            LoadAndSetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientView surgicalConciergePatientView, long practiceDivision, long practiceDivisionUnit, Guid? patientProcedureDetailId, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientView = surgicalConciergePatientView;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;
            PatientProcedureDetailId = patientProcedureDetailId.ToGuid();

            PatientProfileId = surgicalConciergePatientViewModel.PatientProfileId;

            LoadAndSetSurgicalConciergePatientViewModelData(surgicalConciergePatientViewModel);
        }

        //SurgicalConciergePatientView

        public SurgicalConceirgePatientAddNew(SurgicalConciergeDetail surgicalConciergeDetail, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergeDetail = surgicalConciergeDetail;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            SetSurgicalConciergePatientViewModelData();
        }

        //SurgicalConciergePatientPage

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientPage surgicalConciergePatientPage, long practiceDivisionUnit)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientPage = surgicalConciergePatientPage;

            PracticeDivisionUnitDest = practiceDivisionUnit;

            SetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientPage surgicalConciergePatientPage, long practiceDivision, long practiceDivisionUnit, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientPage = surgicalConciergePatientPage;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;

            SetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientPage surgicalConciergePatientPage, long practiceDivision, long practiceDivisionUnit, long patientProfileId, Guid? patientProcedureDetailId, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientPage = surgicalConciergePatientPage;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;
            PatientProfileId = patientProfileId;
            PatientProcedureDetailId = patientProcedureDetailId.ToGuid();

            LoadAndSetSurgicalConciergePatientViewModelData();
        }

        public SurgicalConceirgePatientAddNew(SurgicalConciergePatientPage surgicalConciergePatientPage, long practiceDivision, long practiceDivisionUnit, Guid? patientProcedureDetailId, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, bool isSurgicalConciergeDetailPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergePatientPage = surgicalConciergePatientPage;
            this.isSurgicalConciergeDetailPage = isSurgicalConciergeDetailPage;

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;
            PatientProcedureDetailId = patientProcedureDetailId.ToGuid();

            PatientProfileId = surgicalConciergePatientViewModel.PatientProfileId;

            LoadAndSetSurgicalConciergePatientViewModelData(surgicalConciergePatientViewModel);
        }


        class Department
        {
            public string Name { get; set; }
            public long Value { get; set; }
            public bool IsChecked { get; set; }
        }

        private void PracticeDivisionRadioButton_Clicked(object sender, EventArgs e)
        {
            var item = sender as RadioButton;
            _surgicalConciergePatientViewModel.PatientDivisionId = (item?.Value as Department)?.Value;
        }

        private void CreatePracticeDivisionRadioButtonView(SurgicalConciergePatientViewModel viewModel)
        {

            var departmentList = viewModel.Departments.Select(e => new Department
            {
                Name = e.DivisionName,
                Value = e.ScgDivisionId,
                IsChecked = false,
            }).ToList();
            if (departmentList.Any(e => e.Value == viewModel.PatientDivisionId))
            {
                var department = departmentList.First(e => e.Value == viewModel.PatientDivisionId);
                department.IsChecked = true;
            }

            var radiobuttonGroup = new RadioButtonGroupView();
            foreach (var department in departmentList)
            {
                var item = new RadioButton
                {
                    Text = department.Name,
                    TextFontSize = 16,
                    TextColor = Color.FromHex("#000000"),
                    Value = department,
                    IsChecked = department.IsChecked,
                    Color = Color.FromHex("#009688"),
                    CircleColor = Color.FromHex("#009688")
                };
                item.Clicked += PracticeDivisionRadioButton_Clicked;
                radiobuttonGroup.Children.Add(item);
            }
            PracticeDivisionList.Children.Clear();
            PracticeDivisionList.Children.Add(radiobuttonGroup);
        }

        //SurgicalConciergePatientPage

        private async void SetSurgicalConciergePatientViewModelData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergePatientViewModel data = await restApiService.GetSurgicalConciergePatient();
                _surgicalConciergePatientViewModel = data;
                SurgicalConciergePatientViewModel = _surgicalConciergePatientViewModel;
                PracticeProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
                PatientProfileIdTextBox.Text = Convert.ToString(0);
                string practiceName = _iTokenContainer.ApiPracticeName;

                IsAddMode = true;
                IsEditMode = false;

                if (_surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions != null)
                {
                    PracticeProfilePicker.ItemsSource = _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions.ToList();
                    PracticeProfilePicker.SelectedIndex = AppConstants.PickerDefaultIndex;
                }
                else
                {
                    PracticeProfilePicker.ItemsSource = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = practiceName, Value = PracticeProfileId.ToString() } };
                    PracticeProfilePicker.SelectedIndex = 0;
                }

                EnabledPracticeProfilePicker();

                //if (_surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions != null)
                //{
                //    PracticeProcedurePicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList();
                //}
                //else
                //{
                //    PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
                //}
                //PracticeProcedurePicker.SelectedIndex = 0;

                if (_surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions != null)
                {
                    PracticeProfessionalPicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList();
                }
                else
                {
                    PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
                }
                PracticeProfessionalPicker.SelectedIndex = AppConstants.PickerDefaultIndex;

                //_countryViewModelList = await restApiService.GetCountryList();
                //var countryViewModelList = _countryViewModelList.Where(c => !c.CountryIso.Equals("UM")).ToList();
                //foreach (var countryViewModel in countryViewModelList)
                //{
                //    countryCodePicker.Items.Add("+" + countryViewModel.PhoneCode);
                //}
                var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
                foreach (var countryViewModel in country)
                {
                    countryCodePicker.Items.Add(countryViewModel);
                }
                countryCodePicker.SelectedIndex = countryCodePicker.Items.IndexOf("+1");
            }


        }

        private async void LoadAndSetSurgicalConciergePatientViewModelData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    SurgicalConciergePatientViewModel data = await restApiService.EditSurgicalConciergePatientProfile(PatientProfileId, PatientProcedureDetailId, Convert.ToInt16(PracticeDivisionDest), Convert.ToInt16(PracticeDivisionUnitDest));
                    _surgicalConciergePatientViewModel = data;
                    SurgicalConciergePatientViewModel = _surgicalConciergePatientViewModel;
                    PracticeProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
                    string practiceName = _iTokenContainer.ApiPracticeName;

                    IsAddMode = false;
                    IsEditMode = true;

                    int practiceProfilePickerSelectedIndex = AppConstants.PickerDefaultIndex;
                    if (_surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProfilePicker.ItemsSource = _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions.ToList();
                        practiceProfilePickerSelectedIndex = _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.PracticeProfileId.ToString()).ToInt();
                        PracticeProfilePicker.SelectedIndex = practiceProfilePickerSelectedIndex;
                    }
                    else
                    {
                        PracticeProfilePicker.ItemsSource = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = practiceName, Value = PracticeProfileId.ToString() } };
                        PracticeProfilePicker.SelectedIndex = 0;
                    }

                    EnabledPracticeProfilePicker();

                    int practiceProcedurePickerSelectedIndex = 0;
                    if (_surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProcedurePicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList();
                        practiceProcedurePickerSelectedIndex = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.ProcedureId.ToString()).ToInt();
                    }
                    else
                    {
                        PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
                    }
                    PracticeProcedurePicker.SelectedIndex = practiceProcedurePickerSelectedIndex;

                    int practiceProfessionalPickerSelectedIndex = AppConstants.PickerDefaultIndex;
                    if (_surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProfessionalPicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList();
                        practiceProfessionalPickerSelectedIndex = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.ProfessionalProfileId.ToString()).ToInt();
                    }
                    else
                    {
                        PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
                    }
                    PracticeProfessionalPicker.SelectedIndex = practiceProfessionalPickerSelectedIndex;

                    var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
                    foreach (var countryViewModel in country)
                    {
                        countryCodePicker.Items.Add(countryViewModel);
                    }

                    countryCodePicker.SelectedIndex = countryCodePicker.Items.IndexOf(_surgicalConciergePatientViewModel.PrimaryPhoneCode);

                    PatientProfileIdTextBox.Text = _surgicalConciergePatientViewModel.PatientProfileId.ToString();
                    FirstNameTextBox.Text = _surgicalConciergePatientViewModel.FirstName;
                    LastNameTextBox.Text = _surgicalConciergePatientViewModel.LastName;

                    if ((!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.EmailAddress) || _surgicalConciergePatientViewModel.EmailAddress != ""))
                    {
                        EmailTextBox.Text = _surgicalConciergePatientViewModel.EmailAddress;
                    }

                    if ((!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.PrimaryPhone) || _surgicalConciergePatientViewModel.PrimaryPhone != ""))
                    {
                        PhoneNumber.Text = _surgicalConciergePatientViewModel.PrimaryPhone;
                    }

                    SurgeryDateDatePicker.Date = Convert.ToDateTime(_surgicalConciergePatientViewModel.SurgeryDate);

                    TimeSpan surgeryTime = DateTime.Parse(_surgicalConciergePatientViewModel.SurgeryTime?.Trim()).TimeOfDay;

                    SurgeryTimeTimePicker.Time = surgeryTime;

                    //SurgicalConceirgeRoomTextBox.Text = _surgicalConciergePatientViewModel.SurgicalConceirgeRoom;

                    if (_surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels != null)
                    {
                        PatientAttendeeProfileViewModelList = _surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels.ToList();
                        _patientAttendeeProfileViewModelObservableCollection = new ObservableCollection<PatientAttendeeProfileViewModel>(_surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels);
                        //SurgicalConciergePatientAttendeeListView.ItemsSource = null;

                        //SurgicalConciergePatientAttendeeListView.ItemsSource = _patientAttendeeProfileViewModelObservableCollection;
                        CreateAdditionalContactContentView(_surgicalConciergePatientViewModel);

                        if (SurgicalConciergePatientAttendeeListStackLayout.IsVisible == false)
                        {
                            SurgicalConciergePatientAttendeeListStackLayout.IsVisible = true;
                        }
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void LoadAndSetSurgicalConciergePatientViewModelData(SurgicalConciergePatientViewModel data)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    _surgicalConciergePatientViewModel = data;
                    var departmentList = await restApiService.GetSurgicalConciergeDepartment(data.PracticeProfileId);
                    _surgicalConciergePatientViewModel.Departments = departmentList;
                    CreatePracticeDivisionRadioButtonView(_surgicalConciergePatientViewModel);
                    SurgicalConciergePatientViewModel = _surgicalConciergePatientViewModel;
                    PracticeProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
                    string practiceName = _iTokenContainer.ApiPracticeName;

                    IsAddMode = false;
                    IsEditMode = true;

                    int practiceProfilePickerSelectedIndex = AppConstants.PickerDefaultIndex;
                    if (_surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProfilePicker.ItemsSource = _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions.ToList();
                        practiceProfilePickerSelectedIndex = _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.PracticeProfileDropDownViewModel.PracticeProfileId.ToString()).ToInt();
                        PracticeProfilePicker.SelectedIndex = practiceProfilePickerSelectedIndex;
                    }
                    else
                    {
                        PracticeProfilePicker.ItemsSource = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = practiceName, Value = PracticeProfileId.ToString() } };
                        PracticeProfilePicker.SelectedIndex = 0;
                    }

                    EnabledPracticeProfilePicker();

                    int practiceProcedurePickerSelectedIndex = 0;
                    if (_surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProcedurePicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList();
                        practiceProcedurePickerSelectedIndex = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.ProcedureId.ToString()).ToInt();
                    }
                    else
                    {
                        PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
                    }
                    PracticeProcedurePicker.SelectedIndex = practiceProcedurePickerSelectedIndex;

                    int practiceProfessionalPickerSelectedIndex = AppConstants.PickerDefaultIndex;
                    if (_surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProfessionalPicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList();
                        practiceProfessionalPickerSelectedIndex = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.ProfessionalProfileId.ToString()).ToInt();
                    }
                    else
                    {
                        PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
                    }
                    PracticeProfessionalPicker.SelectedIndex = practiceProfessionalPickerSelectedIndex;

                    //_countryViewModelList = await restApiService.GetCountryList();
                    //var countryViewModelList = _countryViewModelList.Where(c => !c.CountryIso.Equals("UM")).ToList();
                    //foreach (var countryViewModel in countryViewModelList)
                    //{
                    //    countryCodePicker.Items.Add("+" + countryViewModel.PhoneCode);
                    //}
                    var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
                    foreach (var countryViewModel in country)
                    {
                        countryCodePicker.Items.Add(countryViewModel);
                    }
                    countryCodePicker.SelectedIndex = countryCodePicker.Items.IndexOf(_surgicalConciergePatientViewModel.PrimaryPhoneCode);

                    PatientProfileIdTextBox.Text = _surgicalConciergePatientViewModel.PatientProfileId.ToString();
                    FirstNameTextBox.Text = _surgicalConciergePatientViewModel.FirstName;
                    LastNameTextBox.Text = _surgicalConciergePatientViewModel.LastName;

                    if ((!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.EmailAddress) || _surgicalConciergePatientViewModel.EmailAddress != ""))
                    {
                        EmailTextBox.Text = _surgicalConciergePatientViewModel.EmailAddress;
                    }

                    if ((!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.PrimaryPhone) || _surgicalConciergePatientViewModel.PrimaryPhone != ""))
                    {
                        PhoneNumber.Text = _surgicalConciergePatientViewModel.PrimaryPhone;
                    }

                    SurgeryDateDatePicker.Date = Convert.ToDateTime(_surgicalConciergePatientViewModel.SurgeryDate);

                    TimeSpan surgeryTime = DateTime.Parse(_surgicalConciergePatientViewModel.SurgeryTime?.Trim()).TimeOfDay;

                    SurgeryTimeTimePicker.Time = surgeryTime;

                    //SurgicalConceirgeRoomTextBox.Text = _surgicalConciergePatientViewModel.SurgicalConceirgeRoom;

                    if (_surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels != null)
                    {
                        PatientAttendeeProfileViewModelList = _surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels.ToList();
                        _patientAttendeeProfileViewModelObservableCollection = new ObservableCollection<PatientAttendeeProfileViewModel>(_surgicalConciergePatientViewModel.PatientAttendeeProfileViewModels);
                        //SurgicalConciergePatientAttendeeListView.ItemsSource = null;

                        //SurgicalConciergePatientAttendeeListView.ItemsSource = _patientAttendeeProfileViewModelObservableCollection;
                        CreateAdditionalContactContentView(_surgicalConciergePatientViewModel);

                        if (SurgicalConciergePatientAttendeeListStackLayout.IsVisible == false)
                        {
                            SurgicalConciergePatientAttendeeListStackLayout.IsVisible = true;
                        }
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void ProfessionalAddButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    await Navigation.PushModalAsync(new SurgicalConciergeProfessionalAdd(this, PracticeDivisionDest, PracticeDivisionUnitDest));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private void EnabledPracticeProfilePicker()
        {
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();

            if (userIdentityModel != null)
            {
                if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
                {
                    PracticeProfilePicker.IsEnabled = false;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
                {
                    PracticeProfilePicker.IsEnabled = true;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
                {
                    PracticeProfilePicker.IsEnabled = true;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
                {
                    PracticeProfilePicker.IsEnabled = true;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
                {
                    PracticeProfilePicker.IsEnabled = false;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
                {
                    PracticeProfilePicker.IsEnabled = false;
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
                {
                    PracticeProfilePicker.IsEnabled = true;
                }
                else
                {
                    PracticeProfilePicker.IsEnabled = true;
                }
            }
        }

        public void UpdateProfessionalProfileDropdown(IEnumerable<SelectListItem> professionalList)
        {
            PracticeProfessionalPicker.ItemsSource = professionalList.ToList();
            PracticeProfessionalPicker.SelectedIndex = 0;
        }

        public void UpdateSurgicalConciergePatientAttendeeListView()
        {
            try
            {
                _patientAttendeeProfileViewModelObservableCollection = new ObservableCollection<PatientAttendeeProfileViewModel>(PatientAttendeeProfileViewModelList);
                //SurgicalConciergePatientAttendeeListView.ItemsSource = null;
                //SurgicalConciergePatientAttendeeListView.ItemsSource = _patientAttendeeProfileViewModelObservableCollection;

                CreateAdditionalContactContentView(_surgicalConciergePatientViewModel);

                if (SurgicalConciergePatientAttendeeListStackLayout.IsVisible == false)
                {
                    SurgicalConciergePatientAttendeeListStackLayout.IsVisible = true;
                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        public void UpdateSurgicalConciergePatientAttendeeListView(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            try
            {
                _patientAttendeeProfileViewModelObservableCollection = new ObservableCollection<PatientAttendeeProfileViewModel>(PatientAttendeeProfileViewModelList);
                //SurgicalConciergePatientAttendeeListView.ItemsSource = null;
                //SurgicalConciergePatientAttendeeListView.ItemsSource = _patientAttendeeProfileViewModelObservableCollection;

                CreateAdditionalContactContentView(surgicalConciergePatientViewModel);

                if (SurgicalConciergePatientAttendeeListStackLayout.IsVisible == false)
                {
                    SurgicalConciergePatientAttendeeListStackLayout.IsVisible = true;
                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void SurgicalConciergePatientCancelButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync();
            await Navigation.PopAsync();
        }

        private async void SurgicalConciergePatientAddButton_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }

            try
            {
                if (!FormValidationSuccess())
                    return;

                SurgicalConciergePatientViewModel model = new SurgicalConciergePatientViewModel();
                model.PatientProfileId = Convert.ToInt32(PatientProfileIdTextBox.Text?.Trim());
                model.FirstName = FirstNameTextBox.Text?.Trim();
                model.LastName = LastNameTextBox.Text?.Trim();
                //model.PracticeProfileId = PracticeProfileId;
                model.EmailAddress = EmailTextBox.Text?.Trim();
                model.PrimaryPhone = PhoneNumber.Text?.Trim();
                var countryCode = countryCodePicker.SelectedItem.ToString();
                model.PrimaryPhoneCode = countryCode;

                model.PatientProcedureDetailId = PatientProcedureDetailId != null ? PatientProcedureDetailId : null;

                var selectedPractice = PracticeProfilePicker.SelectedItem as SelectListItem;
                if (selectedPractice != null)
                {
                    model.PracticeProfileId = long.Parse(selectedPractice.Value);
                    model.PracticeName = selectedPractice.Text;
                    if (_surgicalConciergePatientViewModel != null && _surgicalConciergePatientViewModel.PatientDivisionId != null)
                    {
                        model.PatientDivisionId = _surgicalConciergePatientViewModel.PatientDivisionId;
                    }
                    else
                    {
                        model.PatientDivisionId = (int)Enums.PracticeDivision.Urology;
                    }
                }

                var selectedProcedure = PracticeProcedurePicker.SelectedItem as SelectListItem;
                if (selectedProcedure != null)
                {
                    model.ProcedureId = long.Parse(selectedProcedure.Value);
                    model.ProcedureName = selectedProcedure.Text;
                }

                var selectedProfessional = PracticeProfessionalPicker.SelectedItem as SelectListItem;
                if (selectedProfessional != null)
                {
                    model.ProfessionalProfileId = long.Parse(selectedProfessional.Value);
                    model.ProfessionalName = selectedProfessional.Text;
                }

                var surgeryDateTime = Convert.ToDateTime(Convert.ToDateTime(SurgeryDateDatePicker.Date + SurgeryTimeTimePicker.Time).ToString("dd/MMM/yyyy hh:mm tt"));

                model.SurgeryDate = surgeryDateTime;
                //model.SurgeryDateInString = model.SurgeryDate.ToString("dd/MMM/yyyy");
                model.SurgeryTime = SurgeryTimeTimePicker.Time.ToString();
                model.SurgeryDateTime = surgeryDateTime;
                //model.SurgicalConceirgeRoom = SurgicalConceirgeRoomTextBox.Text?.Trim();

                model.SurgeryDateString = surgeryDateTime.ToString("MM/dd/yyyy");

                if (PatientAttendeeProfileViewModelList != null && PatientAttendeeProfileViewModelList.Count > 0)
                {
                    model.PatientAttendeeProfileViewModels = PatientAttendeeProfileViewModelList;
                }

                ApiExecutionResult<SurgicalConciergePatientViewModel> result = new ApiExecutionResult<SurgicalConciergePatientViewModel>();
                using (UserDialogs.Instance.Loading(""))
                {
                    result = await restApiService.PostSurgicalConciergePatientAddOrEditNew(model);
                }

                if (result.Success)
                {
                    UtilHelper.ShowToastMessage(result.Message);
                    //await Navigation.PopModalAsync();
                    await Navigation.PopAsync();
                    this._surgicalConciergePatientView.ReLoadData();
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, result.Message, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }



        }

        private async void SurgicalConciergePatientAttendeeAddButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    //List<CountryViewModel> countryList = await restApiService.GetCountryList();
                    List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                    int attendeeProfileTypeId = Enums.AttendeeProfileTypeEnum.Patient.ToInt();
                    string attendeeProfileTypeName = Enums.AttendeeProfileTypeEnum.Patient.ToDescriptionAttr();
                    patientAttendeeProfileTypeViewModelList.Add(new PatientAttendeeProfileTypeViewModel { AttendeeProfileTypeId = attendeeProfileTypeId, AttendeeProfileTypeName = attendeeProfileTypeName, DisplayOrder = 0 });
                    patientAttendeeProfileTypeViewModelList = patientAttendeeProfileTypeViewModelList.OrderBy(x => x.AttendeeProfileTypeId).ToList();
                    //await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeAddPage(this, 0, countryList, patientAttendeeProfileTypeViewModelList));
                    await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeAddPage(this, 0, patientAttendeeProfileTypeViewModelList));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private async void SurgicalConciergeAddButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    await Navigation.PushModalAsync(new SurgicalConciergeProfessionalAdd(this, PracticeDivisionDest, PracticeDivisionUnitDest));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void SurgicalConciergePatientAttendeeListEditButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    if (!InternetConnectHelper.CheckConnection())
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                        return;
                    }
                    Button btn = (Button)sender;
                    long attendeeProfileId = btn.ClassId.ToLong();
                    //btn.Image = "update_icon.png";

                    if (PatientAttendeeProfileViewModelList != null && PatientAttendeeProfileViewModelList.Count > 0)
                    {
                        int attendeeProfileTypeId = Enums.AttendeeProfileTypeEnum.Patient.ToInt();
                        string attendeeProfileTypeName = Enums.AttendeeProfileTypeEnum.Patient.ToDescriptionAttr();
                        PatientAttendeeProfileViewModel patientAttendeeProfileViewModel = PatientAttendeeProfileViewModelList.FirstOrDefault(x => x.AttendeeProfileId == attendeeProfileId);

                        if (patientAttendeeProfileViewModel != null)
                        {
                            //List<CountryViewModel> countryList = await restApiService.GetCountryList();
                            List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                            patientAttendeeProfileTypeViewModelList.Add(new PatientAttendeeProfileTypeViewModel { AttendeeProfileTypeId = attendeeProfileTypeId, AttendeeProfileTypeName = attendeeProfileTypeName, DisplayOrder = 0 });
                            patientAttendeeProfileTypeViewModelList = patientAttendeeProfileTypeViewModelList.OrderBy(x => x.AttendeeProfileTypeId).ToList();
                            //await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileViewModel.PatientProfileId, countryList, patientAttendeeProfileTypeViewModelList));
                            await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileViewModel.PatientProfileId, patientAttendeeProfileTypeViewModelList));
                        }
                        else
                        {
                            UtilHelper.ShowToastMessage(AppConstant.NotFound);
                        }
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NotFound);
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void SurgicalConciergePatientAttendeeListDeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }
                Button btn = (Button)sender;
                long attendeeProfileId = btn.ClassId.ToLong();
                var answer = await DisplayAlert("Delete Confirm", "Are you sure you want to delete this recipient?", "Yes", "No");
                if (answer)
                {
                    using (UserDialogs.Instance.Loading(""))
                    {
                        if (PatientAttendeeProfileViewModelList != null && PatientAttendeeProfileViewModelList.Count > 0)
                        {
                            PatientAttendeeProfileViewModel patientAttendeeProfileViewModel = PatientAttendeeProfileViewModelList.FirstOrDefault(x => x.AttendeeProfileId == attendeeProfileId);

                            if (patientAttendeeProfileViewModel != null)
                            {
                                PatientAttendeeProfileViewModelList.Remove(patientAttendeeProfileViewModel);
                                UtilHelper.ShowToastMessage(AppConstant.DeleteSuccessMessage);
                                UpdateSurgicalConciergePatientAttendeeListView();
                            }
                            else
                            {
                                UtilHelper.ShowToastMessage(AppConstant.NotFound);
                            }
                        }
                        else
                        {
                            UtilHelper.ShowToastMessage(AppConstant.NotFound);
                        }
                    }

                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        private async void SurgicalConciergePatientEditButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    if (!InternetConnectHelper.CheckConnection())
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                        return;
                    }
                    Button btn = (Button)sender;
                    long patientProfileId = btn.ClassId.ToLong();
                    //btn.Image = "update_icon.png";

                    if (_surgicalConciergePatientViewModel.PatientProfileId == patientProfileId)
                    {
                        int attendeeProfileTypeId = Enums.AttendeeProfileTypeEnum.Patient.ToInt();
                        string attendeeProfileTypeName = Enums.AttendeeProfileTypeEnum.Patient.ToDescriptionAttr();
                        PatientAttendeeProfileViewModel patientAttendeeProfileViewModel = new PatientAttendeeProfileViewModel();
                        patientAttendeeProfileViewModel.AttendeeProfileId = 0;
                        patientAttendeeProfileViewModel.AttendeeProfileTypeId = attendeeProfileTypeId;
                        patientAttendeeProfileViewModel.AttendeeProfileTypeName = attendeeProfileTypeName;
                        patientAttendeeProfileViewModel.EmailAddress = _surgicalConciergePatientViewModel.EmailAddress;
                        patientAttendeeProfileViewModel.PrimaryPhone = _surgicalConciergePatientViewModel.PrimaryPhone;
                        patientAttendeeProfileViewModel.PrimaryPhoneCode = _surgicalConciergePatientViewModel.PrimaryPhoneCode;

                        if (patientAttendeeProfileViewModel != null)
                        {
                            //List<CountryViewModel> countryList = await restApiService.GetCountryList();
                            List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                            patientAttendeeProfileTypeViewModelList.Add(new PatientAttendeeProfileTypeViewModel { AttendeeProfileTypeId = attendeeProfileTypeId, AttendeeProfileTypeName = attendeeProfileTypeName, DisplayOrder = 0 });
                            patientAttendeeProfileTypeViewModelList = patientAttendeeProfileTypeViewModelList.OrderBy(x => x.AttendeeProfileTypeId).ToList();
                            //await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileViewModel.PatientProfileId, countryList, patientAttendeeProfileTypeViewModelList));
                            await Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileViewModel.PatientProfileId, patientAttendeeProfileTypeViewModelList));
                        }
                        else
                        {
                            UtilHelper.ShowToastMessage(AppConstant.NotFound);
                        }
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NotFound);
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void SurgicalConciergePatientDeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }
                Button btn = (Button)sender;
                long patientProfileId = btn.ClassId.ToLong();
                var answer = await DisplayAlert("Delete Confirm", "Are you sure you want to delete this recipient?", "Yes", "No");
                if (answer)
                {
                    using (UserDialogs.Instance.Loading(""))
                    {
                        if (_surgicalConciergePatientViewModel.PatientProfileId == patientProfileId)
                        {
                            //Call PatientProfile Deletr Api
                        }
                        else
                        {
                            UtilHelper.ShowToastMessage(AppConstant.NotFound);
                        }
                    }

                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        private bool FormValidationSuccess()
        {
            bool isValid = true;
            ErrorFirstNameTextBox.IsVisible = false;
            ErrorLastNameTextBox.IsVisible = false;
            ErrorPracticeProfilePicker.IsVisible = false;
            ErrorPracticeProcedurePicker.IsVisible = false;
            ErrorPracticeProfessionalPicker.IsVisible = false;
            ErrorEmailTextBox.IsVisible = false;

            //if (!ValidateFirstNameTextBox() || !ValidateLastNameTextBox() || !ValidatePracticeProfilePicker() || !ValidateProcedurePicker() || !ValidateProfessionalPicker() || !ValidateEmailAddress() || !ValidatePhoneNumber())
            if (!ValidateFirstNameTextBox() || !ValidateLastNameTextBox() || !ValidatePracticeProfilePicker() || !ValidateProcedurePicker() || !ValidateProfessionalPicker())
            {
                isValid = false;
            }
            return isValid;

        }

        private void FirstNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateFirstNameTextBox();
        }
        private void LastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateLastNameTextBox();
        }
        private void EmailTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateEmailAddress();
        }
        private void PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            ValidatePhoneNumber();
        }

        private async void PracticeProfilePicker_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            ValidatePracticeProfilePicker();

            if (PracticeProfilePicker.SelectedIndex != AppConstants.PickerDefaultIndex)
            {
                long practiceProfileId = 0;
                var selectedPractice = PracticeProfilePicker.SelectedItem as SelectListItem;
                if (selectedPractice != null)
                {
                    practiceProfileId = long.Parse(selectedPractice.Value);
                    var departmentList = await restApiService.GetSurgicalConciergeDepartment(practiceProfileId);
                    _surgicalConciergePatientViewModel.Departments = departmentList;
                    CreatePracticeDivisionRadioButtonView(_surgicalConciergePatientViewModel);
                    await LoadPracticeProfessionalPickerAsync(practiceProfileId);
                }
            }
        }
        private void PracticeProcedurePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateProcedurePicker();
        }
        private void PracticeProfessionalPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isValidated = ValidateProfessionalPicker();
            if (isValidated)
            {
                var selectedProfessional = PracticeProfessionalPicker.SelectedItem as SelectListItem;
                var selectedPractice = PracticeProfilePicker.SelectedItem as SelectListItem;
                if (selectedProfessional != null && selectedPractice != null)
                {
                    var practiceProfileId = long.Parse(selectedPractice.Value);
                    var professionalProfileId = long.Parse(selectedProfessional.Value);
                    LoadProfessionalProcedurePicker(practiceProfileId, professionalProfileId);
                }
            }
        }

        public bool ValidateFirstNameTextBox()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(FirstNameTextBox.Text))
            {
                isValid = false;
                ErrorFirstNameTextBox.Text = "First Name Required";

                ErrorFirstNameTextBox.IsVisible = true;
            }
            else
            {
                ErrorFirstNameTextBox.IsVisible = false;
            }
            return isValid;
        }

        public bool ValidateLastNameTextBox()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(LastNameTextBox.Text))
            {
                isValid = false;
                ErrorLastNameTextBox.Text = "Last Name Required";

                ErrorLastNameTextBox.IsVisible = true;
            }
            else
            {
                ErrorLastNameTextBox.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidatePhoneNumber()
        {
            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumberMti(PhoneNumber.Text?.Trim());
            if (validationResult.success == false)
            {
                ErrorPhoneNumber.Text = validationResult.message;

                ErrorPhoneNumber.IsVisible = true;
            }
            else
            {
                ErrorPhoneNumber.IsVisible = false;
            }

            return validationResult.success;
        }

        private async void LoadProfessionalProcedurePicker(long practiceProfileId, long professionalProfileId)
        {
            ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = await restApiService.GetProfessionalProcedureDropdown(practiceProfileId, professionalProfileId);

            if (professionalProcedureDropDownViewModel != null && professionalProcedureDropDownViewModel.SelectOptions != null)
            {
                PracticeProcedurePicker.ItemsSource = professionalProcedureDropDownViewModel.SelectOptions.ToList();
                PracticeProcedurePicker.SelectedItem = professionalProcedureDropDownViewModel.SelectOptions.FirstOrDefault(c => c.Value.Equals(_surgicalConciergePatientViewModel.ProcedureId.ToString()));
            }
            else
            {
                PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
            }

        }

        private async Task LoadPracticeProfessionalPickerAsync(long practiceProfileId)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = await restApiService.GetPracticeProcedureDropdown(practiceProfileId);

                //if (professionalProcedureDropDownViewModel != null && professionalProcedureDropDownViewModel.SelectOptions != null)
                //{
                //    PracticeProcedurePicker.ItemsSource = professionalProcedureDropDownViewModel.SelectOptions.ToList();
                //}
                //else
                //{
                //    PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
                //}
                //PracticeProcedurePicker.SelectedIndex = 0;

                ProfessionalProfileDropDownViewModel professionalProfileDropDownViewModel = await restApiService.GetPracticeProfessionalProfileDropdown(practiceProfileId);

                if (professionalProfileDropDownViewModel != null && professionalProfileDropDownViewModel.SelectOptions != null)
                {
                    PracticeProfessionalPicker.ItemsSource = professionalProfileDropDownViewModel.SelectOptions.ToList();
                    //PracticeProfessionalPicker.SelectedIndex = AppConstants.PickerDefaultIndex;
                    if (_surgicalConciergePatientViewModel.ProfessionalProfileId != null && _surgicalConciergePatientViewModel.ProfessionalProfileId > 0)
                    {
                        PracticeProfessionalPicker.SelectedItem = professionalProfileDropDownViewModel.SelectOptions.FirstOrDefault(c => c.Value.Equals(_surgicalConciergePatientViewModel.ProfessionalProfileId.ToString()));
                    }
                }
                else
                {
                    PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
                }


            }
        }

        private async Task LoadPracticeProcedurePickerAndPracticeProfessionalPickerForEditAsync(long practiceProfileId)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = await restApiService.GetPracticeProcedureDropdown(practiceProfileId);

                int practiceProcedurePickerSelectedIndex = 0;
                if (professionalProcedureDropDownViewModel != null && professionalProcedureDropDownViewModel.SelectOptions != null)
                {
                    PracticeProcedurePicker.ItemsSource = professionalProcedureDropDownViewModel.SelectOptions.ToList();
                    practiceProcedurePickerSelectedIndex = professionalProcedureDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.ProcedureId.ToString()).ToInt();
                }
                else
                {
                    PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
                }
                PracticeProcedurePicker.SelectedIndex = practiceProcedurePickerSelectedIndex;

                ProfessionalProfileDropDownViewModel professionalProfileDropDownViewModel = await restApiService.GetPracticeProfessionalProfileDropdown(practiceProfileId);

                int practiceProfessionalPickerSelectedIndex = AppConstants.PickerDefaultIndex;
                if (professionalProfileDropDownViewModel != null && professionalProfileDropDownViewModel.SelectOptions != null)
                {
                    PracticeProfessionalPicker.ItemsSource = professionalProfileDropDownViewModel.SelectOptions.ToList();
                    practiceProfessionalPickerSelectedIndex = professionalProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.ProfessionalProfileId.ToString()).ToInt();
                }
                else
                {
                    PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
                }
                PracticeProfessionalPicker.SelectedIndex = practiceProfessionalPickerSelectedIndex;

            }
        }

        private bool ValidatePracticeProfilePicker()
        {
            bool isValid = true;
            if (PracticeProfilePicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProfilePicker.Text = "Select a Practice";
                ErrorPracticeProfilePicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProfilePicker.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidateProcedurePicker()
        {
            bool isValid = true;
            if (PracticeProcedurePicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProcedurePicker.Text = "Select a Procedure";
                ErrorPracticeProcedurePicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProcedurePicker.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidateProfessionalPicker()
        {
            bool isValid = true;
            if (PracticeProfessionalPicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProfessionalPicker.Text = "Select a Professional";
                ErrorPracticeProfessionalPicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProfessionalPicker.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddressMti(EmailTextBox.Text?.Trim());

            if (validationResult.success == false)
            {
                ErrorEmailTextBox.Text = validationResult.message;

                ErrorEmailTextBox.IsVisible = true;
            }
            else
            {
                ErrorEmailTextBox.IsVisible = false;
            }

            return validationResult.success;
        }

        private void CreateAdditionalContactContentView(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {

            try
            {
                StackLayout mainLayout = new StackLayout();
                mainLayout.Children.Clear();

                if (!string.IsNullOrEmpty(surgicalConciergePatientViewModel.EmailAddress) || !string.IsNullOrEmpty(surgicalConciergePatientViewModel.PrimaryPhone))
                {
                    EmailTextBox.Text = surgicalConciergePatientViewModel.EmailAddress;
                    PhoneNumber.Text = surgicalConciergePatientViewModel.PrimaryPhone;

                    StackLayout mainLayoutInner = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 10, 10, 0),
                        Padding = 0,
                        Spacing = 0,
                        BackgroundColor = Color.FromHex("#FFFFFF")
                    };

                    Grid mainLayoutInnerGrid = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 10, 10, 0),
                        Padding = 0,
                        ColumnSpacing = 0,
                        RowSpacing = 0
                    };
                    mainLayoutInnerGrid.RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition {
                            Height = new GridLength(1, GridUnitType.Star)
                        }
                    };
                    mainLayoutInnerGrid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {
                            Width = new GridLength(80, GridUnitType.Absolute)
                        },
                        new ColumnDefinition {
                            Width = new GridLength(4, GridUnitType.Absolute)
                        },
                        new ColumnDefinition {
                            Width = new GridLength(1, GridUnitType.Star)
                        }
                    };

                    StackLayout column_1Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(5, 0, 5, 0),
                        Spacing = 0
                    };

                    Label attendeeProfileTypeNameLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = "Patient"
                    };

                    column_1Layout.Children.Add(attendeeProfileTypeNameLabel);

                    StackLayout column_2Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(0, 10),
                        Spacing = 0
                    };

                    BoxView boxView = new BoxView
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 4,
                        BackgroundColor = Color.FromHex("#61cfd3")
                    };

                    column_2Layout.Children.Add(boxView);

                    StackLayout column_3Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(5, 10, 0, 10),
                        Spacing = 0
                    };

                    Label attendeeProfileEmailLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = "Email: " + _surgicalConciergePatientViewModel.EmailAddress
                    };

                    Label attendeeProfileMobileLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = "Cell: " + _surgicalConciergePatientViewModel.PrimaryPhoneWithCountryCode
                    };

                    StackLayout attendeeProfileEditLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = 0,
                        Spacing = 0
                    };

                    Button attendeeProfileEditButton = new Button()
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Image = "update_icon.png",
                        BackgroundColor = Color.Transparent,
                        ClassId = _surgicalConciergePatientViewModel.PatientProfileId.ToString()
                    };
                    attendeeProfileEditButton.Clicked += SurgicalConciergePatientEditButton_Clicked;

                    //Button attendeeProfileDeleteButton = new Button()
                    //{
                    //    VerticalOptions = LayoutOptions.FillAndExpand,
                    //    HorizontalOptions = LayoutOptions.EndAndExpand,
                    //    Image = "delete_icon.png",
                    //    BackgroundColor = Color.Transparent,
                    //    ClassId = item.AttendeeProfileId.ToString()
                    //};
                    //attendeeProfileDeleteButton.Clicked += SurgicalConciergePatientDeleteButton_Clicked;

                    attendeeProfileEditLayout.Children.Add(attendeeProfileEditButton);

                    column_3Layout.Children.Add(attendeeProfileEmailLabel);
                    column_3Layout.Children.Add(attendeeProfileMobileLabel);
                    column_3Layout.Children.Add(attendeeProfileEditLayout);

                    mainLayoutInnerGrid.Children.Add(column_1Layout, 0, 0);
                    mainLayoutInnerGrid.Children.Add(column_2Layout, 1, 0);
                    mainLayoutInnerGrid.Children.Add(column_3Layout, 2, 0);

                    mainLayoutInner.Children.Add(mainLayoutInnerGrid);

                    mainLayout.Children.Add(mainLayoutInner);
                }

                foreach (var item in _patientAttendeeProfileViewModelObservableCollection)
                {
                    StackLayout mainLayoutInner = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 10, 10, 0),
                        Padding = 0,
                        Spacing = 0,
                        BackgroundColor = Color.FromHex("#FFFFFF")
                    };

                    Grid mainLayoutInnerGrid = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 10, 10, 0),
                        Padding = 0,
                        ColumnSpacing = 0,
                        RowSpacing = 0
                    };
                    mainLayoutInnerGrid.RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition {
                            Height = new GridLength(1, GridUnitType.Star)
                        }
                    };
                    mainLayoutInnerGrid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {
                            Width = new GridLength(80, GridUnitType.Absolute)
                        },
                        new ColumnDefinition {
                            Width = new GridLength(4, GridUnitType.Absolute)
                        },
                        new ColumnDefinition {
                            Width = new GridLength(1, GridUnitType.Star)
                        }
                    };

                    StackLayout column_1Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(5, 0, 5, 0),
                        Spacing = 0
                    };

                    Label attendeeProfileTypeNameLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = item.AttendeeProfileTypeName
                    };

                    column_1Layout.Children.Add(attendeeProfileTypeNameLabel);

                    StackLayout column_2Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(0, 10),
                        Spacing = 0
                    };

                    BoxView boxView = new BoxView
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 4,
                        BackgroundColor = Color.FromHex("#00a2ff")
                    };

                    column_2Layout.Children.Add(boxView);

                    StackLayout column_3Layout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = new Thickness(5, 10, 0, 10),
                        Spacing = 0
                    };

                    Label attendeeProfileEmailLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = "Email: " + item.EmailAddress
                    };

                    Label attendeeProfileMobileLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        FontSize = 16,
                        Text = "Cell: " + item.MobilePhoneWithCountryCode
                    };

                    StackLayout attendeeProfileEditLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = 0,
                        Padding = 0,
                        Spacing = 0
                    };

                    Button attendeeProfileEditButton = new Button()
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Image = "update_icon.png",
                        BackgroundColor = Color.Transparent,
                        ClassId = item.AttendeeProfileId.ToString()
                    };
                    attendeeProfileEditButton.Clicked += SurgicalConciergePatientAttendeeListEditButton_Clicked;

                    //Button attendeeProfileDeleteButton = new Button()
                    //{
                    //    VerticalOptions = LayoutOptions.FillAndExpand,
                    //    HorizontalOptions = LayoutOptions.EndAndExpand,
                    //    Image = "delete_icon.png",
                    //    BackgroundColor = Color.Transparent,
                    //    ClassId = item.AttendeeProfileId.ToString()
                    //};
                    //attendeeProfileDeleteButton.Clicked += SurgicalConciergePatientAttendeeListDeleteButton_Clicked;

                    attendeeProfileEditLayout.Children.Add(attendeeProfileEditButton);

                    column_3Layout.Children.Add(attendeeProfileEmailLabel);
                    column_3Layout.Children.Add(attendeeProfileMobileLabel);
                    column_3Layout.Children.Add(attendeeProfileEditLayout);

                    mainLayoutInnerGrid.Children.Add(column_1Layout, 0, 0);
                    mainLayoutInnerGrid.Children.Add(column_2Layout, 1, 0);
                    mainLayoutInnerGrid.Children.Add(column_3Layout, 2, 0);

                    mainLayoutInner.Children.Add(mainLayoutInnerGrid);

                    mainLayout.Children.Add(mainLayoutInner);
                }

                SurgicalConciergePatientAttendeeList.Children.Clear();
                SurgicalConciergePatientAttendeeList.Children.Add(mainLayout);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
    }
}
