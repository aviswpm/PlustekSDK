using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace AutoCaptureDemo_CSharp
{
    class Program
    {
        static System.Timers.Timer timer;
        static int interval = 200; // 0.2 seconds
        static int totalTime = 60 * 1000; // 60 seconds
        static int elapsedTime = 0; // Elapsed time in ms
        static DeviceWrapper.LIBWFXEVENTCB m_CBEvent;

        static void Main(string[] args)
        {           
            //do autocapture
            Thread thread1 = new Thread(AutoCapture);
            thread1.Start();

            //The window will automatically close after 60 seconds  then do deinit
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }


        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (elapsedTime > totalTime)
            {
                timer.Stop();
                 
                //Execute deinit when the window is closed
                DeviceWrapper.LibWFX_CloseDevice();
                DeviceWrapper.LibWFX_DeInit();
                Environment.Exit(0);
            }           
            elapsedTime += interval;
        }
       
        static public void AutoCapture()
        {
            ENUM_LIBWFX_ERRCODE enErrCode;
            bool DoScan = false;
            int timer = 0, sum = 0;
            IntPtr pScanImageList, pOCRResultList, pExceptionRet, pEventRet;
            string Command = "{\"device-name\":\"A64\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";

            //get command from bat file "AutoCaptureDemo-CSharp.bat"
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length > 1)           
                Command = arguments[1];

            enErrCode = DeviceWrapper.LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);

            if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR)          
                System.Console.WriteLine(@"Status:[No Recognize Tool]");
            else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR)
                System.Console.WriteLine(@"Status:[No AVI Recognize Tool]");   
            else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR)
                System.Console.WriteLine(@"Status:[No DOC Recognize Tool]");             
            else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG)
            {
                System.Console.WriteLine(@"Status:[Path Is Too Long (max limit: 130 bits)]");
                System.Console.WriteLine(@"Status:[LibWFX_InitEx Fail]");
            }
            else if(enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                System.Console.WriteLine(@"Status:[LibWFX_InitEx Fail [" + ((int)enErrCode).ToString() + "]] "); //get fail message
                return;                
            }

            enErrCode = DeviceWrapper.LibWFX_SetProperty(Command, m_CBEvent, IntPtr.Zero);
            if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                IntPtr pstr;
                DeviceWrapper.LibWFX_GetLastErrorCode(enErrCode, out pstr);
                string szErrorMsg = Marshal.PtrToStringUni(pstr);
                System.Console.WriteLine(@"Status:[LibWFX_SetProperty Fail [" + ((int)enErrCode).ToString() + "]] " + szErrorMsg.ToString()); //get fail message
            }

            while (true)
            {
                timer = 0;
                sum = 0;
                while (timer < 3)
                {
                    Thread.Sleep(100);
                    sum++;                  
                    enErrCode = DeviceWrapper.LibWFX_PaperReady();
                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                        timer++;

                    if (sum == 4)
                    {
                        sum = 0;
                        timer = 0;
                        if (DoScan)
                            DoScan = false;
                        System.Console.WriteLine(@"Please put the card");
                        Thread.Sleep(1000);  //option
                    }
                }

                if (DoScan)
                {
                    System.Console.WriteLine(@"The card is continuously detected, please remove the card.");
                    Thread.Sleep(1000);  //option
                    continue;
                }

                enErrCode = DeviceWrapper.LibWFX_SynchronizeScan(Command, out pScanImageList, out pOCRResultList, out pExceptionRet, out pEventRet);                

                string szExceptionRet = Marshal.PtrToStringUni(pExceptionRet);
                string szEventRet = Marshal.PtrToStringUni(pEventRet);

                if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS && enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                {
                    IntPtr pstr;
                    DeviceWrapper.LibWFX_GetLastErrorCode(enErrCode, out pstr);
                    string szErrorMsg = Marshal.PtrToStringUni(pstr);
                    System.Console.WriteLine(@"Status:[LibWFX_SynchronizeScan Fail [" + ((int)enErrCode).ToString() + "]] " + szErrorMsg.ToString()); //get fail message
                }               
                else if (szEventRet.Length > 1) //event happen
                {
                    System.Console.WriteLine(@"Status:[Device Ready!]");
                    System.Console.WriteLine(szEventRet);  //get event message

                    if (szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]")
                    {
                        System.Console.WriteLine(@"Status:[Scan End]\n");
                        return;
                    }

                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                        System.Console.WriteLine(@"Status:[There are some mismatched key in command]");

                    string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                    string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);


                    string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                    string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                    for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                    {
                        System.Console.WriteLine(ScanImageWords[idx].Trim());  //get each image path
                        System.Console.WriteLine(OCRResultWords[idx].Trim());  //get each ocr result
                    }
                }
                else
                {
                    System.Console.WriteLine(@"Status:[Device Ready!]");

                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                        System.Console.WriteLine(@"Status:[There are some mismatched key in command]");

                    if (szExceptionRet.Length > 1) //exception happen
                    {
                        System.Console.WriteLine(@"Status:[Device Ready!]");
                        System.Console.WriteLine(@szExceptionRet);  //get exception message
                    }

                    string szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                    string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);


                    string[] ScanImageWords = szScanImageList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);
                    string[] OCRResultWords = szOCRResultList.Split(new string[] { "|&|" }, System.StringSplitOptions.None);

                    for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                    {
                        System.Console.WriteLine(ScanImageWords[idx].Trim());  //get each image path
                        System.Console.WriteLine(OCRResultWords[idx].Trim());  //get each ocr result
                    }
                }
                System.Console.WriteLine(@"Status:[Scan End]");            
                DoScan = true;
            }
        }         
    }
}
