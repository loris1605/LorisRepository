using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace ViewModels
{

    public interface IConfigurazioneScreen : IScreen
    {
        RoutingState ConfigurazioneRouter { get; }
        RoutingState ConfigurazioneInputRouter { get; }
        bool GroupEnabled { get; set; }

        void AggiornaGrid(int id);
    }


    public partial class ConfigurazioneViewModel : BaseViewModel, IConfigurazioneScreen
    {
        public RoutingState ConfigurazioneRouter { get; } = new RoutingState();
        public RoutingState ConfigurazioneInputRouter { get; } = new RoutingState();
        public RoutingState Router => ConfigurazioneRouter;

        public ReactiveCommand<Unit, IRoutableViewModel> EsciCommand { get; }

        public ConfigurazioneViewModel(IScreen host) : base(host)
        {
            EsciCommand = ReactiveCommand.CreateFromObservable(() =>
                        HostScreen.Router.NavigateAndReset.Execute(new MenuViewModel(HostScreen)));
            
        }

        protected override void OnFinalDestruction()
        {
            ConfigurazioneRouter.NavigationStack.Clear();
            ConfigurazioneInputRouter.NavigationStack.Clear();
        }

        protected override async Task OnLoading()
        {
            await ConfigurazioneRouter.NavigateAndReset.Execute(new OperatoreGroupViewModel(this));
            
        }
         
        public void AggiornaGrid(int id)
        {
            if (ConfigurazioneRouter.GetCurrentViewModel() is IGroupViewModelBase groupVm)
            {
                // Passiamo l'ID al metodo di caricamento della lista
                groupVm.CaricaDataSource(id);

                // Se hai un comando di ricarica nel GroupViewModel:
                // groupVm.LoadCommand.Execute().Subscribe();
            }
        }
    }

    public partial class ConfigurazioneViewModel
    {
        #region GroupEnabled

        private bool _groupenabled = true;
        public bool GroupEnabled
        {
            get => _groupenabled;
            set => this.RaiseAndSetIfChanged(ref _groupenabled, value);
        }

        #endregion
    }
}
