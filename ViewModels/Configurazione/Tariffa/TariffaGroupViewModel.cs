using Models.Entity;
using Models.Repository;
using ReactiveUI;
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
    public class TariffaGroupViewModel : GroupViewModel<TariffaMap, TariffaR>
    {
        public ReactiveCommand<Unit, Unit> PostazioniCommand { get; }
        public ReactiveCommand<Unit, Unit> SettoriCommand { get; }
        public ReactiveCommand<Unit, Unit> OperatoriCommand { get; }

        public TariffaGroupViewModel(IScreen host) : base(host)
        {
            //var canDel = this.WhenAnyValue(
            //               x => x.GroupBindingT.PrezzoTariffa, // Monitora la proprietà specifica
            //               x => x.GroupBindingT.Id,             // Monitora l'Id
            //               (prezzo, id) =>
            //                   this.GroupBindingT != null &&    // Check di sicurezza sull'oggetto padre
            //                   prezzo == decimal.Zero &&
            //                   id != -1);

            PostazioniCommand = ReactiveCommand.CreateFromTask(OnGoToPostazioni);
            SettoriCommand = ReactiveCommand.CreateFromTask(OnGoToSettori);
            OperatoriCommand = ReactiveCommand.CreateFromTask(OnGoToOperatori);
            DelCommand = ReactiveCommand.CreateFromTask(OnDeleting);
            
            this.WhenActivated(d =>
            {
                PostazioniCommand.DisposeWith(d);
                SettoriCommand.DisposeWith(d);
                OperatoriCommand.DisposeWith(d);
            });
        }

        protected override async Task OnDeleting()
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

        protected override async Task OnUpdating()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                configurazioneHost.GroupEnabled = false;
                await configurazioneHost.ConfigurazioneInputRouter
                    .Navigate
                    .Execute(new OperatoreUpdViewModel(configurazioneHost, GroupBindingT.Id));
            }
        }

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

        private async Task OnGoToSettori()
        {
            if (HostScreen is IConfigurazioneScreen configurazioneHost)
            {
                await configurazioneHost.ConfigurazioneRouter
                    .NavigateAndReset
                    .Execute(new SettoreGroupViewModel(configurazioneHost));
            }
        }
    }
}
