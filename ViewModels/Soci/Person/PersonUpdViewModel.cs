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
    public class PersonUpdViewModel : PersonInputBase
    {
        private PersonR Q { get; set; }
        private readonly int _idDaModificare;
        

        public PersonUpdViewModel(IScreen host, int idperson) : base(host)
        {
            _idDaModificare = idperson;
                        
            Titolo = "Modifica Socio";
            FieldsEnabled = true;
            FieldsVisibile = false;

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
            BindingT = await Q.FirstPerson(_idDaModificare);
            if (BindingT == null)
            {
                InfoLabel = "Errore: Socio non trovato nel database.";
                FieldsEnabled = false;
            }
            await OnCognomeFocus();
        }

        protected override async Task OnSaving()
        {
            if (!await ValidaDati()) return;

            if (await EsisteAnagraficaUpd())
            {
                InfoLabel = "Socio già registrato";
                return;
            }

            InfoLabel = "";

            if (!await Q.Upd(BindingT))
            {
                InfoLabel = "Errore Db modifica person";
                await OnCognomeFocus();
                return;
            }

            OnBack(_idDaModificare);
            
        }

        private async Task OnCognomeFocus()
        {
            // Fondamentale: aspetta un attimo che la View sia "viva" e l'handler registrato
            await Task.Delay(200);

            try
            {
                await CognomeFocus.Handle(Unit.Default);
            }
            catch (Exception ex)
            {
                // Evita crash se l'handler non è ancora pronto o la vista è già chiusa
                System.Diagnostics.Debug.WriteLine("Interaction Focus fallita: " + ex.Message);
            }
        }

        private async Task<bool> EsisteAnagraficaUpd()
        {
            string srvcognome = Cognome;
            string srvnome = Nome;

            if (srvcognome.Length == 2)
            {
                srvcognome += " ";
            }
            if (srvnome.Length == 2)
            {
                srvnome += " ";
            }
            if (BindingT is null) return false;

            BindingT.CodiceUnivoco = string.Concat(srvcognome[..3],
                                                   srvnome[..3],
                                                   BindingT.Natoil.ToString());


            return await Q.EsisteCodiceUnivoco(CodiceUnivoco, BindingT.Id);

        }
    }
}
