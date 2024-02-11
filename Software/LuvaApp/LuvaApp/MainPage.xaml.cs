using LuvaApp.Helpers.BluetoothHelper;
using LuvaApp.Helpers;

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
            TraduzSinalBtn.IsEnabled = true;
        }

        private async void GetCharacteristicValue(object sender, EventArgs e)
        {
            await Task.Run(async() => await RecepcaoController.Instancia.IniciaRecepcao(bluetoothController));        
        }

        private void TraduzSinalBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}
