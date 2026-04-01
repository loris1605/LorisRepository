using Models.Entity;
using Models.Entity.Global;
using Models.Tables;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Models.Mappers
{
    public static class PermessoMapper
    {
        public static Expression<Func<Permesso, PostazioneXC>> ToPostazioneXC => p => new PostazioneXC
        {
            // Usiamo l'ID direttamente dal Permesso (più sicuro della navigazione)
            CODICEPOSTAZIONE = p.PostazioneId,
            // Usiamo il null-conditional per la navigazione
            DESCPOSTAZIONE = p.Postazione != null ? p.Postazione.Nome : "Nessuna",
            TIPOPOSTAZIONE = p.Postazione != null ? p.Postazione.TipoPostazioneId : 0
        };

        public static Expression<Func<Permesso, PostazioneMap>> ToPostazioneMap => p => new PostazioneMap
        {
            Id = p.Id,
            // Attenzione: qui mappi PostazioneId su CodiceTipoPostazione, 
            // verifica che non debba essere p.Postazione.TipoPostazioneId
            CodiceTipoPostazione = p.PostazioneId,
            NomePostazione = p.Postazione != null ? p.Postazione.Nome : "N/A"
        };

        //public static Expression<Func<Postazione, PostazioneElencoMap>> ToPostazioneElencoMap => o =>
        //   new PostazioneElencoMap
        //   {
        //       Id = o.Id,
        //       NomePostazione = o.Nome,
        //       CodiceTipoPostazione = o.TipoPostazioneId,
        //       NomeTipoPostazione = o.TipoPostazione != null ? o.TipoPostazione.Nome : "N/A",
        //       //HasPermesso = o.Permessi.Any()

        //   };

        public static PostazioneElencoMap ToMap(this Postazione o)
        {
            return new PostazioneElencoMap
            {
                Id = o.Id,
                NomePostazione = o.Nome,
                CodiceTipoPostazione = o.TipoPostazioneId,
                NomeTipoPostazione = o.TipoPostazione != null ? o.TipoPostazione.Nome : "N/A" 
                //HasPermesso = o.Permessi.Any()
            };
        }   


    }
}
