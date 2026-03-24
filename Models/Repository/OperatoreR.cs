using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entity;
using Models.Interfaces;
using Models.Projections;
using Models.Tables;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Models.Repository
{
    public class OperatoreR : IDisposable, IGroupQ<OperatoreMap>
    {
        private static int classCount;
        private readonly OperatoreDbContext _ctx;

        public OperatoreR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} " +
                                               $"caricato *****");

            _ctx = new OperatoreDbContext();
        }

#if DEBUG

        ~OperatoreR()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{classCount} DISTRUTTO *****");
        }

#endif

        public virtual void Dispose()
        {
            _ctx.Dispose();
        }

        public async Task<List<OperatoreMap>> Load(int id)
        {
            if (id > 0)
                return await LoadOperatori(x => x.Id == id);
            else
                return await LoadOperatori(p => p.Id > -2);
        }

        public Task<List<OperatoreMap>> LoadByModel(object model) =>
                Task.FromResult((List<OperatoreMap>)model);

        public async Task<List<OperatoreMap>> LoadOperatori(Expression<Func<Operatore, bool>> predicate)
        {
            return await _ctx.Operatori
                .AsNoTracking()
                .Where(predicate)
                .OrderBy(p => p.Nome)
                .SelectMany(
                    o => o.Permessi.DefaultIfEmpty(),
                    OperatoreProjections.ToOperatoreMap // Usiamo la proiezione statica
                )
                .ToListAsync();
        }

        public async Task<OperatoreMap> FirstOperatore(int id)
        {
            // Usiamo la proprietà _context definita a livello di classe
            var result = await _ctx.Operatori
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(OperatoreProjections.ToSimpleOperatoreMap)
                .FirstOrDefaultAsync();

            // Se result è null, restituisce una nuova istanza vuota (come nel tuo codice originale)
            return result ?? new OperatoreMap();
        }

        public async Task<int> Add(OperatoreMap map)
        {
            // 1. Creiamo l'albero degli oggetti collegati
            var operatore = new Operatore
            {
                Nome = map.NomeOperatore,
                Password = map.Password,
                Abilitato = map.Abilitato,
                Pass = map.Badge,
                PersonId = -2

            };

            // 2. Aggiungiamo solo la "radice" (Person). EF aggiungerà i figli a cascata.
            _ctx.Operatori.Add(operatore);

            try
            {
                // 3. Un'unica chiamata al database (Transazione atomica)
                await _ctx.SaveChangesAsync();

                // Dopo SaveChanges, person.Id contiene l'ID reale generato dal DB
                return operatore.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Add: {ex.InnerException?.Message ?? ex.Message}");
                return -1;
            }
        }
        public async Task<bool> Upd(OperatoreMap map)
        {
            // 1. Cerchiamo l'entità (FindAsync è ottimo qui)
            var person = await _ctx.Operatori.FindAsync(map.Id);
            if (person == null) return false;
            // 2. Aggiorniamo le proprietà
            person.Nome = map.NomeOperatore;
            person.Password = map.Password;
            person.Abilitato = map.Abilitato;
            person.Pass = map.Badge;

            try
            {
                // 3. FONDAMENTALE: Salva le modifiche effettive nel DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Logga l'errore (es. violazione di vincoli o database offline)
                Debug.WriteLine($"Errore Update: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> EsisteNomeOperatore(OperatoreMap dT)
        {
            return await _ctx.Operatori.AnyAsync(p => p.Nome == dT.NomeOperatore);
        }
        public async Task<bool> EsisteNomeOperatoreUpd(OperatoreMap dT)
        {
            return await _ctx.Operatori.AnyAsync(p => p.Nome == dT.NomeOperatore && p.Id != dT.Id);
        }

    }
}
