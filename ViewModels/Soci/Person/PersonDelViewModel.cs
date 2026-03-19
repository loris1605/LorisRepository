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
    public class PersonDelViewModel : PersonInputBase
    {
        private PersonR Q { get; set; }
        private readonly int _idDaModificare;
        
        public PersonDelViewModel(IScreen host, int idperson) : base(host)
        {
            _idDaModificare = idperson;
            Titolo = "Elimina Socio";
            FieldsEnabled = false;
            FieldsVisibile = false; ;
            Q = Create<PersonR>.Instance();

            
            SaveCommand = ReactiveCommand.CreateFromTask(OnSaving);

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
            
            await OnEscFocus();
        }

        

        protected async override Task OnSaving()
        {
            
            if (!await Q.Del(BindingT))
            {
                InfoLabel = "Errore Db eliminazione person";
                await OnEscFocus();
                return;
            }
            OnBack(-100);
        }

        
    }
}
