using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using Uno.Extensions;

namespace SpaceShooterGame
{
    public static class PageExtensions
    {
        #region Methods

        #region Public

        public static void SetLocalization(this Page page)
        {
            if (FindVisualChildren<UIElement>(page).Where(s => LocalizationHelper.HasLocalizationKey(s.Name)) is IEnumerable<UIElement> uiElements)
            {
                foreach (var uiElement in uiElements)
                {
                    LocalizationHelper.SetLocalizedResource(uiElement);
                }
            }
        }

        public static void RunProgressBar(this Page page, string progressBarMessage = null)
        {
            if (FindChild<ProgressBar>(parent: page, childName: "ProgressBar") is ProgressBar progressBar)
            {
                if (progressBar.Tag is bool flag && flag)
                    return;

                progressBar.Tag = true;

                progressBar.IsIndeterminate = true;
                progressBar.ShowError = false;
                progressBar.ShowPaused = false;
            }

            if (FindChild<TextBlock>(parent: page, childName: "ProgressBarMessageBlock") is TextBlock messageBlock)
            {
                messageBlock.Foreground = App.Current.Resources["ProgressBarOkColor"] as SolidColorBrush;
                messageBlock.Text = "👍 " + progressBarMessage;
                messageBlock.Visibility = progressBarMessage.IsNullOrBlank() ? Visibility.Collapsed : Visibility.Visible;
            }

            if (FindVisualChildren<Button>(page).Where(s => (string)s.Tag == "ActionButton") is IEnumerable<Button> buttons)
            {
                DisableActionButtons(buttons);
            }
        }

        public static void StopProgressBar(this Page page)
        {
            if (FindChild<ProgressBar>(parent: page, childName: "ProgressBar") is ProgressBar progressBar)
            {
                progressBar.Tag = false;
                progressBar.ShowError = false;
                progressBar.ShowPaused = true;
            }

            if (FindVisualChildren<Button>(page).Where(s => (string)s.Tag == "ActionButton") is IEnumerable<Button> buttons)
            {
                EnableActionButtons(buttons);
            }
        }

        public static void ShowError(this Page page, string progressBarMessage = null)
        {
            if (FindChild<ProgressBar>(parent: page, childName: "ProgressBar") is ProgressBar progressBar)
            {
                progressBar.Tag = false;
                progressBar.ShowPaused = true;
                progressBar.ShowError = true;
            }

            if (FindChild<TextBlock>(parent: page, childName: "ProgressBarMessageBlock") is TextBlock messageBlock)
            {
                messageBlock.Foreground = App.Current.Resources["ProgressBarErrorColor"] as SolidColorBrush;
                messageBlock.Text = "⚠️ " + progressBarMessage;
                messageBlock.Visibility = progressBarMessage.IsNullOrBlank() ? Visibility.Collapsed : Visibility.Visible;
            }

            if (FindVisualChildren<Button>(page).Where(s => (string)s.Tag == "ActionButton") is IEnumerable<Button> buttons)
            {
                EnableActionButtons(buttons);
            }
        }

        public static void SetProgressBarMessage(
            this Page page,
            string message,
            bool isError)
        {
            if (FindChild<TextBlock>(parent: page, childName: "ProgressBarMessageBlock") is TextBlock messageBlock)
            {
                messageBlock.Foreground = isError ? App.Current.Resources["ProgressBarErrorColor"] as SolidColorBrush : App.Current.Resources["ProgressBarOkColor"] as SolidColorBrush;
                messageBlock.Text = (isError ? "⚠️ " : "👍 ") + message;
                messageBlock.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Private

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent is not null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is not null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent is not null && !childName.IsNullOrBlank())
            {
                DependencyObject foundChild = null;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    // If the child is not of the request child type child
                    if (child is not T)
                    {
                        // recursively drill down the tree
                        foundChild = FindChild<T>(child, childName);

                        // If the child is found, break so we do not overwrite the found child. 
                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                    else if (!childName.IsNullOrEmpty())
                    {
                        // If the child's name is set for search
                        if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = (T)child;
                            break;
                        }

                        // Need this in case the element we want is nested
                        // in another element of the same type
                        foundChild = FindChild<T>(child, childName);
                    }
                    else
                    {
                        // child element found.
                        foundChild = (T)child;
                        break;
                    }
                }

                return (T)foundChild;
            }
            else
            {
                return default;
            }
        }

        private static void EnableActionButtons(IEnumerable<Button> buttons)
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = true;
            }
        }

        private static void DisableActionButtons(IEnumerable<Button> buttons)
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = false;
            }
        }

        #endregion

        #endregion
    }
}
