namespace LuvaApp.Views;

public partial class ConfigPage : ContentPage
{
	public ConfigPage()
	{
		InitializeComponent();
	}

    private void btnConfigBluetooth_Clicked(object sender, EventArgs e)
    {
        
    }

    private void btnSalvar_Clicked(object sender, EventArgs e)
    {
        //TODO: Salvar configurações

        DisplayAlert(Constantes.SUCESSO, Constantes.MSG_CONFIG_SALVA, Constantes.OK);

        Navigation.PopAsync();
    }
}