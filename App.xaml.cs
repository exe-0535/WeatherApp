using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WeatherApp.ViewModel;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            WeatherViewModel viewModel = new WeatherViewModel();
            MainWindow.DataContext = viewModel;

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
