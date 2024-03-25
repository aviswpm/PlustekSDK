using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;


namespace LibWFXDemo_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

        private void FormMain_Load(object sender, RoutedEventArgs e)
        {
            string szFilePath = Environment.CurrentDirectory + "\\LibWebFxScan.ini";
            int UseModeBlock = 1;

            this.Hide();
            if (File.Exists(szFilePath))
            {
                UseModeBlock = GetPrivateProfileInt("Style", "UseModeBlock", 1, szFilePath);
            }
            else
            {
#if WIN32
                szFilePath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1", "InstallLocation", "");
#else
                szFilePath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1", "InstallLocation", "");
#endif
                System.IO.Directory.SetCurrentDirectory(szFilePath);

                if (szFilePath.LastIndexOf('\\') != (szFilePath.Length - 1))
                    szFilePath += "\\";

                szFilePath += "LibWebFxScan.ini";

                if (File.Exists(szFilePath))
                {
                    UseModeBlock = GetPrivateProfileInt("Style", "UseModeBlock", 1, szFilePath);
                }
            }

            if (UseModeBlock == 1)
            {
                FormDemo_BlockMode form = new FormDemo_BlockMode();
                form.Show();
            }
            else
            {
                FormDemo_NonBlockMode form = new FormDemo_NonBlockMode();
                form.Show();
            }
        }

        private void FormDemo_FormClosing(object sender, CancelEventArgs e)
        {
            Close();
        }

        private void BTN_EXIT_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }    
}
