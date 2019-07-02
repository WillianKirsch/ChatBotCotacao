using System;
using System.Runtime.Serialization;

namespace CoreBotCotacao.Models
{
    public class LUISResponse
    {
        public Intencao Intencao { get; set; }
        public string Entidade { get; set; }
    }
}
