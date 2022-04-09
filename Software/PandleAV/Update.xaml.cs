using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        [DllImport("user32")] public static extern int FlashWindow(IntPtr hwnd, bool bInvert);

      
        public Update()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            WindowInteropHelper wih = new WindowInteropHelper(this);
            FlashWindow(wih.Handle, true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string StartUpdater = System.IO.Path.Combine(path, "av_update.exe");
            Process.Start(StartUpdater);
            Environment.Exit(0);
        }
    }
}
