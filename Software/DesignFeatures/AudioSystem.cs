using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analyze_Center_AV.DesignFeatures;
using Analyze_Center_AV.GenerellSystems;
using Pretty;
using System.Media;

namespace Analyze_Center_AV.DesignFeatures
{
    internal class AudioSystem
    {
        public static int Time { get; set; }
        public static bool run { get; set; }
        public static void CallMe()
        {
           Thread Call = new Thread(loost);
            try
            {
                run = true;
                Call.Start();
            } catch (Exception ex)
            {
                PrettyMessageBox.Show("Error!", "Error Starting Audio...", ex);
            }
        }
        private static void loost()
        {
          
                Audio1();
            
        }

        public static void Audio1()
        {
            SoundPlayer simpleSound = new SoundPlayer(GenerateData.AUDIO_HUB);
            simpleSound.PlayLooping();
        }
    }
}
