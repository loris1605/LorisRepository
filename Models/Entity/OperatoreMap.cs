namespace Models.Entity
{
    public class OperatoreMap : BaseMap, IMap
    {
        public string NomeOperatore { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Abilitato { get; set; } = false;
        public int Badge { get; set; } = 0;
        public int CodicePermesso { get; set; } = 0;
        public string NomePostazione {  get; set; } = string.Empty;
        public string TipoPostazione {  get; set; } = string.Empty;

        public new string? Titolo => $"{NomeOperatore} - {(Abilitato ? "Abilitato" : "Non abilitato")}";


    }
}
