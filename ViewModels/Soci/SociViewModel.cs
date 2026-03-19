using Models;
using ReactiveUI;
using SysNet;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public interface ISociScreen : IScreen
    {
        RoutingState SociRouter { get; }
        RoutingState SociInputRouter { get; }
        bool GroupEnabled { get; set; }

        void AggiornaGrid(object model);
        void AggiornaGrid(int id);
    }
    public partial class SociViewModel : BaseViewModel, ISociScreen
    {
        private static int classCount;

        public RoutingState SociRouter { get; } = new RoutingState();
        public RoutingState SociInputRouter { get; } = new RoutingState();
        public RoutingState Router => SociRouter;

        private readonly ServiceM service = Create<ServiceM>.Instance();
               
        public ReactiveCommand<Unit, IRoutableViewModel> EsciCommand { get; }
          

        public SociViewModel(IScreen host) : base(host)
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} " +
                                               $"caricato *****");

            base._deadEntries = classCount;

            EsciCommand = ReactiveCommand.CreateFromObservable(() =>
                        HostScreen.Router.NavigateAndReset.Execute(new MenuViewModel(HostScreen)));

            //this.WhenActivated(d =>
            //{

            //    Disposable.Create(() => {
            //        System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} disposed *****");
            //    }).DisposeWith(d);

            //});
        }

        protected override void OnFinalDestruction()
        {
            //Q?.Dispose();
            SociRouter.NavigationStack.Clear();
            SociInputRouter.NavigationStack.Clear();
        }

        protected override async Task OnLoading()
        {
            await SociRouter.NavigateAndReset.Execute(new PersonGroupViewModel(this));
                
        }

        

        public void AggiornaGrid(object model)
        {
            //SociRouter.NavigateAndReset.Execute(new PersonGroupViewModel(this, model));
        }

        public void AggiornaGrid(int id)
        {
            if (SociRouter.GetCurrentViewModel() is IGroupViewModelBase groupVm)
            {
                // Passiamo l'ID al metodo di caricamento della lista
                groupVm.CaricaDataSource(id);

                // Se hai un comando di ricarica nel GroupViewModel:
                // groupVm.LoadCommand.Execute().Subscribe();
            }
        }
    }

    public partial class SociViewModel
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
