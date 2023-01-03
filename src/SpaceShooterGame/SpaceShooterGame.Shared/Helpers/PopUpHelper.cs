using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public static class PopUpHelper
    {
        public static async void ShowGamePlayResultPopUp(GamePlayResult gamePlayResult)
        {
            StackPanel stackPanel = new()
            {
                Width = 320,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Vertical,
            };

            stackPanel.Children.Add(new TextBlock()
            {
                FontSize = 45,
                Text = "🏆",
                TextAlignment = TextAlignment.Center
            });

            stackPanel.Children.Add(new TextBlock() // winning prize text
            {
                Margin = new Thickness(5),
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.WrapWholeWords,
                Text = gamePlayResult.WinningDescriptions.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value,
            });

            stackPanel.Children.Add(new TextBlock() // prize name text
            {
                Margin = new Thickness(5),
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.WrapWholeWords,
                Text = gamePlayResult.PrizeDescriptions.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value,
            });

            if (gamePlayResult.PrizeUrls.Any())
            {
                stackPanel.Children.Add(new HyperlinkButton()  // prize url
                {
                    Margin = new Thickness(5),
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    NavigateUri = new Uri(gamePlayResult.PrizeUrls.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value),
                    Content = LocalizationHelper.GetLocalizedResource("ViewPrizeButton")
                });
            }

            if (!GameProfileHelper.HasUserLoggedIn())
            {
                stackPanel.Children.Add(new TextBlock() //login prompt text
                {
                    Margin = new Thickness(5),
                    FontSize = 19,
                    FontWeight = FontWeights.SemiBold,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Text = LocalizationHelper.GetLocalizedResource("LOGIN_PROMPT"),
                });
            }

            await ShowPopUp(
                title: gamePlayResult.PrizeName,
                content: stackPanel,
                okButtonText: "Okay");
        }

        private static async Task<ContentDialogResult> ShowPopUp(
           string title,
           UIElement content,
           string okButtonText = "Okay",
           string closeButtonText = null)
        {
            GameContentDialog dialog = new()
            {
                //Title = title,
                PrimaryButtonText = okButtonText,
                CloseButtonText = closeButtonText
            };

            dialog.SetContent(content);

            var result = await dialog.ShowAsync();

            return result;
        }
    }
}
