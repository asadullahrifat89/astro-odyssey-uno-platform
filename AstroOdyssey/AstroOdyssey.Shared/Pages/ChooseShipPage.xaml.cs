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
    public sealed partial class ChooseShipPage : Page
    {
        private GameObject selectedShip;

        public ChooseShipPage()
        {
            this.InitializeComponent();
            this.Loaded += ChooseShipPage_Loaded;
        }

        private async void ChooseShipPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (this.ShipsList.Items.Count == 0)
            {
                await Task.Delay(200);

                Uri shipUri = null;

                for (int i = 1; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 1:
                            shipUri = new Uri("ms-appx:///Assets/Images/satellite_B.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 2:
                            shipUri = new Uri("ms-appx:///Assets/Images/satellite_C.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 3:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_C.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 4:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_D.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 5:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_E.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 6:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_F.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 7:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_G.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 8:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_H.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 9:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_I.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 10:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_J.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 11:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_K.png", UriKind.RelativeOrAbsolute);
                            break;
                        case 12:
                            shipUri = new Uri("ms-appx:///Assets/Images/ship_L.png", UriKind.RelativeOrAbsolute);
                            break;
                    }

                    GameObject ship = new GameObject()
                    {
                        Child = new Image()
                        {
                            Stretch = Stretch.Uniform,
                            Source = new BitmapImage(shipUri),
                        },
                        CornerRadius = new CornerRadius(10),
                        Tag = i,
                        Height = 100,
                        Width = 100
                    };

                    this.ShipsList.Items.Add(ship);
                }
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
            selectedShip = ShipsList.SelectedItem as GameObject;
        }
    }
}
