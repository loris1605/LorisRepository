namespace Models.Entity
{
    public class TipoRientroMap : BaseMap, IMap
    {
        public string Nome { get; set; } = string.Empty;
        public int Hours { get; set; }
       
    }
}
