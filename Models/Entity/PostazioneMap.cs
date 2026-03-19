namespace Models.Entity
{
    public class PostazioneMap : BaseMap, IMap
    {
        public int CodiceTipoPostazione { get; set; }
        public string NomePostazione { get; set; } = string.Empty;
        public string NomeTipoPostazione { get; set; } = string.Empty;
        public int CodiceReparto { get; set; }
        public string NomeSettore { get; set; } = string.Empty;
        public string EtichettaSettore { get; set; } = string.Empty;
        public string NomeTipoSettore { get; set; } = string.Empty;
        public int CodiceTipoRientro { get; set; }
        public string NomeTipoRientro { get; set; } = string.Empty;



    }
}
