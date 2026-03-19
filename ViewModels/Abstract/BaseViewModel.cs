using ReactiveUI;
using SysNet;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
    {
        //Sequenza di operazioni
        //Eliminare il public override string UrlPathSegment => "";
        //Eliminare il static int deadentries;
        //Eliminare il public ReactiveCommand<Unit, Unit> LoadCommand { get; }

        // Implementazione di IRoutableViewModel
        public string UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        protected int _deadEntries;

        // Implementazione di IActivatableViewModel
        public ViewModelActivator Activator { get; } = new();

        
        public ReactiveCommand<Unit, Unit> LoadCommand { get; }

        public BaseViewModel(IScreen hostScreen, string urlPathSegment = null)
        {

            HostScreen = hostScreen;
            UrlPathSegment = urlPathSegment ?? this.GetType().Name;
            
            LoadCommand = ReactiveCommand.CreateFromTask(OnLoading);

            
#if DEBUG

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

#endif

            // Gestione dell'attivazione/disattivazione
            this.WhenActivated(disposables =>
            {

                Observable.Return(Unit.Default)
                .InvokeCommand(LoadCommand)
                .DisposeWith(disposables);

                
                this.LoadCommand.ThrownExceptions
                    .Subscribe(ex =>
                    {
                        // Qui gestisci l'errore (es. mostri una notifica o logghi)
                        Debug.WriteLine($"***** [VM] {this.GetType().Name} Errore durante il caricamento: {ex.Message}");
                   
                    })
                .DisposeWith(disposables);

                Disposable.Create(() => {

                    OnFinalDestruction();
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                                       $"#{_deadEntries} disposed *****");
#endif
                }).DisposeWith(disposables);
            });
        }

        
        protected virtual void OnFinalDestruction()
        {
            // Questo log conferma che la LOGICA di rimozione ha funzionato
            Debug.WriteLine($"[BASE] {this.GetType().Name}  " +
                            $"#{_deadEntries} rimosso correttamente dallo stack *****");
        }

#if DEBUG
        ~BaseViewModel()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{_deadEntries} DISTRUTTO *****");
        }
#endif

        protected abstract Task OnLoading();

        
    }
}
