using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        #region Ctor

        public LoginPage()
        {
            InitializeComponent();
            Loaded += LoginPage_Loaded;
        } 

        #endregion

        #region Events

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            //App.EnterFullScreen(false);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginButton.IsEnabled)
            {
                Login(); 
            }
        }      

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        private void PasswordBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && LoginButton.IsEnabled)
                Login();
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        #endregion

        #region Methods

        private void Login()
        {
            App.Account = new Account() { UserName = UserNameBox.Text, Password = PasswordBox.Password, };

            App.SetAccount();

            App.NavigateToPage(typeof(GameStartPage));
        }

        private void EnableLoginButton()
        {
            LoginButton.IsEnabled = UserNameBox.Text.IsNullOrBlank() || PasswordBox.Text.IsNullOrBlank() ? false : true;
        }

        #endregion     
    }
}
