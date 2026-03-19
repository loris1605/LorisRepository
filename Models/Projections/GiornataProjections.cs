using Models.Entity.Global;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Projections
{
    public static class GiornataProjections
    {
        public static Expression<Func<Giornata, GiornataXC>> ToGiornataXC => p => new GiornataXC
        {
            IDGIORNATA = p.Id,
            DATAINIZIO = p.DataInizio,
            DATAFINE = p.DataFine,
            APERTA = p.Aperta
        };
    }
}
