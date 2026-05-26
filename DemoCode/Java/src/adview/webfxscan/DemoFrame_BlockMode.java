package adview.webfxscan;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics2D;

import javax.imageio.ImageIO;
import javax.swing.border.LineBorder;

import java.awt.Image;
import java.awt.Toolkit;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.awt.event.ActionEvent;

import org.json.*;
import adview.webfxscan.WebFXScan.ENUM_LIBWFX_EJECT_DIRECTION;
import adview.webfxscan.WebFXScan.ENUM_PERMISSION_DATA_TYPE;

import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.Window.Type;

import java.io.BufferedWriter;
import java.io.BufferedReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.awt.Dimension;

import java.awt.*;
import javax.swing.border.TitledBorder;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.*;


public class DemoFrame_BlockMode  {
	JFrame frmWfxdemo;
	DemoFrame_WaitMsgDlg window;
	static JTextArea txtArStatus;
	static JLabel labelPreview1;
	static JLabel labelPreview2;
	JButton btnRefresh;
	JComboBox<String> comboDeviceList;
	JComboBox<String> comboScanCmd;
		
	static WebFXScan m_WFXScan;
	WebFXScan.CString m_szDeviceList;
	WebFXScan.CString m_szSerialNumberList;
	WebFXScan.CString m_szFileList;
	WebFXScan.CString m_szErrorMsg;
	WebFXScan.CString m_szScanImageList;
	WebFXScan.CString m_szOCRResultList;
	WebFXScan.CString m_szExceptionRet;
	WebFXScan.CString m_szEventRet;
	WebFXScan.CString m_szPermissionTypeList;
	WebFXScan.CLong m_longTime;
	WebFXScan.CLong m_nPaperStatus;

	static int m_nCount;
	String m_szDevice;
	String m_szIDType;
	String m_FileListIn;
	String m_szCommand;
	
	private JButton btnCalibrate;
	private JButton btnExit;
	private JLabel label;
	private JTextField textECOTime;
	private JLabel label_1;
	private JButton button;
	private JButton btnEjectPaper;
	private JButton btnPaperStatus;
	private JTextField editor;
	private int m_nMaxCMDItems;
	private JButton btnScan;
	private JButton btnPaperready;
	private JButton btnRegister;
	private JComboBox comboEjectDirection;


