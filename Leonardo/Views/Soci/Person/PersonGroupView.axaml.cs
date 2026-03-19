using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ViewModels;

namespace Leonardo;

public partial class PersonGroupView : ReactiveUserControl<PersonGroupViewModel>
{
    public PersonGroupView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (SociDataGrid != null)
            {
                SociDataGrid.LoadingRowGroup += OnLoadingRowGroup;

                Disposable.Create(() => SociDataGrid.LoadingRowGroup -= OnLoadingRowGroup)
                    .DisposeWith(d);
            }
           
        
            // Esc Key Pressed


            // Enter Key Pressed

            #region TwoWay

            //Bind PasswordText to TextBox


            #endregion

            #region OneWay

            this.OneWayBind(ViewModel,
                    vm => vm.EnabledButton,
                    v => v.UpdButton.IsEnabled,
                    l => l)
            .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.EnabledButton,
                    v => v.DelButton.IsEnabled,
                    l => l)
            .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.EnabledButton,
                    v => v.CodiceSocio.IsEnabled,
                    l => l)
            .DisposeWith(d);

            #endregion

            #region Commands

            this.BindCommand(ViewModel,
                vm => vm.AddCommand,
                v => v.AddButton) 
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.PersonUpdCommand,
                v => v.UpdButton) 
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.PersonDelCommand,
                v => v.DelButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.FilterCommand,
                v => v.CancelFilterButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.AddCodiceSocioCommand,
                v => v.AddSocioButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.DelCodiceSocioCommand,
                v => v.DelSocioButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.UpdCodiceSocioCommand,
                v => v.UpdSocioButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.AddTesseraCommand,
                v => v.AddTesseraButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.DelTesseraCommand,
                v => v.DelTesseraButton)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.UpdTesseraCommand,
                v => v.UpdTesseraButton)
                .DisposeWith(d);

            #endregion

            Disposable.Create(() => {
                this.DataContext = null;
                System.Diagnostics.Debug.WriteLine(">>> [VIEW] PersonGroupView deattivata, DataContext rimosso.");
            }).DisposeWith(d);

        });
    }

    private void OnLoadingRowGroup(object? sender, DataGridRowGroupHeaderEventArgs e)
    {
        if (sender is DataGrid grid && e.RowGroupHeader.DataContext is DataGridCollectionViewGroup group)
        {
            // In Avalonia 11 si usa ExpandRowGroup con 'false' per chiudere
            // Il secondo parametro 'false' indica "NON espandere" -> quindi CHIUDI
            Dispatcher.UIThread.Post(() =>
            {
                grid.CollapseRowGroup(group, true);
            }, DispatcherPriority.Render);
        }
    }


}