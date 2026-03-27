namespace Models.Entity
{
    public class TariffaMap : BaseMap, IMap
    {
        public string NomeTariffa { get; set; } = string.Empty;
        public string EtichettaTariffa { get; set; } = string.Empty;
        public decimal PrezzoTariffa { get; set; } = decimal.Zero;
        public bool IsFreeDrink { get; set; }

        public override string Nome => NomeTariffa;

    }
}
