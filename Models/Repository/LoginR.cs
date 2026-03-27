using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entity;
using Models.Entity.Global;
using Models.Mappers;
using Models.Projections;
using Models.Tables;
using SysNet;

namespace Models.Repository
{
    public class LoginR : BaseRepository<LoginDbContext, Operatore>
    {
        private static int classCount;
        public LoginR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");
           
        }

        public async Task<List<LoginMap>> GetOperatoriAbilitati()
        {
            return await GetAll(
                selector: OperatoreMapper.ToLoginMap,
                predicate: p => p.Abilitato == true);
        }

        private async Task<List<PostazioneXC>> ListPostazioniByOperatore(int CodiceOperatore)
        {

            using LoginDbContext _ctx = new();
            IQueryable<Permesso> query = 
                _ctx.Permessi
                    .AsNoTracking()
                    .Where(p => p.OperatoreId == CodiceOperatore);

            return await query.Select(PermessoMapper.ToPostazioneXC).ToListAsync();
        }

        private async Task<List<SettoreXC>> SelectSettoriX(int CodicePostazione)
        {
            using LoginDbContext _ctx = new();
            IQueryable<Reparto> query =
                _ctx.Reparti
                    .AsNoTracking()
                    .Where(p => p.PostazioneId == CodicePostazione);

            return await query.Select(RepartoMapper.ToSettoreXC).ToListAsync();

        }

        private async Task<List<TariffaXC>> SelectTariffeX(int CodiceSettore)
        {
            using LoginDbContext _ctx = new();
            IQueryable<Listino> query =
                _ctx.Listini
                   .AsNoTracking()
                   .Where(p => p.SettoreId == CodiceSettore);

            return await query.Select(ListinoMapper.ToTariffaXC).ToListAsync();

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
                        settore.TARIFFE = await SelectTariffeX(settore.CODICESETTORE) ?? [];
                    }
                }
            }

            XOperatore.GIORNATA = await GetGiornataOpen();
            
            GlobalValuesC.MySetting = XOperatore;

        }

       
        private async Task<GiornataXC?> GetGiornataOpen()
        {
            using LoginDbContext _ctx = new();
            IQueryable<Giornata> query = _ctx.Giornate
                                            .AsNoTracking()
                                            .Where(x => x.Aperta == true);

            return await query.Select(GiornataProjections.ToGiornataXC).FirstOrDefaultAsync();

        }

    }
}
 