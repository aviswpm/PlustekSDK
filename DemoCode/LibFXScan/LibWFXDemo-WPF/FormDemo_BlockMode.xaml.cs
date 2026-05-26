//#define BC_USDL_FIXFIELDVALUE   //option
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Diagnostics;
using System.Text;

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
        FormWaitMsg formWaitMsg = null;
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
            if (m_DeviceWrapper.hLibModule == IntPtr.Zero || m_DeviceWrapper.hCommandModule == IntPtr.Zero)
                Environment.Exit(0);

            ShowDlg(true, "Init");
           
            if (m_DeviceWrapper.m_pfnLibWFX_IsWindowExist("") == true)
            {
                ShowDlg(false, "Init");
                MessageBox.Show("Status:[Please confirm whether the \"CheckWindowTitle\" parameter content in LibWebFxScan.ini are all closed!!]", "Warning");
                DispatcherWriteLog(@"[ Warning ] " + ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SPECIFIC_AP_OPENING.ToString() + " - [" + ((int)ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SPECIFIC_AP_OPENING).ToString() + "]");
                DispatcherWriteLog(@"[ Warning ] " + ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT.ToString() + " - [" + ((int)ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT).ToString() + "]");          
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
            else
            {
                if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG)
                    DispatcherWriteLog(@"Status:[Path Is Too Long (max limit: 130 bits)]");
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
            }
            ShowDlg(false, "Init");
        }
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
        private void FormDemo_FormClosing(object sender, CancelEventArgs e)
        {         
            if (formWaitMsg != null)
            {
                formWaitMsg.Close();
                formWaitMsg = null;
            }

            if (m_DeviceWrapper.hLibModule != IntPtr.Zero)
            {
                m_DeviceWrapper.m_pfnLibWFX_CloseDevice();
                m_DeviceWrapper.m_pfnLibWFX_DeInit();
                FreeLibrary(m_DeviceWrapper.hLibModule);
            }

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
            else
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;

                ENUM_SOURCETYPE enSource;
                bool bDuplex = false, bJpegTransfer = false, bLongPaper = false;
                int nDPI = 0, nMaxPaperSizeX = -1, nMaxPaperSizeY = -1;

                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_GetDeviceCapability(szDevName, out enSource, out bDuplex, out bJpegTransfer, out nDPI, out nMaxPaperSizeX, out nMaxPaperSizeY, out bLongPaper);

                if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                {
                    DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                    return;
                }
                else
                {
                    if (enSource == ENUM_SOURCETYPE.CAMERA)
                        szDefJson += "\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";
                    else if (enSource == ENUM_SOURCETYPE.FLATBED)
                        szDefJson += "\",\"source\":\"Flatbed\"}";
                    else if (enSource == ENUM_SOURCETYPE.SHEETFED)
                        szDefJson += "\",\"source\":\"Sheetfed-Front\"}";
                    else if (enSource == ENUM_SOURCETYPE.ADF || enSource == ENUM_SOURCETYPE.ADF_FLATBED || enSource == ENUM_SOURCETYPE.ADF_SHEETFED)
                        szDefJson += "\",\"source\":\"ADF-Duplex\"}";
                    else
                        return;
                }
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
                System.IO.StreamReader file = new System.IO.StreamReader(szFilePath, Encoding.Default);
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
                using (StreamWriter outputFile = new StreamWriter(szFilePath, false, Encoding.Default))
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
                    DispatcherWriteLog(@"[ Warning ] " + ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES.ToString() + " - [" + ((int)ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES).ToString() + "]");
                }

            }
            else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT || m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL || m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING)
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
            else
                DispatcherWriteLog(@"[ Warning ] " + ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES.ToString() + " - [" + ((int)ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES).ToString() + "]");
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
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
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
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
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
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_GetPaperStatus(out enPaperStatus);

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                DispatcherWriteLog(@"Status:[LibWFX_GetPaperStatus Success][" + ((int)enPaperStatus).ToString() + "]");
            else
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
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
                DispatcherWriteLog(@"Status:[LibWFX_Setproperty Fail [" + ((int)m_enErrCode).ToString() + "]], " + m_enErrCode.ToString());
                return;
            }

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_ECOControl(out ulTime, 0);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                return;
            }

            FormECO windowEco = new FormECO(ulTime);
            windowEco.Show();
           m_DeviceWrapper.m_pfnLibWFX_ECOControl(out ulTime, 1);
        }

        private void BTN_CALIBRATE_Click(object sender, RoutedEventArgs e)
        {
            if (COMBO_DEVICE.Items.Count == 0)
            {
                DispatcherWriteLog(@"[ Warning ] " + ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES.ToString() + " - [" + ((int)ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES).ToString() + "]");
                return;
            }
            String szDevName = COMBO_DEVICE.SelectedItem.ToString();
            String szCommand = "";

            if (szDevName == "A61" || szDevName == "A62" || szDevName == "A63" || szDevName == "A64" || szDevName == "A65" || szDevName == "A66" || szDevName == "J6102" || szDevName == "J1204")
            {
                szCommand += "{\"device-name\":\"";
                szCommand += szDevName;
                szCommand += "\",\"source\":\"Camera\",\"ext-capturetype\":\"g\"}";
            }
            else
            {
                if (COMBO_COMMAND.SelectedIndex == -1)
                    szCommand = COMBO_COMMAND.Text;
                else
                    szCommand = COMBO_COMMAND.SelectedItem.ToString();
            }

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, null, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                return;
            }

            if (szDevName == "A64")
                ShowDlg(true, "Calibrate_xmini");
            else
                ShowDlg(true, "Calibrate_normal");

            m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_Calibrate();
            ShowDlg(false, "");

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
            }
            else
            {
                DispatcherWriteLog(@"Status:[LibWFX_Calibrate Success]");
            }
        }

        private void BTN_MERGETOPDF_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Image Files (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG||";
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
                    WriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                    return;
                }

                m_enErrCode =m_DeviceWrapper.m_pfnLibWFX_MergeToPdf(strImgList);

                if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                    WriteLog(@"Status:[LibWFX_MergeToPdf Success]");
                else
                    WriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
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
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
                return;
            }

            ENUM_LIBWFX_EJECT_DIRECTION enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING;
            if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject backwarding- force"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject forwarding- force"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject backwarding stop- force"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGS;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject forwarding stop- force"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGS;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject backwarding"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGD;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject forwarding"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGD;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject backwarding stop"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGSD;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject forwarding stop"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGSD;
            else if(COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject backwarding by steps"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING_BY_STEPS;
            else if (COMBO_EJECT_DIRECTION.SelectionBoxItem.ToString().Equals("eject forwarding by steps"))
                enEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING_BY_STEPS;

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, out pstr);
            string szErrorMsg = Marshal.PtrToStringUni(pstr);

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                IntPtr pErrorCode = Marshal.AllocHGlobal(260);
                m_DeviceWrapper.m_pfnLibWFX_GetLastErrorCode(m_enErrCode, pErrorCode);
                szErrorMsg = (pstr == IntPtr.Zero) ? "" : Marshal.PtrToStringUni(pErrorCode) ?? "";
                DispatcherWriteLog(@"[ Warning ] " + szErrorMsg + " - [" + ((int)m_enErrCode).ToString() + "]");
            }
            else if(szErrorMsg.Length > 0)
                DispatcherWriteLog(szErrorMsg);
            else
                DispatcherWriteLog(@"Status:[LibWFX_EjectPaperControl Success]");
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
                if (szRtn.Contains("Invalid JSON format"))
                {
                    MessageBox.Show("Invalid JSON format", "Warning");
                    return;
                }
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
            if (COMBO_DEVICE.SelectedItem != null)
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
			if (!File.Exists(strPath))
                return;
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
            IntPtr pstr = Marshal.AllocHGlobal(260);

            ENUM_LIBWFX_ERRCODE enErrCode =m_DeviceWrapper.m_pfnLibWFX_GetCertificatePermission(pstr, ENUM_PERMISSION_DATA_TYPE.LIBWFX_DATA_TYPE_REGINFO);
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
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
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

            ShowDlg(true, "Scan");
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SynchronizeScan(command, out pScanImageList, out pOCRResultList, out pExceptionRet, out pEventRet);
            ShowDlg(false, "");

            string szExceptionRet = Marshal.PtrToStringUni(pExceptionRet);
            string szEventRet = Marshal.PtrToStringUni(pEventRet);

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                IntPtr pstr = Marshal.AllocHGlobal(260);
                m_DeviceWrapper.m_pfnLibWFX_GetLastErrorCode(m_enErrCode, pstr);
                string szErrorMsg = Marshal.PtrToStringUni(pstr);
                WriteLog(@"[ Warning ] " + szErrorMsg + " - [" + ((int)m_enErrCode).ToString() + "]"); //get fail message
            }
            else if (szEventRet.Length > 1) //event happen
            {
                SetCommandString(COMBO_DEVICE.SelectedItem.ToString(), command);
                WriteLog(@"Status:[Device Ready!]");
                WriteLog(szEventRet);  //get event message

                if (szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]")
                {
                    WriteLog(@"Status:[Scan End]");
                    return;
                }
            
                string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);
                string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                int nMaxLength = Math.Max(ScanImageWords.Length, OCRResultWords.Length);
                for (int idx = 0; idx < nMaxLength - 1; idx++)
                {
                    if (idx < ScanImageWords.Length - 1)
                    {
                        WriteLog(ScanImageWords[idx].Trim());  //get each image path
                        if ((ScanImageWords[idx].Contains(".jpg") || ScanImageWords[idx].Contains(".png") || ScanImageWords[idx].Contains(".bmp")) && !ScanImageWords[idx].ToUpper().Contains("_PHOTO") && ScanImageWords[idx].Trim() != String.Empty)
                            DispatcherLoadImage(ScanImageWords[idx]);
                    }

                    if (idx < OCRResultWords.Length - 1)
                    {
#if BC_USDL_FIXFIELDVALUE
                        if (OCRResultWords[idx].Trim().Contains("\"IIN\":\"636028\""))
                            OCRResultWords[idx] = FixUSDLFieldValueToJsonFile(ScanImageWords[idx].Trim(), OCRResultWords[idx].Trim());               
#endif
                        WriteLog(OCRResultWords[idx].Trim());  //get each ocr result
                    }
                }
            }
            else
            {
                SetCommandString(COMBO_DEVICE.SelectedItem.ToString(), command);
                WriteLog(@"Status:[Device Ready!]");
             
                if (szExceptionRet.Length > 1) //exception happen
                {
                    WriteLog(szExceptionRet);  //get exception message
                }

                string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);
                string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                int nMaxLength = Math.Max(ScanImageWords.Length, OCRResultWords.Length);
                for (int idx = 0; idx < nMaxLength - 1; idx++)
                {
                    if (idx < ScanImageWords.Length - 1)
                    {
                        WriteLog(ScanImageWords[idx].Trim());  //get each image path
                        if ((ScanImageWords[idx].Contains(".jpg") || ScanImageWords[idx].Contains(".png") || ScanImageWords[idx].Contains(".bmp")) && !ScanImageWords[idx].ToUpper().Contains("_PHOTO") && ScanImageWords[idx].Trim() != String.Empty)
                            DispatcherLoadImage(ScanImageWords[idx]);
                    }

                    if (idx < OCRResultWords.Length - 1)
                    {
#if BC_USDL_FIXFIELDVALUE
                        if (OCRResultWords[idx].Contains("\"IIN\":\"636028\""))
                            OCRResultWords[idx] = FixUSDLFieldValueToJsonFile(ScanImageWords[idx].Trim(), OCRResultWords[idx].Trim());
#endif
                        WriteLog(OCRResultWords[idx].Trim());  //get each ocr result
                    }
                }
            }
            WriteLog(@"Status:[Scan End]");
        }

        private void ShowDlg(bool enableDlg, string szAction)
        {
            if (enableDlg)
            {              
                this.Hide();
                formWaitMsg = new FormWaitMsg(szAction);
                formWaitMsg.Show();
                formWaitMsg.Focus();
            }
            else
            {
                formWaitMsg.Close();
                formWaitMsg = null;
                this.Show();
            }
        }

        private void BTN_RECYCLESAVEFOLDER_Click(object sender, RoutedEventArgs e)
        {
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_RecycleSaveFolder();

            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"[ Warning ] " + m_enErrCode.ToString() + " - [" + ((int)m_enErrCode).ToString() + "]");
            }
            else
            {
                DispatcherWriteLog(@"Status:[LibWFX_RecycleSaveFolder Success]");
            }
        }

        private void BTN_REGISTER_Click(object sender, RoutedEventArgs e)
        {          
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = m_DeviceWrapper.m_szSdkInstallPath + m_DeviceWrapper.REGISTER_EXENAME;
         
            try
            {                
                Process process = Process.Start(startInfo);
            }
            catch (System.Exception exception)
            {                
                System.Console.WriteLine("An error occurred: " + exception.Message);
            }
        }

        private string FixUSDLFieldValueToJsonFile(String szFilePath, String szOCRData)
        {
            try
            {
                string szdirectory = Path.GetDirectoryName(szFilePath) ?? "";
                string szJsonPath = szdirectory + "\\" + Path.GetFileNameWithoutExtension(szFilePath) + "_OCR.json";
                JObject objroot = JObject.Parse(szOCRData);
                var dataToken = objroot.SelectToken("PDF417.Data") as JObject;

                if (dataToken != null)
                {
                    var cardExpiry = dataToken["CardExpiryDate"];
                    if (cardExpiry != null)
                    {
                        dataToken["ExpiryDateExt"] = "20" + cardExpiry.ToString();                                       
                        string szfinalJson = objroot.ToString(Newtonsoft.Json.Formatting.None);
                        if (File.Exists(szJsonPath))                     
                            File.WriteAllText(szJsonPath, szfinalJson);
                        return szfinalJson;
                    }
                }
            }
            catch (Exception ex)
            {
                DispatcherWriteLog("[Error] JSON Process Fail: " + ex.Message);
            }
            return szOCRData;
        }
    }    
}
