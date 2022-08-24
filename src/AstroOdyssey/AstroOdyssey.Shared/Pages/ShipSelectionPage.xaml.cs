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

                for (int i = 1; i <= 15; i++)
                {
                    switch (i)
                    {
                        case 1:
                            shipUri = "ms-appx:///Assets/Images/satellite_A.png";
                            name = "Gallium";
                            break;
                        case 2:
                            shipUri = "ms-appx:///Assets/Images/satellite_B.png";
                            name = "Antimony";
                            break;
                        case 3:
                            shipUri = "ms-appx:///Assets/Images/satellite_C.png";
                            name = "Bismuth";
                            break;
                        case 4:
                            shipUri = "ms-appx:///Assets/Images/satellite_D.png";
                            name = "Cerium";
                            break;
                        case 5:
                            shipUri = "ms-appx:///Assets/Images/ship_C.png";
                            name = "Cadmium";
                            break;
                        case 6:
                            shipUri = "ms-appx:///Assets/Images/ship_D.png";
                            name = "Krypton";
                            break;
                        case 7:
                            shipUri = "ms-appx:///Assets/Images/ship_E.png";
                            name = "Radon";
                            break;
                        case 8:
                            shipUri = "ms-appx:///Assets/Images/ship_F.png";
                            name = "Cobalt";
                            break;
                        case 9:
                            shipUri = "ms-appx:///Assets/Images/ship_G.png";
                            name = "Radium";
                            break;
                        case 10:
                            shipUri = "ms-appx:///Assets/Images/ship_H.png";
                            name = "Barium";
                            break;
                        case 11:
                            shipUri = "ms-appx:///Assets/Images/ship_I.png";
                            name = "Neon";
                            break;
                        case 12:
                            shipUri = "ms-appx:///Assets/Images/ship_J.png";
                            name = "Xenon";
                            break;
                        case 13:
                            shipUri = "ms-appx:///Assets/Images/ship_K.png";
                            name = "Argon";
                            break;
                        //case 14:
                        //    shipUri = "ms-appx:///Assets/Images/ship_L.png";
                        //    name = "Helium";
                        //    break;
                        case 14:
                            shipUri = "ms-appx:///Assets/Images/ship_sidesA.png";
                            name = "Thorium";
                            break;
                        //case 16:
                        //    shipUri = "ms-appx:///Assets/Images/ship_sidesB.png";
                        //    name = "Lawrencium";
                        //    break;
                        case 15:
                            shipUri = "ms-appx:///Assets/Images/ship_sidesC.png";
                            name = "Promethium";
                            break;
                        //case 16:
                        //    shipUri = "ms-appx:///Assets/Images/ship_sidesD.png";
                        //    name = "Erbium";
                        //    break;

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
