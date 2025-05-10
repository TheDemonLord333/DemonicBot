using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemonicBot.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemonicBot.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IApiService ApiService;
        protected readonly ISettingsService SettingsService;
        protected readonly INavigationService NavigationService;

        private bool _isBusy;
        private string _title;
        private string _errorMessage;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public BaseViewModel()
        {
            ApiService = DependencyService.Get<IApiService>();
            SettingsService = DependencyService.Get<ISettingsService>();
            NavigationService = DependencyService.Get<INavigationService>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
