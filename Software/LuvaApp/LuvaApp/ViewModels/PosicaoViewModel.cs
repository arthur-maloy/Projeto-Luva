using CommunityToolkit.Mvvm.ComponentModel;

namespace LuvaApp.ViewModels
{
    internal partial class PosicaoViewModel : ObservableObject
    {
        [ObservableProperty]
        private long posicao = new();
    }
}
