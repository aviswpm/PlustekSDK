package adview.webfxscan;
import java.awt.EventQueue;

public class DemoFrame {
	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {

		EventQueue.invokeLater(new Runnable() {
			public void run() {						
				String	directoryName = System.getProperty("user.dir");				
				LoadINIData rc = new LoadINIData(directoryName + "\\LibWebFxScan.ini");
			    String UseModeBlock = rc.getValue("UseModeBlock");
		        
			   
		    	
	    		
			    if(UseModeBlock.equals("0"))
			    {
			    	try {
							DemoFrame_NonBlockMode window = new DemoFrame_NonBlockMode();
							window.frmWfxdemo.setVisible(true);
					} catch (Exception e) {
							e.printStackTrace();
							System.exit(1);
					}
			    }
			    else
			    {
			    	try {
			    		  DemoFrame_BlockMode window = new DemoFrame_BlockMode();
			    		  window.frmWfxdemo.setVisible(true);			    		  			    						    		
				    		
					} catch (Exception e) {
						e.printStackTrace();
						System.exit(1);
					}
			    }			
			}
		});
	}

	/**
	 * Create the application.
	 */
	public DemoFrame() {
		initialize();		
	}

	/**
	 * Initialize the contents of the frame.
	 */
	private void initialize() {
	}	
}


