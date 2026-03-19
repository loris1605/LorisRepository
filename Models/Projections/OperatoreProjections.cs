using Models.Entity;
using Models.Tables;
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

        
    }
}
