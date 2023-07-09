using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
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
    public class NursePatientInfoPatientViewPageViewModel : OntrackHealthApp.ViewModel.BaseViewModel
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly SurgicalConciergeRestApiService _restApiService;
        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }
        private const int PageSize = (int)Enums.SearchCrieteria.DefaultPageSize;
        private int TotalRecord = 0;
        private int TotalPageCount = 0;
        public NursePatientInfoPatientViewPageNew _nursePatientInfoPatientViewPageNew;
        public Command LoadPatientProfilesCommand { get; set; }
        public ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergePatientViewModelList;
        public InfiniteScrollCollection<SurgicalConciergePatientViewModel> SurgicalConciergePatientViewModeslInfiniteScroll { get; set; }
        public NursePatientInfoPatientViewPageViewModel(NursePatientInfoPatientViewPageNew nursePatientInfoPatientViewPageNew) {
            _iTokenContainer = new TokenContainer();
            _restApiService = new SurgicalConciergeRestApiService();
            surgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>();
            _nursePatientInfoPatientViewPageNew = nursePatientInfoPatientViewPageNew;
        }
        public void ReDownloadSurgicalConciergePatientViews()
        {
            SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>();
            TotalRecord = GetSurgicalConciergePatientProfileWithProfessionalCount();
            //TotalPageCount = 0;
            SurgicalConciergePatientViewModeslInfiniteScroll = new InfiniteScrollCollection<SurgicalConciergePatientViewModel>
            {
                OnLoadMore = async () =>
                {
                    int page = TotalPageCount / PageSize; //SurgicalConciergePatientViewModeslInfiniteScroll.Count / PageSize;
                    IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientViewsAsync(page + 1);
                    return items;
                },
                OnCanLoadMore = () => TotalPageCount < TotalRecord
            };
            DownloadSurgicalConciergePatientViewsAsync();
        }
        private async void DownloadSurgicalConciergePatientViewsAsync()
        {
            IEnumerable<SurgicalConciergePatientViewModel> items = await GetSurgicalConciergePatientViewsAsync(1);
            SurgicalConciergePatientViewModeslInfiniteScroll.AddRange(items);           
        }
        private async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetSurgicalConciergePatientViewsAsync(int page)
        {
            App.ShowUserDialog();

            DateTime? pastDay = _nursePatientInfoPatientViewPageNew.PastDay == null ? null : _nursePatientInfoPatientViewPageNew.PastDay;
            IEnumerable<SurgicalConciergePatientViewModel> items = await _restApiService.GetSurgicalConciergePatientProfileWithProfessionalNoCountAsync(
                _nursePatientInfoPatientViewPageNew.PracticeDivisionUnitDest, page, PageSize, "", _nursePatientInfoPatientViewPageNew.ProfessionalName, _nursePatientInfoPatientViewPageNew.PatientName, "", "", "", "", _nursePatientInfoPatientViewPageNew.SurgeryDate, pastDay, null,

                _nursePatientInfoPatientViewPageNew.SelectedPracticeProfile,
                _nursePatientInfoPatientViewPageNew.SelectedProfessionalProfile,
                _nursePatientInfoPatientViewPageNew.SelectedProcedure);
            int loopPanelCount = 0;
            items.ToList().ForEach(x =>
            {
                x.SelectedSurgicalConciergePatientViewModel = x;
                x.IsFirstCommandVisible = _nursePatientInfoPatientViewPageNew.PracticeDivisionDest != 4;
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
            App.HideUserDialog();
            return items;
        }        

        private int GetSurgicalConciergePatientProfileWithProfessionalCount()
        {
            App.ShowUserDialog();
            DateTime? pastDay = _nursePatientInfoPatientViewPageNew.PastDay == null ? null : _nursePatientInfoPatientViewPageNew.PastDay;
            return AsyncHelper.RunSync<int>(() => _restApiService.GetSurgicalConciergePatientProfileWithProfessionalCountAsync(
                _nursePatientInfoPatientViewPageNew.PracticeDivisionUnitDest,
                1,
                PageSize,
                "",
                _nursePatientInfoPatientViewPageNew.ProfessionalName,
                _nursePatientInfoPatientViewPageNew.PatientName,
                "",
                "",
                "",
                "",
                _nursePatientInfoPatientViewPageNew.SurgeryDate,
                pastDay,
                null,

                _nursePatientInfoPatientViewPageNew.SelectedPracticeProfile,
                _nursePatientInfoPatientViewPageNew.SelectedProfessionalProfile,
                _nursePatientInfoPatientViewPageNew.SelectedProcedure

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
