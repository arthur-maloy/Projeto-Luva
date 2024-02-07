using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvaApp.Interfaces
{
    internal interface IBluetoothController
    {
        public IDevice? ConnectedDevice { get; set; }
        public Task<IEnumerable<IDevice>> AsyncGetDevices();
        public Task AsyncConnectToDeviceByName(string deviceName);
        public void DisconnectFromDevice();
        public Task AsyncRequestBluetoothPermissions();
    }
}
