using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;

namespace SpaceShooterGame
{
    public sealed partial class GameStartPage : Page
    {
        #region Fields

        private PeriodicTimer _gameViewTimer;
        private readonly TimeSpan _frameTime = TimeSpan.FromMilliseconds(Constants.DEFAULT_FRAME_TIME);

        private readonly Random _random = new();

        private double _windowHeight, _windowWidth;
        private double _scale;

        private readonly int _gameSpeed = 5;

        private readonly IBackendService _backendService;

        #endregion

        #region Ctor

        public GameStartPage()
        {
            InitializeComponent();
            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            _windowHeight = Window.Current.Bounds.Height;
            _windowWidth = Window.Current.Bounds.Width;

            Loaded += GameStartPage_Loaded;
            Unloaded += GameStartPage_Unloaded;
        }

        #endregion

        #region Events

        #region Page

        private async void GameStartPage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;
            StartAnimation();

            await AppSettingsHelper.LoadAppSettings();

            LocalizationHelper.CheckLocalizationCache();
            await LocalizationHelper.LoadLocalizationKeys(() =>
            {
                this.SetLocalization();

                AudioHelper.LoadGameSounds(() =>
                {
                    AudioHelper.StopSound();
                    AudioHelper.PlaySound(SoundType.INTRO);
                    AssetHelper.PreloadAssets(progressBar: ProgressBar, messageBlock: ProgressBarMessageBlock, completed: () =>
                    {
                        GameStartPage_PlayButton.IsEnabled = true;
                    });

                    PopulateGameViews();
                });
            });

            if (await GetCompanyBrand())
                await CheckUserSession();
        }

        private void GameStartPage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= GamePage_SizeChanged;
            StopAnimation();
        }

        private void GamePage_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            _windowWidth = args.NewSize.Width;
            _windowHeight = args.NewSize.Height;

            SetViewSize();

#if DEBUG
            Console.WriteLine($"WINDOWS SIZE: {_windowWidth}x{_windowHeight}");
