using Models.Repository;
using ReactiveUI;
using SysNet;
using SysNet.Converters;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Threading.Tasks;
using System.Windows;

namespace ViewModels
{
    public partial class PersonSearchViewModel : PersonInputBase
    {
        private PersonR Q { get; set; }

        public PersonSearchViewModel(IScreen host) : base(host)
        {
            Titolo = "Trova Socio";
            Q = Create<PersonR>.Instance();

            this.WhenActivated(d =>
            {
                OnCognomeFocus().FireAndForget();

                Disposable.Create(() => {
                    System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} disposed *****");
                }).DisposeWith(d);

            });
        }

        protected override void OnFinalDestruction()
        {
            Q?.Dispose();
            Q = null;
            DataSource = null;
        }

        protected override async Task OnLoading()
        {
            ResetAllCombos();
            AnagraficaChecked = true;
            await OnCognomeFocus();
        }

        protected async override Task OnSaving()
        {
            await Task.CompletedTask;
        }

        void ResetAllCombos()
        {
            CognomeSelectedValue = 0;
            NomeSelectedValue = 0;
            NatoilSelectedValue = 0;
        }

        private async Task OnCognomeFocus()
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);
            await CognomeFocus.Handle(Unit.Default).ToTask();

        }

        private async Task OnNomeFocus()
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);
            await NomeFocus.Handle(Unit.Default).ToTask();

        }
    }

    public partial class PersonSearchViewModel
    {
        #region Enabled

        #region TesseraEnabled

        private bool _mytesseraenabled;
        public bool TesseraEnabled
        {
            get => _mytesseraenabled;
            set => this.RaiseAndSetIfChanged(ref _mytesseraenabled, value);
        }

        #endregion

        #region SocioEnabled

        private bool _mysocioenabled;
        public bool SocioEnabled
        {
            get => _mysocioenabled;
            set => this.RaiseAndSetIfChanged(ref _mysocioenabled, value);
        }

        #endregion

        #region NomeEnabled

        private bool _mynomeenabled;
        public bool NomeEnabled
        {
            get => _mynomeenabled;
            set => this.RaiseAndSetIfChanged(ref _mynomeenabled, value);
        }

        #endregion

        #region CognomeEnabled

        private bool _mycognomeenabled;
        public bool CognomeEnabled
        {
            get => _mycognomeenabled;
            set => this.RaiseAndSetIfChanged(ref _mycognomeenabled, value);
        }

        #endregion

        #region NatoilEnabled

        private bool _mynatoilenabled;
        public bool NatoilEnabled
        {
            get => _mynatoilenabled;
            set => this.RaiseAndSetIfChanged(ref _mynatoilenabled, value);
        }

        #endregion

        #region CognomeComboEnabled

        private bool _mycognomecomboenabled;
        public bool CognomeComboEnabled
        {
            get => _mycognomecomboenabled;
            set => this.RaiseAndSetIfChanged(ref _mycognomecomboenabled, value);
        }

        #endregion

        #region NomeComboEnabled

        private bool _mynomecombonabled;
        public bool NomeComboEnabled
        {
            get => _mynomecombonabled;
            set => this.RaiseAndSetIfChanged(ref _mynomecombonabled, value);
        }

        #endregion

        #region NatoilComboEnabled

        private bool _mynatoilcomboenabled;
        public bool NatoilComboEnabled
        {
            get => _mynatoilcomboenabled;
            set => this.RaiseAndSetIfChanged(ref _mynatoilcomboenabled, value);
        }

        #endregion

        #endregion

        #region Combo SelectedValue

        #region CognomeSelectedValue

        private int _mycognomeselectedvalue;
        public int CognomeSelectedValue
        {
            get => _mycognomeselectedvalue;
            set => this.RaiseAndSetIfChanged(ref _mycognomeselectedvalue, value);
        }

        #endregion

        #region NomeSelectedValue

        private int _mynomeselectedvalue;
        public int NomeSelectedValue
        {
            get => _mynomeselectedvalue;
            set => this.RaiseAndSetIfChanged(ref _mynomeselectedvalue, value);
        }

        #endregion

        #region NatoilSelectedValue

        private int _mynatoilselectedvalue;
        public int NatoilSelectedValue
        {
            get => _mynatoilselectedvalue;
            set => this.RaiseAndSetIfChanged(ref _mynatoilselectedvalue, value);
        }

        #endregion

        #endregion

        #region Checked

        #region AnagraficaChecked

        //private static void OnAnagraficaCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    PersonSearchVM me = (PersonSearchVM)d;
        //    me.CognomeFocus = (bool)e.NewValue;
        //}

        private bool _myanagraficachecked;
        public bool AnagraficaChecked
        {
            get => _myanagraficachecked;
            set => this.RaiseAndSetIfChanged(ref _myanagraficachecked, value);
        }

        #endregion

        #region TesseraChecked
        //private static void OnTesseraCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    PersonSearchVM me = (PersonSearchVM)d;
        //    me.TesseraEnabled = (bool)e.NewValue;
        //    me.NumeroTesseraFocus = (bool)e.NewValue;
        //}

        private bool _mytesserachecked;
        public bool TesseraChecked
        {
            get => _mytesserachecked;
            set => this.RaiseAndSetIfChanged(ref _mytesserachecked, value);
        }

        #endregion

        #region SocioChecked

        //private static void OnSocioCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    PersonSearchVM me = (PersonSearchVM)d;
        //    me.SocioEnabled = (bool)e.NewValue;
        //    me.NumeroSocioFocus = (bool)e.NewValue;
        //}

        private bool _mysociochecked;
        public bool SocioChecked
        {
            get => _mysociochecked;
            set => this.RaiseAndSetIfChanged(ref _mysociochecked, value);
        }

        #endregion

        #endregion
    }
}
