using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public static class PageTransitionHelper
    {
        #region Methods

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

        public static void ShowError(this Page page, ProgressBar progressBar, TextBlock errorContainer, string error, params Button[] actionButtons)
        {
            ShowErrorProgressBar(page, progressBar);
            ShowErrorMessage(page, errorContainer, error);
            foreach (var actionButton in actionButtons)
            {
                EnableActionButton(actionButton);
            }
        }

        public static void RunProgressBar(this Page page, ProgressBar progressBar, params Button[] actionButtons)
        {
            progressBar.ShowError = false;
            progressBar.ShowPaused = false;

            foreach (var actionButton in actionButtons)
            {
                DisableActionButton(actionButton);
            }
        }

        public static void StopProgressBar(this Page page, ProgressBar progressBar, params Button[] actionButtons)
        {
            progressBar.ShowError = false;
            progressBar.ShowPaused = true;

            foreach (var actionButton in actionButtons)
            {
                EnableActionButton(actionButton);
            }
        }

        private static void EnableActionButton(Button button)
        {
            button.IsEnabled = true;
        }

        private static void DisableActionButton(Button button)
        {
            button.IsEnabled = false;
        }

        private static void ShowErrorProgressBar(this Page page, ProgressBar progressBar)
        {
            progressBar.ShowPaused = true;
            progressBar.ShowError = true;
        }

        private static void ShowErrorMessage(this Page page, TextBlock errorContainer, string error)
        {
            errorContainer.Text = error;
            errorContainer.Visibility = Visibility.Visible;
        }

        #endregion
    }
}
