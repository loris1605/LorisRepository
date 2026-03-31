using Models.Entity;
using Models.Repository;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public class SettoreGroupViewModel : GroupViewModel<SettoreMap, SettoreR>
    {
        public ReactiveCommand<Unit, Unit> OperatoriCommand { get; }
        public ReactiveCommand<Unit, Unit> PostazioniCommand { get; }
        public ReactiveCommand<Unit, Unit> TariffeCommand { get; }

        public SettoreGroupViewModel(IScreen host) : base(host)
        {
            var canDel = this.WhenAnyValue(
                           x => x.GroupBindingT.CodiceListino, // Monitora la proprietà specifica
                           x => x.GroupBindingT.Id,             // Monitora l'Id
                           (permesso, id) =>
                               this.GroupBindingT != null &&    // Check di sicurezza sull'oggetto padre
                               permesso == 0 &&
                               id != -1);

            OperatoriCommand = ReactiveCommand.CreateFromTask(OnGoToOperatori);
            PostazioniCommand = ReactiveCommand.CreateFromTask(OnGoToPostazioni);
            TariffeCommand = ReactiveCommand.CreateFromTask(OnGoToTariffe);

            DelCommand = ReactiveCommand.CreateFromTask(OnDeleting, canDel);
            
            this.WhenActivated(d =>
            {

                
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

                OperatoriCommand.DisposeWith(d);
                PostazioniCommand.DisposeWith(d);
                TariffeCommand.DisposeWith(d);
            });


        }

        protected override async Task OnDeleting()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new SettoreDelViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        protected override async Task OnUpdating()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new SettoreUpdViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

        protected override async Task OnAdding()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new SettoreAddViewModel(configurazioneHost));
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

        private async Task OnGoToPostazioni()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                await configurazioneHost.ConfigurazioneRouter
                    .NavigateAndReset
                    .Execute(new PostazioneGroupViewModel(configurazioneHost));
            }
        }

        private async Task OnGoToTariffe()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                await configurazioneHost.ConfigurazioneRouter
                    .NavigateAndReset
                    .Execute(new TariffaGroupViewModel(configurazioneHost));
            }
        }
    }
}
