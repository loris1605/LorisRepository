using Models.Entity;
using Models.Tables;
using System;
using System.Linq.Expressions;

namespace Models.Projections
{
    public static class OperatoreProjections
    {
        public static Expression<Func<Operatore, LoginMap>> ToLoginMap => p => new LoginMap
        {
            Id = p.Id,
            NomeOperatore = p.Nome,
            Password = p.Password
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

        public static Expression<Func<Operatore, OperatoreMap>> ToSimpleOperatoreMap => o => new OperatoreMap
        {
            Id = o.Id,
            NomeOperatore = o.Nome,
            Password = o.Password,
            Abilitato = o.Abilitato,
            Badge = o.Pass

            
        };
    }

}

    
