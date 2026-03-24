using Avalonia.Collections;
using Models.Interfaces;
using ReactiveUI;
using SysNet;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace ViewModels
{
    public interface IGroupViewModelBase
    {
        ViewModelActivator Activator { get; }
        
        bool GroupFocus { get; set; }
        int IdValue { get; set; }
        int Param1 { get; set; }
        int Param2 { get; set; }

        Task CaricaDataSource(int id = 0);
        Task CaricaByModel(object model);

    }

    

    public abstract partial class GroupViewModel<T, W> : BaseViewModel, IGroupViewModelBase
                                           where T : class where W : class, IGroupQ<T>
    {
        public IGroupQ<T> Q { get; set; }

        public virtual int Param1 { get; set;}
        public virtual int Param2 { get; set; }

        public ReactiveCommand<Unit,Unit> AddCommand { get; }

        public GroupViewModel(IScreen host) : base(host)
        {
            AddCommand = ReactiveCommand.CreateFromTask(OnAdding);

            Q = Create<W>.Instance();

            this.WhenActivated(d =>
            {

                this.WhenAnyValue(x => x.GroupBindingT)
                    .Select(pass => pass is not null)
                    .BindTo(this, x => x.EnabledButton)
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.GroupBindingT)
                    .Subscribe(val =>
                    {
                        BindingT = val;
                        CheckNullBindingT = val == null;
                    })
                    .DisposeWith(d);

                
            });
        }

        protected override void OnFinalDestruction()
        {
            Q?.Dispose();
            Q = null;
            base.OnFinalDestruction();
        }
        
        protected async override Task OnLoading()
        {
  
            try
            {
                // 2. Caricamento dati (assicurati che CaricaDataSource sia atteso)
                await CaricaDataSource();

                // 3. Controllo sicurezza sui dati ricevuti
                if (DataSource != null && DataSource.Count > 0)
                {
                    GroupBindingT = DataSource[0];
                }
            }
            catch (Exception ex)
            {
                // Logga l'errore invece di ignorarlo, aiuta nel debug
                System.Diagnostics.Debug.WriteLine($"Errore in OnLoading: {ex.Message}");
            }
      
        }

        protected abstract Task OnAdding();

        //public void InitWithModel()
        //{
        //    Q = Create<W>.Instance();
        //    if (ActiveModel?.GetType() == typeof(int))
        //    {
        //        CaricaDataSource((int)ActiveModel);
        //    }
        //    else
        //    {
        //        CaricaByModel(ActiveModel);
        //    }

        //    try
        //    {

        //    }
        //    catch (NullReferenceException)
        //    {
        //        return;
        //    }

        //    if (DataSource.Count > 0)
        //    {
        //        GroupBindingT = DataSource[0];
        //    }
        //}

        private async Task OnFilter()
        {
            await CaricaDataSource();
            try
            {

            }
            catch (NullReferenceException)
            {
                return;
            }

            if (DataSource.Count > 0)
            {
                GroupBindingT = DataSource[0];
                GroupFocus = true;
            }

        }

        public async Task CaricaDataSource(int id = 0)
        {
            DataSource = await Q.Load(id);
            GroupedDataSource = new DataGridCollectionView(DataSource);
            GroupedDataSource.GroupDescriptions.Add(new DataGridPathGroupDescription("Titolo"));
            GroupFocus = true;
            IdIndex = id;
        }

        public async Task CaricaByModel(object model)
        {
            DataSource = await Q.LoadByModel(model);
            GroupedDataSource = new DataGridCollectionView(DataSource);
            GroupedDataSource.GroupDescriptions.Add(new DataGridPathGroupDescription("Titolo"));

            GroupFocus = true;
            SelectedIndex = 0;
        }


        #region Conditions

        private bool Can() => Q != null && GroupBindingT != null;

        #endregion


    }

    public partial class GroupViewModel<T, W>
    {
        #region DataSource

        private List<T> _datasource = [];
        public List<T> DataSource
        {
            get => _datasource;
            set => this.RaiseAndSetIfChanged(ref _datasource, value);
        }

        #endregion

        #region BindingT

        private T _mybindingt = Create<T>.Instance();

        public T BindingT
        {
            get => _mybindingt;
            set => this.RaiseAndSetIfChanged(ref _mybindingt, value);
        }

        #endregion

        #region CheckNullBindingT

        private bool _checknullbindingt = false;

        public bool CheckNullBindingT
        {
            get => _checknullbindingt;
            set => this.RaiseAndSetIfChanged(ref _checknullbindingt, value);
        }

        #endregion

        #region GroupBindingT

        private T _mygroupbindingt = null;

        public T GroupBindingT
        {
            get => _mygroupbindingt;
            set => this.RaiseAndSetIfChanged(ref _mygroupbindingt, value);
        }

        #endregion

        #region GroupFocus

        private bool _groupfocus = false;
        public bool GroupFocus
        {
            get => _groupfocus;
            set => this.RaiseAndSetIfChanged(ref _groupfocus, value);
        }

        #endregion

        #region IdValue

        private int _idvalue = 0;
        public int IdValue
        {
            get => _idvalue;
            set => this.RaiseAndSetIfChanged(ref _idvalue, value);
        }

        #endregion

        #region IdIndex

        private int _idindex = 0;
        public int IdIndex
        {
            get => _idindex;
            set => this.RaiseAndSetIfChanged(ref _idindex, value);
        }

        #endregion

        #region SelectedIndex

        private int _selectedindex = 0;
        public int SelectedIndex
        {
            get => _selectedindex;
            set => this.RaiseAndSetIfChanged(ref _selectedindex, value);
        }

        #endregion

        #region EnabledButton

        private bool _enabledbutton;
        public bool EnabledButton
        {
            get => _enabledbutton;
            set => this.RaiseAndSetIfChanged(ref _enabledbutton, value);
        }

        #endregion

        private DataGridCollectionView _groupedDataSource;
        public DataGridCollectionView GroupedDataSource
        {
            get => _groupedDataSource;
            set => this.RaiseAndSetIfChanged(ref _groupedDataSource, value);
        }
    }
}
