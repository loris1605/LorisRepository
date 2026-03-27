using Models.Interfaces;
using Models.Tables;

namespace Models.Entity
{
    public class PostazioneMap : BaseMap, IMap, IMappable<Postazione>
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

        public override string Nome => NomePostazione;

        public new string? Titolo => $"{NomePostazione} - {NomeTipoPostazione}";

        public Postazione ToTable() => Mappers.PostazioneMapper.ToTable(this);

        public void UpdateTable(Postazione existing)
        {
            // Aggiorniamo solo i campi che possono cambiare
            existing.Nome = this.NomePostazione;
            existing.TipoPostazioneId = this.CodiceTipoPostazione;
            existing.TipoRientroId = this.CodiceTipoRientro;
            
            // Non tocchiamo l'ID!
        }

    }
}
