using Models.Entity;
using ReactiveUI;
using SysNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public partial class TariffaInputBase : InputViewModel
    {
        public string Name => BindingT.NomeTariffa.Trim() is null ? "" : BindingT.NomeTariffa.Trim();
        string Label => BindingT.EtichettaTariffa.Trim() is null ? "" : BindingT.EtichettaTariffa.Trim();
        int CodiceTariffa => BindingT is null ? 0 : BindingT.Id;

        protected bool IsNameEmpty => BindingT is not null && (Name == "");
        protected bool CheckLess2Name => Name.Length < 2;
        public bool IsLabelEmpty => BindingT is not null && (Label == "");
        public bool CheckLess2Label => Label.Length < 2;

        public TariffaInputBase(IScreen host) : base(host)
        {
            EscPressedCommand = ReactiveCommand.Create(OnBackEsc);

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.NomeTariffa)
                    .Where(_ => BindingT != null)
                    .Subscribe(val => BindingT.NomeTariffa = val)
                    .DisposeWith(d);
                this.WhenAnyValue(x => x.EtichettaTariffa)
                    .Where(_ => BindingT != null)
                    .Subscribe(val => BindingT.EtichettaTariffa = val)
                    .DisposeWith(d);
                this.WhenAnyValue(x => x.PrezzoTariffa)
                    .Where(_ => BindingT != null)
                    .Subscribe(val => BindingT.PrezzoTariffa = val)
                    .DisposeWith(d);


            });
        }

        protected async override Task OnSaving() { await Task.CompletedTask; }

        protected async Task OnFocus(Interaction<Unit, Unit> control)
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);

            try
            {
                await control.Handle(Unit.Default);
            }
            catch (Exception ex)
            {
                // Evita crash se l'handler non è ancora pronto o la vista è già chiusa
                System.Diagnostics.Debug.WriteLine("Interaction Focus fallita: " + ex.Message);
            }
        }
    }

    public partial class TariffaInputBase
    {
        private string nome = string.Empty;
        public string NomeTariffa
        {
            get => nome;
            set => this.RaiseAndSetIfChanged(ref nome, value);
        }

        private string etichetta = string.Empty;
        public string EtichettaTariffa
        {
            get => etichetta;
            set => this.RaiseAndSetIfChanged(ref etichetta, value);
        }

        private decimal prezzo = 0M;
        public decimal PrezzoTariffa
        {
            get => prezzo;
            set => this.RaiseAndSetIfChanged(ref prezzo, value);
        }

        private TariffaMap bindingt = Create<TariffaMap>.Instance();
        public TariffaMap BindingT
        {
            get => bindingt;
            set
            {
                // 1. Aggiorna il riferimento (fondamentale per RaiseAndSetIfChanged)
                this.RaiseAndSetIfChanged(ref bindingt, value);

                // 2. Se carichi una postazione, allinea la UI al modello
                if (value != null)
                {
                    this.NomeTariffa = value.NomeTariffa ?? "";
                    this.EtichettaTariffa = value.EtichettaTariffa ?? "";
                    this.PrezzoTariffa = value.PrezzoTariffa;

                }
            }
        }

    }
}
