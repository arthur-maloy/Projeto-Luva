using static Microsoft.Maui.ApplicationModel.Permissions;

namespace LuvaApp.Helpers.AndroidPermissions
{
    internal class BluetoothPermissions : BasePlatformPermission
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
            ("android.permission.BLUETOOTH", true),
            ("android.permission.BLUETOOTH_ADMIN", true),
            ("android.permission.BLUETOOTH_SCAN", true),
            ("android.permission.BLUETOOTH_CONNECT", true),
            ("android.permission.ACCESS_COARSE_LOCATION", true),
            ("android.permission.ACCESS_FINE_LOCATION", true)
            }.ToArray();
#endif

    }
}
