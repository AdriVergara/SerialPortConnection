using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Interfaces
{
    public interface ISerialPort
    {
        Task<bool> Open(string portName, uint baudRate = 9600,
            /*SerialParity*/ uint parity = 0, ushort dataBits = 8,
            /*SerialStopBitCount*/ uint stopBits = 0);

        void Close();

        Task<string> GetScannerPortByVidPid(ushort vid, ushort pid);
    }
}
