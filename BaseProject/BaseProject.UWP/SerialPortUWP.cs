using BaseProject.Interfaces;
using BaseProject.UWP;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Xamarin.Forms;

[assembly: Dependency(typeof(SerialPortUWP))]
namespace BaseProject.UWP
{
    public class SerialPortUWP : ISerialPort
    {
        /// The serial device used to interface to the COM port
        private SerialDevice serialDevice { get; set; }

        /// The data reader used to read data from the COM port
        private DataReader dataReaderObject;

        /// The data writer used to send data to the COM port
        private DataWriter dataWriterObject;

        private SerialPort serialPort { get; set; }

        public static string ScannedText {get; set;}

        public SerialPortUWP()
        {
            serialPort = new SerialPort();

            ScannedText = string.Empty;

            IsOpen = false;
        }

        public async Task<bool> Open(string portName, uint baudRate = 9600,
            uint parity_ = 0, ushort dataBits = 8,
            uint stopBits_ = 0)
        {
            serialPort = new SerialPort();

            Close();

            //DependencyService cast arguments workaround (Works)
            Parity parity = (Parity)parity_;
            StopBits stopBits = (StopBits)stopBits_;

            serialPort.PortName = portName;
            serialPort.BaudRate = (int)baudRate;
            serialPort.Parity = parity;
            serialPort.DataBits = (int)dataBits;
            serialPort.StopBits = stopBits;

            // Set the read/write timeouts
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            try
            {
                serialPort.Open();
                this.IsOpen = true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }

        private void DataReceivedHandler(object sender, EventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            ScannedText = sp.ReadExisting();

            //Send the value to the ViewModel
            ValueChanged(ScannedText);
        }

        private void ValueChanged(string value)
        {
            BaseViewModel.Current.ScannedText = value;
        }

        /// Flag that indicates if COM port is open
        public bool IsOpen { get; private set; }

        /// Close the open port connection to the current device
        public void Close()
        {
            // If serial device defined...
            if (serialPort != null)
            {
                // Dispose and clear device
                serialPort.Dispose();
            }

            // If data reader defined...
            if (this.dataReaderObject != null)
            {
                // Detatch reader stream
                this.dataReaderObject.DetachStream();

                // Dispose and clear data reader
                this.dataReaderObject.Dispose();
                this.dataReaderObject = null;
            }

            // If data writer defined...
            if (this.dataWriterObject != null)
            {
                // Detatch writer stream
                this.dataWriterObject.DetachStream();

                // Dispose and clear data writer
                this.dataWriterObject.Dispose();
                this.dataWriterObject = null;
            }
            // Port now closed
            IsOpen = false;
        }

        public async Task<string> GetScannerPortByVidPid(ushort vid, ushort pid)
        {
            string PortName = "";

            string selector = SerialDevice.GetDeviceSelectorFromUsbVidPid(vid, pid);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);

            if (devices.Any())
            {
                // Get first device (should be only device)
                DeviceInformation deviceInfo = devices.First();

                // Create a serial port device from the COM port device ID
                serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);

                PortName = serialDevice.PortName;

                serialDevice.Dispose();
            }
            return PortName;
        }

    }
}
