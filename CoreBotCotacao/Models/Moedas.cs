using System;
using System.Runtime.Serialization;

namespace CoreBotCotacao.Models
{
    [DataContract]
    public class Moeda
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string codein { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string varBid { get; set; }
        [DataMember]
        public string pctChange { get; set; }
        [DataMember]
        public string bid { get; set; }
        [DataMember]
        public string ask { get; set; }
        [DataMember]
        public string timestamp { get; set; }
        [DataMember]
        public string create_date { get; set; }
    }
}
