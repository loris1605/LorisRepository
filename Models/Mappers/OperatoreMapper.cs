using Models.Entity;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Mappers
{
    public static class OperatoreMapper
    {
        public static OperatoreMap ToMap(this Operatore entity)
        {
            return new OperatoreMap
            {
                Id = entity.Id,
                NomeOperatore = entity.Nome, // Mappa Nome -> NomeOperatore
                Password = entity.Password,
                Abilitato = entity.Abilitato,
                Badge = entity.Pass,  
                CodicePerson = entity.PersonId// Mappa Pass -> Badge
                                              // Se hai relazioni (es. Permessi), puoi mapparle qui o lasciarle nulle
            };
        }

        public static Operatore ToTable(this OperatoreMap map)
        {
            return new Operatore
            {
                Id = map.Id,
                Nome = map.NomeOperatore,
                Password = map.Password,
                Abilitato = map.Abilitato,
                Pass = map.Badge,
                PersonId = map.CodicePerson 
            };
        }


        public static Expression<Func<Operatore, LoginMap>> ToLoginMap => entity => new LoginMap
        {
            Id = entity.Id,
            NomeOperatore = entity.Nome,
            Password = entity.Password
        };

        public static Expression<Func<Operatore, OperatoreMap>> ToSimpleOperatoreMap => o => new OperatoreMap
        {
            Id = o.Id,
            NomeOperatore = o.Nome,
            Password = o.Password,
            Abilitato = o.Abilitato,
            Badge = o.Pass


        };

        public static Expression<Func<Operatore, Permesso?, OperatoreMap>> ToOperatoreMap =>
        (o, p) => new OperatoreMap
        {
            Id = o.Id,
            NomeOperatore = o.Nome,
            Password = o.Password,
            Abilitato = o.Abilitato,
            Badge = o.Pass,
            CodicePermesso = p != null ? p.Id : 0,
            NomePostazione = p!.Postazione!.Nome ?? "Nessuna",
            TipoPostazione = p!.Postazione!.TipoPostazione!.Nome ?? "N/A"
        };
    }
}
