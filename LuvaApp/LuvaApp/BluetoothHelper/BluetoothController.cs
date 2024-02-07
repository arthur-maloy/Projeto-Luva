using LuvaApp.AndroidPermissions;
using LuvaApp.Interfaces;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvaApp.BluetoothHelper
{
    internal class BluetoothController : IBluetoothController
    {
        public IDevice? ConnectedDevice { get; set; }
        public IAdapter? Adapter { get; set; }

        public async Task<IEnumerable<IDevice>> AsyncGetDevices()
        {
            Adapter = CrossBluetoothLE.Current.Adapter;
            await Adapter.StartScanningForDevicesAsync();            
            return Adapter.DiscoveredDevices.Where(device => device.Name != null);
        }

        public async Task AsyncConnectToDeviceByName(string deviceName)
        {
            IEnumerable<IDevice> devicesFound = await AsyncGetDevices();
            ConnectedDevice = devicesFound.FirstOrDefault(device => device.Name == deviceName);
            await Adapter!.ConnectToDeviceAsync(ConnectedDevice);
        }

        public void DisconnectFromDevice()
        {
            ConnectedDevice!.Dispose();
            ConnectedDevice = null;
        }

        public async Task AsyncRequestBluetoothPermissions()
        {
            await Permissions.RequestAsync<BluetoothPermissions>();
        }        
    }
}
