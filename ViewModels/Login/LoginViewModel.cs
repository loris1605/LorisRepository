using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Models.Entity;
using Models.Repository;
using ReactiveUI;
using SysNet;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private LoginR Q;

        public ReactiveCommand<Unit, Unit> EntraCommand { get; }
        //public ReactiveCommand<Unit, Unit> EsciCommand { get; }

        //public LoginViewModel() { }

        public LoginViewModel(IScreen host) : base(host)
        {
            var canEntra = this.WhenAnyValue(
               x => x.PasswordText,
               x => x.BindingT,
               (pass, operatore) =>
                   !string.IsNullOrWhiteSpace(pass) &&
                   operatore != null &&
                   pass == operatore.Password);


            EntraCommand = ReactiveCommand.CreateFromTask(OnEntra, canEntra);

            //EsciCommand = ReactiveCommand.Create(() =>
            //{
            //    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            //        lifetime.Shutdown();
            //});

            this.WhenActivated(d =>
            {

//                this.WhenAnyValue(
//                   x => x.PasswordText,
//                   x => x.BindingT, // Osserva l'intero oggetto
//                   (pass, binding) =>
//                       !string.IsNullOrWhiteSpace(pass) &&
//                       binding != null && // Controllo di sicurezza
//                       pass == binding.Password)
//.               Subscribe(value => EnabledEntra = value)
//.               DisposeWith(d);

                EntraCommand.DisposeWith(d);
                //EsciCommand.DisposeWith(d);
            });
 
        }
        
        protected override void OnFinalDestruction()
        {
            Q?.Dispose();
            Q = null;
            DataSource = null;
            BindingT = null;
            base.OnFinalDestruction();
        }

        protected override async Task OnLoading()
        {
         
            try
            {
                IsLoading = true;
                Q = new (); // Istanza locale del Repository
                DataSource = await Q.GetOperatoriAbilitati();
                if (DataSource?.Count > 0) BindingT = DataSource[0];
            }
            catch (OperationCanceledException) { Debug.WriteLine("Caricamento Login Annullato."); }
            catch (Exception) { Debug.WriteLine("Caricamento Login fallito."); }
            finally { IsLoading = false; }

            await OnPasswordFocus();
            
        }

        private async Task OnPasswordFocus()
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);

            try
            {
                await PasswordFocus.Handle(Unit.Default);
            }
            catch (Exception ex)
            {
                // Evita crash se l'handler non è ancora pronto o la vista è già chiusa
                System.Diagnostics.Debug.WriteLine("Interaction Focus fallita: " + ex.Message);
            }
        }

        private async Task OnEntra()
        {
            try
            {
                // Salva le impostazioni dell'operatore selezionato
                await Q.SaveSettings(BindingT);

                // Naviga al Menu principale resettando lo stack di navigazione
                await HostScreen.Router.NavigateAndReset.Execute(new MenuViewModel(HostScreen));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> [ERROR] Login fallito durante il salvataggio o la navigazione: {ex.Message}");
                // Qui potresti aggiungere un'interaction per mostrare un messaggio di errore all'utente
            }


        }
    }

    public partial class LoginViewModel
    {
        //DataSource della ComboBox
        private List<LoginMap> _datasource = [];
        public List<LoginMap> DataSource
        {
            get => _datasource;
            set => this.RaiseAndSetIfChanged(ref _datasource, value);
        }


        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }


        private string passwordtext = string.Empty;
        public string PasswordText
        {
            get => passwordtext;
            set => this.RaiseAndSetIfChanged(ref passwordtext, value);
        }

        private LoginMap bindingt = Create<LoginMap>.Instance();
        public LoginMap BindingT
        {
            get => bindingt;
            set => this.RaiseAndSetIfChanged(ref bindingt, value);
        }

        private int codiceoperatore;
        public int CodiceOperatore
        {
            get => codiceoperatore;
            set => this.RaiseAndSetIfChanged(ref codiceoperatore, value);
        }

        #region Observable

        public Interaction<Unit, Unit> PasswordFocus { get; } = new();

        #endregion

    }
}
