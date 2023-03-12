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
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using static System.Net.Mime.MediaTypeNames;
    using static System.Net.WebRequestMethods;

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

        private string _imageSourceUrl;
        public string ImageSourceUrl
        {
            get => _imageSourceUrl;
            set
            {
                _imageSourceUrl = value;
                OnPropertyChanged(nameof(ImageSourceUrl));
            }
        }

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
                
                
                Info.main.temp = Info.main.temp.ToString() + " °C";
                Info.main.feels_like = Info.main.feels_like.ToString() + " °C";
                Info.main.temp_min = Info.main.temp_min.ToString() + " °C";
                Info.main.temp_max = Info.main.temp_max.ToString() + " °C";
                Info.main.pressure = Info.main.pressure.ToString() + " hPa";
                Info.main.humidity = Info.main.humidity.ToString() + " %";
                Info.sys.sunset = convertDateTime(Convert.ToInt64(Info.sys.sunset)).ToShortTimeString();
                Info.sys.sunrise = convertDateTime(Convert.ToInt64(Info.sys.sunrise)).ToShortTimeString();
                Info.wind.speed = Info.wind.speed.ToString() + " m/s";
                Info.wind.deg = convertDegToText(Convert.ToInt32(Info.wind.deg));
                Info.weather[0].description = FirstCharToUpper(Info.weather[0].description);

                ImageSourceUrl = string.Format("https://openweathermap.org/img/w/{0}.png", Info.weather[0].icon);

                Model = Info;
                
            }
        }

        DateTime convertDateTime(long milisec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(milisec).ToLocalTime();
            return day;
        }

        string convertDegToText(int deg)
        {
            if (deg >= 330 || deg <= 30) return "Północny";
            if (deg > 30 && deg < 60) return "Północno-wschodni";
            if (deg >= 60 && deg <= 120) return "Wschodni";
            if (deg > 120 && deg < 150) return "Południowo-wschodni";
            if (deg >= 150 && deg <= 210) return "Południowy";
            if (deg > 210 && deg < 240) return "Południowo-zachodni";
            if (deg >= 240 && deg <= 300) return "Zachodni";
            if (deg > 300 && deg < 330) return "Północno-zachodni";

            return string.Empty;
        }

        string FirstCharToUpper(string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

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
