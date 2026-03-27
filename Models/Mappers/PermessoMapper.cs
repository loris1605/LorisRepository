using Models.Entity;
using Models.Entity.Global;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Mappers
{
    public static class PermessoMapper
    {
        public static Expression<Func<Permesso, PostazioneXC>> ToPostazioneXC => p => new PostazioneXC
        {
            CODICEPOSTAZIONE = p.PostazioneId,
            DESCPOSTAZIONE = p.Postazione!.Nome!,
            TIPOPOSTAZIONE = p.Postazione.TipoPostazioneId
        };

        public static Expression<Func<Permesso, PostazioneMap>> ToPostazioneMap => p => new PostazioneMap
        {
            Id = p.Id,
            CodiceTipoPostazione = p.PostazioneId,
            NomePostazione = p.Postazione!.Nome
        };

        //public static PostazioneXC ToPostazioneXC(Permesso p)
        //{
        //    return new PostazioneXC
        //    {
        //        // Usiamo l'operatore ?. perché Postazione è nullable nel modello
        //        CODICEPOSTAZIONE = p.PostazioneId,
        //        DESCPOSTAZIONE = p.Postazione?.Nome ?? "Postazione Sconosciuta",
        //        TIPOPOSTAZIONE = p.Postazione!.TipoPostazioneId
        //        // ... altri campi ...
        //    };
        //}
    }
}
