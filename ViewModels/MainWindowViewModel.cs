using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Repository;
using ReactiveUI;
using SysNet;
using System.Diagnostics;
using System.Reactive.Disposables;

namespace ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject, 
                                               IScreen, 
                                               IRoutableViewModel,
                                               IActivatableViewModel
                                                
    {
        int currentVersion = 2; // Versione attuale del database, da aggiornare quando si modificano le entità
        
        public RoutingState Router { get; } = new RoutingState();
        public string UrlPathSegment => "main";
        public IScreen HostScreen => this;

        public ViewModelActivator Activator { get; } = new();

        public MainWindowViewModel()
        {
            // Spostiamo la logica di navigazione all'attivazione
            this.WhenActivated(async (CompositeDisposable disposables) =>
            {
                await InitializeNavigation();
            });
        }

        private async Task InitializeNavigation()
        {
            Connection.TestConnection();

            if (Flags.ServerAttivo)
            {
                SettingR r = new();
                if (!await r.CheckAppVersion(currentVersion))
                    await VerificaNecessitaAggiornamento();
                GoToLogin();
            }
            else
                GoToConnection();
        }

        public async Task VerificaNecessitaAggiornamento()
        {
            using var _ctx = new AppDbContext();

            // 1. Verifica se il database è raggiungibile
            if (!await _ctx.Database.CanConnectAsync())
            {
                throw new Exception("Errore SQL: Impossibile connettersi al database. Controlla la stringa di connessione.");
            }

            // 2. Recupera le migrazioni non ancora applicate
            var pendingMigrations = await _ctx.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                Debug.WriteLine("⚠️ Il database NECESSITA di un aggiornamento.");
                Debug.WriteLine("Migrazioni mancanti:");
                foreach (var m in pendingMigrations) Console.WriteLine($" - {m}");

                await _ctx.Database.MigrateAsync();

                // OPZIONALE: Scommenta la riga sotto per applicare gli aggiornamenti automaticamente
                // await _ctx.Database.MigrateAsync(); 
            }
            else
            {
                Debug.WriteLine("✅ Il database è aggiornato all'ultima versione del modello.");
            }
        }

        private void GoToLogin()
        {
            Router.NavigateAndReset.Execute(new LoginViewModel(this));
        }

        private void GoToConnection()
        {
            Router.NavigateAndReset.Execute(new ConnectionViewModel(this));
        }


    }
}
