using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public sealed partial class GameInstructionsPage : Page
    {
        #region Fields

        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;

        #endregion

        #region Ctor

        public GameInstructionsPage()
        {
            this.InitializeComponent();
            Loaded += GameInstructionsPage_Loaded;
            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
        }

        #endregion

        #region Events

        private async void GameInstructionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
            await this.PlayLoadedTransition();
            InstructionDetailsStack.AnimateChildElements();
        }

        public async void GameInstructionsPage_PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GamePlayPage));
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
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
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_Tagline);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_PlayButton);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_ControlsText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_ControlsText2);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_EnemiesText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_EnemiesText2);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_BossesText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_BossesText2);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_HealthText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_HealthText2);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_PowerupText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_PowerupText2);

            _localizationHelper.SetLocalizedResource(GameInstructionsPage_CollectiblesText);
            _localizationHelper.SetLocalizedResource(GameInstructionsPage_CollectiblesText2);
        }

        #endregion
    }
}
