using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class SurgicalConciergePatientViewPageViewModel : OntrackHealthApp.ViewModel.BaseViewModel
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly SurgicalConciergeRestApiService _restApiService;
        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }
        private const int PageSize = (int)Enums.SearchCrieteria.DefaultPageSize;
        private int TotalRecord = 0;
        private int TotalPageCount = 0;
        public SurgicalConciergePatientView _surgicalConciergePatientView;
        public SurgicalConciergePatientPage _surgicalConciergePatientPage;
        public ProfessionalPatientPage _professionalPatientPage;
        public Command LoadPatientProfilesCommand { get; set; }
        public ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergePatientViewModelList;
        public InfiniteScrollCollection<SurgicalConciergePatientViewModel> SurgicalConciergePatientViewModeslInfiniteScroll { get; set; }

        public SurgicalConciergePatientViewPageViewModel(SurgicalConciergePatientView surgicalConciergePatientView)
        {
            _iTokenContainer = new TokenContainer();
            _restApiService = new SurgicalConciergeRestApiService();
            surgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            _surgicalConciergePatientView = surgicalConciergePatientView;
        }
        public SurgicalConciergePatientViewPageViewModel(SurgicalConciergePatientPage surgicalConciergePatientPage)
        {
            _iTokenContainer = new TokenContainer();
            _restApiService = new SurgicalConciergeRestApiService();
            surgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            _surgicalConciergePatientPage = surgicalConciergePatientPage;
        }
        public SurgicalConciergePatientViewPageViewModel(ProfessionalPatientPage professionalPatientPage)
        {
            _iTokenContainer = new TokenContainer();
            _restApiService = new SurgicalConciergeRestApiService();
            surgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            _professionalPatientPage = professionalPatientPage;
        }


        //SurgicalConciergePatientView
        public void ReDownloadSurgicalConciergePatientViews()
        {
            try
            {
                TotalRecord = GetSurgicalConciergePatientViewWithProfessionalCount();
                SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        int page = SurgicalConciergePatientViewModeslInfiniteScroll.Count / PageSize;
                        IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientViewListAsync(page + 1);
                        IsBusy = false;
                        return items;
                    },
                    OnCanLoadMore = () => SurgicalConciergePatientViewModeslInfiniteScroll.Count < TotalRecord
                };
                DownloadSurgicalConciergePatientViewsAsync();
            }
            catch (Exception)
            {
                throw;
            }
            //TotalPageCount = 0;           
        }
        private async void DownloadSurgicalConciergePatientViewsAsync()
        {
            try
            {
                IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientViewListAsync(1);
                SurgicalConciergePatientViewModeslInfiniteScroll.AddRange(items);
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }

        }
        private async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetSurgicalConciergePatientViewListAsync(int page)
        {
            IEnumerable<SurgicalConciergePatientViewModel> items = new List<SurgicalConciergePatientViewModel>();
            try
            {
                
                App.ShowUserDialogAsync();
                DateTime? pastDay = _surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay;
                items = await _restApiService.GetSurgicalConciergePatientProfileWithProfessionalNoCountAsync(
                    _surgicalConciergePatientView.PracticeDivisionUnitDest, page, PageSize, "", _surgicalConciergePatientView.ProfessionalName, _surgicalConciergePatientView.PatientName, "", "", "", "", _surgicalConciergePatientView.SurgeryDate, pastDay, null,

                _surgicalConciergePatientView.SelectedPracticeProfile,
                _surgicalConciergePatientView.SelectedProfessionalProfile,
                _surgicalConciergePatientView.SelectedProcedure,
                _surgicalConciergePatientView.SelectedPracticeLocation);
                int loopPanelCount = 0;
                items.ToList().ForEach(x =>
                {
                    x.SelectedSurgicalConciergePatientViewModel = x;
                    x.IsFirstCommandVisible = _surgicalConciergePatientView.PracticeDivisionDest != 4;
                    x.IsSecondCommandVisible = !x.IsFirstCommandVisible;

                    x.ItemBackgroundColor = GetItemBackgroundColor(loopPanelCount);

                    loopPanelCount++;
                    if (loopPanelCount == 4)
                    {
                        loopPanelCount = 0;
                    }
                });
                if (items != null)
                {
                    TotalPageCount += items.Count();
                }
                App.HideUserDialogAsync();
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }
            return items;
        }       
        private int GetSurgicalConciergePatientViewWithProfessionalCount()
        {
            App.ShowUserDialog();
            DateTime? pastDay = _surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay;
            return AsyncHelper.RunSync<int>(() => _restApiService.GetSurgicalConciergePatientProfileWithProfessionalCountAsync(
                _surgicalConciergePatientView.PracticeDivisionUnitDest,
                1,
                PageSize,
                "",
                _surgicalConciergePatientView.ProfessionalName,
                _surgicalConciergePatientView.PatientName,
                "",
                "",
                "",
                "",
                _surgicalConciergePatientView.SurgeryDate,
                pastDay,
                null,

                _surgicalConciergePatientView.SelectedPracticeProfile,
                _surgicalConciergePatientView.SelectedProfessionalProfile,
                _surgicalConciergePatientView.SelectedProcedure,
                _surgicalConciergePatientView.SelectedPracticeLocation
                ));
        }

        //SurgicalConciergePatientPage
        public void ReDownloadSurgicalConciergePatientPage()
        {
            try
            {
                TotalRecord = GetSurgicalConciergePatientPageWithProfessionalCount();
                SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        int page = SurgicalConciergePatientViewModeslInfiniteScroll.Count / PageSize;
                        IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientPageListAsync(page + 1);
                        IsBusy = false;
                        return items;
                    },
                    OnCanLoadMore = () => SurgicalConciergePatientViewModeslInfiniteScroll.Count < TotalRecord
                };
                DownloadSurgicalConciergePatientPageAsync();
            }
            catch (Exception)
            {
                throw;
            }
            //TotalPageCount = 0;           
        }
        private async void DownloadSurgicalConciergePatientPageAsync()
        {
            try
            {
                IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientPageListAsync(1);
                SurgicalConciergePatientViewModeslInfiniteScroll.AddRange(items);
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }

        }
        private async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetSurgicalConciergePatientPageListAsync(int page)
        {
            IEnumerable<SurgicalConciergePatientViewModel> items = new List<SurgicalConciergePatientViewModel>();
            try
            {
                App.ShowUserDialogAsync();
                DateTime? pastDay = _surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay;
                items = await _restApiService.GetSurgicalConciergePatientProfileWithProfessionalNoCountAsync(
                    _surgicalConciergePatientView.PracticeDivisionUnitDest, page, PageSize, "", _surgicalConciergePatientView.ProfessionalName, _surgicalConciergePatientView.PatientName, "", "", "", "", _surgicalConciergePatientView.SurgeryDate, pastDay, null,

                _surgicalConciergePatientView.SelectedPracticeProfile,
                _surgicalConciergePatientView.SelectedProfessionalProfile,
                _surgicalConciergePatientView.SelectedProcedure,
                _surgicalConciergePatientView.SelectedPracticeLocation);
                int loopPanelCount = 0;
                items.ToList().ForEach(x =>
                {
                    x.SelectedSurgicalConciergePatientViewModel = x;
                    x.IsFirstCommandVisible = _surgicalConciergePatientView.PracticeDivisionDest != 4;
                    x.IsSecondCommandVisible = !x.IsFirstCommandVisible;

                    x.ItemBackgroundColor = GetItemBackgroundColor(loopPanelCount);

                    loopPanelCount++;
                    if (loopPanelCount == 4)
                    {
                        loopPanelCount = 0;
                    }
                });
                if (items != null)
                {
                    TotalPageCount += items.Count();
                }
                App.HideUserDialogAsync();
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }
            return items;
        }
        private int GetSurgicalConciergePatientPageWithProfessionalCount()
        {
            App.ShowUserDialog();
            DateTime? pastDay = _surgicalConciergePatientView.PastDay == null ? null : _surgicalConciergePatientView.PastDay;
            return AsyncHelper.RunSync<int>(() => _restApiService.GetSurgicalConciergePatientProfileWithProfessionalCountAsync(
                _surgicalConciergePatientView.PracticeDivisionUnitDest,
                1,
                PageSize,
                "",
                _surgicalConciergePatientView.ProfessionalName,
                _surgicalConciergePatientView.PatientName,
                "",
                "",
                "",
                "",
                _surgicalConciergePatientView.SurgeryDate,
                pastDay,
                null,

                _surgicalConciergePatientView.SelectedPracticeProfile,
                _surgicalConciergePatientView.SelectedProfessionalProfile,
                _surgicalConciergePatientView.SelectedProcedure,
                _surgicalConciergePatientView.SelectedPracticeLocation
                ));
        }

        //ProfessionalPatientPage
        public void ReDownloadProfessionalPatientPage()
        {
            try
            {
                TotalRecord = GetProfessionalPatientPageWithProfessionalCount();
                SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        int page = SurgicalConciergePatientViewModeslInfiniteScroll.Count / PageSize;
                        IEnumerable<SurgicalConciergePatientViewModel> items = await GetProfessionalPatientPageListAsync(page + 1);
                        IsBusy = false;
                        return items;
                    },
                    OnCanLoadMore = () => SurgicalConciergePatientViewModeslInfiniteScroll.Count < TotalRecord
                };
                DownloadProfessionalPatientPageAsync();
            }
            catch (Exception)
            {
                throw;
            }
            //TotalPageCount = 0;           
        }
        private async void DownloadProfessionalPatientPageAsync()
        {
            try
            {
                IEnumerable<SurgicalConciergePatientViewModel> items = await GetProfessionalPatientPageListAsync(1);
                SurgicalConciergePatientViewModeslInfiniteScroll.AddRange(items);
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }

        }
        private async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetProfessionalPatientPageListAsync(int page)
        {
            IEnumerable<SurgicalConciergePatientViewModel> items = new List<SurgicalConciergePatientViewModel>();
            try
            {
                App.ShowUserDialogAsync();
                DateTime? pastDay = _professionalPatientPage.PastDay == null ? null : _professionalPatientPage.PastDay;
                items = await _restApiService.GetPatientProfileWithProfessionalNoCountAsync(
                    _professionalPatientPage.PracticeDivisionUnitDest, page, PageSize, "", _professionalPatientPage.ProfessionalName, _professionalPatientPage.PatientName, "", "", "", "", _professionalPatientPage.SurgeryDate, pastDay, null,

                _professionalPatientPage.SelectedPracticeProfile,
                _professionalPatientPage.SelectedProfessionalProfile,
                _professionalPatientPage.SelectedProcedure,
                _professionalPatientPage.SelectedPracticeLocation);
                int loopPanelCount = 0;
                items.ToList().ForEach(x =>
                {
                    x.SelectedSurgicalConciergePatientViewModel = x;
                    x.IsFirstCommandVisible = _professionalPatientPage.PracticeDivisionDest != 4;
                    x.IsSecondCommandVisible = !x.IsFirstCommandVisible;

                    x.ItemBackgroundColor = GetItemBackgroundColor(loopPanelCount);

                    loopPanelCount++;
                    if (loopPanelCount == 4)
                    {
                        loopPanelCount = 0;
                    }
                });
                if (items != null)
                {
                    TotalPageCount += items.Count();
                }
                App.HideUserDialogAsync();
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
            }
            return items;
        }
        private int GetProfessionalPatientPageWithProfessionalCount()
        {
            App.ShowUserDialog();
            DateTime? pastDay = _professionalPatientPage.PastDay == null ? null : _professionalPatientPage.PastDay;
            return AsyncHelper.RunSync<int>(() => _restApiService.GetPatientProfileWithProfessionalCountAsync(
                _professionalPatientPage.PracticeDivisionUnitDest,
                1,
                PageSize,
                "",
                _professionalPatientPage.ProfessionalName,
                _professionalPatientPage.PatientName,
                "",
                "",
                "",
                "",
                _professionalPatientPage.SurgeryDate,
                pastDay,
                null,

                _professionalPatientPage.SelectedPracticeProfile,
                _professionalPatientPage.SelectedProfessionalProfile,
                _professionalPatientPage.SelectedProcedure,
                _professionalPatientPage.SelectedPracticeLocation
                ));
        }

        private string GetItemBackgroundColor(int count)
        {
            string bgColor = "#00a2ff";

            if (count == Enums.PatientPageItemBackgroundColorEnum.BackgroundColorOne.ToInt())
            {
                bgColor = Enums.PatientPageItemBackgroundColorEnum.BackgroundColorOne.ToDescriptionAttr();
            }
            else if (count == Enums.PatientPageItemBackgroundColorEnum.BackgroundColorTwo.ToInt())
            {
                bgColor = Enums.PatientPageItemBackgroundColorEnum.BackgroundColorTwo.ToDescriptionAttr();
            }
            else if (count == Enums.PatientPageItemBackgroundColorEnum.BackgroundColorThree.ToInt())
            {
                bgColor = Enums.PatientPageItemBackgroundColorEnum.BackgroundColorThree.ToDescriptionAttr();
            }
            else if (count == Enums.PatientPageItemBackgroundColorEnum.BackgroundColorFour.ToInt())
            {
                bgColor = Enums.PatientPageItemBackgroundColorEnum.BackgroundColorFour.ToDescriptionAttr();
            }
            else if (count == Enums.PatientPageItemBackgroundColorEnum.BackgroundColorFive.ToInt())
            {
                bgColor = Enums.PatientPageItemBackgroundColorEnum.BackgroundColorFive.ToDescriptionAttr();
            }

            return bgColor;
        }
    }
}
