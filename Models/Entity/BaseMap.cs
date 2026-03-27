namespace Models.Entity
{
    public class BaseMap
    {
        public int Id { get; set; }
        public string Titolo { get; set; } = string.Empty;
        public virtual string? Nome { get; set; }        
    }

    public interface IMap
    {
        int Id { get; set; }
        string? Nome { get; set; }
    }
}
