﻿using LuvaApp.Helpers.BluetoothHelper;
using LuvaApp.Models;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Text;

namespace LuvaApp.Helpers
{
    public class RecepcaoController
    {
        private static List<string> _listaRecepcao;

        #region SINGLETON
        private static RecepcaoController _instancia;

        // Private constructor to prevent instance creation
        private RecepcaoController() { }

        public static RecepcaoController Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new RecepcaoController();
                }

                if (_listaRecepcao == null)
                {
                    _listaRecepcao = new List<string>();
                }

                return _instancia;
            }
        }
        #endregion

        public async Task IniciaRecepcao(BluetoothController bluetoothController)
        {
            IService luvaService = await bluetoothController.ConnectedDevice.GetServiceAsync(Guid.Parse("00000015-0000-1000-8000-00805F9B34FB"));
            ICharacteristic luvaCharacteristic = (await luvaService.GetCharacteristicsAsync())[0];

            luvaCharacteristic.ValueUpdated -= ValueUpdatedHandler;
            luvaCharacteristic.ValueUpdated += ValueUpdatedHandler;

            await luvaCharacteristic.StartUpdatesAsync();
        }

        public SensoresModel ObtemUltimoValorRecebido()
        {
            sendDataThroughAPI("12,12,12,12,12");

            var penultimoValor = _listaRecepcao[_listaRecepcao.Count - 2];
            var retorno = new SensoresModel(penultimoValor);

            return retorno;
        }

        private void ValueUpdatedHandler(object? sender, CharacteristicUpdatedEventArgs e)
        {
            var bytes = e.Characteristic.Value;
            var valor = Encoding.UTF8.GetString(bytes);

            AdicionaValorALista(valor);
        }

        private void AdicionaValorALista(string valor)
        {
            if (valor.First() == '0')
            {
                valor = valor.Substring(1); //Removemos o primeiro caractere, referente ao index do pacote.
                _listaRecepcao.Add(valor);
            }
            else
            {
                valor = valor.Substring(1); //Removemos o primeiro caractere, referente ao index do pacote.
                var ultimoValorAdicionado = _listaRecepcao.Last();
                var indexUltimo = _listaRecepcao.IndexOf(ultimoValorAdicionado);

                ultimoValorAdicionado += valor;
                _listaRecepcao[indexUltimo] = ultimoValorAdicionado;
            }

            RemoveValorDaLista();
        }

        public static async void sendDataThroughAPI(string dados)
        {
            string url = "http://127.0.0.1:5000/receiveValues";
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(dados);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }
        }

        private void RemoveValorDaLista()
        {
            if (_listaRecepcao.Count > 10)
            {
                _listaRecepcao.RemoveAt(0);
            }
        }
    }
}
