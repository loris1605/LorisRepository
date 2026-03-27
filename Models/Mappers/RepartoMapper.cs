using Models.Entity.Global;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Mappers
{
    public static class RepartoMapper
    {
        public static Expression<Func<Reparto, SettoreXC>> ToSettoreXC => p => new SettoreXC
        {
            CODICESETTORE = p.SettoreId,
            DESCSETTORE = p.Settore!.Label!,
            NOMESETTORE = p.Settore.Nome!
        };
    }
}
