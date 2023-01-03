using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class ShipSelectionPage : Page
    {
        #region Fields

        private PlayerShip _selectedShip;
        private readonly IBackendService _backendService;

        #endregion

        #region Ctor

        public ShipSelectionPage()
        {
            InitializeComponent();

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();
            Loaded += ShipSelectionPage_Loaded;
        }

        #endregion

        #region Events

        #region Page

        private async void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();

            _selectedShip = null;

            App.Ship = null;

            ShipSelectionPage_ChooseButton.IsEnabled = false;

            var shipButtons = ShipsPanel.Children.OfType<ToggleButton>();

            var playerShipTemplates = AssetHelper.PLAYER_SHIP_TEMPLATES;

            foreach (var shipButton in shipButtons)
            {
                var playerShipTemplate = playerShipTemplates.FirstOrDefault(x => ((ShipClass)x.Size).ToString() == (string)shipButton.Tag);

                var playerShip = new PlayerShip()
                {
                    Name = LocalizationHelper.GetLocalizedResource(((ShipClass)playerShipTemplate.Size).ToString()),
                    ImageUrl = playerShipTemplate.AssetUri,
                    ShipClass = (ShipClass)playerShipTemplate.Size,
                };

                shipButton.DataContext = playerShip;
            }

            ShowUserName();
            await GetCompanyBrand();
        }

        #endregion

        #region Button

        private async void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShip is not null)
            {
                App.Ship = _selectedShip;

                if (GameProfileHelper.HasUserLoggedIn() ? await GenerateSession() : true)
                    NavigateToPage(typeof(GamePlayPage));
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        }

        private void Ship_Selected(object sender, RoutedEventArgs e)
        {
            var playerShipButton = sender as ToggleButton;
            _selectedShip = playerShipButton.DataContext as PlayerShip;

            foreach (var item in ShipsPanel.Children.OfType<ToggleButton>().Where(x => x.Tag != playerShipButton.Tag))
            {
                item.IsChecked = false;
            }

            ShipSelectionPage_ControlInstructions.Text = LocalizationHelper.GetLocalizedResource($"{_selectedShip.ShipClass}_DESCRIPTION");

            EnableChooseButton();
        }

        #endregion

        #endregion

        #region Methods

        #region Page

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        #endregion

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

                if (Company is not null && !Company.WebSiteUrl.IsNullOrBlank())
                {
                    CompanyHelper.Company = Company;
                }
            }

            if (CompanyHelper.Company is not null)
                BrandButton.NavigateUri = new Uri(CompanyHelper.Company.WebSiteUrl);

            return true;
        }

        private async Task<bool> GenerateSession()
        {
            (bool IsSuccess, string Message) = await _backendService.GenerateUserSession();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private void ShowUserName()
        {
            if (GameProfileHelper.HasUserLoggedIn())
            {
                Page_UserName.Text = GameProfileHelper.GameProfile.User.UserName;
                Page_UserPicture.Initials = GameProfileHelper.GameProfile.Initials;
                PlayerNameHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerNameHolder.Visibility = Visibility.Collapsed;
            }
        }

        private void EnableChooseButton()
        {
            ShipSelectionPage_ChooseButton.IsEnabled = _selectedShip is not null;
        }

        #endregion

        #endregion
    }
}
