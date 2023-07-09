using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class NursePatientInfoPatientDataService : BaseViewModel
    {
        private const int PageSize = (int)Enums.SearchCrieteria.pageSize;
        private int pageNo = 1;
        private readonly SurgicalConciergeRestApiService _dataService = new SurgicalConciergeRestApiService();
        public InfiniteScrollCollection<SurgicalConciergePatientViewModel> Items { get; }
        public bool IsLoadingCompleted { get; set; }

        NursePatientInfoPatientView _nursePatientInfoPatientView;

        public NursePatientInfoPatientDataService(NursePatientInfoPatientView nursePatientInfoPatientView)
        {
            try
            {
                this._nursePatientInfoPatientView = nursePatientInfoPatientView;
                Items = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
                {

                    OnLoadMore = async () =>
                    {
                        IsLoadingMore = true;
                        //int page = Items.Count / PageSize;
                        ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetNursePatientInfoPatientList(
                            ++this.pageNo
                            , PageSize);

                        IsLoadingMore = false;
                        return items;

                    },
                    OnCanLoadMore = () =>
                    {
                        IsLoadingCompleted = pageNo < 4;
                        return IsLoadingCompleted;
                    }
                };

                DownloadDataAsync(_nursePatientInfoPatientView);
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }


        }

        public NursePatientInfoPatientDataService(NursePatientInfoPatientView nursePatientInfoPatientView, bool isSearch)
        {
            this._nursePatientInfoPatientView = nursePatientInfoPatientView;
            Items = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
            {

                OnLoadMore = async () =>
                {
                    IsLoadingMore = true;
                    //int page = Items.Count / PageSize;
                    ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetNursePatientInfoPatientListSearchList(
                        ++this.pageNo
                        , PageSize
                        , (_nursePatientInfoPatientView.PracticeName == null ? string.Empty : _nursePatientInfoPatientView.PracticeName)
                        , (_nursePatientInfoPatientView.ProfessionalName == null ? string.Empty : _nursePatientInfoPatientView.ProfessionalName)
                        , (_nursePatientInfoPatientView.PatientName == null ? string.Empty : _nursePatientInfoPatientView.PatientName)
                        , (_nursePatientInfoPatientView.PatientName == null ? string.Empty : _nursePatientInfoPatientView.PatientName)
                        , (_nursePatientInfoPatientView.DateofBirth == null ? string.Empty : _nursePatientInfoPatientView.DateofBirth)
                        , (_nursePatientInfoPatientView.PatientPhoneCode == null ? string.Empty : _nursePatientInfoPatientView.PatientPhoneCode)
                        , (_nursePatientInfoPatientView.PatientPhone == null ? string.Empty : _nursePatientInfoPatientView.PatientPhone)
                        , (_nursePatientInfoPatientView.SurgeryDate == null ? null : _nursePatientInfoPatientView.SurgeryDate)
                        );
                    IsLoadingMore = false;
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    IsLoadingCompleted = pageNo < 4;
                    return IsLoadingCompleted;
                }
            };

            DownloadDataAsync(_nursePatientInfoPatientView, isSearch);

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

        private async Task DownloadDataAsync(NursePatientInfoPatientView nursePatientInfoPatientView)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetNursePatientInfoPatientList(
                        pageNo
                        , PageSize);
            Items.AddRange(items);
        }

        private async Task DownloadDataAsync(NursePatientInfoPatientView nursePatientInfoPatientView, bool isSearch)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> items = await _dataService.GetNursePatientInfoPatientListSearchList(
                        pageNo
                        , PageSize
                        , (nursePatientInfoPatientView.PracticeName == null ? string.Empty : nursePatientInfoPatientView.PracticeName)
                        , (nursePatientInfoPatientView.ProfessionalName == null ? string.Empty : nursePatientInfoPatientView.ProfessionalName)
                        , (nursePatientInfoPatientView.PatientName == null ? string.Empty : nursePatientInfoPatientView.PatientName)
                        , (nursePatientInfoPatientView.PatientEmail == null ? string.Empty : nursePatientInfoPatientView.PatientEmail)
                        , (nursePatientInfoPatientView.DateofBirth == null ? string.Empty : nursePatientInfoPatientView.DateofBirth)
                        , (nursePatientInfoPatientView.PatientPhoneCode == null ? string.Empty : nursePatientInfoPatientView.PatientPhoneCode)
                        , (nursePatientInfoPatientView.PatientPhone == null ? string.Empty : nursePatientInfoPatientView.PatientPhone)
                        , (nursePatientInfoPatientView.SurgeryDate == null ? null : nursePatientInfoPatientView.SurgeryDate));
            Items.AddRange(items);
        }
    }
}
