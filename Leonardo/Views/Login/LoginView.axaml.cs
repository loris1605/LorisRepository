using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
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

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel?.PasswordFocus
                    .RegisterHandler(interaction =>
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            PasswordBox.Focus();
                            PasswordBox.SelectAll();
                        });
                        interaction.SetOutput(Unit.Default);
                    })
                    .DisposeWith(d);

            // Esc Key Pressed
            Observable.FromEventPattern<EventHandler<KeyEventArgs>, KeyEventArgs>(
                        h => PasswordBox.KeyUp += h,
                        h => PasswordBox.KeyUp -= h)
            .Where(e => e.EventArgs.Key == Key.Escape)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ =>
            {
                if (Application.Current?.ApplicationLifetime
                    is IClassicDesktopStyleApplicationLifetime lifetime)
                {
                    lifetime.Shutdown();
                }
            })
            .DisposeWith(d);

            

            // Enter Key Pressed
            Observable.FromEventPattern<KeyEventArgs>(PasswordBox, nameof(PasswordBox.KeyUp))
                      .Where(e => e.EventArgs.Key == Key.Enter)
                      .Select(_ => Unit.Default)
                      .InvokeCommand(ViewModel?.EntraCommand)
                      .DisposeWith(d);

            #region TwoWay

            //Bind PasswordText to TextBox
            this.Bind(ViewModel,
                      vm => vm.PasswordText,
                      v => v.PasswordBox.Text)
                .DisposeWith(d);

            // Bind SelectedItem to ComboBox
            this.Bind(ViewModel,
                      vm => vm.BindingT,
                      v => v.OperatoreCombo.SelectedItem)
                .DisposeWith(d);

            #endregion

            #region OneWay

            this.OneWayBind(ViewModel,
                    vm => vm.EnabledEntra,
                    v => v.EntraButton.IsEnabled,
                    l => l)
            .DisposeWith(d);

            #endregion

            #region Commands

            this.BindCommand(ViewModel,
                             vm => vm.EntraCommand,
                             v => v.EntraButton).DisposeWith(d);

            #endregion

            //Evento DropDownClose sulla Combo
            Observable.FromEventPattern<EventHandler, EventArgs>(
                        h => OperatoreCombo.DropDownClosed += h,
                        h => OperatoreCombo.DropDownClosed -= h)
            .Subscribe(_ =>
            {
                // Rimando al dispatcher per sicurezza
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    PasswordBox.Focus();
                    PasswordBox.SelectAll();
                });
            })
            .DisposeWith(d);



            

            Disposable.Create(() => {
                this.DataContext = null;
                System.Diagnostics.Debug.WriteLine(">>> [VIEW] LoginView deattivata, DataContext rimosso.");
            }).DisposeWith(d);

        });
    }


}