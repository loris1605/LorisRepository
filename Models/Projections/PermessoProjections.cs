using Models.Entity;
using Models.Entity.Global;
using Models.Tables;
using System.Linq.Expressions;

namespace Models.Projections
{
    public static class PermessoProjections
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
    }
}
