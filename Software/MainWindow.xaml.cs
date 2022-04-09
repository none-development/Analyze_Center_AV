using Analyze_Center_AV.GenerellSystems;
using Pretty;
using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TAnalyze_Center_AV.GenerellSystems;
using Analyze_Center_AV.AccountData;
using Analyze_Center_AV.PandleAV;
using System.Diagnostics;
using Analyze_Center_AV.DesignFeatures;

namespace Analyze_Center_AV
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            inisys ini2 = new inisys(GenerateData.ConfigFile);
            if (ini2.Read("CheckForUpdates", "General") == "true")
            {
                LoginAPIStuff.CheckForUpdate();
            }
            if (ini2.Read("DiscordRPC", "Sanner") == "true")
            {
                GenerateData.DiscordC(true);
            } else
            {
                GenerateData.DiscordName = "NODISCORDRPC";
            }
            this.MouseLeftButtonDown += delegate { DragMove(); };
          
            GenerateData.StatCheck();
            GetText();
            GetTheme();
            if (ini2.Read("PlayAudio", "General") == "true")
            {
                AudioSystem.CallMe();
            }
            if (ini2.Read("AutoLogin", "Login") == "true")
            {
                AutoLogin();
            }
          

        }
    

        //Startup Funktionen

        void AutoLogin()
        {
            try
            {
               
            }
            catch
            { }

        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

      

        private void log_loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != "" && Username_Imput.Text != "")
            {
            }
            else
            {
                PrettyMessageBox.Show("Login Error", "Please fill out Username and Password");
            }
            
        }

        private void log_CreateAccount_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Patreon_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.patreon.com/none_dev");
        }
    }
}
