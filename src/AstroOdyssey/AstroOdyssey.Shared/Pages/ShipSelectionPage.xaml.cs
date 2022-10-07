using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Linq;

namespace AstroOdyssey
{
    public sealed partial class ShipSelectionPage : Page
    {
        #region Fields

        private PlayerShip selectedShip;
        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;

        #endregion

        #region Ctor

        public ShipSelectionPage()
        {
            InitializeComponent();
            Loaded += ShipSelectionPage_Loaded;

            _audioHelper = (Application.Current as App).Host.Services.GetRequiredService<IAudioHelper>();
            _localizationHelper = (Application.Current as App).Host.Services.GetRequiredService<ILocalizationHelper>();
        }

        #endregion

        #region Events

        private async void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            selectedShip = null;
            App.Ship = null;

            ShipSelectionPage_ChooseButton.IsEnabled = false;

            var shipButtons = ShipsPanel.Children.OfType<ToggleButton>();

            foreach (var playerShipTemplate in GameObjectTemplates.PLAYER_SHIP_TEMPLATES)
            {
                var playerShip = new PlayerShip()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _localizationHelper.GetLocalizedResource(playerShipTemplate.Name),
                    ImageUrl = playerShipTemplate.AssetUri,
                    ShipClass = playerShipTemplate.ShipClass,
                };

                if (shipButtons.FirstOrDefault(x => x.Name == playerShipTemplate.Name) is ToggleButton shipButton)
                    shipButton.DataContext = playerShip;
            }

            await this.PlayLoadedTransition();
            ShowUserName();
        }

        private async void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShip is not null)
            {
                _audioHelper.PlaySound(SoundType.MENU_SELECT);

                App.Ship = selectedShip;

                await this.PlayUnLoadedTransition();

                App.NavigateToPage(typeof(GameInstructionsPage));
            }
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }

        private void Ship_Selected(object sender, RoutedEventArgs e)
        {
            var playerShipButton = sender as ToggleButton;
            selectedShip = playerShipButton.DataContext as PlayerShip;

            foreach (var item in ShipsPanel.Children.OfType<ToggleButton>().Where(x => x.Name != playerShipButton.Name))
            {
                item.IsChecked = false;
            }

            EnableChooseButton();
        }

        #endregion

        #region Methods

        private void ShowUserName()
        {
            if (App.HasUserLoggedIn)
            {
                Page_UserName.Text = App.GameProfile.User.UserName;
                Page_UserPicture.Initials = App.GameProfile.Initials;
                PlayerNameHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerNameHolder.Visibility = Visibility.Collapsed;
            }
        }

        private void SetLocalization()
        {
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_Header);
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_Tagline);
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_ControlInstructions);
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_ChooseButton);
            _localizationHelper.SetLocalizedResource(ApplicationName_Header);
        }

        private void EnableChooseButton()
        {
            ShipSelectionPage_ChooseButton.IsEnabled = selectedShip is not null;
        }

        #endregion
    }
}
