namespace Models.Entity
{
    public class RepartoMap : BaseMap, IMap
    {
        public int CodicePostazione { get; set; }
        public int CodiceSettore { get; set; }  
        public string NomeSettore { get; set; } = string.Empty;
        public string EtichettaSettore { get; set; } = string.Empty;

        public override string Nome => NomeSettore;

    }
}
