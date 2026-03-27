using Models.Interfaces;
using Models.Tables;

namespace Models.Entity
{
    public class SettoreMap : BaseMap, IMap, IMappable<Settore>
    {
        public string NomeSettore { get; set; } = string.Empty;
        public string EtichettaSettore { get; set; } = string.Empty;
        public int CodiceTipoSettore { get; set; } = 0;
        public string NomeTipoSettore { get; set; } = string.Empty;
        public int CodiceListino { get; set; } = 0;
        public string NomeTariffa { get; set; } = string.Empty;
        public string EtichettaTariffa { get; set; } = string.Empty;
        public decimal PrezzoTariffa { get; set; } = decimal.Zero;

        public override string Nome => NomeSettore;

        public new string? Titolo => $"{NomeSettore} - {NomeTipoSettore}";

        public Settore ToTable() => Mappers.SettoreMapper.ToTable(this);

        public void UpdateTable(Settore existing)
        {
            // Aggiorniamo solo i campi che possono cambiare
            existing.Nome = this.NomeSettore;
            existing.Label = this.EtichettaTariffa;
            existing.TipoSettoreId = this.CodiceTipoSettore;

            // Non tocchiamo l'ID!
        }

    }
}
