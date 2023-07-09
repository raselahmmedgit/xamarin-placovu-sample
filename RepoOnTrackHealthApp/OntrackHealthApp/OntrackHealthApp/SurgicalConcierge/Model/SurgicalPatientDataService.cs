using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalPatientDataService : BaseViewModel
    {
        private const int PageSize = (int)Enums.SearchCrieteria.DefaultPageSize;
        private int pageNo = 1;
        private readonly SurgicalConciergeRestApiService _dataService = new SurgicalConciergeRestApiService();
        public InfiniteScrollCollection<SurgicalConciergePatientViewModel> Items { get; set; }
        public bool IsLoadingCompleted { get; set; }

        SurgicalConciergePatientView _surgicalConciergePatientView;

        public SurgicalPatientDataService()
        {
            _dataService = new SurgicalConciergeRestApiService();
            Items = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>();
        }

        public SurgicalPatientDataService(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            this._surgicalConciergePatientView = surgicalConciergePatientView;
            Items = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
            {

                OnLoadMore = async () =>
                {
                    IsLoadingMore = true;
                    //int page = Items.Count / PageSize;
                    ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetPatientList(
                        _surgicalConciergePatientView.PracticeDivisionUnitDest
                        , ++this.pageNo
                        , PageSize
                        , (_surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay));

                    IsLoadingMore = false;
                    items.ToList().ForEach(x => {
                        x.SelectedSurgicalConciergePatientViewModel = x;
                    });
                    return items;

                },
                OnCanLoadMore = () =>
                {
                    IsLoadingCompleted = pageNo < 4;
                    return IsLoadingCompleted;
                }
            };
            Task.Run(async () => { await DownloadDataAsync(surgicalConciergePatientView); });
        }

        public SurgicalPatientDataService(SurgicalConciergePatientView surgicalConciergePatientView, bool isSearch)
        {

            this._surgicalConciergePatientView = surgicalConciergePatientView;
            Items = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
            {

                OnLoadMore = async () =>
                {
                    IsLoadingMore = true;
                    //int page = Items.Count / PageSize;
                    ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetPatientSearchList(
                        _surgicalConciergePatientView.PracticeDivisionUnitDest
                        , ++this.pageNo
                        , PageSize
                        , (_surgicalConciergePatientView.PracticeName == null ? string.Empty : _surgicalConciergePatientView.PracticeName)
                        , (_surgicalConciergePatientView.ProfessionalName == null ? string.Empty : _surgicalConciergePatientView.ProfessionalName)
                        , (_surgicalConciergePatientView.PatientName == null ? string.Empty : _surgicalConciergePatientView.PatientName)
                        , (_surgicalConciergePatientView.PatientEmail == null ? string.Empty : _surgicalConciergePatientView.PatientEmail)
                        , (_surgicalConciergePatientView.DateofBirth == null ? string.Empty : _surgicalConciergePatientView.DateofBirth)
                        , (_surgicalConciergePatientView.PatientPhoneCode == null ? string.Empty : _surgicalConciergePatientView.PatientPhoneCode)
                        , (_surgicalConciergePatientView.PatientPhone == null ? string.Empty : _surgicalConciergePatientView.PatientPhone)
                        , (_surgicalConciergePatientView.SurgeryDate == null ? null : _surgicalConciergePatientView.SurgeryDate)
                        , (_surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay));
                    IsLoadingMore = false;
                    items.ToList().ForEach(x => {
                        x.SelectedSurgicalConciergePatientViewModel = x;
                    });
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    IsLoadingCompleted = pageNo < 4;
                    return IsLoadingCompleted;
                }
            };

            Task.Run(async () => { await DownloadDataAsync(surgicalConciergePatientView, isSearch); });
        }

        bool _isLoadingMore;
        bool IsLoadingMore
        {
            get
            {
                return _isLoadingMore;
            }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged(nameof(IsLoadingMore));
            }
        }

        private async Task DownloadDataAsync(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetPatientList(
                        _surgicalConciergePatientView.PracticeDivisionUnitDest
                        , pageNo
                        , PageSize
                        , (_surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay));
            Items.AddRange(items);
            items.ToList().ForEach(x => {
                x.SelectedSurgicalConciergePatientViewModel = x;
            });
        }

        private async Task DownloadDataAsync(SurgicalConciergePatientView surgicalConciergePatientView, bool isSearch)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetPatientSearchList(
                      _surgicalConciergePatientView.PracticeDivisionUnitDest
                      , pageNo
                      , PageSize
                      , (_surgicalConciergePatientView.PracticeName == null ? string.Empty : _surgicalConciergePatientView.PracticeName)
                      , (_surgicalConciergePatientView.ProfessionalName == null ? string.Empty : _surgicalConciergePatientView.ProfessionalName)
                      , (_surgicalConciergePatientView.PatientName == null ? string.Empty : _surgicalConciergePatientView.PatientName)
                      , (_surgicalConciergePatientView.PatientEmail == null ? string.Empty : _surgicalConciergePatientView.PatientEmail)
                      , (_surgicalConciergePatientView.DateofBirth == null ? string.Empty : _surgicalConciergePatientView.DateofBirth)
                      , (_surgicalConciergePatientView.PatientPhoneCode == null ? string.Empty : _surgicalConciergePatientView.PatientPhoneCode)
                      , (_surgicalConciergePatientView.PatientPhone == null ? string.Empty : _surgicalConciergePatientView.PatientPhone)
                      , (_surgicalConciergePatientView.SurgeryDate == null ? null : _surgicalConciergePatientView.SurgeryDate)
                      , (_surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay));
            items.ToList().ForEach(x =>
            {
                x.SelectedSurgicalConciergePatientViewModel = x;
            });
            Items.AddRange(items);
        }

        public async Task<bool> DeleteSurgicalConciergePatientProfileAndReLoad(long patientProfileId)
        {
            var result = await _dataService.DeleteSurgicalConciergePatientProfile(patientProfileId);
            return result.Success;
        }
    }
}
