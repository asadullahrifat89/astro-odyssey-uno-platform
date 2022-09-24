using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

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

            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
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
                Name = _localizationHelper.GetLocalizedResource(x.Name),
                ImageUrl = x.AssetUri,
                ShipClass = x.ShipClass,
            }).ToArray();

            ShipsList.ItemsSource = ships.ToList();

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

        private void ShipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedShip = ShipsList.SelectedItem as PlayerShip;
            ShipSelectionPage_ChooseButton.IsEnabled = true;
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
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_Tagline);
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_ControlInstructions);
            _localizationHelper.SetLocalizedResource(ShipSelectionPage_ChooseButton);
            _localizationHelper.SetLocalizedResource(ApplicationName_Header);
        }

        #endregion
    }
}
