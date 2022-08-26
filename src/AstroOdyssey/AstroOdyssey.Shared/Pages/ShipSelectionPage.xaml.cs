using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using static AstroOdyssey.Constants;

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

                for (int i = 1; i <= 3; i++)
                {
                    switch (i)
                    {
                        case 1:
                            shipUri = "ms-appx:///Assets/Images/player_ship1.png";
                            name = "Antimony";
                            break;
                        case 2:
                            shipUri = "ms-appx:///Assets/Images/player_ship2.png";
                            name = "Bismuth";
                            break;
                        case 3:
                            shipUri = "ms-appx:///Assets/Images/player_ship3.png";
                            name = "Curium";
                            break;
                    }

                    ships.Add(new Ship()
                    {
                        Id = i.ToString(),
                        Name = name,
                        ImageUrl = shipUri
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
