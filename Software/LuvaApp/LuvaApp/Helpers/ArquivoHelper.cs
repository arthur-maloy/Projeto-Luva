using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvaApp.Helpers
{
    public static class ArquivoHelper
    {
        /// <summary>
        /// Se o arquivo modelo não existir, ele é copiado do pacote para o diretório de dados do aplicativo.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> TrataArquivosModelo()
        {
            var caminho = Path.Combine(FileSystem.AppDataDirectory, Constantes.ONNX_MODEL_PATH);

            if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, Constantes.ONNX_MODEL_PATH)))
            {
                using (var fileStream = await FileSystem.Current.OpenAppPackageFileAsync(Constantes.ONNX_MODEL_PATH))
                {
                    using (var ms = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(ms);
                        byte[] bytes = ms.ToArray();

                        File.WriteAllBytes(caminho, bytes);
                    }
                }
            }

            return caminho;
        }

    }
}
