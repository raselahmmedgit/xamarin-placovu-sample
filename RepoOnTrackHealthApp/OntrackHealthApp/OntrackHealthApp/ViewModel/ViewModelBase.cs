using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        string pageTitle = string.Empty;
        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }
        string patientName = string.Empty;
        public string PatientName
        {
            get { return patientName; }
            set { SetProperty(ref patientName, value); }
        }

        bool isSuccess = true;
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { SetProperty(ref isSuccess, value); }
        }

        string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "",  Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
