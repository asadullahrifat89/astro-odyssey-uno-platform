using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Frame = Microsoft.UI.Xaml.Controls.Frame;
#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace AstroOdyssey
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        #region Fields

        private static Window _window;
        private readonly SystemNavigationManager _systemNavigationManager;
        private readonly List<Type> _goBackNotAllowedToPages;
        private readonly List<(Type IfGoingBackTo, Type RouteTo)> _goBackPageRoutes;
        private static string _baseUrl;

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
            Container = ConfigureDependencyInjection();

            Uno.UI.ApplicationHelper.RequestedCustomTheme = "Dark";

#if HAS_UNO || NETFX_CORE
            Suspending += OnSuspending;
#endif
            UnhandledException += App_UnhandledException;

            Uno.UI.FeatureConfiguration.Page.IsPoolingEnabled = true;

            _systemNavigationManager = SystemNavigationManager.GetForCurrentView();

            _goBackNotAllowedToPages = new List<Type>() { typeof(GamePlayPage) };
            _goBackPageRoutes = new List<(Type IfGoingBackTo, Type RouteTo)>() { (IfGoingBackTo: typeof(GameOverPage), RouteTo: typeof(GameStartPage)) };

            CurrentCulture = "en";
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

        public static IServiceProvider Container { get; set; }

        public static PlayerCredentials AuthCredentials { get; set; }

        public static GameProfile GameProfile { get; set; }

        public static AuthToken AuthToken { get; set; }

        public static PlayerScore GameScore { get; set; }

        public static bool GameScoreSubmissionPending { get; set; }

        public static PlayerShip Ship { get; set; }

        public static string CurrentCulture { get; set; }

        public static bool HasUserLoggedIn => GameProfile is not null && GameProfile.User is not null && !GameProfile.User.UserId.IsNullOrBlank();

        #endregion

        #region Events

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
            _window = new Window();
            _window.Activate();
#else
            _window = Window.Current;
#endif
            var rootFrame = _window.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.Background = SolidColorBrushHelper.FromARGB(255, 7, 10, 37); // App.Current.Resources["ApplicationPageBackgroundThemeBrush"] as SolidColorBrush;

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.IsNavigationStackEnabled = true;

                if (args.UWPLaunchActivatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: App: Load state from previously suspended application
                }

                // Place the frame in the current Window
                _window.Content = rootFrame;
            }

#if !(NET6_0_OR_GREATER && WINDOWS)
            if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
#endif
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(GameStartPage), args.Arguments);
                }

                // Ensure the current window is active
                _window.Activate();
            }

            _systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            _systemNavigationManager.BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = _window.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                var backPage = rootFrame.BackStack.LastOrDefault();

                if (_goBackNotAllowedToPages.Contains(backPage.SourcePageType))
                    return;

                if (_goBackPageRoutes.Any(x => x.IfGoingBackTo == backPage.SourcePageType))
                {
                    var reroute = _goBackPageRoutes.FirstOrDefault(x => x.IfGoingBackTo == backPage.SourcePageType).RouteTo;

                    rootFrame.Navigate(reroute);
                    return;
                }

                rootFrame.GoBack();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
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
            //TODO: App: Save application state and stop any background activity
            deferral.Complete();
        }

        #endregion

        #region Methods

        public static void SetScore(PlayerScore gameScore)
        {
            GameScore = gameScore;
        }

        public static void SetIsBusy(bool isBusy)
        {
            var rootFrame = _window.Content as Frame;
            rootFrame.IsEnabled = !isBusy;
            rootFrame.Opacity = isBusy ? 0.6 : 1;
        }

        /// <summary>
        /// Toggle fullscreen mode.
        /// </summary>
        /// <param name="value"></param>
        public static void EnterFullScreen(bool value)
        {
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
        }

        /// <summary>
        /// Navigate to provided page.
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="parameter"></param>
        public static void NavigateToPage(Type pageType, object parameter = null)
        {
            var rootFrame = _window.Content as Frame;
            rootFrame.Navigate(pageType, parameter);
        }

        /// <summary>
        /// Get base url for the app.
        /// </summary>
        public static string GetBaseUrl()
        {
            if (_baseUrl.IsNullOrBlank())
            {
                var indexUrl = Uno.Foundation.WebAssemblyRuntime.InvokeJS("window.location.href;");
                var appPackage = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");
                _baseUrl = $"{indexUrl}{appPackage}";

#if DEBUG
                Console.WriteLine(_baseUrl);
#endif 
            }
            return _baseUrl;
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

        private IServiceProvider ConfigureDependencyInjection()
        {
            // Create new service collection which generates the IServiceProvider
            var serviceCollection = new ServiceCollection();

            // Register the MessageService with the container
            serviceCollection.AddHttpService(lifeTime: 300, retryCount: 3, retryWait: 1);

            serviceCollection.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();
            serviceCollection.AddSingleton<IGameApiHelper, GameApiHelper>();
            serviceCollection.AddSingleton<IAudioHelper, AudioHelper>();
            serviceCollection.AddSingleton<ILocalizationHelper, LocalizationHelper>();
            serviceCollection.AddSingleton<ICacheHelper, CacheHelper>();
            serviceCollection.AddSingleton<IPaginationHelper, PaginationHelper>();

            serviceCollection.AddFactories();

            // Build the IServiceProvider and return it
            return serviceCollection.BuildServiceProvider();
        }

        #endregion
    }
}
