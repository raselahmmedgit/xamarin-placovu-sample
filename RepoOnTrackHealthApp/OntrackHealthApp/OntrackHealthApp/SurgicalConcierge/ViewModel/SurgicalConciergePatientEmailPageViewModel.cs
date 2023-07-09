using System;
using System.Collections.Generic;
using System.Text;

using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{  

    public class SurgicalConciergePatientEmailPageViewModel : OntrackHealthApp.ViewModel.BaseViewModel
    {
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
        private long PracticeDivisionUnitDest { get; set; }  = 0;

        public SurgicalConciergePatientEmailPageViewModel()
        {
            _iTokenContainer = new TokenContainer();
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }
            //PatientFullName = "Patient : " + _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName = "Procedure : " + _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName = "Professional : " + _surgicalConciergePatientViewModel.ProfessionalName;
        } 
    }
}
