using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entity;
using Models.Projections;
using Models.Tables;
using System.Diagnostics;

namespace Models.Repository
{
    public class MenuR : IDisposable
    {
        private static int classCount;
        private readonly MenuDbContext _ctx;

        public MenuR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");
            _ctx = new MenuDbContext();
        }

#if DEBUG

        ~MenuR()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{classCount} DISTRUTTO *****");
        }

#endif

        public virtual void Dispose() { _ctx.Dispose(); }

        public async Task<bool> EsisteGiornataAperta() => await _ctx.Giornate.AnyAsync(p => p.Aperta);

        public async Task<List<PostazioneMap>> CaricaPostazioniCassa(int CodiceOperatore)
        {
            IQueryable<Permesso> query = 
                _ctx.Permessi
                    .AsNoTracking()
                    .Where(p => p.OperatoreId == CodiceOperatore)
                    .Where(p => p.Postazione!.TipoPostazioneId == 2)
                    .Where(p => p.PostazioneId > 0);

            return await query.Select(PermessoProjections.ToPostazioneMap).ToListAsync();

        }

    }
}
