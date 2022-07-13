using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Ctor

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods  

        #region Functionality

        /// <summary>
        /// Navigate to the target page.
        /// </summary>
        /// <param name="targetUri"></param>
        public void Navigate(Type page)
        {
            PageContainerFrame.Navigate(page);
        }

        #endregion

        #endregion
    }
}
