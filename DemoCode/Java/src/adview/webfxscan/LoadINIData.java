package adview.webfxscan;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Properties;

public class LoadINIData {

	private Properties propertie;
    private FileInputStream inputFile;

    public LoadINIData()
    {
        propertie = new Properties();
    }

    public LoadINIData(String filePath)
    {
        propertie = new Properties();

        try {

            inputFile = new FileInputStream(filePath);

            propertie.load(inputFile);

            inputFile.close();

        } catch (FileNotFoundException ex) {

            System.out.println("file not find!");
            ex.printStackTrace();

        } catch (IOException ex) {

            System.out.println("load file fail!");

            ex.printStackTrace();

        }

    }//end ReadConfigInfo(¡K)


    public String getValue(String key)
    {
        if(propertie.containsKey(key)){
            String value = propertie.getProperty(key);
            return value;
        }
        else 
            return "";
    }//end getValue(¡K)

    public String getValue(String fileName, String key)
    {
        try {
            String value = "";
            inputFile = new FileInputStream(fileName);
            propertie.load(inputFile);
            inputFile.close();

            if(propertie.containsKey(key)){
                value = propertie.getProperty(key);
                return value;
            }else

                return value;

        } catch (FileNotFoundException e) {

            e.printStackTrace();

            return "";

        } catch (IOException e) {

            e.printStackTrace();

            return "";

        } catch (Exception ex) {

            ex.printStackTrace();

            return "";

        }

    }//end getValue(¡K)
}
