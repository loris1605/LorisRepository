namespace Models.Entity
{
    public class BaseMap : IMap
    {
        public int Id { get; set; }
        public string Titolo { get; set; } = string.Empty;
        
    }

    public interface IMap
    {
        int Id { get; set; }
    }
}
