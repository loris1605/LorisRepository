using Models.Repository;
using Models.Tables;
using ReactiveUI;
using SysNet;

namespace ViewModels
{
    public class OperatoreUpdViewModel : OperatoreInputBase
    {
        private OperatoreR Q { get; set; }
        private readonly int _idDaModificare;

        public OperatoreUpdViewModel(IScreen host, int idoperatore) : base(host)
        {
            _idDaModificare = idoperatore;

            Titolo = "Modifica Operatore";
            FieldsEnabled = true;
            
            Q = Create<OperatoreR>.Instance();
        }

        protected override void OnFinalDestruction()
        {
            Q?.Dispose();
            Q = null;
        }

        protected override async Task OnLoading()
        {
            BindingT = await Q.FirstOperatore(_idDaModificare);
            if (BindingT == null)
            {
                InfoLabel = "Errore: Operatore non trovato nel database.";
                FieldsEnabled = false;
            }
            await OnFocus(NomeFocus);
        }

        protected override async Task OnSaving()
        {
            if (!await ValidaDati()) return;

            if (await Q.EsisteNomeOperatoreUpd(BindingT))
            {
                InfoLabel = "Operatore già registrato";
                return;
            }

            InfoLabel = "";

            if (!await Q.Upd(BindingT))
            {
                InfoLabel = "Errore Db modifica operatore";
                await OnFocus(NomeFocus);
                return;
            }

            OnBack(_idDaModificare);

        }
    }
}
