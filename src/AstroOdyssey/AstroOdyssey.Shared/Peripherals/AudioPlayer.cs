using Microsoft.UI.Xaml;
using System;
using Uno.UI.Runtime.WebAssembly;

namespace AstroOdyssey
{
    [HtmlElement("audio")]
    public sealed class AudioPlayer : FrameworkElement
    {
        //public event EventHandler OnCompleted;

        public AudioPlayer(string source, double volume, bool loop = false)
        {
            var audio = "element.style.display = \"none\"; " +
                "element.controls = false; " +
                $"element.src = \"{source}\"; " +
                $"element.volume = {volume}; " +
                $"element.loop = {loop.ToString().ToLower()}; ";

            this.ExecuteJavascript(audio);
            //this.RegisterHtmlEventHandler("onended", OnCompleted);
#if DEBUG
            Console.WriteLine("source: " + source + " volume: " + volume.ToString() + " loop: " + loop.ToString().ToLower());
#endif
        }

        public void SetSource(string source)
        {
            this.ExecuteJavascript($"element.src = \"{source}\"; ");
        }

        public void Play()
        {
            this.ExecuteJavascript("element.currentTime = 0; element.play();");
        }

        public void Stop()
        {
            this.ExecuteJavascript("element.pause(); element.currentTime = 0;");
        }

        public void Pause()
        {
            this.ExecuteJavascript("element.pause();");
        }

        public void Resume()
        {
            this.ExecuteJavascript("element.play();");
        }
    }
}
