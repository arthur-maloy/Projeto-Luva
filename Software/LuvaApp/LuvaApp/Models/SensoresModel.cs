
namespace LuvaApp.Models
{
    public class SensoresModel
    {
        public float Flexao1 { get; set; }
        public float Flexao2 { get; set; }
        public float Flexao3 { get; set; }
        public float Flexao4 { get; set; }
        public float Flexao5 { get; set; }
        public float Acc_EixoX { get; set; }
        public float Acc_EixoY { get; set; }

        public SensoresModel()
        {
            
        }

        public SensoresModel(string valorRecebido)
        {
            var aux = valorRecebido.Split(';');

            Flexao1 = float.Parse(aux[0]);
            Flexao2 = float.Parse(aux[1]);
            Flexao3 = float.Parse(aux[2]);
            Flexao4 = float.Parse(aux[3]);
            Flexao5 = float.Parse(aux[4]);
            Acc_EixoX = float.Parse(aux[5]);
            Acc_EixoY = float.Parse(aux[6]);
        }
    }
}
