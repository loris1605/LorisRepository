namespace Models.Tables
{
    public class Permesso
    {
        public int Id { get; set; }
        public int OperatoreId { get; set; }
        public int PostazioneId { get; set; }

        public Operatore? Operatore { get; set; }
        public Postazione? Postazione { get; set; }
    }
}
