using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BaseProject
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static BaseViewModel Current;

        private string _scannedText;
        public string ScannedText
        {
            get { return _scannedText; }
            set
            {
                _scannedText = value;
                OnPropertyChanged("ScannedText");
            }
        }

        public BaseViewModel()
        {

        }

        public void cleanScannedText()
        {
            ScannedText = "";
        }
    }
}
