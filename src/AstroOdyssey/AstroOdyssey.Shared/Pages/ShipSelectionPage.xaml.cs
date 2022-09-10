using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{    
    public sealed partial class ShipSelectionPage : Page
    {
        #region Fields

        private PlayerShip selectedShip;

        #endregion

        #region Ctor

        public ShipSelectionPage()
        {
            InitializeComponent();
            Loaded += ShipSelectionPage_Loaded;
        }

        #endregion

        #region Events

        private async void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            selectedShip = null;
            App.Ship = null;
            ShipSelectionPage_ChooseButton.IsEnabled = false;

            var ships = new PlayerShip[] { };

            ships = GameObjectTemplates.PLAYER_SHIP_TEMPLATES.Select(x => new PlayerShip()
            {
                Id = Guid.NewGuid().ToString(),
                Name = LocalizationHelper.GetLocalizedResource(x.Name),
                ImageUrl = x.AssetUri,
                ShipClass = x.ShipClass,
            }).ToArray();

            ShipsList.ItemsSource = ships.ToList();

            await this.PlayPageLoadedTransition();
        }

        private async void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShip is not null)
            {
                AudioHelper.PlaySound(SoundType.MENU_SELECT);
                App.Ship = selectedShip;

                await this.PlayPageUnLoadedTransition();

                App.NavigateToPage(typeof(GamePlayPage));
            }
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }

        private void ShipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedShip = ShipsList.SelectedItem as PlayerShip;
            ShipSelectionPage_ChooseButton.IsEnabled = true;
        }

        #endregion

        #region Methods

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_Tagline);
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_ControlInstructions);
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_ChooseButton);
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
        }

        #endregion
    } 
}
