using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameLeaderboardPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;

        private int _pageIndex = 0;
        private int _pageSize = 10;

        #endregion

        #region Ctor

        public GameLeaderboardPage()
        {
            this.InitializeComponent();
            Loaded += GameLeaderboardPage_Loaded;

            // Get a local instance of the container
            var container = ((App)App.Current).Container;
            _gameApiHelper = (IGameApiHelper)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GameApiHelper));
        }

        #endregion

        #region Events

        private async void GameLeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
            ScoreText.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} " + App.GameScore.Score;

            await this.PlayPageLoadedTransition();

            // get game profile
            if (!await GetGameProfile())
                return;

            // get game scores

            await GetGameScores();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));

            AudioHelper.PlaySound(SoundType.GAME_INTRO);
        }

        #endregion

        #region Methods

        private async Task<bool> GetGameProfile()
        {
            // get game profile
            var recordResponse = await _gameApiHelper.GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                this.ShowErrorMessage(errorContainer: GameLeaderboardPage_ErrorText, error: string.Join("\n", error));
                this.ShowErrorProgressBar(GameLeaderboardPage_ProgressBar);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            //TODO: show personal best in UI

            return true;
        }

        private async Task<bool> GetGameScores()
        {
            // get game scores
            var recordsResponse = await _gameApiHelper.GetGameScores(pageIndex: _pageIndex, pageSize: _pageSize);

            if (!recordsResponse.IsSuccess)
            {
                var error = recordsResponse.Errors.Errors;
                this.ShowErrorMessage(errorContainer: GameLeaderboardPage_ErrorText, error: string.Join("\n", error));
                this.ShowErrorProgressBar(GameLeaderboardPage_ProgressBar);

                return false;
            }
            
            var result = recordsResponse.Result;
            var records = result.Records;

            //TODO: show game scores in UI           

            return true;
        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(GameLeaderboardPage_Tagline);
            LocalizationHelper.SetLocalizedResource(GameOverPage_PlayAgainButton);
        }

        #endregion
    }
}
