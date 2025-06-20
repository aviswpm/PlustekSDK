package adview.autocapture;
import java.lang.Thread;

public class AutoCaptureDemo extends Thread {
	static WebFXScan m_WFXScan;
	static WebFXScan.CString m_szCommand;
	static WebFXScan.CString m_szErrorMsg;
	static WebFXScan.CString m_szScanImageList;
	static WebFXScan.CString m_szOCRResultList;
	static WebFXScan.CString m_szExceptionRet;
	static WebFXScan.CString m_szEventRet;
	static EventCB m_EventCB;
	static int TIMEOUT;
	static boolean DoScan;
	
	class EventCB implements WebFXScan.WFXScanEventCallBack {
		public void DoCallBack(int enEventCode, final int nParam) {			
		}
	}
	
	public static void main(String[] args) {
		new AutoCaptureDemo().start();
	}
    
    public void run() {     
    	m_WFXScan = new WebFXScan();
    	m_EventCB = new EventCB();
    	//m_szCommand = m_WFXScan.new CString("{\"device-name\":\"A64\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}");
    	m_szCommand = m_WFXScan.new CString("{\"device-name\":\"7C1U\",\"source\":\"Sheetfed-Front\",\"filename-format\":\"test\",\"savepath\":\"C:\\\\clinic\"}");
    	
    	
    	m_szErrorMsg = m_WFXScan.new CString("");
    	m_szScanImageList = m_WFXScan.new CString("");
    	m_szOCRResultList = m_WFXScan.new CString("");
    	m_szExceptionRet = m_WFXScan.new CString("");
    	m_szEventRet = m_WFXScan.new CString("");    	
    	TIMEOUT = 60*1000;  //60sec
			
		int ret = m_WFXScan.WFXScan_InitEx(WebFXScan.ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);
			
		if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR == ret) {
			System.out.println("Status:[No Recognize tool]");			
		}
		else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR == ret) {
			System.out.println("Status:[No AVI Recognize tool]");
        }
		else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR == ret) {
			System.out.println("Status:[No DOC Recognize tool]");
        }
		else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG == ret) {
			System.out.println("Status:[Path Is Too Long (max limit: 130 bits)]");
			System.out.println("Status:[LibWFX_InitEx Fail [" + ret + "]]");
        }
		else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret){
			System.out.println("Status:[LibWFX_InitEx Fail [" + ret + "]]");
			return;
		}
		
		ret = m_WFXScan.WFXScan_SetProperty(m_szCommand.szValue, m_EventCB);
		
		if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret ) {
			m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);			
			System.out.println("Status:[LibWFX_SetProperty Fail [" + ret + "]]  " + m_szErrorMsg.szValue);							
		}
		
		int timer = 0, sum = 0;
		int totaltime = 0;
		
		while(totaltime < TIMEOUT) {
		
			timer = 0;
			sum = 0;
			while(timer < 3) {
				try {		           
		            Thread.sleep(300);          
		        }catch (InterruptedException ex) {
		            System.out.println("sleep error");
		        }

				totaltime += 300;
				sum++;
				ret = m_WFXScan.WFXScan_PaperReady();
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS == ret) {
				
					timer++;
				}
				
				if(sum == 4) {
					sum = 0;
					timer = 0;
					if (DoScan) {
						DoScan = false;
					}
					System.out.println("Please put the card."); 
					try {		           
			            Thread.sleep(1000);  //option          
			        }catch (InterruptedException ex) {
			            System.out.println("sleep error");
			        }
					totaltime += 1000;
				}
			}
			
			if (DoScan) {
				System.out.println("The card is continuously detected, please remove the card.");
				try {		           
		            Thread.sleep(1000);  //option          
		        }catch (InterruptedException ex) {
		            System.out.println("sleep error");
		        }
				totaltime += 1000;
				continue;
			}
			
			ret = m_WFXScan.WFXScan_SynchronizeScan(m_szCommand.szValue, m_szScanImageList, m_szOCRResultList, m_szExceptionRet, m_szEventRet);			

			if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret && WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH != ret) {
				m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);			
				System.out.println("Status:[LibWFX_SynchronizeScan Fail [" + ret + "]]  " + m_szErrorMsg.szValue);  //get fail message							
			}
			else {
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH == ret)
					System.out.println("Status:[LibWFX_SetProperty Warning : There are some mismatched key in command]");
				
				String szExceptionRet = new String(m_szExceptionRet.szValue);
			    String szEventRet = new String(m_szEventRet.szValue);
				    
				
				if(szEventRet.length() > 1) {  //event happen			
					System.out.println("Status:[Device Ready!]");
					System.out.println(szEventRet);   //get event message
					
					if(szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]") {
		                System.out.println("Status:[Scan End]");
		                continue;
		            }
		                
		            System.out.println("Status:[LibWFX_SynchronizeScan Success]");
											
					String szScanImageList = new String(m_szScanImageList.szValue);
					String szOCRResultList = new String(m_szOCRResultList.szValue);
					String ScanImageList[] = szScanImageList.split("]");
					String OCRResultLis[] = szOCRResultList.split("]");
					for(int i=0; i< ScanImageList.length; i++) {
						 System.out.println(ScanImageList[i]);   //get each image path
					   	 System.out.println(OCRResultLis[i]);    //get each ocr result
					}
				}
				else {
					System.out.println("Status:[LibWFX_SynchronizeScan Success]");
								
					if(szExceptionRet.length() > 1) { //exception happen				
						System.out.println("Status:[Device Ready!]");
						System.out.println(szExceptionRet);   //get exception message
					}
					
					String szScanImageList = new String(m_szScanImageList.szValue);
					String szOCRResultList = new String(m_szOCRResultList.szValue);
					String ScanImageList[] = szScanImageList.split("]");
					String OCRResultLis[] = szOCRResultList.split("]");
					for(int i=0; i< ScanImageList.length; i++) {
					  	 System.out.println(ScanImageList[i]);   //get each image path
					   	 System.out.println(OCRResultLis[i]);    //get each ocr result
					}	   
				}			
			}
			
			System.out.println("Status:[Scan End]");
			DoScan = true;
		}
		
		//do de-init before end
		m_WFXScan.WFXScan_CloseDevice();
		m_WFXScan.WFXScan_DeInit();
		System.exit(1);
	}
}


