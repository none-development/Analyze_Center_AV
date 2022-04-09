using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TAnalyze_Center_AV.GenerellSystems;
using Analyze_Center_AV.DesignFeatures;
using static Analyze_Center_AV.GenerellSystems.GenerateData;
using Pretty;
using Analyze_Center_AV.GenerellSystems;
using System.Diagnostics;

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

      
        public Settings()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            listaddThmes();
            CCheckStartPandle();
        }

        private void save_Click(object sender, RoutedEventArgs ess)
        {
            inisys config = new inisys(ConfigFile);
            if (ThemeBox.SelectedItem != null)
            {
                config.Write("SelectedTheme", ThemeBox.SelectedItem + ".ini", "General");
            }
            if(Language.SelectedItem != null)
            {
                config.Write("SelectedLanguage", Language.SelectedItem + ".ini", "General");
            }
            if(AudioList.SelectedItem != null)
            {
                config.Write("SelectedLanguage", AudioList.SelectedItem + ".wav", "General");
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateData.SettingsTabOpen = false;
            this.Close();
        }

        public void listaddThmes()
        {
            foreach (string a in files_themes())
            {
                if (a.Contains(".ini"))
                {
                    string b = System.IO.Path.GetFileNameWithoutExtension(a);
                    ThemeBox.Items.Add(b);
                }
            }
            foreach (string a in files_Language())
            {
                if (a.Contains(".ini"))
                {
                    string b = System.IO.Path.GetFileNameWithoutExtension(a);
                    Language.Items.Add(b);
                }
            }
            foreach (string a in files_Musik())
            {
                if (a.Contains(".wav"))
                {
                    string b = System.IO.Path.GetFileNameWithoutExtension(a);
                    AudioList.Items.Add(b);
                }
            }
        }
      


        public static string[] files_themes()
        {

            string[] b = Directory.GetFiles(themes);
            return b;
        }
        //plugins
        public static string[] files_Language()
        {

            string[] b = Directory.GetFiles(lang);
            return b;
        }
        public static string[] files_Musik()
        {

            string[] b = Directory.GetFiles(plugins);
            return b;
        }



        private void Autologin_Checked(object sender, RoutedEventArgs e)
        {
            if (Autologin.IsChecked == true)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("AutoLogin", "true", "Sanner");
            }
            if (Autologin.IsChecked == false)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("AutoLogin", "false", "Sanner");
            }
        }

        private void music_Checked(object sender, RoutedEventArgs e)
        {

            if (music.IsChecked == true)
            {
                if (AudioSystem.run != true)
                {
                    AudioSystem.CallMe();
                    inisys loadconfiogtext = new inisys(ConfigFile);
                    loadconfiogtext.Write("PlayAudio", "true", "General");
                }
            }
            if(music.IsChecked == false)
            {
                if (AudioSystem.run)
                {
                    AudioSystem.run = false;
                    inisys loadconfiogtext = new inisys(ConfigFile);
                    loadconfiogtext.Write("PlayAudio", "false", "General");
                }
            }
        }

        private void Autodelete_Checked(object sender, RoutedEventArgs e)
        {
            if (Autodelete.IsChecked == true)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("AutoDeleteBadFiles", "true", "Sanner");
            }
            if (Autodelete.IsChecked == false)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("AutoDeleteBadFiles", "false", "Sanner");
            }
        }

        private void Unpack_unitypackage_Checked(object sender, RoutedEventArgs e)
        {
            if (Unpack_unitypackage.IsChecked == true)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("Unpack_unitypackage", "true", "Sanner");
            }
            if (Unpack_unitypackage.IsChecked == false)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("Unpack_unitypackage", "false", "Sanner");
            }
        }

        private void CCheckStartPandle()
        {
            inisys loadconfiogtext = new inisys(ConfigFile);
            bool a = Boolean.Parse(loadconfiogtext.Read("PlayAudio", "General"));
            if (a) music.IsChecked = true;
            bool b = Boolean.Parse(loadconfiogtext.Read("AutoLogin", "Login"));
            if (b) Autologin.IsChecked = true;
            bool c = Boolean.Parse(loadconfiogtext.Read("AutoDeleteBadFiles", "Sanner"));
            if(c) Autodelete.IsChecked = true;
            bool d = Boolean.Parse(loadconfiogtext.Read("Unpack_unitypackage", "Sanner"));
            if(d) Unpack_unitypackage.IsChecked = true;
            bool e = Boolean.Parse(loadconfiogtext.Read("Auto_Clean_Package", "Sanner"));
            if(e) Auto_Clean_Package.IsChecked = true;
            bool f = Boolean.Parse(loadconfiogtext.Read("DiscordRPC", "Sanner"));
            if (f) DiscordRPC.IsChecked = true;

        }

        private void Auto_Clean_Package_Checked(object sender, RoutedEventArgs e)
        {
            if (Auto_Clean_Package.IsChecked == true)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("Auto_Clean_Package", "true", "Sanner");
            }
            if (Auto_Clean_Package.IsChecked == false)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("Auto_Clean_Package", "false", "Sanner");
            }
        }

        private void DiscordRPC_Checked(object sender, RoutedEventArgs e)
        {
            if (DiscordRPC.IsChecked == true)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("DiscordRPC", "true", "Sanner");
            }
            if (DiscordRPC.IsChecked == false)
            {
                inisys loadconfiogtext = new inisys(ConfigFile);
                loadconfiogtext.Write("DiscordRPC", "false", "Sanner");
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(baseFolder);
        }
    }
}
