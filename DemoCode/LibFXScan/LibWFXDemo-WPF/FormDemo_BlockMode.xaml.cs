using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Interop;
using System.Windows.Threading;

namespace LibWFXDemo_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FormDemo_BlockMode : Window
    {
        DeviceWrapper m_DeviceWrapper = new DeviceWrapper();
        ENUM_LIBWFX_ERRCODE m_enErrCode;       
        int m_nCount;
        FormScanning formScanning = null;
        private List<String> m_szlistDevice;
        private List<String> m_szlistFile;
        private int m_MaxCMDItems;
        
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        
        private System.Drawing.Imaging.ColorPalette GetColorPalette(uint nColors)
        {
            System.Drawing.Imaging.PixelFormat bitscolordepth = System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
            System.Drawing.Imaging.ColorPalette palette;

            System.Drawing.Bitmap bitmap;
            if (nColors > 2)
                bitscolordepth = System.Drawing.Imaging.PixelFormat.Format4bppIndexed;

            if (nColors > 16)
                bitscolordepth = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;


            bitmap = new System.Drawing.Bitmap(1, 1, bitscolordepth);
            palette = bitmap.Palette;
            bitmap.Dispose();

            return palette;
        }
       
        public FormDemo_BlockMode()
        {
            InitializeComponent();           
            m_szlistDevice = new List<String>();
            m_szlistFile = new List<String>();          
            m_MaxCMDItems = 5;           
        }

        private void FormMain_Load(object sender, RoutedEventArgs e)
        {
            if (m_DeviceWrapper.m_pfnLibWFX_IsWindowExist("") == true)
            {

                MessageBox.Show("Status:[Please confirm whether the \"CheckWindowTitle\" parameter content in LibWebFxScan.ini are all closed!!]", "Warning");
                DispatcherWriteLog(@"Status:[LibWFX_InitEx Fail]");
                return;
            }

            //since we can't debug OCR engine, for debuging UI flow, use LIBWFX_INIT_MODE_NOOCR
            //OCR will not work, but easier to debug UI while developing
#if DEBUG
            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NOOCR);
#else
            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);
