namespace Models.Entity
{
    public class LoginMap : BaseMap, IMap
    {
        public string NomeOperatore {  get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
    }

    
}
