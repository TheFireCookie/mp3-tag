using System.Windows;
using Caliburn.Micro;
using mp3tag.ViewModels;

namespace mp3tag
{
    public class Mp3TagBootstrapper : BootstrapperBase
    {
        public Mp3TagBootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<MainWindowViewModel>();
        }
    }
}