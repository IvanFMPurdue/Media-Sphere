using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Media_Sphere
{
    internal class VideoPlayer
    {
        //Creates instances of a MediaElement
        private MediaElement me;

        //this.me is set to the passed MediaElement parameter from MainWindow
        public VideoPlayer(MediaElement inputMdEl)
        {
            me = inputMdEl;
        }   

        public void Play(string input)//Performs the logic for playing the given .mp4 file path
        {
            me.Source = new Uri(input, UriKind.Relative);
            me.LoadedBehavior = MediaState.Manual;
            me.UnloadedBehavior = MediaState.Manual;
            me.Play();
            //whatever
        }
    }
}
