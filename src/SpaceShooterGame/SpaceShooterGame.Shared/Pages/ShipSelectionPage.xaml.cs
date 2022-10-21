using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Linq;

namespace SpaceShooterGame
{
    public sealed partial class ShipSelectionPage : Page
    {
        #region Fields

        private PlayerShip _selectedShip;

        #endregion

        #region Ctor

        public ShipSelectionPage()
        {
            InitializeComponent();
            Loaded += ShipSelectionPage_Loaded;
        }

        #endregion

        #region Events

        #region Page

        private void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
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
        }

        #endregion

        #region Button

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShip is not null)
            {
                App.Ship = _selectedShip;
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

            EnableChooseButton();
        }

        #endregion

        #endregion

        #region Methods

        #region page

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        #endregion

        #region Logic

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
