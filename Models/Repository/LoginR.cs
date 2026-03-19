using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entity;
using Models.Entity.Global;
using Models.Projections;
using Models.Tables;
using SysNet;
using System.Diagnostics;

namespace Models.Repository
{
    public class LoginR : IDisposable
    {
        private static int classCount;
        private readonly LoginDbContext _ctx;

        public LoginR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");
            _ctx = new LoginDbContext();
        }

#if DEBUG

        ~LoginR()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{classCount} DISTRUTTO *****");
        }

#endif

        
        public void Dispose() { _ctx.Dispose(); }

        public async Task<List<LoginMap>> GetOperatoriAbilitati()
        {
            IQueryable<Operatore> query = _ctx.Operatori
                                              .AsNoTracking()
                                              .Where(p => p.Abilitato == true);

            return await query.Select(OperatoreProjections.ToLoginMap).ToListAsync();

        }

        private async Task<List<PostazioneXC>> ListPostazioniByOperatore(int CodiceOperatore)
        {
            IQueryable<Permesso> query = 
                _ctx.Permessi
                    .AsNoTracking()
                    .Where(p => p.OperatoreId == CodiceOperatore);

            return await query.Select(PermessoProjections.ToPostazioneXC).ToListAsync();
        }

        private async Task<List<SettoreXC>> SelectSettoriX(int CodicePostazione)
        {

            IQueryable<Reparto> query =
                _ctx.Reparti
                    .AsNoTracking()
                    .Where(p => p.PostazioneId == CodicePostazione);

            return await query.Select(RepartoProjections.ToSettoreXC).ToListAsync();

        }

        private async Task<List<TariffaXC>> SelectTariffeX(int CodiceSettore)
        {
            IQueryable<Listino> query =
                _ctx.Listini
                   .AsNoTracking()
                   .Where(p => p.SettoreId == CodiceSettore);

            return await query.Select(ListinoProjections.ToTariffaXC).ToListAsync();

        }

        public async Task SaveSettings(LoginMap dT)
        {
            
            OperatoreXC XOperatore = Create<OperatoreXC>.Instance();

            XOperatore.IDOPERATORE = dT.Id;
            XOperatore.NOMEOPERATORE = dT.NomeOperatore;
            XOperatore.PASSWORD = dT.Password;

            XOperatore.POSTAZIONI = await ListPostazioniByOperatore(dT.Id);

            if (XOperatore.POSTAZIONI.Count == 0)
            {
                GlobalValuesC.MySetting = XOperatore;
                await Task.CompletedTask;
            }

            foreach (var postazione in XOperatore.POSTAZIONI)
            {
                postazione.SETTORI = await SelectSettoriX(postazione.CODICEPOSTAZIONE);

                if (postazione.SETTORI.Count > 0)
                {
                    foreach (var settore in postazione.SETTORI)
                    {
                        settore.TARIFFE = await SelectTariffeX(settore.CODICESETTORE) ?? new List<TariffaXC>();
                    }
                }
            }

            

            XOperatore.GIORNATA = await GetGiornataOpen();

            
            GlobalValuesC.MySetting = XOperatore;

        }

        private async Task<GiornataXC?> GetGiornataOpen()
        {
            IQueryable<Giornata> query = _ctx.Giornate
                                            .AsNoTracking()
                                            .Where(x => x.Aperta == true);

            return await query.Select(GiornataProjections.ToGiornataXC).FirstOrDefaultAsync();

        }

    }
}
 