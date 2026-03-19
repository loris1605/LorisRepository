using Models.Repository;
using ReactiveUI;
using SysNet;
using SysNet.Converters;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public class TesseraDelViewModel : TesseraInputBase
    {
        private PersonR Q { get; set; }
        private readonly int _idDaModificare;
        private readonly int _idRitorno;

        public TesseraDelViewModel(IScreen host, int idtessera, int idperson) : base(host)
        {
            _idDaModificare = idtessera;
            _idRitorno = idperson;

            FieldVisibile = false;
            FieldsEnabled = false;

            Q = Create<PersonR>.Instance();

            this.WhenActivated(d =>
            {
                OnEscFocus().FireAndForget();

                Disposable.Create(() => {
                    System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} disposed *****");
                }).DisposeWith(d);
            });
        }

        protected override void OnFinalDestruction()
        {
            Q.Dispose();
            Q = null;
        }
        
        protected override async Task OnLoading()
        {
            BindingT = await Q.FirstTessera(_idDaModificare);
            if (BindingT == null)
            {
                InfoLabel = "Errore: Tesera non trovata nel database.";
                FieldsEnabled = false;
            }
            Titolo = "Elimina Tessera : " + GetNumeroTessera;
            Titolo1 = "per " + GetNomeCognome;

            await OnEscFocus();
        }

        protected async override Task OnSaving()
        {

            if (!await Q.DelTessera(BindingT))
            {
                InfoLabel = "Errore Db eliminazione person";
                await OnEscFocus();
                return;
            }
            OnBack(_idRitorno);
        }
    }
}
