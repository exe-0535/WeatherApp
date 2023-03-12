using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;

namespace WeatherApp.ViewModel
{
    using Model;
    using System.Windows.Input;

    public class WeatherViewModel : INotifyPropertyChanged
    {
        private WeatherModel _model;
        public WeatherModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public ICommand SearchCommand { get; }

        public WeatherViewModel()
        {
            SearchCommand = new RelayCommand(GetWeather);
        }

        string APIKey = "ca1da7eb5478efa3e7f585b76fc698cd";
        private async void GetWeather()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=pl", City, APIKey);
                var json = web.DownloadString(url);
                WeatherModel Info = JsonConvert.DeserializeObject<WeatherModel>(json);

                Model = Info;
                
            }
        }










        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action execute) : this(execute, null)
            {
            }

            public RelayCommand(Action execute, Func<bool> canExecute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute();
            }

            public void Execute(object parameter)
            {
                _execute();
            }
        }
    }
}
