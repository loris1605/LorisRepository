using Models.Repository;
using ReactiveUI;
using SysNet;

namespace ViewModels
{
    public class OperatoreAddViewModel : OperatoreInputBase
    {
        private OperatoreR Q { get; set; }

        public OperatoreAddViewModel(IScreen host) : base(host)
        {
            Titolo = "Aggiungi Nuovo Operatore";
            FieldsVisibile = true;
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
            await OnFocus(NomeFocus);
        }

        protected async override Task OnSaving()
        {
            if (!await ValidaDati()) return;

            if (await Q.EsisteNomeOperatore(BindingT))
            {
                InfoLabel = "Operatore già registrato";
                await OnFocus(NomeFocus);
                return;
            }

            InfoLabel = "";

            int newOperatoreId = await Q.Add(BindingT);

            if (newOperatoreId == -1)
            {
                InfoLabel = "Errore Db inserimento Socio";
                await OnFocus(NomeFocus);
                return;
            }

            OnBack(newOperatoreId);
        }
    }
}
