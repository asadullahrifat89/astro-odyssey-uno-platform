using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShipSelectionPage : Page
    {
        #region Fields

        private Ship selectedShip;

        #endregion

        #region Ctor

        public ShipSelectionPage()
        {
            this.InitializeComponent();
            this.Loaded += ShipSelectionPage_Loaded;
        }

        #endregion

        #region Events

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }

        private async void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            selectedShip = null;
            App.Ship = null;
            ShipSelectionPage_ChooseButton.IsEnabled = false;

            //if (this.ShipsList.ItemsSource is null)
            //{
            var ships = new Ship[] { };

            ships = Constants.PLAYER_SHIP_TEMPLATES.Select(x => new Ship()
            {
                Id = Guid.NewGuid().ToString(),
                Name = LocalizationHelper.GetLocalizedResource(x.Name),
                ImageUrl = x.AssetUri,
                ShipClass = x.ShipClass,
            }).ToArray();

            this.ShipsList.ItemsSource = ships.ToList();
            //}

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

        private void ShipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedShip = ShipsList.SelectedItem as Ship;
            ShipSelectionPage_ChooseButton.IsEnabled = true;
        }

        #endregion

        #region Methods

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_Tagline);
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_ControlInstructions);
            LocalizationHelper.SetLocalizedResource(ShipSelectionPage_ChooseButton);
        }

        #endregion


    }
}
