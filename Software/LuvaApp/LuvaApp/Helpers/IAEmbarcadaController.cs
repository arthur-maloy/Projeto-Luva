using LuvaApp.Models;
using Microsoft.ML;

namespace LuvaApp.Helpers
{
    public class IAEmbarcadaController
    {
        private async static Task<PredictionEngine<OnnxInput, OnnxOutput>> GetPredictionPipeline(MLContext mLContext)
        {
            var inputColumns = new string[] { "float_input" };
            var outputColumns = new string[] { "label", "probabilities" };
            var caminho = await ArquivoHelper.TrataArquivosModelo();

            var onnxPredictionPipeline = mLContext.Transforms
                                                  .ApplyOnnxModel(outputColumnNames: outputColumns,
                                                                  inputColumnNames: inputColumns,
                                                                  caminho);
            var emptyDv = mLContext.Data.LoadFromEnumerable(new OnnxInput[] { });

            var model = onnxPredictionPipeline.Fit(emptyDv);

            return mLContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>(model);
        }

        public async static Task<string> Predicao(OnnxInput entrada)
        {
            MLContext mlContext = new MLContext();

            var predEngine = await GetPredictionPipeline(mlContext);
            var result = predEngine.Predict(entrada);

            return result.ToString();
        }

    }
}
