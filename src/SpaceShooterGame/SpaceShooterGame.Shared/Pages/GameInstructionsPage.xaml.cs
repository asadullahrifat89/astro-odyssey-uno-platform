using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace SpaceShooterGame
{
    public sealed partial class GameInstructionsPage : Page
    {
        #region Ctor

        public GameInstructionsPage()
        {
            this.InitializeComponent();
            Loaded += GameInstructionsPage_Loaded;
        }

        #endregion

        #region Events

        #region Page

        private void GameInstructionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
        }

        #endregion

        #region Button

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsCount = InstructionsContainer.Items.Count - 1;

            // once the last instruction is reached, make the start game button visible and hide the next button
            if (InstructionsContainer.SelectedIndex == itemsCount)
            {
                // traverse back to first instruction
                for (int i = 0; i < itemsCount; i++)
                {
                    InstructionsContainer.SelectedIndex--;
                }

                GameInstructionsPage_NextButton.Visibility = Visibility.Collapsed;
                GameInstructionsPage_PlayButton.Visibility = Visibility.Visible;
            }
            else
            {
                InstructionsContainer.SelectedIndex++;
            }

            AudioHelper.PlaySound(SoundType.MENU_SELECT);
        }

        public void GameInstructionsPage_PlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShipSelectionPage));
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        } 

        #endregion

        #endregion

        #region Methods

        #region Page

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        #endregion

        #region Logic

        private void SetLocalization()
        {
            PageExtensions.SetLocalization(this);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_ControlsText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_ControlsText2);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_EnemiesText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_EnemiesText2);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_BossesText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_BossesText2);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_HealthText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_HealthText2);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_PowerupText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_PowerupText2);

            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_CollectiblesText);
            LocalizationHelper.SetLocalizedResource(GameInstructionsPage_CollectiblesText2);
        } 

        #endregion

        #endregion
    }
}
