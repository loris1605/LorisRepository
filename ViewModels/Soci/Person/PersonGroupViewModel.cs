using Models.Entity;
using Models.Repository;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public class PersonGroupViewModel : GroupViewModel<PersonMap, PersonR>
    {

        public ReactiveCommand<Unit, Unit> AddCodiceSocioCommand { get; }
        public ReactiveCommand<Unit, Unit> DelCodiceSocioCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> UpdCodiceSocioCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> AddTesseraCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> DelTesseraCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> UpdTesseraCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> PersonSearchCommand { get; }

        public PersonGroupViewModel(IScreen host) : base(host)
        {
            var canDel = this.WhenAnyValue(
                             x => x.GroupBindingT.CodiceSocio,
                             (codice) => codice == 0);

            //ridefiniamo il DelCommand aggiungendo il canDel
            DelCommand = ReactiveCommand.CreateFromTask(OnDeleting, canDel);

            AddCodiceSocioCommand = ReactiveCommand.CreateFromTask(OnAddCodiceSocio);
            PersonSearchCommand = ReactiveCommand.CreateFromTask(OnPersonSearch);

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.GroupBindingT)
                    .Select(selection => selection != null)
                    .BindTo(this, x => x.EnabledButton)
                    .DisposeWith(d); // Usa l'activator della base


                var canSocioUpd = this.WhenAnyValue(
                            x => x.GroupBindingT.CodiceSocio,
                            (codice) => codice != 0);

                var canTesseraAdd = this.WhenAnyValue(
                                x => x.GroupBindingT,
                                x => x.GroupBindingT.CodiceSocio,
                                (bt, codice) => codice != 0 && bt is not null);

                var canTesseraDel = this.WhenAnyValue(
                                x => x.GroupBindingT,
                                x => x.GroupBindingT.NumeroTessera,
                                (bt, codice) => codice != "" && bt is not null);

                var canSocioDel = this.WhenAnyValue(
                            x => x.GroupBindingT.CodiceSocio,
                            x => x.GroupBindingT.NumeroTessera,
                            (codice, tessera) => codice != 0 && string.IsNullOrWhiteSpace(tessera));
                
               
                AddCodiceSocioCommand.DisposeWith(d);
                DelCodiceSocioCommand = ReactiveCommand.CreateFromTask(OnDelCodiceSocio, canSocioDel)
                .DisposeWith(d);
                UpdCodiceSocioCommand = ReactiveCommand.CreateFromTask(OnUpdCodiceSocio, canSocioUpd)
                .DisposeWith(d);
                AddTesseraCommand = ReactiveCommand.CreateFromTask(OnAddTessera, canTesseraAdd)
                .DisposeWith(d);
                DelTesseraCommand = ReactiveCommand.CreateFromTask(OnDelTessera, canTesseraDel)
                .DisposeWith(d);
                UpdTesseraCommand = ReactiveCommand.CreateFromTask(OnUpdTessera, canTesseraDel)
                .DisposeWith(d);
                
            });

        }
        
        public override int Param1 => BindingT is null ? 0 : BindingT.CodiceSocio;
        public override int Param2 => BindingT is null ? 0 : BindingT.CodiceTessera;
        public string NumeroSocio => BindingT is null ? "" : BindingT.NumeroSocio;
        public string NumeroTessera => BindingT is null ? "" : BindingT.NumeroTessera;
        public int CodiceSocio => BindingT is null ? 0 : BindingT.CodiceSocio;
        public int CodiceTessera => BindingT is null ? 0 : BindingT.CodiceTessera;
        public int Scadenza => BindingT is null ? 0 : BindingT.Scadenza;

        protected override async Task OnAdding()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter.Navigate.Execute(new PersonAddViewModel(sociHost));
            }
        }

        protected override async Task OnUpdating()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter.Navigate.Execute(new PersonUpdViewModel(sociHost, GroupBindingT.Id));
            }
        }

        protected override async Task OnDeleting()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter.Navigate.Execute(new PersonDelViewModel(sociHost, GroupBindingT.Id));
            }
        }

        private async Task OnAddCodiceSocio()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new CodiceSocioAddViewModel(sociHost, GroupBindingT.Id));
            }
            await Task.CompletedTask;
        }

        private async Task OnDelCodiceSocio()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new CodiceSocioDelViewModel(sociHost, 
                                                                    GroupBindingT.CodiceSocio,
                                                                    GroupBindingT.Id));
            }
            await Task.CompletedTask;
        }

        private async Task OnUpdCodiceSocio()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new CodiceSocioUpdViewModel(sociHost,
                                                                    GroupBindingT.CodiceSocio,
                                                                    GroupBindingT.Id));
            }
            await Task.CompletedTask;
        }

        private async Task OnAddTessera()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new TesseraAddViewModel(sociHost, 
                                        GroupBindingT.Id, GroupBindingT.CodiceSocio));
            }
            await Task.CompletedTask;
        }

        private async Task OnDelTessera()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new TesseraDelViewModel(sociHost,
                                        GroupBindingT.CodiceTessera, GroupBindingT.Id ));
            }
            await Task.CompletedTask;
        }

        private async Task OnUpdTessera()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter
                              .Navigate
                              .Execute(new TesseraUpdViewModel(sociHost,
                                        GroupBindingT.CodiceTessera, GroupBindingT.Id));
            }
            await Task.CompletedTask;
        }

        private async Task OnPersonSearch()
        {
            if (HostScreen is ISociScreen sociHost)
            {
                sociHost.GroupEnabled = false;
                await sociHost.SociInputRouter.Navigate.Execute(new PersonSearchViewModel(sociHost));
            }
        }

        
    }
}
