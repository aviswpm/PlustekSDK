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
    public partial class FormDemo_NonBlockMode : Window
    {
        ENUM_LIBWFX_ERRCODE m_enErrCode;
        DeviceWrapper m_DeviceWrapper = new DeviceWrapper();
        static DeviceWrapper.LIBWFXEVENTCB m_CBEvent;
        static DeviceWrapper.LIBWFXCB m_CBNotify;
        int m_nCount;

        private List<String> m_szlistDevice;
        private List<String> m_szlistFile;
        private Boolean m_bIPexception;
        private int m_nWarmupTotalTime;
        private int m_MaxCMDItems;
        private string m_command;

        public void LibWFXCallBack_Event(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, IntPtr pUserDef)
        {
            //MainWindow form = (MainWindow)HwndSource.FromHwnd(pUserDef).RootVisual;
            FormDemo_NonBlockMode form = (FormDemo_NonBlockMode)HwndSource.FromHwnd(pUserDef).RootVisual;
            switch (enEventCode)
            {
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_PAPER_DETECTED:
                    if (nParam == 0)
                        form.DispatcherWriteLog("LIBWFX_EVENT_PAPER_DETECTED");
                    else if (nParam == 1)
                        form.DispatcherWriteLog("LIBWFX_EVENT_PASSPORT_DETECTED");
                    else if (nParam == 2)
                        form.DispatcherWriteLog("LIBWFX_EVENT_CARD_DETECTED");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_NO_PAPER:
                    form.DispatcherWriteLog("LIBWFX_EVENT_NO_PAPER");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_PAPER_JAM:
                    form.DispatcherWriteLog("LIBWFX_EVENT_PAPER_JAM");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_MULTIFEED:
                    form.DispatcherWriteLog("LIBWFX_EVENT_MULTIFEED");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_NO_CALIBRATION_DATA:
                    form.DispatcherWriteLog("LIBWFX_EVENT_NO_CALIBRATION_DATA");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_WARMUP_COUNTDOWN:
                    form.HandleWarmupProgress(nParam);
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_SCAN_PROGRESS:
                    form.HandleScanProgress(nParam);
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_BUTTON_DETECTED:
                    //when press the scan button on machine, it's callback the number of control panel
                    form.DispatcherWriteLog(Convert.ToString(nParam));
                    form.HandleStartScan();
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_PAPER_FEEDING_ERROR:
                    form.DispatcherWriteLog("LIBWFX_EVENT_PAPER_FEEDING_ERROR");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_UVSECURITY_DETECTED:
                    if (nParam == 0)
                        form.DispatcherWriteLog("LIBWFX_EVENT_UVSECURITY_DETECTED[0]");
                    else
                        form.DispatcherWriteLog("LIBWFX_EVENT_UVSECURITY_DETECTED[1]");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_PLUG_UNPLUG:
                    form.DispatcherWriteLog("LIBWFX_EVENT_PLUG_UNPLUG");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_COVER_OPEN:
                    form.DispatcherWriteLog("LIBWFX_EVENT_COVER_OPEN");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_OVER_TIME_SCAN:
                    form.DispatcherWriteLog("LIBWFX_EVENT_OVER_TIME_SCAN");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_CANCEL_SCAN:
                    form.WriteLog("LIBWFX_EVENT_CANCEL_SCAN");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_CAMERA_RGB_DISLOCATION:
                    form.WriteLog("LIBWFX_EVENT_CAMERA_RGB_DISLOCATION");
                    break;
                case ENUM_LIBWFX_EVENT_CODE.LIBWFX_EVENT_CAMERA_TIMEOUT:
                    form.WriteLog("LIBWFX_EVENT_CAMERA_TIMEOUT");
                    break;
                default:
                    break;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        public static void LibWFXCallBack_Notify(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, IntPtr pUserDef, IntPtr pParam1, IntPtr pParam2)
        {
            //MainWindow form = (MainWindow)HwndSource.FromHwnd(pUserDef).RootVisual;
            FormDemo_NonBlockMode form = (FormDemo_NonBlockMode)HwndSource.FromHwnd(pUserDef).RootVisual;

            if (enNotifyCode == ENUM_LIBWFX_NOTIFY_CODE.LIBWFX_NOTIFY_IMAGE_DONE)
            {
                if (pParam1 != IntPtr.Zero)
                {
                    if (form.m_command.IndexOf("\"rawdata\":true") == -1)
                    {
                        String szPath = Marshal.PtrToStringUni(pParam1);
                        if (File.Exists(szPath) == false)
                            return;

                        form.DispatcherWriteLog(szPath);
                        if (!szPath.Contains(".pdf") && !szPath.Contains(".tif") && !szPath.Equals("CustomPhotoZone"))
                        {
                            form.DispatcherLoadImage(szPath);
                        }
                    }
                    else
                    {
                        ST_IMAGE_INFO stImgInfo = (ST_IMAGE_INFO)Marshal.PtrToStructure(pParam1, typeof(ST_IMAGE_INFO));
                        form.DrawImage(stImgInfo);
                    }
                }

                if (pParam2 != IntPtr.Zero)
                {
                    String szJson = Marshal.PtrToStringUni(pParam2);
                    form.DispatcherWriteLog(szJson);
                }
            }
            else if (enNotifyCode == ENUM_LIBWFX_NOTIFY_CODE.LIBWFX_NOTIFY_SHOWPATHONLY)
            {
                if (pParam1 != IntPtr.Zero)
                {
                    String szPath = Marshal.PtrToStringUni(pParam1);
                    form.DispatcherWriteLog(szPath);
                }
            }
            else if (enNotifyCode == ENUM_LIBWFX_NOTIFY_CODE.LIBWFX_NOTIFY_END)
            {
                form.DispatcherWriteLog("Status:[Scan End]");

                if ((form.m_command.IndexOf("\"device-name\":\"776U\"") != -1 || form.m_command.IndexOf("\"device-name\":\"777U\"") != -1 || form.m_command.IndexOf("\"device-name\":\"778U\"") != -1) && form.m_command.IndexOf("\"backward-eject\":true") != -1)
                    form.HandleEjectPaper(ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING);

                if ((form.m_command.IndexOf("\"device-name\":\"776U\"") != -1 || form.m_command.IndexOf("\"device-name\":\"777U\"") != -1 || form.m_command.IndexOf("\"device-name\":\"778U\"") != -1) && form.m_command.IndexOf("\"backward-eject\":false") != -1)
                    form.HandleEjectPaper(ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING);
#if DoResetIfExcept
                if (form.m_bIPexception)
		        {
                    form.HandleSetProperty();        
                    form.HandleStartScan();
                              
		        }
#endif
            }
            else if (enNotifyCode == ENUM_LIBWFX_NOTIFY_CODE.LIBWFX_NOTIFY_EXCEPTION)
            {
                form.m_bIPexception = false;
                ENUM_LIBWFX_EXCEPTION_CODE enCode = (ENUM_LIBWFX_EXCEPTION_CODE)pParam1;
                if (enCode == ENUM_LIBWFX_EXCEPTION_CODE.LIBWFX_EXC_TIFF_SAVE_FINSIHED || enCode == ENUM_LIBWFX_EXCEPTION_CODE.LIBWFX_EXC_PDF_SAVE_FINSIHED)
                {
                    String szLog;
                    szLog = Marshal.PtrToStringUni(pParam2) + "[SAVE_FINISHED]";
                    form.DispatcherWriteLog(szLog);
                }
                else if (pParam2 != IntPtr.Zero)
                {
                    String szMsg = Marshal.PtrToStringUni(pParam2);
                    form.DispatcherWriteLog(szMsg);

                    if (enCode == ENUM_LIBWFX_EXCEPTION_CODE.LIBWFX_EXC_IP_EXCEPTION)
                        form.m_bIPexception = true;
                }
            }
        }

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

        private void DrawImage(ST_IMAGE_INFO stImgInfo)
        {
            int nWidth = (int)stImgInfo.ulPixel;
            int nHeight = (int)stImgInfo.ulLine;
            uint nSize = stImgInfo.ulPerLawByte * stImgInfo.ulLine;
            int nStride = (int)stImgInfo.ulPerLawByte;

            System.Drawing.Imaging.ColorPalette palette = null;
            System.Drawing.Imaging.PixelFormat enColorFmt = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            switch (stImgInfo.enColorMode)
            {
                case ENUM_LIBWFX_COLOR_MODE.LIBWFX_COLOR_MODE_BW:
                    enColorFmt = System.Drawing.Imaging.PixelFormat.Format1bppIndexed;

                    palette = GetColorPalette(2);
                    palette.Entries[0] = System.Drawing.Color.FromArgb(255, 0, 0, 0);
                    palette.Entries[1] = System.Drawing.Color.FromArgb(255, 255, 255, 255);
                    break;
                case ENUM_LIBWFX_COLOR_MODE.LIBWFX_COLOR_MODE_GRAY:
                    enColorFmt = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

                    palette = GetColorPalette(256);
                    for (int nIdx = 0; nIdx < 256; nIdx++)
                    {
                        palette.Entries[nIdx] = System.Drawing.Color.FromArgb(255, nIdx, nIdx, nIdx);
                    }
                    break;
                case ENUM_LIBWFX_COLOR_MODE.LIBWFX_COLOR_MODE_COLOR:
                    enColorFmt = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                    break;
            }

            IntPtr copyRawData = IntPtr.Zero;
            copyRawData = Marshal.AllocHGlobal((int)nSize);
            CopyMemory(copyRawData, stImgInfo.pRawDate, nSize);


            Bitmap bmpImage = new Bitmap(nWidth, nHeight, nStride, enColorFmt, copyRawData);
            if (stImgInfo.enColorMode != ENUM_LIBWFX_COLOR_MODE.LIBWFX_COLOR_MODE_COLOR)
            {
                bmpImage.Palette = palette;
            }

            m_nCount++;
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                bmpImage.Dispose();
                ms.Close();
                ms.Dispose();
            }

            //HandleDrawImage(bmpImage);
            if (m_nCount % 2 == 1)
                HandleDrawImage(bitmapImage, true);
            else
                HandleDrawImage(bitmapImage, false);

            if (copyRawData != null)
            {
                Marshal.FreeHGlobal(copyRawData);
                copyRawData = IntPtr.Zero;
            }
            GC.Collect();
        }


        public FormDemo_NonBlockMode()
        {
            InitializeComponent();
            m_CBEvent = new DeviceWrapper.LIBWFXEVENTCB(LibWFXCallBack_Event);
            m_CBNotify = new DeviceWrapper.LIBWFXCB(LibWFXCallBack_Notify);
            m_szlistDevice = new List<String>();
            m_szlistFile = new List<String>();
            m_bIPexception = false;
            m_nWarmupTotalTime = 0;
            m_MaxCMDItems = 5;
            m_command = "";
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
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NOOCR);
#else
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);
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
                szDefJson += "\",\"source\":\"Camera\",\"autoscan\":true,\"recognize-type\":\"passport\"}";
            }
            else if (szDevName == "7C1U" || szDevName == "7C8U" || szDevName == "7C9U" || szDevName == "7CAU" || szDevName == "773U" || szDevName == "7CCU")
            {
                szDefJson += "{\"device-name\":\"";
                szDefJson += szDevName;
                szDefJson += "\",\"source\":\"Sheetfed-Duplex\",\"autoscan\":true,\"recognize-type\":\"passport\"}";
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
                szDefJson += "\",\"source\":\"Sheetfed-Front\",\"autoscan\":true}";
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

        private void HandleWarmupProgress(int nProgress)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                }));
            }
        }

        private void HandleScanProgress(int nProgress)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    PROGRESS_BAR.Value = nProgress;
                }));
            }
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

        private void HandleStartScan()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    BTN_SCAN_Click(null, null);
                }));
            }
            else
            {
                BTN_SCAN_Click(null, null);
            }
        }

        private void HandleSetProperty()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    BTN_SET_Click(null, null);
                }));
            }
            else
            {
                BTN_SET_Click(null, null);
            }
        }

        private void HandleEjectPaper(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirect)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    IntPtr pstr;
                    m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, out pstr);
                    string szErrorMsg = Marshal.PtrToStringUni(pstr);

                    if (szErrorMsg.Length > 0)
                        WriteLog(szErrorMsg);
                    else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                        WriteLog(@"Status:[LibWFX_EjectPaperControl Success]");
                    else
                        WriteLog(@"Status:[LibWFX_EjectPaperControl Fail [" + ((int)m_enErrCode).ToString() + "]]");
                }));
            }
            else
            {
                IntPtr pstr;
                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, out pstr);
                string szErrorMsg = Marshal.PtrToStringUni(pstr);

                if (szErrorMsg.Length > 0)
                    WriteLog(szErrorMsg);
                else if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                    WriteLog(@"Status:[LibWFX_EjectPaperControl Success]");
                else
                    WriteLog(@"Status:[LibWFX_EjectPaperControl Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
        }

        private void BTN_REFRESH_Click(object sender, RoutedEventArgs e)
        {
            ENUM_LIBWFX_ERRCODE m_enErrCode = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS;
            m_szlistDevice.Clear();
            IntPtr pstr = IntPtr.Zero;
            IntPtr pstr2 = IntPtr.Zero;
            m_enErrCode = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS;

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_GetDeviesListWithSerial(out pstr, out pstr2);

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

        private void BTN_SET_Click(object sender, RoutedEventArgs e)
        {
            if (COMBO_COMMAND.SelectedIndex == -1)
                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SetProperty(COMBO_COMMAND.Text, m_CBEvent, new WindowInteropHelper(this).Handle);
            else
                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SetProperty(COMBO_COMMAND.SelectedItem.ToString(), m_CBEvent, new WindowInteropHelper(this).Handle);

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING)
                WriteLog(@"Status:[LibWFX_SetProperty Fail [" + ((int)m_enErrCode).ToString() + "]] Scanning Fail");
            else if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS && m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
            {
                IntPtr pstr;
                m_DeviceWrapper.m_pfnLibWFX_GetLastErrorCode(m_enErrCode, out pstr);
                string szErrorMsg = Marshal.PtrToStringUni(pstr);
                DispatcherWriteLog(@"Status:[LibWFX_SetProperty Fail [" + ((int)m_enErrCode).ToString() + "]] " + szErrorMsg.ToString());
            }
            else
            {
                SetCommandString(COMBO_DEVICE.SelectedItem.ToString(), COMBO_COMMAND.Text);
                DispatcherWriteLog(@"Status:[Device Ready!]");

                m_command = COMBO_COMMAND.Text;

                if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                    DispatcherWriteLog(@"Status:[There are some mismatched key in command]");
            }
        }

        private void BTN_PAPER_READY_Click(object sender, RoutedEventArgs e)
        {
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_PaperReady();

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

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_GetPaperStatus(out enPaperStatus);

            if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                DispatcherWriteLog(@"Status:[LibWFX_GetPaperStatus Success][" + ((int)enPaperStatus).ToString() + "]");
            else
                DispatcherWriteLog(@"Status:[LibWFX_GetPaperStatus Fail [" + ((int)m_enErrCode).ToString() + "][" + ((int)enPaperStatus).ToString() + "]]");

        }

        private void BTN_ECO_Click(object sender, RoutedEventArgs e)
        {
            uint ulTime = 0;

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_ECOControl(out ulTime, 0);
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
            String szCommand = "{\"device-name\":\"A64\",\"source\":\"Camera\",\"ext-capturetype\":\"ir\"}";

            if (COMBO_DEVICE.SelectedItem.ToString() == "A64")
            {
                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, m_CBEvent, new WindowInteropHelper(this).Handle);
                if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                {
                    DispatcherWriteLog(@"Status:[LibWFX_Calibrate Fail [" + ((int)m_enErrCode).ToString() + "]]");
                    return;
                }
            }

            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_Calibrate();

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

                m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_MergeToPdf(strImgList);

                if (m_enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                    WriteLog(@"Status:[LibWFX_MergeToPdf Success]");
                else
                    WriteLog(@"Status:[LibWFX_MergeToPdf Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
        }

        private void BTN_EJECT_PAPER_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pstr;
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

            ENUM_LIBWFX_ERRCODE enErrCode = m_DeviceWrapper.m_pfnLibWFX_GetCertificatePermission(out pstr, ENUM_PERMISSION_DATA_TYPE.LIBWFX_DATA_TYPE_REGINFO);
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
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_StartScan(m_CBNotify, new WindowInteropHelper(this).Handle);
            if (m_enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                DispatcherWriteLog(@"Status:[LibWFX_StartScan Fail [" + ((int)m_enErrCode).ToString() + "]]");
            }
            //else
            //{
            //    DispatcherWriteLog(@"Start Scan!");
            //}
        }

        private void BTN_RECYCLESAVEFOLDER_Click(object sender, RoutedEventArgs e)
        {
            m_enErrCode = m_DeviceWrapper.m_pfnLibWFX_RecycleSaveFolder();

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
