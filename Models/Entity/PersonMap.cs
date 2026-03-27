using SysNet.Converters;

namespace Models.Entity
{
    public class PersonMap : BaseMap, IMap
    {
        public string Cognome { get; set; } = string.Empty;
        public int Natoil { get; set; }
        //public DateTime? NatoilDate { get; set; }
        public int CodiceSocio { get; set; }
        public string NumeroSocio { get; set; } = string.Empty;
        public int CodiceTessera { get; set; }
        public string NumeroTessera { get; set; } = string.Empty;
        public int Scadenza { get; set; }
        //public DateTime? ScadenzaDate { get; set; } = DateTime.Now;
        
        public string CodiceUnivoco { get; set; } = string.Empty;

        // OVERRIDE o ASSEGNAZIONE del Titolo ereditato
        // Viene calcolato ogni volta che serve (ReadOnly)
        public new string? Titolo => $"{Nome} {Cognome} ({NatoilDate.ToShortDateString()})";

        public DateTime NatoilDate => Natoil.DateIntToDate();
        public DateTime ScadenzaDate => Scadenza.DateIntToDate();

        public bool IsMaggiorenne => Natoil.IsLegalAge();

    }

    
}
