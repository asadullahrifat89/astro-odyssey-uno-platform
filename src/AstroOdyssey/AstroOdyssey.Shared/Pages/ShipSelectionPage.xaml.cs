using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShipSelectionPage : Page
    {
        private Ship selectedShip;

        public ShipSelectionPage()
        {
            this.InitializeComponent();
            this.Loaded += ShipSelectionPage_Loaded;
        }

        private void ShipSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            selectedShip = null;
            App.Ship = null;
            ChooseButton.IsEnabled = false;

            if (this.ShipsList.Items.Count == 0)
            {
                var ships = new List<Ship>();

                string shipUri = null;
                string name = null;

                for (int i = 0; i <= 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            shipUri = "ms-appx:///Assets/Images/player_ship1.png";
                            name = "Antimony";
                            break;
                        case 1:
                            shipUri = "ms-appx:///Assets/Images/player_ship2.png";
                            name = "Bismuth";
                            break;
                        case 2:
                            shipUri = "ms-appx:///Assets/Images/player_ship3.png";
                            name = "Curium";
                            break;
                    }

                    ships.Add(new Ship()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        ImageUrl = shipUri,
                        ShipClass = (ShipClass)i,
                    });
                }

                this.ShipsList.ItemsSource = ships.OrderBy(x => x.Name).ToList();
            }
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedShip is not null)
            {
                AudioHelper.PlaySound(SoundType.MENU_SELECT);
                App.Ship = selectedShip;
                App.NavigateToPage(typeof(GamePlayPage));
            }
        }

        private void ShipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedShip = ShipsList.SelectedItem as Ship;
            ChooseButton.IsEnabled = true;
        }
    }
}
