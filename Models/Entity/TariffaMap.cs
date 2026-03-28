namespace Models.Entity
{
    public class TariffaMap : BaseMap, IMap
    {
        public string NomeTariffa { get; set; } = string.Empty;
        public string EtichettaTariffa { get; set; } = string.Empty;
        public decimal PrezzoTariffa { get; set; } = decimal.Zero;
        public bool IsFreeDrink { get; set; }

        // Sincronizzazione Getter/Setter per l'interfaccia IMap
        public override string Nome
        {
            get => NomeTariffa;
            set => NomeTariffa = value ?? string.Empty;
        }

        // Titolo descrittivo per ComboBox o liste: Nome + Prezzo
        public override string? Titolo => $"{NomeTariffa} - {PrezzoTariffa:C2} {(IsFreeDrink ? "(Free Drink)" : "")}";
    }

}
