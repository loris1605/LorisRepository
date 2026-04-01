using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;
using ViewModels;

namespace Leonardo
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var culture = new System.Globalization.CultureInfo("it-IT");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            RegisterViews();

            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void RegisterViews()
        {
            Locator.CurrentMutable.Register(() =>
                        new MainWindow(), typeof(IViewFor<MainWindowViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new ConnectionView(), typeof(IViewFor<ConnectionViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new LoginView(), typeof(IViewFor<LoginViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new MenuView(), typeof(IViewFor<MenuViewModel>));

            #region Soci

            Locator.CurrentMutable.Register(() =>
                            new SociView(), typeof(IViewFor<SociViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PersonGroupView(), typeof(IViewFor<PersonGroupViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PersonInputView(), typeof(IViewFor<PersonAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PersonInputView(), typeof(IViewFor<PersonUpdViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PersonInputView(), typeof(IViewFor<PersonDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PersonSearchView(), typeof(IViewFor<PersonSearchViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SocioInputView(), typeof(IViewFor<CodiceSocioAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SocioInputView(), typeof(IViewFor<CodiceSocioDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SocioInputView(), typeof(IViewFor<CodiceSocioUpdViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TesseraInputView(), typeof(IViewFor<TesseraAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TesseraInputView(), typeof(IViewFor<TesseraDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TesseraInputView(), typeof(IViewFor<TesseraUpdViewModel>));

#endregion

            #region Configurazione

            Locator.CurrentMutable.Register(() =>
                        new ConfigurazioneView(), typeof(IViewFor<ConfigurazioneViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new OperatoreGroupView(), typeof(IViewFor<OperatoreGroupViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new OperatoreInputView(), typeof(IViewFor<OperatoreAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new OperatoreInputView(), typeof(IViewFor<OperatoreUpdViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new OperatoreInputView(), typeof(IViewFor<OperatoreDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PermessiView(), typeof(IViewFor<PermessiViewModel>));



            Locator.CurrentMutable.Register(() =>
                        new PostazioneGroupView(), typeof(IViewFor<PostazioneGroupViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PostazioneInputView(), typeof(IViewFor<PostazioneAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PostazioneInputView(), typeof(IViewFor<PostazioneDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new PostazioneInputView(), typeof(IViewFor<PostazioneUpdViewModel>));


            Locator.CurrentMutable.Register(() =>
                        new SettoreGroupView(), typeof(IViewFor<SettoreGroupViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SettoreInputView(), typeof(IViewFor<SettoreAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SettoreInputView(), typeof(IViewFor<SettoreDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new SettoreInputView(), typeof(IViewFor<SettoreUpdViewModel>));


            Locator.CurrentMutable.Register(() =>
                        new TariffaGroupView(), typeof(IViewFor<TariffaGroupViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TariffaInputView(), typeof(IViewFor<TariffaAddViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TariffaInputView(), typeof(IViewFor<TariffaDelViewModel>));

            Locator.CurrentMutable.Register(() =>
                        new TariffaInputView(), typeof(IViewFor<TariffaUpdViewModel>));


            #endregion



        }

        
    }
}