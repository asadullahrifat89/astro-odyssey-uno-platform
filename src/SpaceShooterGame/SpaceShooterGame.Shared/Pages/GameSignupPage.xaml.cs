using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameSignupPage : Page
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

        public GameSignupPage()
        {
            this.InitializeComponent();
            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            _windowHeight = Window.Current.Bounds.Height;
            _windowWidth = Window.Current.Bounds.Width;

            PopulateGameViews();

            Loaded += GameSignupPage_Loaded;
            Unloaded += GamePage_Unloaded;
        }

        #endregion

        #region Events

        #region Page

        private void GameSignupPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();

            SizeChanged += GamePage_SizeChanged;
            StartAnimation();
        }

        private void GamePage_Unloaded(object sender, RoutedEventArgs e)
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

        #region Input Fields

        private void UserFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void UserEmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void ConfirmPasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private void ConfirmCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EnableSignupButton();
        }

        private void ConfirmCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableSignupButton();
        }

        #endregion

        #region Button

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        #endregion

        #endregion

        #region Methods

        #region Logic

        private async Task PerformSignup()
        {
            this.RunProgressBar();

            if (await Signup() && await Authenticate())
            {
                this.StopProgressBar();
                NavigateToPage(typeof(GameLoginPage));
            }
        }

        private async Task<bool> Signup()
        {
            (bool IsSuccess, string Message) = await _backendService.SignupUser(
               fullName: GameSignupPage_UserFullNameBox.Text.Trim(),
               userName: GameSignupPage_UserNameBox.Text.Trim(),
               email: GameSignupPage_UserEmailBox.Text.ToLower().Trim(),
               password: GameSignupPage_PasswordBox.Text.Trim());

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private async Task<bool> Authenticate()
        {
            (bool IsSuccess, string Message) = await _backendService.AuthenticateUser(
              userNameOrEmail: GameSignupPage_UserNameBox.Text.Trim(),
              password: GameSignupPage_PasswordBox.Text.Trim());

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private void EnableSignupButton()
        {
            GameSignupPage_SignupButton.IsEnabled =
                !GameSignupPage_UserFullNameBox.Text.IsNullOrBlank()
                && IsValidFullName()
                && IsStrongPassword()
                && DoPasswordsMatch()
                && !GameSignupPage_UserNameBox.Text.IsNullOrBlank()
                && !GameSignupPage_UserEmailBox.Text.IsNullOrBlank()
                && IsValidEmail()
                && GameSignupPage_ConfirmCheckBox.IsChecked == true;
        }

        private bool IsValidFullName()
        {
            var (IsValid, Message) = StringExtensions.IsValidFullName(GameSignupPage_UserFullNameBox.Text);

            if (!IsValid)
                this.SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource(Message), isError: true);
            else
                ProgressBarMessageBlock.Visibility = Visibility.Collapsed;

            return IsValid;
        }

        private bool IsStrongPassword()
        {
            var (IsStrong, Message) = StringExtensions.IsStrongPassword(GameSignupPage_PasswordBox.Text);
            this.SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource(Message), isError: !IsStrong);

            return IsStrong;
        }

        private bool DoPasswordsMatch()
        {
            if (GameSignupPage_PasswordBox.Text.IsNullOrBlank() || GameSignupPage_ConfirmPasswordBox.Text.IsNullOrBlank())
                return false;

            if (GameSignupPage_PasswordBox.Text != GameSignupPage_ConfirmPasswordBox.Text)
            {
                this.SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource("PASSWORDS_DIDNT_MATCH"), isError: true);

                return false;
            }
            else
            {
                this.SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource("PASSWORDS_MATCHED"), isError: false);
            }

            return true;
        }

        private bool IsValidEmail()
        {
            return StringExtensions.IsValidEmail(GameSignupPage_UserEmailBox.Text);
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
            {
                RecyleStar(star);
            }
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
                top: _random.Next((int)star.Height, (int)UnderView.Height) * -1);
        }

        #endregion

        #endregion

        #endregion
    }
}
