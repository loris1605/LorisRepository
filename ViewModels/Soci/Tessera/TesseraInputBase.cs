using Models.Entity;
using ReactiveUI;
using SysNet;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace ViewModels
{
    public partial class TesseraInputBase : InputViewModel
    {
        string Cognome => BindingT is null ? "" : BindingT.Cognome.Trim();
        string Nome => BindingT is null ? "" : BindingT.Nome.Trim();
        string NumeroSocio => BindingT is null ? string.Empty : BindingT.NumeroSocio;
        int CodiceSocio => BindingT is null ? 0 : BindingT.CodiceSocio;
        int CodicePerson => BindingT is null ? 0 : BindingT.Id;

        protected string GetNumeroTessera => NumeroTessera;
        protected string GetNumeroSocio => NumeroSocio;
        protected int GetCodiceSocio => CodiceSocio;
        protected string GetNomeCognome => Nome + " " + Cognome;
        protected int GetCodicePerson => CodicePerson;

        protected void ResetNumeroTessera() => BindingT.NumeroTessera = string.Empty;

        public Interaction<Unit, Unit> NumeroTesseraFocus { get; } = new();
        
        public TesseraInputBase(IScreen host) : base(host)
        {
            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.NumeroTessera)
                    .Where(_ => BindingT != null)
                    .Subscribe(val => BindingT.NumeroTessera = val)
                    .DisposeWith(d);
            });
        }

        protected async override Task OnSaving() { await Task.CompletedTask; }

        public async Task OnNumeroTesseraFocus()
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);
            await NumeroTesseraFocus.Handle(Unit.Default).ToTask();
        }
    }

    public partial class TesseraInputBase
    {

        private string numerotessera = string.Empty;
        public string NumeroTessera
        {
            get => numerotessera;
            set => this.RaiseAndSetIfChanged(ref numerotessera, value);

        }

        private PersonMap bindingt = Create<PersonMap>.Instance();
        public PersonMap BindingT
        {
            get => bindingt;
            set
            {
                // 1. Aggiorna il riferimento (fondamentale per RaiseAndSetIfChanged)
                this.RaiseAndSetIfChanged(ref bindingt, value);

                // 2. Se carichi un socio, allinea la UI al modello
                if (value != null)
                {
                    this.NumeroTessera = value.NumeroTessera;
                }
            }
        }
    }
}
