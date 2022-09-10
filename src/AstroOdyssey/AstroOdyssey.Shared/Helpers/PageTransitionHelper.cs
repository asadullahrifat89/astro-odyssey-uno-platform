using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public static class PageTransitionHelper
    {
        public async static Task PlayPageLoadedTransition(this Page page)
        {
            //var content = page.Content as Grid;

            if (page is not null)
            {
                page.Opacity = 0;
                var skipAnimation = 100;

                while (skipAnimation > 0)
                {
                    skipAnimation--;
                    page.Opacity += 0.1d;
                    await Task.Delay(TimeSpan.FromMilliseconds(18));

                    if (page.Opacity >= 1)
                        break;
                }
            }
        }

        public async static Task PlayPageUnLoadedTransition(this Page page)
        {
            //var content = page.Content as Grid;            

            if (page is not null)
            {
                page.Opacity = 1;
                var skipAnimation = 100;

                while (skipAnimation > 0)
                {
                    skipAnimation--;
                    page.Opacity -= 0.1d;
                    await Task.Delay(TimeSpan.FromMilliseconds(18));

                    if (page.Opacity <= 0)
                        break;
                }
            }
        }

        public static void RunProgressBar(this Page page, ProgressBar progressBar)
        {
            progressBar.ShowError = false;
            progressBar.ShowPaused = false;
        }

        public static void StopProgressBar(this Page page, ProgressBar progressBar)
        {           
            progressBar.ShowError = false;
            progressBar.ShowPaused = true;
        }

        public static void ShowErrorProgressBar(this Page page, ProgressBar progressBar)
        {
            progressBar.ShowPaused = true;
            progressBar.ShowError = true;
        }

        public static void ShowErrorMessage(this Page page, TextBlock errorContainer, string error)
        {
            errorContainer.Text = error;
            errorContainer.Visibility = Visibility.Visible;
        }
    }
}