#endif
        }

        #endregion

        #region Button

        private void HowToPlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameInstructionsPage));
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShipSelectionPage));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameSignupPage));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        private void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLeaderboardPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            PerformLogout();
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                AudioHelper.PlaySound(SoundType.MENU_SELECT);

                LocalizationHelper.CurrentCulture = tag;

                if (CookieHelper.IsCookieAccepted())
                    LocalizationHelper.SaveLocalizationCache(tag);

                this.SetLocalization();
            }
        }

        private void GameStartPage_CookieAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            CookieHelper.SetCookieAccepted();
            CookieToast.Visibility = Visibility.Collapsed;
            LocalizationHelper.SaveLocalizationCache(LocalizationHelper.CurrentCulture);
        }

        private void GameStartPage_CookieDeclineButton_Click(object sender, RoutedEventArgs e)
        {
            CookieHelper.SetCookieDeclined();
            CookieToast.Visibility = Visibility.Collapsed;
        }

        #endregion

        #endregion

        #region Methods

        #region Logic

        private async Task<bool> GetCompanyBrand()
        {
            // if company is not already fetched, fetch it
            if (CompanyHelper.Company is null)
            {
                (bool IsSuccess, string Message, Company Company) = await _backendService.GetCompanyBrand();

                if (!IsSuccess)
                {
                    var error = Message;
                    this.ShowError(error);
                    return false;
                }

                if (Company is not null)
                    CompanyHelper.Company = Company;
            }

            if (CompanyHelper.Company is not null)
            {
                if (!CompanyHelper.Company.WebSiteUrl.IsNullOrBlank())
                    BrandButton.NavigateUri = new Uri(CompanyHelper.Company.WebSiteUrl);

                if (!CookieHelper.IsCookieAccepted() && !CompanyHelper.Company.DefaultLanguage.IsNullOrBlank())
                {
                    LocalizationHelper.CurrentCulture = CompanyHelper.Company.DefaultLanguage;
                    this.SetLocalization();
                }
            }

            return true;
        }

        private void PerformLogout()
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            AuthTokenHelper.RemoveCachedRefreshToken();

            AuthTokenHelper.AuthToken = null;
            AuthTokenHelper.RefreshToken = null;

            GameProfileHelper.GameProfile = null;
            PlayerScoreHelper.PlayerScore = null;
            App.Ship = null;

            SetLoginContext();
        }

        private async Task CheckUserSession()
        {
            AuthTokenHelper.TryLoadRefreshToken();

            if (GameProfileHelper.HasUserLoggedIn())
            {
                SetLogoutContext();
            }
            else
            {
                if (!AuthTokenHelper.RefreshToken.IsNullOrEmpty() && await GetGameProfile())
                {
                    SetLogoutContext();
                    ShowWelcomeBackToast();
                }
                else
                {
                    SetLoginContext();
                    ShowCookieToast();
                }
            }
        }

        private async void ShowWelcomeBackToast()
        {
            AudioHelper.PlaySound(SoundType.POWER_UP);
            GameStartPage_UserName.Text = GameProfileHelper.GameProfile.User.UserName;

            WelcomeBackToast.Opacity = 1;
            await Task.Delay(TimeSpan.FromSeconds(5));
            WelcomeBackToast.Opacity = 0;
        }

        private void ShowCookieToast()
        {
            if (!CookieHelper.IsCookieAccepted())
                CookieToast.Visibility = Visibility.Visible;
        }

        private void SetLogoutContext()
        {
            GameStartPage_LogoutButton.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
            GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
            GameLoginPage_RegisterButton.Visibility = Visibility.Collapsed;
        }

        private void SetLoginContext()
        {
            GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
        }

        private async Task<bool> GetGameProfile()
        {
            (bool IsSuccess, string Message, _) = await _backendService.GetUserGameProfile();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        #endregion

        #region Page

        private void SetViewSize()
        {
            _scale = ScalingHelper.GetGameObjectScale(_windowWidth);

            UnderView.SetSize(_windowHeight, _windowWidth);
        }

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
            App.EnterFullScreen(true);
        }

        #endregion

        #region Animation

        #region Game

        private void PopulateGameViews()
        {
#if DEBUG
            Console.WriteLine("INITIALIZING GAME");
#endif
            SetViewSize();
            PopulateUnderView();
        }

        private void PopulateUnderView()
        {
            // add some clouds underneath
            for (int i = 0; i < 15; i++)
            {
                SpawnStar();
            }

            for (int i = 0; i < 1; i++)
            {
                SpawnStar(CelestialObjectType.Planet);
            }
        }

        private void StartAnimation()
        {
#if DEBUG
            Console.WriteLine("GAME STARTED");
#endif      
            RecycleGameObjects();
            RunGame();
        }

        private void RecycleGameObjects()
        {
            foreach (CelestialObject x in UnderView.Children.OfType<CelestialObject>())
            {
                switch ((ElementType)x.Tag)
                {
                    case ElementType.CELESTIAL_OBJECT:
                        {
                            RecyleStar(x);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private async void RunGame()
        {
            _gameViewTimer = new PeriodicTimer(_frameTime);

            while (await _gameViewTimer.WaitForNextTickAsync())
            {
                GameViewLoop();
            }
        }

        private void GameViewLoop()
        {
            UpdateGameObjects();
        }

        private void UpdateGameObjects()
        {
            foreach (CelestialObject x in UnderView.Children.OfType<CelestialObject>())
            {
                switch ((ElementType)x.Tag)
                {
                    case ElementType.CELESTIAL_OBJECT:
                        {
                            UpdateStar(x);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void StopAnimation()
        {
            _gameViewTimer?.Dispose();
        }

        #endregion

        #region Star

        private void SpawnStar(CelestialObjectType celestialObjectType = CelestialObjectType.Star)
        {
            CelestialObject star = new();
            star.SetAttributes(scale: _scale, celestialObjectType: celestialObjectType);

            RandomizeStarPosition(star);
            UnderView.Children.Add(star);
        }

        private void UpdateStar(CelestialObject star)
        {
            star.SetY(star.GetY() + (star.CelestialObjectType == CelestialObjectType.Planet ? _gameSpeed / 1.5 : _gameSpeed));

            if (star.GetY() > UnderView.Height)
                RecyleStar(star);
        }

        private void RecyleStar(CelestialObject star)
        {
            if (star.CelestialObjectType == CelestialObjectType.Planet)
                star.SetAttributes(scale: _scale, celestialObjectType: star.CelestialObjectType);

            RandomizeStarPosition(star);
        }

        private void RandomizeStarPosition(CelestialObject star)
        {
            star.SetPosition(
                left: _random.Next(0, (int)UnderView.Width) - (100 * _scale),
                top: _random.Next(800, 1400) * -1);
        }

        #endregion

        #endregion

        #endregion
    }
}
