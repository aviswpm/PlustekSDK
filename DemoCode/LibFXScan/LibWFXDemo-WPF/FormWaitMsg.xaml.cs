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

namespace LibWFXDemo_CSharp
{
    /// <summary>
    /// Interaction logic for FormWaitMsg.xaml
    /// </summary>
    public partial class FormWaitMsg : Window
    {
        public FormWaitMsg(string labelToShow)
        {
            InitializeComponent();
            ShowLabel(labelToShow);
           // AdjustWindowSize();
        }
        private void ShowLabel(string labelName)
        {            
            if (labelName == "Scan")
            {
                this.Width = 300;
                this.Height = 112;
                label_scan.Visibility = Visibility.Visible;
                label_cal_normal_1.Visibility = Visibility.Hidden;
                label_cal_normal_2.Visibility = Visibility.Hidden;
                label_cal_xmini.Visibility = Visibility.Hidden;
                label_init.Visibility = Visibility.Hidden;
            }
            else if (labelName == "Calibrate_xmini")  // Calibrate_normal  Scan")
            {
                this.Width = 550;
                this.Height = 112;
                label_scan.Visibility = Visibility.Hidden;
                label_cal_normal_1.Visibility = Visibility.Hidden;
                label_cal_normal_2.Visibility = Visibility.Hidden;
                label_cal_xmini.Visibility = Visibility.Visible;
                label_init.Visibility = Visibility.Hidden;
            }
            else if (labelName == "Calibrate_normal")
            {
                this.Width = 300;
                this.Height = 112;
                label_scan.Visibility = Visibility.Hidden;
                label_cal_normal_1.Visibility = Visibility.Visible;
                label_cal_normal_2.Visibility = Visibility.Visible;
                label_cal_xmini.Visibility = Visibility.Hidden;
                label_init.Visibility = Visibility.Hidden;
            }
            else if (labelName == "Init")
            {
                this.Width = 300;
                this.Height = 112;
                label_scan.Visibility = Visibility.Hidden;
                label_cal_normal_1.Visibility = Visibility.Hidden;
                label_cal_normal_2.Visibility = Visibility.Hidden;
                label_cal_xmini.Visibility = Visibility.Hidden;
                label_init.Visibility = Visibility.Visible;
            }
        }

        // 用於創建窗口的靜態方法
      //  public static void ShowWindow(string labelToShow)
      //  {
      //      MainWindow window = new MainWindow(labelToShow);
      //      window.ShowDialog(); // 顯示窗口
       // }
    }
}
