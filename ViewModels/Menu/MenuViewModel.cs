using Models;
using Models.Entity;
using Models.Entity.Global;
using Models.Repository;
using ReactiveUI;
using SysNet;
using System.Reactive;

namespace ViewModels
{
    public partial class MenuViewModel : BaseViewModel
    {
        private static int classCount;
        private MenuR Q { get; set; }
        private readonly ServiceM service = Create<ServiceM>.Instance();
        
        public ReactiveCommand<string, Unit> NavigateCommand { get; }
        public ReactiveCommand<string, Unit> CassaCommand { get; }

       
        public MenuViewModel(IScreen host) : base(host)
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");
            
            base._deadEntries = classCount;
            
            NavigateCommand = ReactiveCommand.Create<string>(param =>
            {
                //if (param == null)
                //    MessageBox.Show("Parametro ricevuto NULL");
                //else
                //    MessageBox.Show($"Ricevuto: {param} di tipo {param.GetType()}");

                IRoutableViewModel CurrentPage = param switch
                {

                    "Login" => new LoginViewModel(HostScreen),
                    "Connection" => new ConnectionViewModel(HostScreen),
                    "Soci" => new SociViewModel(HostScreen),
                    //"Configurazione" => null,
                    //"Servizi" => null,
                    //"Gestore" => null,
                    //"OpenPostazioneCassa" => null,
                    //"OpenGiornata" => null,
                    //"CloseGiornata" => null,
                    //"OpenTurno" => null,
                    //"CloseTurno" => null,
                    _ => null
                };
                if (CurrentPage != null)
                {
                    HostScreen.Router.NavigateAndReset.Execute(CurrentPage);
                }

            });

            CassaCommand = ReactiveCommand.Create<string>(param => OnCassa(param));

        }

        protected override void OnFinalDestruction()
        {
            Q.Dispose();
            Q = null;
            base.OnFinalDestruction();
        }

        protected override async Task OnLoading()
        {

            Q = new();
            if (GlobalValuesC.MySetting != null)
            {
                AttivaPermessi();
            }

            CassaPostazioniDataSource = await Q.CaricaPostazioniCassa(GlobalValuesC.MySetting.IDOPERATORE);
            ApriGiornataEnabled = !(await Q.EsisteGiornataAperta());
            if (GlobalValuesC.MySetting is null) await Task.CompletedTask;
            if (GlobalValuesC.MySetting.POSTAZIONI is not null)
            {
                if (GlobalValuesC.MySetting.POSTAZIONI.Count == 0) ApriPostazioneEnabled = false;
            }

            await Task.CompletedTask;
        }

        private void AttivaPermessi()
        {
            if (GlobalValuesC.MySetting is null) return;

            OperatoreName = "Operatore : " + GlobalValuesC.MySetting.NOMEOPERATORE;
            SessioneContabile = "Sessione Contabile " + (ApriGiornataEnabled ? "Chiusa" : "Aperta");

            if (GlobalValuesC.MySetting.POSTAZIONI is null) return;

            try
            {
                foreach (PostazioneXC Element in GlobalValuesC.MySetting.POSTAZIONI)
                {
                    switch (Element.TIPOPOSTAZIONE)
                    {
                        case (int)Enums.Postazioni.Amministratore:
                            AmministratoreVisible = true;
                            ReportVisible = true;
                            break;

                        case (int)Enums.Postazioni.Cassa:
                            CassaVisible = true;
                            ReportVisible = true;
                            break;

                        case (int)Enums.Postazioni.Bar:
                            BarVisible = true;
                            break;

                        case (int)Enums.Postazioni.Guardaroba:
                            GuardarobaVisible = true;
                            break;

                        case (int)Enums.Postazioni.Pulizie:
                            PulizieVisible = true;
                            break;

                    }
                }
            }
            catch (NullReferenceException)
            {
                return;
            }

            IsMenuReady = true;


        }

        private void OnCassa(string x)
        {
            GlobalValuesC.MyPostazione = new()
            {
                DESCPOSTAZIONE = x
            };
            //MessageBox.Show(x);
            //MainNavigator.NotifyColleagues("CassaBase");
        }


    }

    public partial class MenuViewModel
    {
        //Voci visibili nel Menu
        private List<bool> _visibile = [];
        public List<bool> Visibile
        {
            get => _visibile;
            set => this.RaiseAndSetIfChanged(ref _visibile, value);
        }

        private bool _myamministratorevisible;
        public bool AmministratoreVisible
        {
            get => _myamministratorevisible;
            set => this.RaiseAndSetIfChanged(ref _myamministratorevisible, value);
        }

        private bool _myreportvisible;
        public bool ReportVisible
        {
            get => _myreportvisible;
            set => this.RaiseAndSetIfChanged(ref _myreportvisible, value);
        }

        private bool _mycassavisible;
        public bool CassaVisible
        {
            get => _mycassavisible;
            set => this.RaiseAndSetIfChanged(ref _mycassavisible, value);
        }

        private bool _mybarvisible;
        public bool BarVisible
        {
            get => _mybarvisible;
            set => this.RaiseAndSetIfChanged(ref _mybarvisible, value);
        }

        private bool _myguardarobavisible;
        public bool GuardarobaVisible
        {
            get => _myguardarobavisible;
            set => this.RaiseAndSetIfChanged(ref _myguardarobavisible, value);
        }

        private bool _mypulizievisible;
        public bool PulizieVisible
        {
            get => _mypulizievisible;
            set => this.RaiseAndSetIfChanged(ref _mypulizievisible, value);
        }

        private List<PostazioneMap> _mycassapostazionidatasource = null;
        public List<PostazioneMap> CassaPostazioniDataSource
        {
            get => _mycassapostazionidatasource;
            set => this.RaiseAndSetIfChanged(ref _mycassapostazionidatasource, value);
        }

        private string _myoperatorename = string.Empty;
        public string OperatoreName
        {
            get => _myoperatorename;
            set => this.RaiseAndSetIfChanged(ref _myoperatorename, value);
        }

        private string _mysessionecontabile = string.Empty;
        public string SessioneContabile
        {
            get => _mysessionecontabile;
            set => this.RaiseAndSetIfChanged(ref _mysessionecontabile, value);
        }

        private bool _myaprigiornataenabled = false;
        public bool ApriGiornataEnabled
        {
            get => _myaprigiornataenabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _myaprigiornataenabled, value);
                ChiudiGiornataEnabled = !value;
                Visibile[(int)Enums.Postazioni.Cassa] = !value;
                SessioneContabile = "Sessione Contabile " + (value ? "Chiusa" : "Aperta");
            }
        }

        private bool _myapripostazioneenabled = false;
        public bool ApriPostazioneEnabled
        {
            get => _myapripostazioneenabled;
            set => this.RaiseAndSetIfChanged(ref _myapripostazioneenabled, value);
        }

        private bool _mychiudigiornataenabled = false;
        public bool ChiudiGiornataEnabled
        {
            get => _mychiudigiornataenabled;
            set => this.RaiseAndSetIfChanged(ref _mychiudigiornataenabled, value);
        }

        private bool _isMenuReady = false;
        public bool IsMenuReady
        {
            get => _isMenuReady;
            set => this.RaiseAndSetIfChanged(ref _isMenuReady, value);
        }
        
    }

    
}
