using Microsoft.Identity.Client;
using Models.Entity;
using Models.Repository;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public class OperatoreGroupViewModel : GroupViewModel<OperatoreMap, OperatoreR>
    {
        public ReactiveCommand<Unit, Unit> FilterCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdCommand { get; }
        public ReactiveCommand<Unit, Unit> DelCommand { get; private set; }

        public OperatoreGroupViewModel(IScreen host) : base(host)
        {
            FilterCommand = ReactiveCommand.CreateFromTask(OnCancelFilter);
            UpdCommand = ReactiveCommand.CreateFromTask(OnUpd);
            
            this.WhenActivated(d =>
            {

                var canDel = this.WhenAnyValue(
                            x => x.GroupBindingT.CodicePermesso,
                            (codice) => codice == 0);

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

                DelCommand = ReactiveCommand.CreateFromTask(OnDel, canDel).DisposeWith(d);

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


            });

            

        }

        private async Task OnDel()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new OperatoreDelViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        protected override async Task OnAdding()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new OperatoreAddViewModel(configurazioneHost));
            }
        }

        private async Task OnUpd()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new OperatoreUpdViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        private async Task OnCancelFilter()
        {
            await OnLoading();

        }
    }
}
