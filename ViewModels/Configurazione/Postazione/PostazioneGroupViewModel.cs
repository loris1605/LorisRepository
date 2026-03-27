using Models.Entity;
using Models.Repository;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public class PostazioneGroupViewModel : GroupViewModel<PostazioneMap, PostazioneR>
    {
        public ReactiveCommand<Unit, Unit> UpdCommand { get; }
        public ReactiveCommand<Unit, Unit> DelCommand { get; }
        public ReactiveCommand<Unit, Unit> OperatoriCommand { get; }
        public ReactiveCommand<Unit, Unit> FilterCommand { get; }

        public PostazioneGroupViewModel(IScreen host) : base(host)
        {
            FilterCommand = ReactiveCommand.CreateFromTask(OnCancelFilter);
            UpdCommand = ReactiveCommand.CreateFromTask(OnUpd);
            OperatoriCommand = ReactiveCommand.CreateFromTask(OnGoToOperatori);

            DelCommand = ReactiveCommand.CreateFromTask(
                OnDel,
                this.WhenAnyValue(
                           x => x.GroupBindingT.CodiceReparto, // Monitora la proprietà specifica
                           x => x.GroupBindingT.Id,             // Monitora l'Id
                           (permesso, id) =>
                               this.GroupBindingT != null &&    // Check di sicurezza sull'oggetto padre
                               permesso == 0 &&
                               id != -1));

            this.WhenActivated(d =>
            {

                var canDel = this.WhenAnyValue(
                           x => x.GroupBindingT.CodiceReparto, // Monitora la proprietà specifica
                           x => x.GroupBindingT.Id,             // Monitora l'Id
                           (permesso, id) =>
                               this.GroupBindingT != null &&    // Check di sicurezza sull'oggetto padre
                               permesso == 0 &&
                               id != -1);

                //var canSocioUpd = this.WhenAnyValue(
                //            x => x.GroupBindingT.CodiceSocio,
                //            (codice) => codice != 0);

                //var canTesseraAdd = this.WhenAnyValue(
                //                x => x.GroupBindingT,
                //                x => x.GroupBindingT.CodiceSocio,
                //                (bt, codice) => codice != 0 && bt is not null);

                //var canTesseraDel = this.WhenAnyValue(
                //                x => x.GroupBindingT,
                //                x => x.GroupBindingT.NumeroTessera,
                //                (bt, codice) => codice != "" && bt is not null);

                //var canSocioDel = this.WhenAnyValue(
                //            x => x.GroupBindingT.CodiceSocio,
                //            x => x.GroupBindingT.NumeroTessera,
                //            (codice, tessera) => codice != 0 && string.IsNullOrWhiteSpace(tessera));

                //DelCommand = ReactiveCommand.CreateFromTask(OnDel, canDel).DisposeWith(d);

                //DelCodiceSocioCommand = ReactiveCommand.CreateFromTask(OnDelCodiceSocio, canSocioDel)
                //.DisposeWith(d);
                //UpdCodiceSocioCommand = ReactiveCommand.CreateFromTask(OnUpdCodiceSocio, canSocioUpd)
                //.DisposeWith(d);
                //AddTesseraCommand = ReactiveCommand.CreateFromTask(OnAddTessera, canTesseraAdd)
                //.DisposeWith(d);
                //DelTesseraCommand = ReactiveCommand.CreateFromTask(OnDelTessera, canTesseraDel)
                //.DisposeWith(d);
                //UpdTesseraCommand = ReactiveCommand.CreateFromTask(OnUpdTessera, canTesseraDel)
                //.DisposeWith(d);

                UpdCommand.DisposeWith(d);
                OperatoriCommand.DisposeWith(d);
                DelCommand.DisposeWith(d);
                FilterCommand.DisposeWith(d);
                
            });



        }

        private async Task OnDel()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new PostazioneDelViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        protected override async Task OnAdding()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new PostazioneAddViewModel(configurazioneHost));
            }
        }

        private async Task OnUpd()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new PostazioneUpdViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        private async Task OnCancelFilter() => await OnLoading();
       

        private async Task OnGoToOperatori()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                await configurazioneHost.ConfigurazioneRouter
                    .NavigateAndReset
                    .Execute(new OperatoreGroupViewModel(configurazioneHost));
            }
        }
    }
}
