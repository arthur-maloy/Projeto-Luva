using Microsoft.Maui.Controls.PlatformConfiguration;
using Plugin.BLE;
using LuvaApp.AndroidPermissions;
using Plugin.BLE.Abstractions.Contracts;
using LuvaApp.BluetoothHelper;
using System.Text;

namespace LuvaApp
{
    public partial class MainPage : ContentPage
    {
        BluetoothController bluetoothController = new BluetoothController();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ConnectToBluetoothDevice(object sender, EventArgs e)
        {
            await bluetoothController.AsyncRequestBluetoothPermissions();
            await bluetoothController.AsyncConnectToDeviceByName("LuvaController");

            CharacteristicBluetoothBtn.IsEnabled = true;
        }

        private async void GetCharacteristicValue(object sender, EventArgs e)
        {
            IReadOnlyList<IService> servicesReadOnlyList = await bluetoothController.ConnectedDevice!.GetServicesAsync();
            List<string> serviceNameList = servicesReadOnlyList.Select(b => b.Name).ToList();

            IService luvaService = await bluetoothController.ConnectedDevice.GetServiceAsync(Guid.Parse("00000015-0000-1000-8000-00805F9B34FB"));
            ICharacteristic luvaCharacteristic = (await luvaService.GetCharacteristicsAsync())[0];

            byte[] characteristicValueBytes = (await luvaCharacteristic.ReadAsync()).ToTuple().Item1;
            var characteristicValue = Encoding.UTF8.GetString(characteristicValueBytes);
            var characteristicValue2 = BitConverter.ToString(characteristicValueBytes);            
        }
    }

}
