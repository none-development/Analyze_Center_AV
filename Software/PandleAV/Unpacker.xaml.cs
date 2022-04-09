using Analyze_Center_AV.GenerellSystems;
using Pretty;
using System;
using System.Collections.Generic;
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

namespace Analyze_Center_AV.PandleAV
{
    /// <summary>
    /// Interaktionslogik für Unpacker.xaml
    /// </summary>
    public partial class Unpacker : Window
    {
        public Unpacker()
        {
            InitializeComponent();
        }

























        //THEMES

        void GetText()
        {
            try
            {
                inisys ini = new inisys(GenerateData.REAL_LANG_PATH);
                programmname.Content = ini.Read("Title_Label", "Unpacker");
               
            }
            catch (Exception ex)
            {
                PrettyMessageBox.Show("Language Error", "An error with the Language:", ex);
            }
        }
        void GetTheme()
        {

            try
            {
                inisys ini = new inisys(GenerateData.REAL_THEME_PATH);
                //Background
                Background.Background = new BrushConverter().ConvertFromString(ini.Read("Background", "Unpacker")) as SolidColorBrush;

            }
            catch (Exception ex)
            {
                PrettyMessageBox.Show("Theme Error", "An error with the Theme:", ex);
            }
        }
    }
}
