using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SpaceShooterGame
{
    public sealed partial class GameContentDialog : ContentDialog
    {
        public GameContentDialog()
        {
            InitializeComponent();
        }

        public void SetContent(UIElement content)
        {
            Content_Container.Child = content;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }
    }
}
