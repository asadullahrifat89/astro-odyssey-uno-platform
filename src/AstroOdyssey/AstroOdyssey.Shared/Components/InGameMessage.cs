using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstroOdyssey
{
    public class InGameMessage : StackPanel
    {
        public InGameMessage(string message, double scale, Image image = null)
        {
            Margin = new Microsoft.UI.Xaml.Thickness(5);

            if (image is not null)
            {
                image.Height = 100 * scale;
                image.Width = 100 * scale;
                this.Children.Add(image);
            }

            if (!message.IsNullOrBlank())
            {
                var border = new Border()
                {
                    HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
                    VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
                    CornerRadius = new Microsoft.UI.Xaml.CornerRadius(15),
                    Background = new SolidColorBrush(Colors.Gray) { Opacity = 0.5d },
                };

                var textBlock = new TextBlock()
                {
                    Margin = new Microsoft.UI.Xaml.Thickness(10 * scale),
                    HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
                    VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
                    FontSize = 30 * scale,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextAlignment = Microsoft.UI.Xaml.TextAlignment.Center,
                    TextWrapping = Microsoft.UI.Xaml.TextWrapping.WrapWholeWords,
                    Text = message,
                };

                border.Child = textBlock;

                this.Children.Add(border);
            }
        }

        public int ContentVisibilityCounter { get; set; } = 110;

        public bool CoolDown()
        {
            ContentVisibilityCounter--;

            if (ContentVisibilityCounter < 10)
                this.Opacity -= 0.1;

            if (ContentVisibilityCounter <= 0)
                return true;

            return false;
        }
    }
}
