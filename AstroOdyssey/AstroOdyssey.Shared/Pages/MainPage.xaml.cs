using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using static AstroOdyssey.Constants;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Fields

        private NavigationHelper navigationHelper;

        #endregion

        #region Ctor

        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;

            navigationHelper = new NavigationHelper(
                navigationView: NavView,
                frame: ContentFrame,
                pageMap: new Dictionary<string, Type>(),
                goBackNotAllowedToPages: new List<Type>() { typeof(GamePlayPage) },                
                goBackPageRoutes: new List<(Type IfGoingBackTo, Type RouteTo)>()
                {
                    (IfGoingBackTo: typeof(GameOverPage), RouteTo: typeof(GameStartPage)),
                });

            DataContext = this;
        }

        #endregion      

        #region Events

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate(typeof(LoginPage));
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            // log errors
            // show error window
        }

        #endregion

        #region Methods

        public void PlaySound(string baseUrl, SoundType soundType)
        {
            this.ExecuteJavascript($"playGameSound('{baseUrl}','{soundType}');");
        }

        public void StopSound()
        {
            this.ExecuteJavascript("stopSound();");
        }

        public void SetAccount()
        {
            AccountUserNameBlock.Text = App.Account.UserName;
        }

        /// <summary>
        /// Navigate to the target page.
        /// </summary>
        /// <param name="targetUri"></param>
        public void Navigate(Type page)
        {
            ContentFrame.Navigate(page);
        }

        #endregion
    }
}
