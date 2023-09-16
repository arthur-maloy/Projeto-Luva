using BluetoothTest.BLPermissions;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Exceptions;
using System.Text;

namespace BluetoothTest;

public partial class MainPage : ContentPage
{
	int count = 0;


	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
    {
        await Permissions.RequestAsync<BluetoothPermissions>();
        var ble = CrossBluetoothLE.Current;
        var adapter = CrossBluetoothLE.Current.Adapter;
		await adapter.StartScanningForDevicesAsync();
        var bla = adapter.DiscoveredDevices.FirstOrDefault(a => a.Name == null ? false : a.Name.Contains("Luva"));
        try
        {
            if (adapter.ConnectedDevices.Count == 0)
                await adapter.ConnectToDeviceAsync(bla);
            else
                bla = adapter.ConnectedDevices[0];
            var services = await bla.GetServicesAsync();
            foreach (var service in services)
            {
                var characteristics = await service.GetCharacteristicsAsync();

                foreach(var characteristic in characteristics)
                {
                    var algo = characteristic.Value;
                    var lago = characteristic.StringValue;
                    try
                    {
                        int i = BitConverter.ToInt32(algo); 
                        var str = Encoding.UTF8.GetString(algo);
                    }
                    catch
                    {
                        continue;
                    }
                    var descriptors = await characteristic.GetDescriptorsAsync();
                    if (descriptors.Count == 0)
                        continue;
                    var bytess = await descriptors[0].ReadAsync();
                }
                
            }
            

        }
        catch (DeviceConnectionException err)
        {
            Console.WriteLine(err.Message);
        }
        count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

