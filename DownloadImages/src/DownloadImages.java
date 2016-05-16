import java.io.*;
import java.net.URL;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.io.*;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.jsoup.Jsoup;

import org.jsoup.nodes.Attributes;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;
import java.io.IOException;
import java.net.URL;

public class DownloadImages {

    //The url of the website. This is just an example
    //http://address.sc/alhnjw
    //The path of the folder that you want to save the images to
    private static final String folderPath = "G:\\img";

    public static void main(String[] args) {
        int counter = 0;
        for (char i = 'a'; i <='z' ; i++) {
            for (char j = 'a'; j <= 'z' ; j++) {
                for (char k = 'a'; k <= 'z' ; k++) {
//                    for (char l = 'a'; l <= 'z' ; l++) {
//                        for (char m = 'a'; m <= 'z' ; m++) {
//                            for (char n = 'a'; n < 'z'; n++) {
//
//                            }
//                        }
//                    }
                    System.out.println("images : " + counter++ );
                    System.out.println("http://address.sc/" + i + j + k +"aaa");
                    String webSiteURL = "http://address.sc/" + i + j + k + "aaa";
                    connect(webSiteURL);
                }

            }

        }


    }
    public static void connect(String webSiteURL){
        try {

            //Connect to the website and get the html
            Document doc = Jsoup.connect(webSiteURL).userAgent("Mozilla/5.0 (Windows NT 6.2; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko)" +
                    " Chrome/32.0.1667.0 Safari/537.36").get();

            //Get all elements with img tag ,
            Element img = doc.getElementById("screenshot-image");  //getElementsByTag("img");


            //for (Element el : img) {

                //for each element get the srs url
                String src = img.absUrl("src");

                System.out.println("Image Found!");
               // System.out.println("src attribute is : "+src);

                getImages(src);

           // }

        } catch (IOException ex) {
            System.err.println("There was an error");
            Logger.getLogger(DownloadImages.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    private static void getImages(String src) throws IOException {

        String folder = null;

        //Exctract the name of the image from the src attribute
        int indexname = src.lastIndexOf("/");

        if (indexname == src.length()) {
            src = src.substring(1, indexname);
        }

        indexname = src.lastIndexOf("/");
        String name = src.substring(indexname, src.length());

        URL url = new URL(src);
        InputStream in = url.openStream();

        OutputStream out = new BufferedOutputStream(new FileOutputStream( folderPath+ name));

        for (int b; (b = in.read()) != -1;) {
            out.write(b);
        }
        out.close();
        in.close();

    }
}