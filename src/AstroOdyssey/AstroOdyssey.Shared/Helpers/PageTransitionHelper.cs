using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public static class PageTransitionHelper
    {
        public async static Task PlayPageLoadedTransition(this Page page) 
        {
            page.Opacity = 0;
            var skipAnimation = 700;

            while (skipAnimation > 0)
            {
                skipAnimation--;
                page.Opacity += 0.1d;
                await Task.Delay(TimeSpan.FromMilliseconds(18));

                if (page.Opacity >= 1)
                    break;
            }
        }

        public async static Task PlayPageUnLoadedTransition(this Page page)
        {
            var skipAnimation = 700;

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
}
