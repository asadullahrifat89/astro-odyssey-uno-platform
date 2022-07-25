using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        #region Fields

        private Window _window;

        private static MainPage _mainPage;

        public static double Score { get; set; }

        public static Ship Ship { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeLogging();

            InitializeComponent();

            Uno.UI.ApplicationHelper.RequestedCustomTheme = "Dark";

#if HAS_UNO || NETFX_CORE
            Suspending += OnSuspending;
#endif
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
#if DEBUG
            Console.WriteLine(e.Message);
#endif
            e.Handled = true;
        }

        #endregion

        #region Properties

        public static Account Account { get; set; }

        #endregion

        #region Methods

        public static void PlaySound(string baseUrl, SoundType soundType)
        {
            _mainPage.PlaySound(baseUrl, soundType);
        }

        public static void StopSound()
        {
            _mainPage.StopSound();
        }

        public static void EnterFullScreen(bool value)
        {
#if !DEBUG
            var view = ApplicationView.GetForCurrentView();

            if (view is not null)
            {
                if (value)
                {
                    view.TryEnterFullScreenMode();
                }
                else
                {
                    view.ExitFullScreenMode();
                }
            }
#endif
        }

        public static void SetScore(double score)
        {
            Score = score;
        }

        public static double GetScore()
        {
            return Score;
        }

        public static void SetAccount()
        {
            _mainPage.SetAccount();
        }

        public static void NavigateToPage(Type page)
        {
            _mainPage.Navigate(page);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = Window.Current;
            _mainPage = _window.Content as MainPage;

            if (_mainPage == null)
            {
                _mainPage = new MainPage();
                _window.Content = _mainPage;
            }

            if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
            {
                _window.Activate();
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Configures global Uno Platform logging
        /// </summary>
        private static void InitializeLogging()
        {
#if DEBUG
            // Logging is disabled by default for release builds, as it incurs a significant
            // initialization cost from Microsoft.Extensions.Logging setup. If startup performance
            // is a concern for your application, keep this disabled. If you're running on web or 
            // desktop targets, you can use url or command line parameters to enable it.
            //
            // For more performance documentation: https://platform.uno/docs/articles/Uno-UI-Performance.html

            var factory = LoggerFactory.Create(builder =>
            {
#if __WASM__
                builder.AddProvider(new Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__
                builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
#elif NETFX_CORE
                builder.AddDebug();
#else
                builder.AddConsole();
#endif
                // Exclude logs below this level
                builder.SetMinimumLevel(LogLevel.Information);

                // Default filters for Uno Platform namespaces
                builder.AddFilter("Uno", LogLevel.Warning);
                builder.AddFilter("Windows", LogLevel.Warning);
                builder.AddFilter("Microsoft", LogLevel.Warning);


                // Generic Xaml events
                //builder.AddFilter("Microsoft.UI.Xaml", LogLevel.Debug);
                //builder.AddFilter("Microsoft.UI.Xaml.VisualStateGroup", LogLevel.Debug);
                //builder.AddFilter("Microsoft.UI.Xaml.StateTriggerBase", LogLevel.Debug);
                //builder.AddFilter("Microsoft.UI.Xaml.UIElement", LogLevel.Debug);
                //builder.AddFilter("Microsoft.UI.Xaml.FrameworkElement", LogLevel.Trace);

                // Layouter specific messages
                // builder.AddFilter("Microsoft.UI.Xaml.Controls", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.Controls.Layouter", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.Controls.Panel", LogLevel.Debug );

                builder.AddFilter("Windows.Storage", LogLevel.Debug);

                // Binding related messages
                //builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug);
                //builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug);

                // Binder memory references tracking
                builder.AddFilter("Uno.UI.DataBinding.BinderReferenceHolder", LogLevel.Debug);

                // RemoteControl and HotReload related
                builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Information);

                // Debug JS interop
                builder.AddFilter("Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug);
            });

            Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
            Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif

#endif
        }



        #endregion
    }
}
