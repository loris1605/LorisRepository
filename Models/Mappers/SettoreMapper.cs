using Models.Entity;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Mappers
{
    public static class SettoreMapper
    {
        public static SettoreMap ToMap(this Settore entity)
        {
            return new SettoreMap
            {
                Id = entity.Id,
                NomeSettore = entity.Nome, // Mappa Nome -> NomeSettore
                EtichettaSettore = entity.Label,
                CodiceTipoSettore = entity.TipoSettoreId
            };
        }

        public static Settore ToTable(this SettoreMap map)
        {
            return new Settore
            {
                Id = map.Id,
                Nome = map.NomeSettore,
                Label = map.EtichettaSettore,
                TipoSettoreId = map.CodiceTipoSettore
            };
        }

        public static Expression<Func<Settore, Listino?, SettoreMap>> ToSettoreMap =>
        (o, p) => new SettoreMap
        {
            Id = o.Id,
            CodiceTipoSettore = o.TipoSettore!.Id,
            NomeSettore = o.Nome,
            EtichettaSettore = o.Label,
            NomeTipoSettore = o.TipoSettore!.Nome,
            CodiceListino = p != null ? p.Id : 0,
            NomeTariffa = p!.Tariffa!.Nome ?? "Nessuna",
            EtichettaTariffa = p!.Tariffa!.Label ?? "Nessuna",
            PrezzoTariffa = p!.Tariffa!.Prezzo
        };

        public static Expression<Func<Settore, SettoreMap>> ToSimpleSettoreMap => o =>
           new SettoreMap
           {
               Id = o.Id,
               NomeSettore = o.Nome,
               EtichettaSettore = o.Label,
               CodiceTipoSettore = o.TipoSettoreId

           };
    }
}
