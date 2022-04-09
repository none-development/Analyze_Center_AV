using Analyze_Center_AV.GenerellSystems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für _Supporter.xaml
    /// </summary>
    public partial class _Supporter : Window
    {
        public _Supporter()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DragMove(); };

        }

        private void Patreon_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.patreon.com/none_dev");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateData.SupportTabOpen = false;
            this.Close();
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }

}
