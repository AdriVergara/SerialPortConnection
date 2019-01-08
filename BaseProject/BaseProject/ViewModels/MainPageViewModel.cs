using BaseProject.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseProject.Views;

namespace BaseProject.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        //public ICommand NextPageCommand { get; set; }

        public INavigation _navigationService { get; set; }

        //vid and pid depends on the used device
        public ushort vid = 0x05E0;
        public ushort pid = 0x1701;

        public MainPageViewModel(INavigation nav)
        {
            _navigationService = nav;

            Current = this;
            asyncFunc();

            //NextPageCommand = new Command(async () => await ExecuteNextPageCommand());
        }

        //public async Task ExecuteNextPageCommand()
        //{
        //    await _navigationService.PushAsync(new SecondPageView());
        //}

        private async void asyncFunc()
        {
            string BarcodePortName = await DependencyService.Get<ISerialPort>().GetScannerPortByVidPid(vid, pid);
            bool IsOpen = await DependencyService.Get<ISerialPort>().Open(BarcodePortName, stopBits: 1);
        }
    }
}