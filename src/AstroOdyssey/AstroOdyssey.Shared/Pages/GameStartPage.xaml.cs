using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameStartPage : Page
    {
        #region Ctor
        
        public GameStartPage()
        {
            InitializeComponent();

            Loaded += StartPage_Loaded;
        } 

        #endregion

        #region Events  

        private void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            App.EnterFullScreen(true);
            App.NavigateToPage(typeof(ChooseShipPage));
        }

        #endregion
    }
}
