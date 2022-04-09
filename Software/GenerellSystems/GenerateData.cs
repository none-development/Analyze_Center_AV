using Analyze_Center_AV.DesignFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TAnalyze_Center_AV.GenerellSystems;

namespace Analyze_Center_AV.GenerellSystems
{
    internal class GenerateData
    {
        //Folder Path
        public static string baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\");

        public static string fddd = Path.Combine(Path.GetTempPath(), "/DefaultCompany");
        public static string themes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\themes\");
        public static string CleanedUnityPackages = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\cleanpackages\");
        public static string ExtracktedFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\unpacked\");
        public static string lang = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\languages\");
        public static string plugins = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\audiofiles\");
        public static string NEWUPDATE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\1.0.0.4.ANALYZEVERSION");

        //Default Data to Generate Beginn.
        public static string BaseTheme = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\themes\" + "DefaultTheme.ini");
        public static string BaseLang = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\languages\" + "DefaultLanguage.ini");
        public static string ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\" + "config.ini");
        public static string AccountFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\" + "data.ini");
        public static string Logs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\" + "logs.txt");
        public static string AccountFileCrypt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AnalyzeCenter\" + "data.account");
        //Loaded Data
        public static string REAL_THEME_PATH { get; set; }
        public static string REAL_LANG_PATH { get; set; }
        public static string AUDIO_HUB { get; set; }


        public static string DiscordName { get; set; }
        public static string DiscordID { get; set; }
        public static string Webpagename { get; set; }

        public static bool mininizestart = false;
        //Discord Stufdf


        //Account Informations
        public static int Plan;
        public static string AuthCookie;
        public static bool Check = true;


        //Pandles
        public static bool SupportTabOpen = false;
        public static bool SettingsTabOpen = false;

        public static void StatCheck()
        {
           

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
            if (!File.Exists(NEWUPDATE))
            {
                Directory.Delete(baseFolder, true);
                Directory.CreateDirectory(baseFolder);
                File.Create(NEWUPDATE);
            }
            if (!Directory.Exists(themes))
            {
                Directory.CreateDirectory(themes);
            }
            if (!Directory.Exists(lang))
            {
                Directory.CreateDirectory(lang);
            }
            if (!Directory.Exists(CleanedUnityPackages))
            {
                Directory.CreateDirectory(CleanedUnityPackages);
            }
            if (!Directory.Exists(ExtracktedFolder))
            {
                Directory.CreateDirectory(ExtracktedFolder);
            }
            if (!Directory.Exists(plugins))
            {
                Directory.CreateDirectory(plugins);
                download_DefaultMusik();
            }
            if (!File.Exists(BaseTheme))
            {
                download_BaseTheme();
            }
            if (!File.Exists(BaseLang))
            {
                download_DefaultLanguage();
            }
            if (!File.Exists(ConfigFile))
            {
                GenerateConfig();
            }
            if (!File.Exists(AccountFile))
            {
                if (!File.Exists(AccountFileCrypt))
                {
                    GenerateAccFile();
                }
            }
            loadData();
            Check = false;
        }


        static string access_value = "CVjerzhHCe7UHJFChefsfhr56zjHrhrhdcgsgwerDS";
        static void download_BaseTheme()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Token", access_value);
                client.DownloadFile("", themes + "DefaultTheme");
             
            }
        }

        static void download_DefaultLanguage()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Token", access_value);
                client.DownloadFile("https://as.mba/tool/DefaultLanguage.ini", lang + "DefaultLanguage.ini");
            }
        }

        static void download_DefaultMusik()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Token", access_value);
                client.DownloadFile("", plugins + "DefaultAudio.wav");
            }
        }

        static void GenerateConfig()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Token", access_value);
                client.DownloadFile("", baseFolder + "config.ini");
            }

        }

        public static void DiscordC(bool check = false)
        {
            if (check)
            {
                Thread THR = new Thread(RunDIS);
                THR.Start();
            }
        }


        static void RunDIS()
        {
            DiscordRPCM M = new DiscordRPCM();
            M.MainDiscord();
        }
        static void GenerateAccFile()
        {
            inisys ini = new inisys(AccountFile);
            ini.Write("UserName", "default");
            ini.Write("Password", "default");
            CrypteFileSystem.encrypto(AccountFile, AccountFileCrypt);
        }


        public static void loadData()
        {
            inisys loadconfiogtext = new inisys(ConfigFile);
            REAL_LANG_PATH = Path.Combine(lang, loadconfiogtext.Read("SelectedLanguage", "General"));
            REAL_THEME_PATH = Path.Combine(themes, loadconfiogtext.Read("SelectedTheme", "General"));
            AUDIO_HUB = Path.Combine(plugins, loadconfiogtext.Read("BackGroundAudio", "General"));
        }
    }
}
