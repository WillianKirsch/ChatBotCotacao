using CoreBotCotacao.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CoreBotCotacao.Services
{
    [Serializable]
    public class CotacaoMoeda
    {
        private async Task<dynamic> Consultar(string url)
        {
            var client = new RestClient($"https://economia.awesomeapi.com.br/{url}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var result = response.Content;

            return JsonConvert.DeserializeObject(result);
        }

        public async Task<List<Moeda>> Listagem()
        {
            var list = new List<Moeda>();

            var result = await Consultar("all/");

            foreach (var moeda in result)
            {
                var js = new DataContractJsonSerializer(typeof(Moeda));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(((Newtonsoft.Json.Linq.JContainer)moeda).First.ToString()));
                list.Add((Moeda)js.ReadObject(ms));
            }

            return list;
        }

        public async Task<List<Moeda>> Cotacao(string moeda)
        {
            var lista = new List<Moeda>();
            var siglaMoeda = string.Empty;

            if (!string.IsNullOrEmpty(moeda))
            {
                var currentScore = 0f;
                var listaMoedas = await Listagem();
                listaMoedas.ForEach(x =>
                {
                    var moedaScore = JaroWinklerDistance.GetDistance(x.name, moeda);
                    if (moedaScore > currentScore)
                    {
                        currentScore = moedaScore;
                        siglaMoeda = x.code;
                    }
                });
            }

            var url = string.IsNullOrEmpty(siglaMoeda) ? "all" : $"all/{siglaMoeda}";
            var result = await Consultar(url);

            foreach (var cotacao in result)
            {
                var js = new DataContractJsonSerializer(typeof(Moeda));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(((Newtonsoft.Json.Linq.JContainer)cotacao).First.ToString()));
                lista.Add((Moeda)js.ReadObject(ms));
            }

            return lista;
        }
    }
}
