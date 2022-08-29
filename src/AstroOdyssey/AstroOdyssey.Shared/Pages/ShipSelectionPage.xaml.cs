﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            if (this.ShipsList.ItemsSource is null)
            {
                var ships = new Ship[] { };

                ships = Constants.PLAYER_SHIP_TEMPLATES.Select(x => new Ship()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = x.Name,
                    ImageUrl = x.AssetUri,
                    ShipClass = x.ShipClass,
                }).ToArray();

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
