using Models.Entity;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Mappers
{
    public static class PostazioneMapper
    {
        public static PostazioneMap ToMap(this Postazione entity)
        {
            return new PostazioneMap
            {
                Id = entity.Id,
                NomePostazione = entity.Nome, // Mappa Nome -> NomePostazione
                CodiceTipoPostazione = entity.TipoPostazioneId,
                CodiceTipoRientro = entity.TipoRientroId
            };
        }

        public static Postazione ToTable(this PostazioneMap map)
        {
            return new Postazione
            {
                Id = map.Id,
                Nome = map.NomePostazione,
                TipoPostazioneId = map.CodiceTipoPostazione,
                TipoRientroId = map.CodiceTipoRientro
            };
        }

        public static Expression<Func<Postazione, Reparto?, PostazioneMap>> ToPostazioneMap =>
        (o, p) => new PostazioneMap
        {
            Id = o.Id,
            CodiceTipoPostazione = o.TipoPostazione!.Id,
            NomePostazione = o.Nome,
            NomeTipoPostazione = o.TipoPostazione!.Nome,
            CodiceReparto = p != null ? p.Id : 0,
            NomeSettore = p!.Settore!.Nome ?? "Nessuna",
            EtichettaSettore = p!.Settore!.Label ?? "Nessuna",
            NomeTipoSettore = p!.Settore!.TipoSettore!.Nome ?? "N/A"
        };

        public static Expression<Func<Postazione, PostazioneMap>> ToSimplePostazioneMap => o => 
           new PostazioneMap
            {
                Id = o.Id,
                NomePostazione = o.Nome,
                CodiceTipoPostazione = o.TipoPostazioneId,
                CodiceTipoRientro = o.TipoRientroId

            };
    }
}