	/**
	 * Launch the application.
	 */
	/*
	 public static void main(String[] args) {
	 

		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					DemoFrame_BlockMode window = new DemoFrame_BlockMode();
					window.frmWfxdemo.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
					System.exit(1);
				}
			}
		});
	}
	*/
	/**
	 * Create the application.
	 */
	public DemoFrame_BlockMode() {
		initialize();
		m_WFXScan = new WebFXScan();
		m_szDeviceList = m_WFXScan.new CString("");
		m_szSerialNumberList = m_WFXScan.new CString("");
		m_szFileList = m_WFXScan.new CString("");
		m_szErrorMsg = m_WFXScan.new CString("");
		m_szScanImageList = m_WFXScan.new CString("");
		m_szOCRResultList = m_WFXScan.new CString("");
		m_szExceptionRet = m_WFXScan.new CString("");
		m_szEventRet = m_WFXScan.new CString("");
		m_szPermissionTypeList = m_WFXScan.new CString("");
		m_longTime = m_WFXScan.new CLong(0);
		m_nPaperStatus = m_WFXScan.new CLong(0);
		m_nCount = 0;
		m_szDevice = "";
		m_szIDType = "";
		m_nMaxCMDItems = 5;
	
		DemoFrame_WaitMsgDlg window = new DemoFrame_WaitMsgDlg("Init");
    	window.setVisible(true);		    	
    	frmWfxdemo.setVisible(false);

		int ret = m_WFXScan.WFXScan_InitEx(WebFXScan.ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);

		if(m_WFXScan.WFXScan_IsWindowExist("") == true) {		
			javax.swing.JOptionPane.showMessageDialog(null, "Please confirm whether the \"CheckWindowTitle\" parameter content in LibWebFxScan.ini are all closed!!");
			m_WFXScan.WFXScan_GetLastErrorCode(WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SPECIFIC_AP_OPENING, m_szErrorMsg);
			WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SPECIFIC_AP_OPENING + "]\n");
			m_WFXScan.WFXScan_GetLastErrorCode(WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT, m_szErrorMsg);
			WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT + "]\n");
			//btnRefresh.doClick();
		}
		else
		{
			if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS == ret) {
				WriteLog("Status:[LibWFX_InitEx Success]\n");
				btnRefresh.doClick();
				GetCertificatePermission();
			}
			else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR == ret) {
				WriteLog("Status:[No Recognize tool]\n");
				btnRefresh.doClick();
				GetCertificatePermission();
			}
			else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR == ret) {
	            WriteLog("Status:[No AVI Recognize tool]\n");
	            btnRefresh.doClick();
	            GetCertificatePermission();
	        }
			else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR == ret) {	          
				WriteLog("Status:[No DOC Recognize tool]\n");
	            btnRefresh.doClick();
	            GetCertificatePermission();
	        }
			else
			{
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG == ret)
		            WriteLog("Status:[Path Is Too Long (max limit: 130 bits)]\n");
				m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
				WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
			}
		}
		window.setVisible(false);
		window.dispose();
		frmWfxdemo.setVisible(true);
	}

	/**
	 * Initialize the contents of the frame.
	 */
	private void initialize() {
		frmWfxdemo = new JFrame();
		frmWfxdemo.setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
		frmWfxdemo.setType(Type.UTILITY);
		frmWfxdemo.addWindowListener(new WindowAdapter() {
			@Override
		    public void windowClosing(WindowEvent e) {
				m_WFXScan.WFXScan_CloseDevice();
				m_WFXScan.WFXScan_DeInit();
		        System.exit(0);
		    }
		});
		frmWfxdemo.setResizable(false);
		frmWfxdemo.setTitle("Demo - Java");
		frmWfxdemo.setLocation((Toolkit.getDefaultToolkit().getScreenSize().width  - 773) / 2, (Toolkit.getDefaultToolkit().getScreenSize().height - 517) / 2);
		frmWfxdemo.setSize(773, 610);
		frmWfxdemo.getContentPane().setBackground(new Color(255, 255, 255));
		frmWfxdemo.getContentPane().setLayout(null);
		

		JLabel lbDeviceList = new JLabel("Device List");
		lbDeviceList.setBounds(8, 22, 96, 23);
		lbDeviceList.setFont(new Font("Consolas", Font.PLAIN, 14));
		frmWfxdemo.getContentPane().add(lbDeviceList);

		comboDeviceList = new JComboBox<String>();
		comboDeviceList.setForeground(new Color(0, 0, 0));
		comboDeviceList.setBackground(new Color(204, 204, 204));
		comboDeviceList.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				String szCommandtmp = "";
				
				m_szDevice = String.valueOf(comboDeviceList.getSelectedItem());
				if(GetCommandString() == true)
				{
					return;
				}
				else
				{		
					int[] caps = new int[7];
					int ret = m_WFXScan.WFXScan_GetDeviceCapability(m_szDevice, caps);
					if(ret == WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
					{
						if(caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.CAMERA)
							szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";
						else if(caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.SHEETFED)
							szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Sheetfed-Front\"}";
						else if(caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.FLATBED)
							szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Flatbed\"}";
						else if(caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.ADF || caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.ADF_FLATBED || caps[0] == (int)WebFXScan.ENUM_SOURCETYPE.ADF_SHEETFED)
							szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"ADF-Duplex\"}";
						else
							return;
					}
					else
					{
						m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
						WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
						return;
					}
						
				}
			/*	else if (m_szDevice.equals("7C1U") || m_szDevice.equals("7C8U") || m_szDevice.equals("7C9U") || m_szDevice.equals("7CAU") || m_szDevice.equals("773U") || m_szDevice.equals("7CCU")) 
				{
					szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Sheetfed-Duplex\",\"recognize-type\":\"passport\"}";
				} 
				else if (m_szDevice.equals("776U") || m_szDevice.equals("777U") || m_szDevice.equals("778U")) 
                {
                    szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Sheetfed-Duplex\"}";
                } 
				else if (m_szDevice.equals("A61") || m_szDevice.equals("A62")  || m_szDevice.equals("A63") || m_szDevice.equals("A64") || m_szDevice.equals("A65") || m_szDevice.equals("A66") || m_szDevice.equals("J6102"))
				{
					szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";
				} 
				else if (m_szDevice.equals("74RU") || m_szDevice.equals("74BU")  || m_szDevice.equals("7P1U")  || m_szDevice.equals("M11U") || m_szDevice.equals("7B3U") || m_szDevice.equals("M12U") || m_szDevice.equals("FE5020") || m_szDevice.equals("7B7U") || m_szDevice.equals("FE5030")) 
				{
					szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Sheetfed-Front\"}";
				}
				else if (m_szDevice.equals("256U") ||
						 m_szDevice.equals("258U") ||
						 m_szDevice.equals("258U_259U") ||
						 m_szDevice.equals("25AU") ||
				         m_szDevice.equals("271U") ||
				         m_szDevice.equals("273U") ||
				         m_szDevice.equals("273U_274U") ||
				         m_szDevice.equals("275U") ||
				         m_szDevice.equals("276U") ||
				         m_szDevice.equals("261U") ||
				         m_szDevice.equals("BAG")  ||
				         m_szDevice.equals("7K1U") ||
				         m_szDevice.equals("6C6U") ||
				         m_szDevice.equals("BB1U") ||
				         m_szDevice.equals("BAGU") ||
				         m_szDevice.equals("2B2U") ||
				         m_szDevice.equals("2B3U") ||
					     m_szDevice.equals("7N1U") ||
					     m_szDevice.equals("2D1U") ||
					     m_szDevice.equals("2C1U") ||
					     m_szDevice.equals("797U") ||
					     m_szDevice.equals("7K7U") ||
					     m_szDevice.equals("2G1U") ||
					     m_szDevice.equals("2G2U") ||
					     m_szDevice.equals("678U") ||
					     m_szDevice.equals("7K8U") ||
					     m_szDevice.equals("B85U") ||
					     m_szDevice.equals("2D3U"))
				{
					szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Flatbed\"}";
				}
				else
				{
					szCommandtmp = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"ADF-Front\"}";
				}*/
				comboScanCmd.removeAllItems();
				comboScanCmd.addItem(szCommandtmp);
				comboScanCmd.setSelectedIndex(0);
			}
		});
		comboDeviceList.setBounds(106, 19, 169, 28);
		comboDeviceList.setFont(new Font("Consolas", Font.PLAIN, 14));
		frmWfxdemo.getContentPane().add(comboDeviceList);

		btnRefresh = new JButton("Refresh");
		btnRefresh.setBackground(new Color(204, 204, 204));
		btnRefresh.setForeground(new Color(0, 0, 0));
		btnRefresh.setBounds(285, 19, 92, 28);
		btnRefresh.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
				int ret = m_WFXScan.WFXScan_GetDevicesListWithSerial(m_szDeviceList, m_szSerialNumberList);
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_INIT == ret || WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL == ret || WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING == ret) {		
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
					return;
				}
				else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS == ret ) {
					if (m_szDeviceList.szValue != null) {
						JSONArray jsonDeviceList = new JSONArray(m_szDeviceList.szValue);
						JSONArray jsonSerialNumberList = new JSONArray(m_szSerialNumberList.szValue);
						comboDeviceList.removeAllItems();
						for (int i = 0; i < jsonDeviceList.length(); i++) {
							comboDeviceList.addItem(jsonDeviceList.get(i).toString());
							String sn = (jsonSerialNumberList != null) ? jsonSerialNumberList.optString(i, "") : "";
							    if (!sn.isEmpty()) {
							        WriteLog("Device: " + jsonDeviceList.get(i).toString() + "   " + "Serial Number: " + jsonSerialNumberList.get(i).toString() + "\n");
								} else {
							        WriteLog("Device: " + jsonDeviceList.get(i).toString() + "   " + "Serial Number: \n");
							    }
							}
					}
				}
				else {
					m_WFXScan.WFXScan_GetLastErrorCode(WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES + "]\n");
					return;
				}
			}
		});
		btnRefresh.setFont(new Font("Consolas", Font.PLAIN, 14));
		frmWfxdemo.getContentPane().add(btnRefresh);

		labelPreview1 = new JLabel();
		labelPreview1.setBounds(387, 26, 369, 200);
		labelPreview1.setBorder(new LineBorder(new Color(0, 0, 0)));
		frmWfxdemo.getContentPane().add(labelPreview1);

		labelPreview2 = new JLabel();
		labelPreview2.setBounds(387, 236, 369, 200);
		labelPreview2.setBorder(new LineBorder(new Color(0, 0, 0)));
		frmWfxdemo.getContentPane().add(labelPreview2);

		txtArStatus = new JTextArea();
		txtArStatus.setEditable(false);
		txtArStatus.setFont(new Font("Arial Unicode MS", Font.PLAIN, 14));
		JScrollPane scroll = new JScrollPane(txtArStatus, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS,
				JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
		scroll.setSize(369, 242);
		scroll.setLocation(8, 113);

		frmWfxdemo.getContentPane().add(scroll);
        
		btnExit = new JButton("Exit");
		btnExit.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				m_WFXScan.WFXScan_CloseDevice();
				m_WFXScan.WFXScan_DeInit();
				System.exit(0);
			}
		});
		btnExit.setBackground(new Color(204, 204, 204));
		btnExit.setFont(new Font("Consolas", Font.PLAIN, 14));
		btnExit.setBounds(669, 478, 87, 28);
		frmWfxdemo.getContentPane().add(btnExit);
		
		JPanel panel = new JPanel();
		panel.setBackground(UIManager.getColor("List.selectionBackground"));
		panel.setBorder(new LineBorder(new Color(0, 0, 0)));
		panel.setBounds(8, 368, 369, 42);
		frmWfxdemo.getContentPane().add(panel);
		panel.setLayout(null);
		
		label = new JLabel("ECO Time :");
		label.setBounds(10, 11, 87, 23);
		panel.add(label);
		label.setFont(new Font("Consolas", Font.PLAIN, 14));
		
		textECOTime = new JTextField();
		textECOTime.setBounds(96, 6, 129, 28);
		panel.add(textECOTime);
		textECOTime.setFont(new Font("Consolas", Font.PLAIN, 14));
		textECOTime.setColumns(10);
		
		label_1 = new JLabel("mins");
		label_1.setBounds(229, 11, 44, 23);
		panel.add(label_1);
		label_1.setFont(new Font("Consolas", Font.PLAIN, 14));
		
		button = new JButton("Set");
		button.setBounds(270, 8, 89, 28);
		panel.add(button);
		button.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				String str = textECOTime.getText();
				if(str == null || str.trim().isEmpty())
					return;
				m_longTime.nValue = Long.parseLong(textECOTime.getText());
				
				int ret = 0;
				if(comboScanCmd.getSelectedIndex() == -1)
				{
					editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					ret = m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
				}
				else
					ret = m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);		
				
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
                {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
                    return;
                }
				ret = m_WFXScan.WFXScan_ECOControl(m_longTime, 1);
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
				{
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");	
				}
				else
				{
					WriteLog("Status:[LibWFX_ECOControl Success]\n");
				}
			}
		});
		button.setFont(new Font("Consolas", Font.PLAIN, 14));
		button.setBackground(new Color(204, 204, 204));
				
		comboScanCmd = new JComboBox<String>();	      	
		comboScanCmd.setSize( new Dimension(500, 25 ));
		comboScanCmd.setBounds(8, 56, 500, 30);
		comboScanCmd.setEditable(true);
		
		JScrollPane jsp = new JScrollPane(JScrollPane.VERTICAL_SCROLLBAR_NEVER, JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);		
		jsp.setBounds(8, 56, 267, 45);
		jsp.setViewportView(comboScanCmd);
		frmWfxdemo.getContentPane().add(jsp, BorderLayout.CENTER);
		
		
		comboScanCmd.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {           
	                int newWidth = 0;
	                for (int idx = 0; idx < comboScanCmd.getItemCount(); idx++)
	                {	     	 	                	
	                	newWidth = Math.max(newWidth, comboScanCmd.getItemAt(idx).length()*6 + 20); 
	                }
	                
	                if(newWidth == 0)
	                	return;
	                
	                comboScanCmd.setSize(newWidth, 30);
	                comboScanCmd.setPreferredSize(new Dimension(newWidth, 30));
			}
		});
		
		JButton btnEdit = new JButton("Edit");
		btnEdit.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
				 String szCommand;
				 WebFXScan.CString szRtn  = m_WFXScan.new CString("");
				 ArrayList<String> commandLists = new ArrayList<String>(5);
				 
				 editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					
		            if (comboScanCmd.getSelectedIndex() == -1)
		                szCommand = editor.getText();
		            else
		                szCommand = String.valueOf(comboScanCmd.getSelectedItem());

		            
		            m_WFXScan.WFXScan_EditCommand(szCommand, szRtn);

		            if ((szRtn != null) && (szRtn.szValue.length() > 0))
		            {
		            	if(szRtn.szValue.contains("Invalid JSON format") == true)
		            	{
		            		javax.swing.JOptionPane.showMessageDialog(null, "Invalid JSON format");
		        			return;
		            	}
		                commandLists.add(szRtn.szValue);
		                for (int idx = 0; idx < comboScanCmd.getItemCount(); idx++)
		                {
		                    if (idx == (m_nMaxCMDItems - 1))
		                        break;
		                   
		                    commandLists.add(String.valueOf(comboScanCmd.getItemAt(idx)));
		                }
		                comboScanCmd.removeAllItems();
		                for (int idx = 0; idx < commandLists.size(); idx++)
		                {
		                	comboScanCmd.addItem(commandLists.get(idx));
		                }
		                comboScanCmd.setSelectedIndex(0);
		            }
			}
		});
		btnEdit.setFont(new Font("Consolas", Font.PLAIN, 14));
		btnEdit.setBounds(285, 63, 92, 28);
		frmWfxdemo.getContentPane().add(btnEdit);
		
		JPanel panel_normal = new JPanel();
		FlowLayout flowLayout = (FlowLayout) panel_normal.getLayout();
		flowLayout.setVgap(8);
		flowLayout.setHgap(15);
		panel_normal.setBackground(Color.WHITE);
		panel_normal.setBorder(new TitledBorder(null, "Normal", TitledBorder.LEADING, TitledBorder.TOP, null, null));
		panel_normal.setBounds(8, 433, 369, 137);
		frmWfxdemo.getContentPane().add(panel_normal);
		
		btnScan = new JButton("Scan");
		btnScan.setForeground(Color.WHITE);
		btnScan.setFont(new Font("Consolas", Font.BOLD, 16));
		btnScan.setBackground(new Color(0, 153, 255));
		panel_normal.add(btnScan);
		btnScan.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {					    	
		    	m_szErrorMsg = m_WFXScan.new CString("");
		    	m_szScanImageList = m_WFXScan.new CString("");
		    	m_szOCRResultList = m_WFXScan.new CString("");
		    	m_szExceptionRet = m_WFXScan.new CString("");
		    	m_szEventRet = m_WFXScan.new CString("");	    	

		    	if(comboScanCmd.getSelectedIndex() == -1)
				{
					editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					m_szCommand = editor.getText();					
				}
				else
					m_szCommand = String.valueOf(comboScanCmd.getSelectedItem());		
				
		    	if (m_szCommand.indexOf("\"autoscan\":true") != -1) {
		    		javax.swing.JOptionPane.showMessageDialog(null, "BlockScan do not support autoscan!! If you want to implement autoscan, please refer to the AutoCaptureDemo.");
		    		return;
		    	 }
		    	
		    	DemoFrame_WaitMsgDlg window = new DemoFrame_WaitMsgDlg("Scan");
		    	window.setVisible(true);		    	
		    	frmWfxdemo.setVisible(false);

				int ret = WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS;
				ret = m_WFXScan.WFXScan_SynchronizeScan(m_szCommand, m_szScanImageList, m_szOCRResultList, m_szExceptionRet, m_szEventRet);			

				window.setVisible(false);
				window.dispose();
		    	frmWfxdemo.setVisible(true);
		       				 
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);			
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");   //get fail message							
				}
				else {						
					String szExceptionRet = new String(m_szExceptionRet.szValue);
				    String szEventRet = new String(m_szEventRet.szValue);
				   
					if(szEventRet.length() > 1) {  //event happen
						SetCommandString();
						WriteLog("Status:[Device Ready!]\n");
						WriteLog(szEventRet + "\n");   //get event message
						
						if(szEventRet.equals("LIBWFX_EVENT_UVSECURITY_DETECTED[0]") == false && szEventRet.equals("LIBWFX_EVENT_UVSECURITY_DETECTED[1]") == false && szEventRet.equals("LIBWFX_EVENT_NO_PAPER") == false) {
							WriteLog("Status:[Scan End]\n");
			                return;
			            }
			                
						WriteLog("Status:[LibWFX_SynchronizeScan Success]\n");

						String szScanImageList = new String(m_szScanImageList.szValue);
						String szOCRResultList = new String(m_szOCRResultList.szValue);
						String ScanImageList[] = szScanImageList.split("\\|&\\|");
						String OCRResultLis[] = szOCRResultList.split("\\|&\\|");
												
						int nMaxLength = Math.max(ScanImageList.length, OCRResultLis.length);
						for (int i = 0; i < nMaxLength; i++) {
							if (i < ScanImageList.length) {
								WriteLog(ScanImageList[i] + "\n");   //get each image path
								if (ScanImageList[i].contains(".pdf") == false && ScanImageList[i].contains(".tif") == false && ScanImageList[i].toUpperCase().contains("_PHOTO") == false && ScanImageList[i].equals("") == false) {
		        					BufferedImage img = null;
		        					try {
		        						File file = new File(ScanImageList[i]);
		        						if (!file.exists() || !file.isFile()) {
		        						   continue;
		        						}
		        						img = ImageIO.read(new File(ScanImageList[i]));
		        					} catch (IOException e) {
		        						e.printStackTrace();
		        					}
		        					float ratioW = 0;
		        					float ratioH = 0;  
		        					try {
		        					  ratioW = (float) DemoFrame_BlockMode.labelPreview1.getWidth() / img.getWidth();
		        					  ratioH = (float) DemoFrame_BlockMode.labelPreview1.getHeight() / img.getHeight();
		        					} catch (final ArithmeticException e) {
		        						e.printStackTrace();
		        					}
		        					float ratio = (ratioW < ratioH) ? ratioW : ratioH;
		        					BufferedImage fitImg = convertToBufferedImage(img.getScaledInstance((int) (img.getWidth() * ratio),
		        							(int) (img.getHeight() * ratio), Image.SCALE_SMOOTH));
		        					DemoFrame_BlockMode.m_nCount++;
		        					if (DemoFrame_BlockMode.m_nCount % 2 == 1) {
		        						DemoFrame_BlockMode.labelPreview1.setIcon(new ImageIcon(fitImg));
		        						DemoFrame_BlockMode.labelPreview1.setHorizontalAlignment(JLabel.CENTER);
		        					} else {
		        						DemoFrame_BlockMode.labelPreview2.setIcon(new ImageIcon(fitImg));
		        						DemoFrame_BlockMode.labelPreview2.setHorizontalAlignment(JLabel.CENTER);
		        					}
		    				    }
							}					
							if(i < OCRResultLis.length)
								WriteLog(OCRResultLis[i] + "\n");    //get each ocr result		  	
						}
					}
					else {
						WriteLog("Status:[LibWFX_SynchronizeScan Success]\n");
						SetCommandString();
						WriteLog("Status:[Device Ready!]\n");
						if(szExceptionRet.length() > 1) { //exception happen							
							WriteLog(szExceptionRet + "\n");   //get exception message
						}

						String szScanImageList = new String(m_szScanImageList.szValue);
						String szOCRResultList = new String(m_szOCRResultList.szValue);
						String ScanImageList[] = szScanImageList.split("\\|&\\|");
						String OCRResultLis[] = szOCRResultList.split("\\|&\\|");
						int nMaxLength = Math.max(ScanImageList.length, OCRResultLis.length);
						for (int i = 0; i < nMaxLength; i++) {
							if (i < ScanImageList.length) {
								WriteLog(ScanImageList[i] + "\n");   //get each image path
								if (ScanImageList[i].contains(".pdf") == false && ScanImageList[i].contains(".tif") == false && ScanImageList[i].toUpperCase().contains("_PHOTO") == false && ScanImageList[i].equals("") == false) {
		        					BufferedImage img = null;
		        					try {
		        						File file = new File(ScanImageList[i]);
		        						if (!file.exists() || !file.isFile()) {
		        						   continue;
		        						}
		        						img = ImageIO.read(new File(ScanImageList[i]));
		        					} catch (IOException e) {
		        						e.printStackTrace();
		        					}
		        					float ratioW = 0;
		        					float ratioH = 0;  
		        					try {
		        					  ratioW = (float) DemoFrame_BlockMode.labelPreview1.getWidth() / img.getWidth();
		        					  ratioH = (float) DemoFrame_BlockMode.labelPreview1.getHeight() / img.getHeight();
		        					} catch (final ArithmeticException e) {
		        						e.printStackTrace();
		        					}
		        					float ratio = (ratioW < ratioH) ? ratioW : ratioH;
		        					BufferedImage fitImg = convertToBufferedImage(img.getScaledInstance((int) (img.getWidth() * ratio),
		        							(int) (img.getHeight() * ratio), Image.SCALE_SMOOTH));
		        					DemoFrame_BlockMode.m_nCount++;
		        					if (DemoFrame_BlockMode.m_nCount % 2 == 1) {
		        						DemoFrame_BlockMode.labelPreview1.setIcon(new ImageIcon(fitImg));
		        						DemoFrame_BlockMode.labelPreview1.setHorizontalAlignment(JLabel.CENTER);
		        					} else {
		        						DemoFrame_BlockMode.labelPreview2.setIcon(new ImageIcon(fitImg));
		        						DemoFrame_BlockMode.labelPreview2.setHorizontalAlignment(JLabel.CENTER);
		        					}
		    				    }
							}					
							if(i < OCRResultLis.length)
								WriteLog(OCRResultLis[i] + "\n");    //get each ocr result		  	
						}	   
					}			
				}				
				WriteLog("Status:[Scan End]\n");
		
			}
			
			public BufferedImage convertToBufferedImage(Image image) {
				BufferedImage newImage = new BufferedImage(image.getWidth(null), image.getHeight(null),
						BufferedImage.TYPE_INT_ARGB);
				Graphics2D g = newImage.createGraphics();
				g.drawImage(image, 0, 0, null);
				g.dispose();
				return newImage;
			}
			
		});
		
		
		btnPaperStatus = new JButton("PaperStatus");
		panel_normal.add(btnPaperStatus);
		btnPaperStatus.addActionListener(new ActionListener() {
		    public void actionPerformed(ActionEvent e) {
		    	int ret = 0;
				if(comboScanCmd.getSelectedIndex() == -1)
				{
					editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					ret = m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
				}
				else
					ret = m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);		
				
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
                {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");					
                    return;
                }
				
		         ret = m_WFXScan.WFXScan_GetPaperStatus(m_nPaperStatus);
		        if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
		        	m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
		        	WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
		        }
		        else
		        	WriteLog("Status:[LibWFX_GetPaperStatus Success [" + m_nPaperStatus.nValue + "]]\n");
		    }
		});
		btnPaperStatus.setBackground(new Color(204, 204, 204));
		btnPaperStatus.setFont(new Font("Consolas", Font.PLAIN, 14));
		
		btnPaperready = new JButton("PaperReady");
		panel_normal.add(btnPaperready);
		btnPaperready.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				int ret = 0;
				if(comboScanCmd.getSelectedIndex() == -1)
				{
					editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					ret = m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
				}
				else
					ret = m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);		
				
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
                {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");			
                    return;
                }
				
				ret = m_WFXScan.WFXScan_PaperReady();
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS == ret)
					WriteLog("[Paper is ready]\n");
				else if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PAPER_NOT_READY == ret)
					WriteLog("[Paper is NOT ready]\n");
				else
				{					
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");		
				}
			}
		});
		btnPaperready.setFont(new Font("Consolas", Font.PLAIN, 14));
		btnPaperready.setBackground(new Color(204, 204, 204));
		panel_normal.add(btnPaperready);
		
		btnCalibrate = new JButton("Calibrate");
		panel_normal.add(btnCalibrate);
		btnCalibrate.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				if(comboDeviceList.getSelectedIndex() == -1)
				{
					m_WFXScan.WFXScan_GetLastErrorCode((int)WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES + "]\n");
					return;
				}
				m_szDevice = String.valueOf(comboDeviceList.getSelectedItem());
				if (m_szDevice.equals("A61") || m_szDevice.equals("A62")  || m_szDevice.equals("A63") || m_szDevice.equals("A64") || m_szDevice.equals("A65") || m_szDevice.equals("A66") || m_szDevice.equals("J6102"))
				{
					m_szCommand = "{\"device-name\":\"" + m_szDevice + "\",\"source\":\"Camera\",\"ext-capturetype\":\"g\"}";
				}
				else
				{
					if(comboScanCmd.getSelectedIndex() == -1)
					{
						editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
						m_szCommand = editor.getText();					
					}
					else
						m_szCommand = String.valueOf(comboScanCmd.getSelectedItem());		
				}	
	            
	           	int ret = m_WFXScan.WFXScan_SetProperty(m_szCommand, null);
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
	            {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");						
	                return;
	            }
	            
				frmWfxdemo.setVisible(false);
				if (m_szDevice.equals("A64"))				
					window = new DemoFrame_WaitMsgDlg("Calibrate_xmini");		    	
				else
					window = new DemoFrame_WaitMsgDlg("Calibrate_normal");
				window.setVisible(true);	
		    	
				ret = m_WFXScan.WFXScan_Calibrate();
				
				window.setVisible(false);
				window.dispose();
		    	frmWfxdemo.setVisible(true);
		    	
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");	
				}
				else
					WriteLog("Status:[LibWFX_Calibrate Success]\n");
			}
		});
		btnCalibrate.setBackground(new Color(204, 204, 204));
		btnCalibrate.setFont(new Font("Consolas", Font.PLAIN, 14));
		
				JButton btnMergePdf = new JButton("MergePdf");
				panel_normal.add(btnMergePdf);
				btnMergePdf.setBackground(new Color(204, 204, 204));
				btnMergePdf.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent e) {
						//m_FileListIn = "D:\testpdf\IMG_113692078_00001.jpg*D:\testpdf\IMG_113692078_00002.jpg";
														
						 JFileChooser fileChooser = new JFileChooser();
						 FileNameExtensionFilter filter = new FileNameExtensionFilter(
					                "Image Files", "jpg", "png", "bmp");
						 	fileChooser.setAcceptAllFileFilterUsed(false);
					        fileChooser.setFileFilter(filter);
						    StringBuilder filePaths = new StringBuilder();
					        fileChooser.setMultiSelectionEnabled(true);
					     
					        int result = fileChooser.showOpenDialog(new JFrame());

					        if (result == JFileChooser.APPROVE_OPTION) {					          
					            File[] selectedFiles = fileChooser.getSelectedFiles();
					       
					            for (File file : selectedFiles) {
					                if (filePaths.length() > 0) {
					                 
					                    filePaths.append("*");
					                }					             
					                filePaths.append(file.getAbsolutePath());
					            }
					           
					            System.out.println("filepath: " + filePaths.toString());
					        } else {
					            System.out.println("cancel");
					            return;
					        }
									
						int ret = 0;
						if(comboScanCmd.getSelectedIndex() == -1)
						{
							editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
							ret = m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
						}
						else
							ret = m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);		
						
						if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
		                {
							m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
							WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");	
		                    return;
		                }	
																							
						m_FileListIn = filePaths.toString().trim();					
						ret = m_WFXScan.WFXScan_MergeToPdf(m_FileListIn);					
						
						if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
						{
							m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
							WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
							return;
						}
						WriteLog("Status:[LibWFX_MergeToPdf Success]\n");
						
					}
					
				});
				
			
			
						JButton btnRecycleSaveFolder = new JButton("RecycleSaveFolder");
						panel_normal.add(btnRecycleSaveFolder);
						btnRecycleSaveFolder.setBackground(new Color(204, 204, 204));
						btnRecycleSaveFolder.addActionListener(new ActionListener() {
							public void actionPerformed(ActionEvent e) {
								int ret = m_WFXScan.WFXScan_RecycleSaveFolder();
								if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
									m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
									WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
									return;
								}
								WriteLog("Status:[LibWFX_RecycleSaveFolder Success]\n");						
							}
						});
						btnRecycleSaveFolder.setFont(new Font("Consolas", Font.PLAIN, 14));
		
		JPanel panel_vtm300 = new JPanel();
		FlowLayout flowLayout_1 = (FlowLayout) panel_vtm300.getLayout();
		flowLayout_1.setVgap(8);
		flowLayout_1.setHgap(15);
		panel_vtm300.setBackground(Color.WHITE);
		panel_vtm300.setBorder(new TitledBorder(null, "VTM", TitledBorder.LEADING, TitledBorder.TOP, null, null));
		panel_vtm300.setBounds(387, 447, 207, 91);
		frmWfxdemo.getContentPane().add(panel_vtm300);
		
		comboEjectDirection = new JComboBox<>(new String[]{"eject backwarding- force", "eject forwarding- force", "eject backwarding stop- force", "eject forwarding stop- force", "eject backwarding", "eject forwarding", "eject backwarding stop", "eject forwarding stop", "eject backwarding by steps", "eject forwarding by steps"});
		panel_vtm300.add(comboEjectDirection);
		
		btnEjectPaper = new JButton("Eject");
		
		panel_vtm300.add(btnEjectPaper);
		btnEjectPaper.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {                
            	int ret = 0;
				if(comboScanCmd.getSelectedIndex() == -1)
				{
					editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
					ret = m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
				}
				else
					ret = m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);		
				
				if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret)
                {
					m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
					WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
                    return;
                }
            	
            	int nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING;
            	if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject backwarding- force")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject forwarding- force")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject backwarding stop- force")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGS;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject forwarding stop- force")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGS;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject backwarding")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGD;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject forwarding")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGD;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject backwarding stop")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDINGSD;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject forwarding stop")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDINGSD;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject backwarding by steps")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING_BY_STEPS;
            	else if(String.valueOf(comboEjectDirection.getSelectedItem()) == "eject forwarding by steps")
            		nEjectDirect = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING_BY_STEPS;
            		
                ret = m_WFXScan.WFXScan_EjectPaperControlWithMsg(nEjectDirect, m_szErrorMsg);
                
                
                if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
                    m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
                    WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
                }
                else if(m_szErrorMsg.szValue.length() > 0)
                	WriteLog(m_szErrorMsg.szValue + "\n");
                else
                    WriteLog("Status:[LibWFX_EjectPaperControl Success]\n");
            }
        });
		btnEjectPaper.setBackground(new Color(204, 204, 204));
		btnEjectPaper.setFont(new Font("Consolas", Font.PLAIN, 14));
		
		btnRegister = new JButton();
		btnRegister.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				 try {
					 m_WFXScan.WFXScan_CallRegisterAP();
			        } catch (Exception ex) {
			        	 System.out.println("load register file fail!");
			             ex.printStackTrace();			        
			        }				   
			}
		});
		btnRegister.setSize(24,24);
		ImageIcon icon = new ImageIcon(DemoFrame_NonBlockMode.class.getResource("/adview/webfxscan/Image/license_register.png")); 
		Image scaledImage = icon.getImage().getScaledInstance(btnRegister.getWidth(), btnRegister.getHeight(), Image.SCALE_SMOOTH);	        	    
	    ImageIcon scaledIcon = new ImageIcon(scaledImage);		
		btnRegister.setLocation(733, 0);		
		btnRegister.setIcon(scaledIcon);
		frmWfxdemo.getContentPane().add(btnRegister);
						
		frmWfxdemo.setVisible(true);
	}
		
	 public  synchronized int setproperty() {
	        System.out.println("Java method started.");
	        int ret = 0;
	        if(comboScanCmd.getSelectedIndex() == -1)
			{
				editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
				ret =  m_WFXScan.WFXScan_SetProperty(editor.getText(), null);					
			}
			else
				ret =  m_WFXScan.WFXScan_SetProperty(String.valueOf(comboScanCmd.getSelectedItem()), null);	
	        System.out.println("setproperty completed.");
	        return ret;
	    }
	 
	private boolean GetCommandString() {
		try {
			String dirPath = new String("C:\\ProgramData\\Plustek\\") +  String.valueOf(comboDeviceList.getSelectedItem()) + new String("\\");
			File dir = new File(dirPath);
			if (!dir.isDirectory())
				dir.mkdir();
			String szCommandPath = dirPath + new String("Command.txt");   
			
			File file = new File(szCommandPath);						
			if (!file.exists() || file.length() == 0) 
				return false;
					
			FileInputStream inputStream = new FileInputStream(szCommandPath);
			//BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream, "Big5"));

			String line = "";
			int idx = 0;
			comboScanCmd.removeAllItems();
			while ((line = bufferedReader.readLine()) != null && idx < m_nMaxCMDItems)
            {
                idx++;         
				comboScanCmd.addItem(line);
				comboScanCmd.setPrototypeDisplayValue(line);
            }
			inputStream.close();
			bufferedReader.close();
			comboScanCmd.setSelectedIndex(0);
			return true;
	    }catch(IOException e){
	         e.printStackTrace();
	         return false;
	    }		
	}
	
	private void SetCommandString() {
		try {
			String dirPath = new String("C:\\ProgramData\\Plustek\\") +  String.valueOf(comboDeviceList.getSelectedItem()) + new String("\\");;
			File dir = new File(dirPath);
			if (!dir.isDirectory())
				dir.mkdir();
			String szCommandPath = dirPath + new String("Command.txt");
        
			File file = new File(szCommandPath);						
			if (!file.exists()) 
				file.createNewFile();
					
			FileWriter fileWriter = new FileWriter(file);
			BufferedWriter bufferedWriter = new BufferedWriter(fileWriter);
			ArrayList<String> commandLists = new ArrayList<String>(5);
			editor = (JTextField) comboScanCmd.getEditor().getEditorComponent();
			commandLists.add(editor.getText());		
			bufferedWriter.write(editor.getText() + "\r\n");			
			
			for (int idx = 0; idx < comboScanCmd.getItemCount(); idx++)
            {			
                if (commandLists.size() == m_nMaxCMDItems || (editor.getText().equals(String.valueOf(comboScanCmd.getItemAt(0))) && idx == 0))
                    continue;
               
                bufferedWriter.write(String.valueOf(comboScanCmd.getItemAt(idx)) + "\r\n");
                commandLists.add(String.valueOf(comboScanCmd.getItemAt(idx))); 
            }
			
			bufferedWriter.close();
			comboScanCmd.removeAllItems();
			for (int idx=0; idx < commandLists.size(); idx++)
            {
				comboScanCmd.addItem(commandLists.get(idx));
            }
			comboScanCmd.setSelectedIndex(0);
		}
		 catch (IOException e) {
			e.printStackTrace();
	}}
	
	private void GetCertificatePermission() {
		try {		
			int ret = m_WFXScan.WFXScan_GetCertificatePermission(m_szPermissionTypeList, ENUM_PERMISSION_DATA_TYPE.LIBWFX_DATA_TYPE_REGINFO);
			if (WebFXScan.ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS != ret) {
				m_WFXScan.WFXScan_GetLastErrorCode(ret, m_szErrorMsg);
				WriteLog("[ Warning ] " + m_szErrorMsg.szValue + " - ["  + ret + "]\n");
			}
			else {
				if(m_szPermissionTypeList.szValue.length() > 0)
					WriteLog("License: " + m_szPermissionTypeList.szValue + " \n");
				else
					WriteLog("License: none\n");
			}			
		}
		 catch (Exception e) {
			e.printStackTrace();
	}}
	
	private void WriteLog(String szMsg) {
		try {
			DemoFrame_BlockMode.txtArStatus.append(szMsg);	
			m_WFXScan.WFXScan_WriteAPLog(szMsg);
		}
		 catch (Exception e) {
			e.printStackTrace();
	}}
}


