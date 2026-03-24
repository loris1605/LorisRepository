using ReactiveUI;
using System;
using System.Collections.Generic;

namespace Leonardo
{

    public class ConventionViewLocator : IViewLocator
    {
        // Aggiungi questo campo privato all'interno della classe ConventionViewLocator
        private readonly System.Runtime.CompilerServices.ConditionalWeakTable<object, IViewFor> _viewCache = new();

        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            // 1. Controllo se il ViewModel è nullo
            if (viewModel is null) return null;

            // 2. CONTROLLO CACHE: Se abbiamo già creato la View per questo specifico ViewModel, la restituiamo.
            // Usiamo ConditionalWeakTable così se il ViewModel viene rimosso dalla memoria, 
            // anche la View viene rimossa automaticamente (niente memory leak).
            if (_viewCache.TryGetValue(viewModel, out var cachedView))
            {
                return cachedView;
            }

            // 3. Ottieni il nome completo con controllo null
            string? viewModelName = viewModel.GetType().FullName;
            if (string.IsNullOrEmpty(viewModelName)) return null;

            // 4. Trasforma il namespace e il nome della classe
            string viewName = viewModelName
                .Replace("ViewModels", "Leonardo")
                .Replace("ViewModel", "View");

            // 5. Ottieni l'assembly dal tipo MainWindow
            var viewAssembly = typeof(MainWindow).Assembly;

            // 6. Cerca il tipo della View
            var viewType = viewAssembly.GetType(viewName);
            if (viewType is null) return null;

            // 7. Istanzia la View in modo sicuro
            try
            {
                var instance = Activator.CreateInstance(viewType) as IViewFor;

                if (instance != null)
                {
                    // 8. SALVA IN CACHE: Collega questa istanza della View a questo ViewModel
                    _viewCache.Add(viewModel, instance);
                }

                return instance;
            }
            catch
            {
                return null;
            }
        }

    }


}
