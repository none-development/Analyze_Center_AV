using Pretty;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für Logs.xaml
    /// </summary>
    public partial class Logs : Window
    {
        public Logs()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            GetBadFiles();
        }

        private void GetBadFiles()
        {
            try
            {
                foreach (var item in Analyze_Center_AV.PandleAV.AVMainpandle.filess)
                {
                    LoggPandle.Items.Add(item);

                }
            }
            catch { }
           
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string paths = LoggPandle.SelectedItem.ToString();
                File.Delete(paths);
                PrettyMessageBox.Show("Info", "File Deleted");
            }
            catch { }
       
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string paths = LoggPandle.SelectedItem.ToString();
                Process.Start(System.IO.Path.GetFullPath(paths));
            } catch { }
  
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
