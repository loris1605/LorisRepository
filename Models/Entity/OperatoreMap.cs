using Models.Interfaces;
using Models.Tables;

namespace Models.Entity
{
    public class OperatoreMap : BaseMap, IMap, IMappable<Operatore>
    {
        public string NomeOperatore { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Abilitato { get; set; } = false;
        public int Badge { get; set; } = 0;
        public int CodicePermesso { get; set; } = 0;
        public string NomePostazione {  get; set; } = string.Empty;
        public string TipoPostazione {  get; set; } = string.Empty;
        public int CodicePerson { get; set; } = 0;

        public new string? Titolo => $"{NomeOperatore} - {(Abilitato ? "Abilitato" : "Non abilitato")}";

        public override string Nome => NomeOperatore;

        public Operatore ToTable()
        {
            return Mappers.OperatoreMapper.ToTable(this);
        }

        public void UpdateTable(Operatore existing)
        {
            // Aggiorniamo solo i campi che possono cambiare
            existing.Nome = this.NomeOperatore;
            existing.Password = this.Password;
            existing.Abilitato = this.Abilitato;
            existing.Pass = this.Badge;
            // Non tocchiamo l'ID!
        }
    }
}