#endif
            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                BTN_REFRESH_Click(null, null);
                GetCertificatePermission();
            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR)
            {
                DispatcherWriteLog(@"Status:[No Recognize Tool]");
                BTN_REFRESH_Click(null, null);
                GetCertificatePermission();
            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR)
            {
                DispatcherWriteLog(@"Status:[No AVI Recognize Tool]");
                BTN_REFRESH_Click(null, null);
                GetCertificatePermission();
            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR)
            {
                DispatcherWriteLog(@"Status:[No DOC Recognize Tool]");
                BTN_REFRESH_Click(null, null);
                GetCertificatePermission();
            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG)
            {
                DispatcherWriteLog(@"Status:[Path Is Too Long (max limit: 130 bits)]");
                DispatcherWriteLog(@"Status:[LibWFX_InitEx Fail]");
            }
            else
                DispatcherWriteLog(@"Status:[LibWFX_InitEx Fail]");
        }
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
        private void FormDemo_FormClosing(object sender, CancelEventArgs e)
        {
            m_DeviceWrapper.m_pfnLibWFX_CloseDevice();
            m_DeviceWrapper.m_pfnLibWFX_DeInit();

            if (m_DeviceWrapper.hLibModule != IntPtr.Zero)
                FreeLibrary(m_DeviceWrapper.hLibModule);

            if (m_DeviceWrapper.hCommandModule != IntPtr.Zero)
                FreeLibrary(m_DeviceWrapper.hCommandModule);
            Environment.Exit(0);
        }

        private void WriteLog(String szMsg)
        {
            TXT_LOG.AppendText(szMsg);
            TXT_LOG.AppendText("\r\n");
            TXT_LOG.ScrollToEnd();

           m_DeviceWrapper.m_pfnLibWFX_WriteAPLog(szMsg);
        }

        private void DispatcherWriteLog(String szMsg)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<String>(WriteLog), szMsg);
            }
            else
            {
                WriteLog(szMsg);
            }
        }

        private void SetJsonCmd(String szDevName)
        {
            string szDefJson = "";

            if (GetCommandString(szDevName))
            {
                return;
            }
            else if (szDevName == "A61" || szDevName == "A62" || szDevName == "A63" || szDevName == "A64" || szDevName == "A65" || szDevName == "A66" || szDevName == "J6102R")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";
            }
            else if (szDevName == "7C1U" || szDevName == "7C8U" || szDevName == "7C9U" || szDevName == "7CAU" || szDevName == "773U" || szDevName == "7CCU")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Sheetfed-Duplex\",\"recognize-type\":\"passport\"}";
            }
            else if (szDevName == "776U" || szDevName == "777U" || szDevName == "778U")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Sheetfed-Duplex\"}";
            }
            else if (szDevName == "74RU" || szDevName == "74BU" || szDevName == "7P1U" || szDevName == "M11U" || szDevName == "7B3U" || szDevName == "M12U")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Sheetfed-Front\"}";
            }
            else if (szDevName == "256U" ||
                     szDevName == "258U" ||
                     szDevName == "258U_259U" ||
                     szDevName == "25AU" ||
                     szDevName == "271U" ||
                     szDevName == "273U" ||
                     szDevName == "273U_274U" ||
                     szDevName == "275U" ||
                     szDevName == "276U" ||
                     szDevName == "261U" ||
                     szDevName == "BAG" ||
                     szDevName == "7K1U" ||
                     szDevName == "6C6U" ||
                     szDevName == "BB1U" ||
                     szDevName == "BAGU" ||
                     szDevName == "2B2U" ||
                     szDevName == "2B3U" ||
                     szDevName == "7N1U" ||
                     szDevName == "2D1U" ||
                     szDevName == "2C1U" ||
                     szDevName == "797U" ||
                     szDevName == "7K7U" ||
                     szDevName == "2G1U" ||
                     szDevName == "2G2U" ||
                     szDevName == "678U" ||
                     szDevName == "7K8U" ||
                     szDevName == "B85U" ||
                     szDevName == "2D3U")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Flatbed\"}";
            }
            else
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"ADF-Duplex\"}";
            }
            COMBO_COMMAND.Items.Add(szDefJson);
            COMBO_COMMAND.SelectedIndex = 0;
        }

        private bool GetCommandString(String szDevName)
        {

            string szFilePath = "C:\\ProgramData\\Plustek\\" + szDevName + "\\";
            if (!Directory.Exists(szFilePath))
                Directory.CreateDirectory(szFilePath);
            szFilePath += "Command.txt";

            if (File.Exists(szFilePath))
            {
                COMBO_COMMAND.Items.Clear();
                string line = "";
                int idx = 0;
                System.IO.StreamReader file = new System.IO.StreamReader(szFilePath);
                while ((line = file.ReadLine()) != null && idx < m_MaxCMDItems)
                {
                    idx++;
                    COMBO_COMMAND.Items.Add(line);
                }
                COMBO_COMMAND.SelectedIndex = 0;
                file.Close();
                return true;
            }
            else
                return false;
        }

        private bool SetCommandString(String szDevName, String szCommand)
        {
            //Confirm whether the devicename in the command is correct
            if (szCommand.Contains(szDevName) == false)
                return false;

            string szFilePath = "C:\\ProgramData\\Plustek\\" + szDevName + "\\";
            if (!Directory.Exists(szFilePath))
                Directory.CreateDirectory(szFilePath);
            szFilePath += "Command.txt";

            if (File.Exists(szFilePath))
            {
                List<string> commandLists = new List<string>();
                using (StreamWriter outputFile = new StreamWriter(szFilePath))
                {
                    outputFile.WriteLine(szCommand);
                    commandLists.Add(szCommand);

                    COMBO_COMMAND.SelectedIndex = 0;
                    for (int idx = 0; idx < COMBO_COMMAND.Items.Count; idx++)
                    {
                        if (commandLists.Count == m_MaxCMDItems || (szCommand == COMBO_COMMAND.SelectedItem.ToString() && idx == 0))
                            continue;

                        COMBO_COMMAND.SelectedIndex = idx;
                        outputFile.WriteLine(COMBO_COMMAND.SelectedItem.ToString());
                        commandLists.Add(COMBO_COMMAND.SelectedItem.ToString());
                    }
                    outputFile.Close();

                    COMBO_COMMAND.Items.Clear();
                    for (int idx = 0; idx < commandLists.Count; idx++)
                    {
                        COMBO_COMMAND.Items.Add(commandLists[idx]);
                    }
                    COMBO_COMMAND.SelectedIndex = 0;
                    return true;
                }
            }
            else
                return false;
        }
             
        private void HandleDrawImage(BitmapImage bitmapImage, bool bFront)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<BitmapImage, bool>(UpdatePreview), bitmapImage, bFront);
            }
            else
            {
                UpdatePreview(bitmapImage, bFront);
            }
        }

        private void UpdatePreview(BitmapImage bitmapImage, bool bFront)
        {
            if (bFront == true)
            {
                PIC_1.BeginInit();
                PIC_1.Source = bitmapImage;
                PIC_1.EndInit();
            }
            else
            {
                PIC_2.BeginInit();
                PIC_2.Source = bitmapImage;
                PIC_2.EndInit();
            }
        }

        private void BTN_REFRESH_Click(object sender, RoutedEventArgs e)
        {
            ENUM_LIBWFX_ERRCODE m_enErrCode = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS;
            m_szlistDevice.Clear();
            IntPtr pstr = IntPtr.Zero;
            IntPtr pstr2 = IntPtr.Zero;
            m_enErrCode = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS;

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_GetDeviesListWithSerial(out pstr, out pstr2);

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                string json = Marshal.PtrToStringUni(pstr);
                string json2 = Marshal.PtrToStringUni(pstr2);
                try
                {
                    m_szlistDevice = JsonConvert.DeserializeObject<List<string>>(json);
                    List<String> szlistSerialNumber = JsonConvert.DeserializeObject<List<string>>(json2);

                    COMBO_DEVICE.ItemsSource = m_szlistDevice.ToArray();
                    COMBO_DEVICE.SelectedIndex = 0;

                    for (var i = 0; i < m_szlistDevice.Count; i++)
                        WriteLog("Device: " + m_szlistDevice[i] + "   " + "Serial Number: " + szlistSerialNumber[i]);
                }
                catch
                {
                    DispatcherWriteLog(@"Status:[LibWFX_GetDeviesList Fail]");
                }

            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES)
                DispatcherWriteLog(@"Status:[No Device]");
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL)
                DispatcherWriteLog(@"Status:[Load MRTD DLL Fail]");
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING)
                WriteLog(@"Status:[Scanning Fail]");
            else
                DispatcherWriteLog(@"Status:[LibWFX_GetDeviesList Fail]");
        }
       
        private void BTN_PAPER_READY_Click(object sender, RoutedEventArgs e)
        {
            String szCommand;

            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_PaperReady();

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Paper is ready!");
            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PAPER_NOT_READY)
            {
                DispatcherWriteLog(@"Paper is NOT ready!");
            }
            else
            {
                DispatcherWriteLog(@"Status:[LibWFX_PaperReady Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
        }

        private void BTN_PAPER_STATUS_Click(object sender, RoutedEventArgs e)
        {
            ENUM_LIBWFX_EVENT_CODE enPaperStatus;
            String szCommand;

            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_GetPaperStatus(out enPaperStatus);

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                DispatcherWriteLog(@"Status:[LibWFX_GetPaperStatus Success][" + ((int)enPaperStatus).ToString() + "]");
            else
                DispatcherWriteLog(@"Status:[LibWFX_GetPaperStatus Fail [" + ((int)m_enErrCode).ToString() + "][" + ((int)enPaperStatus).ToString() + "]]");

        }

        private void BTN_ECO_Click(object sender, RoutedEventArgs e)
        {
            uint ulTime = 0;
            String szCommand;

            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_ECOControl(out ulTime, 0);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_ECOControl Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            FormECO windowEco = new FormECO(ulTime);
            windowEco.Show();
           m_DeviceWrapper.m_pfnLibWFX_ECOControl(out ulTime, 1);
        }

        private void BTN_CALIBRATE_Click(object sender, RoutedEventArgs e)
        {
            String szCommand;

            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_Calibrate();

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Calibrate Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
            else
            {
                DispatcherWriteLog(@"Status:[LibWFX_Calibrate Success");
            }
        }

        private void BTN_MERGETOPDF_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter =
            "Images (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|" +
            "All files (*.*)|*.*";

            openFileDlg.Multiselect = true;
            openFileDlg.Title = "My Image Browser";

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                string strImgList = "";
                // Read the files
                foreach (string strFile in openFileDlg.FileNames)
                {
                    strImgList += strFile;
                    strImgList += "*";
                }

                String szCommand;

                if (COMBO_COMMAND.SelectedIndex == -1)
                    szCommand = COMBO_COMMAND.Text;
                else
                    szCommand = COMBO_COMMAND.SelectedItem.ToString();

                m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
                if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                {
                    DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                    return;
                }

                m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_MergeToPdf(strImgList);

                if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                    WriteLog(@"Status:[LibWFX_MergeToPdf Success]");
                else
                    WriteLog(@"Status:[LibWFX_MergeToPdf Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
        }

        private void BTN_EJECT_PAPER_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pstr;
            String szCommand;

            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]]");
                return;
            }

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_EjectPaperControlWithMsg(CHK_EJECT_DIRECT.IsChecked == true ? ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING : ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING, out pstr);
            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING)
            {
                DispatcherWriteLog(@"Status:[Scanning Fail]");
                return;
            }

            string szErrorMsg = Marshal.PtrToStringUni(pstr);

            if (szErrorMsg.Length > 0)
                DispatcherWriteLog(szErrorMsg);
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                DispatcherWriteLog(@"Status:[LibWFX_EjectPaperControl Success]");
            else
                DispatcherWriteLog(@"Status:[LibWFX_EjectPaperControl Fail [" + ((int)m_enErrCode).ToString() + "]]");
        }

        private void BTN_EDIT_Click(object sender, RoutedEventArgs e)
        {
            IntPtr ptCommandOut;
            String szCommand, szRtn;
            if (COMBO_COMMAND.SelectedIndex == -1)
                szCommand = COMBO_COMMAND.Text;
            else
                szCommand = COMBO_COMMAND.SelectedItem.ToString();


            m_DeviceWrapper.m_pfnLibWFX_EditCommand(szCommand, out ptCommandOut);
            szRtn = Marshal.PtrToStringUni(ptCommandOut);

            if ((szRtn != String.Empty) && (szRtn.Length > 0))
            {
                List<string> commandLists = new List<string>();
                commandLists.Add(szRtn);
                for (int idx = 0; idx < COMBO_COMMAND.Items.Count; idx++)
                {
                    if (idx == (m_MaxCMDItems - 1))
                        break;

                    COMBO_COMMAND.SelectedIndex = idx;
                    commandLists.Add(COMBO_COMMAND.SelectedItem.ToString());
                }
                COMBO_COMMAND.Items.Clear();
                for (int idx = 0; idx < commandLists.Count; idx++)
                {
                    COMBO_COMMAND.Items.Add(commandLists[idx]);
                }
                COMBO_COMMAND.SelectedIndex = 0;
            }
        }

        private void COMBO_DEVICE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetJsonCmd(COMBO_DEVICE.SelectedItem.ToString());
        }

        private void DispatcherLoadImage(String strPath)
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<String>(LoadImage), strPath);
            }
            else
            {
                LoadImage(strPath);
            }
        }

        private void LoadImage(String strPath)
        {
            m_nCount++;

            FileStream fstream = new FileStream(strPath, FileMode.Open);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = fstream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            fstream.Close();

            //HandleDrawImage(bmpImage);
            if (m_nCount % 2 == 1)
                HandleDrawImage(bitmap, true);
            else
                HandleDrawImage(bitmap, false);

            GC.Collect();
        }

        private void GetCertificatePermission()
        {
            IntPtr pstr;

            ENUM_LIBWFX_ERRCODE enErrCode =m_DeviceWrapper.m_pfnLibWFX_GetCertificatePermission(out pstr, ENUM_PERMISSION_DATA_TYPE.LIBWFX_DATA_TYPE_REGINFO);
            if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                string szPermission = Marshal.PtrToStringUni(pstr);

                if (szPermission != "")
                    DispatcherWriteLog("License: " + szPermission);
                else
                    DispatcherWriteLog("License: none");

            }
            else
            {
                DispatcherWriteLog("Status:[LibWFX_GetCertificatePermission Fail [" + ((int)enErrCode).ToString() + "]]");
            }
        }       

        private void BTN_SCAN_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pScanImageList, pOCRResultList, pExceptionRet, pEventRet;
            string command;

            if (COMBO_COMMAND.SelectedIndex == -1)
                command = COMBO_COMMAND.Text;
            else
                command = COMBO_COMMAND.SelectedItem.ToString();

            if (command.IndexOf("\"autoscan\":true") != -1)
            {
                MessageBox.Show("BlockScan do not support autoscan!! If you want to implement autoscan, please refer to the AutoCaptureDemo-CSharp.]", "Warning");
                return;
            }

            ShowScanningDlg(true);
            ENUM_LIBWFX_ERRCODE enErrCode =m_DeviceWrapper.m_pfnLibWFX_SynchronizeScan(command, out pScanImageList, out pOCRResultList, out pExceptionRet, out pEventRet);
            ShowScanningDlg(false);

            string szExceptionRet = Marshal.PtrToStringUni(pExceptionRet);
            string szEventRet = Marshal.PtrToStringUni(pEventRet);

            if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS && enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
            {
                IntPtr pstr;
               m_DeviceWrapper.m_pfnLibWFX_GetLastErrorCode(enErrCode, out pstr);
                string szErrorMsg = Marshal.PtrToStringUni(pstr);
                WriteLog(@"Status:[LibWFX_SynchronizeScan Fail [" + ((int)enErrCode).ToString() + "]] " + szErrorMsg.ToString()); //get fail message
            }
            else if (szEventRet.Length > 1) //event happen
            {
                SetCommandString(COMBO_DEVICE.SelectedItem.ToString(), command);
                WriteLog(@"Status:[Device Ready!]");
                WriteLog(szEventRet);  //get event message

                if (szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]" && szEventRet != "LIBWFX_EVENT_NO_PAPER")
                {
                    WriteLog(@"Status:[Scan End]");
                    return;
                }

                if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                    WriteLog(@"Status:[There are some mismatched key in command]");

                string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);
                string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                {
                    WriteLog(ScanImageWords[idx].Trim());  //get each image path
                    WriteLog(OCRResultWords[idx].Trim());  //get each ocr result

                    if (!ScanImageWords[idx].Contains(".pdf") && !ScanImageWords[idx].Contains(".tif") && !ScanImageWords[idx].Equals("CustomPhotoZone") && ScanImageWords[idx].Trim() != String.Empty)
                        DispatcherLoadImage(ScanImageWords[idx]);
                }
            }
            else
            {
                SetCommandString(COMBO_DEVICE.SelectedItem.ToString(), command);
                WriteLog(@"Status:[Device Ready!]");

                if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                    WriteLog(@"Status:[There are some mismatched key in command]");

                if (szExceptionRet.Length > 1) //exception happen
                {
                    WriteLog(szExceptionRet);  //get exception message
                }

                string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);
                string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                {
                    WriteLog(ScanImageWords[idx].Trim());  //get each image path
                    WriteLog(OCRResultWords[idx].Trim());  //get each ocr result

                    if (!ScanImageWords[idx].Contains(".pdf") && !ScanImageWords[idx].Contains(".tif") && !ScanImageWords[idx].Equals("CustomPhotoZone") && ScanImageWords[idx].Trim() != String.Empty)
                        DispatcherLoadImage(ScanImageWords[idx]);
                }
            }
            WriteLog(@"Status:[Scan End]");
        }

        private void ShowScanningDlg(bool enableDlg)
        {
            if (enableDlg)
            {
                formScanning = new FormScanning();
                formScanning.Show();
                this.Hide();
            }
            else if (formScanning != null)
            {
                formScanning.Close();
                formScanning = null;
                this.Show();
            }
        }

        private void BTN_RECYCLESAVEFOLDER_Click(object sender, RoutedEventArgs e)
        {
            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_RecycleSaveFolder();

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_RecycleSaveFolder Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
            else
            {
                DispatcherWriteLog(@"Status:[LibWFX_RecycleSaveFolder Success]");
            }
        }
    }    
}
