package adview.webfxscan;
import java.awt.Toolkit;
import javax.swing.*;
import java.awt.Window.Type;

public class DemoFrame_WaitMsgDlg extends JFrame {
    
    public DemoFrame_WaitMsgDlg(String text) {
    	setType(Type.UTILITY);
    	setAlwaysOnTop(true);
    	  
    	String szMsg = "";
    	if(text == "Scan")
    	{
    		setSize(180, 20);
    		szMsg = "Scanning...Please Wait";
    	}
    	else if(text == "Calibrate_normal")
    	{
    		setSize(360, 20);
    		szMsg = "Calibration in progress...    Please wait 3~40 seconds";     		
    	}
    	else if(text == "Calibrate_xmini")
    	{
    		setSize(640, 20);
    		szMsg = "Note: The calibration process may take up to 40 seconds to complete. During this time, you may hear beeps."; 
    	}
    	else if(text == "Init")
    	{
    		setSize(180, 20);
    		szMsg = "Init...Please Wait"; 
    	}
    				
    	setTitle(szMsg);
    	setLocation((Toolkit.getDefaultToolkit().getScreenSize().width  - 773) / 2, (Toolkit.getDefaultToolkit().getScreenSize().height - 517) / 2);    	
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);                            
        setVisible(true);        
    }
   
}


