using Microsoft.ML.Data;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.Transforms.Onnx;

namespace LuvaApp.Models
{
    public class OnnxOutput
    {
        [ColumnName("probabilities"), OnnxMapType(typeof(Float16), typeof(Single))]
        public float[] Probabilities { get; set; }

        [ColumnName("label"), OnnxMapType(typeof(Int64), typeof(Single))]
        public Int64[] Resultado { get; set; }
    }
}
