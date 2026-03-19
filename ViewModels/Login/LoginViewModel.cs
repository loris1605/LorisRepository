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
        private static int classCount;

        private LoginR Q;

        public ReactiveCommand<Unit, Unit> EntraCommand { get; }
        public ReactiveCommand<Unit, Unit> EsciCommand { get; }

        //public LoginViewModel() { }

        public LoginViewModel(IScreen host) : base(host)
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");

            base._deadEntries = classCount;

            EntraCommand = ReactiveCommand.CreateFromTask(OnEntra);
           
            this.WhenActivated(d =>
            {

                this.WhenAnyValue(
                    x => x.PasswordText,
                    (pass) =>
                    !string.IsNullOrWhiteSpace(pass) &&
                    pass == BindingT.Password)
                .Subscribe(value => EnabledEntra = value)
                .DisposeWith(d);
            });
 
        }
        
        protected override void OnFinalDestruction()
        {
            Q.Dispose();
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
            if (EnabledEntra)
            {
                await Q.SaveSettings(BindingT);
                //var weak = new WeakReference(this); // Monitora me stesso

                await HostScreen.Router.NavigateAndReset.Execute(new MenuViewModel(HostScreen));
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

        private bool _enabledentra;
        public bool EnabledEntra
        {
            get => _enabledentra;
            set => this.RaiseAndSetIfChanged(ref _enabledentra, value);
        }

        #region Observable

        public Interaction<Unit, Unit> PasswordFocus { get; } = new();

        #endregion

    }
}
