using Microsoft.UI.Xaml;
using System;
using Uno.UI.Runtime.WebAssembly;

namespace AstroOdyssey
{
    [HtmlElement("audio")]
    public sealed class AudioPlayer : FrameworkElement
    {
        public AudioPlayer(string source, double volume = 1.0, bool loop = false)
        {
            this.SetHtmlAttribute("style.display", "none");
            this.SetHtmlAttribute("src", source);
            this.SetHtmlAttribute("volume", volume.ToString());
            this.SetHtmlAttribute("loop", loop.ToString());
            this.SetHtmlAttribute("controls", "none");

            Console.WriteLine(source + volume.ToString() + loop.ToString());
        }

        public void Play()
        {
            this.SetHtmlAttribute("currentTime", "0");
            this.ExecuteJavascript("element.play();");
        }

        public void Pause()
        {
            this.ExecuteJavascript("element.pause();");
            this.SetHtmlAttribute("currentTime", "0");
        }
    }
}
