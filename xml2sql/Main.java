package com.patch.database;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.*;
import java.sql.*;
import org.w3c.dom.Element;

public class Main {


    public static void main(String[] args) {
        //напечатать всю таблицу
        //printTable();

        String URL = "***";
        String chars = "/P-";
        String xml = ".xml";
        String a64 = "A64-";
        String a86 = "A86-";
        String l64 = "L64-";
        String l86 = "L86-";
        String m64 = "M64-";
        String dr = "DR-";


        for (int i = 0; i < 1000 ; i++) { //1к строк
            String is = Integer.toString(i);

            String urlCore = URL + is + chars + is + xml;
            String urla64 = URL + a64 + is + chars + a64 + is + xml;
            String urla86 = URL + a86 + is + chars + a86 + is + xml;
            String urll64 = URL + l64 + is + chars + l64 + is + xml;
            String urll86 = URL + l86 + is + chars + l86 + is + xml;
            String urlm64 = URL + m64 + is + chars + m64 + is + xml;
            String urlDR = URL + dr + is + chars + dr + is + xml;

            urlXmlSql(urlCore);
            urlXmlSql(urla64);
            urlXmlSql(urla86);
            urlXmlSql(urll64);
            urlXmlSql(urll86);
            urlXmlSql(urlm64);
            urlXmlSql(urlDR);
        }
    }

    public static void urlXmlSql(String url) {
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();

        try {
            DocumentBuilder builder = factory.newDocumentBuilder();
            URL u = new URL(url);
            org.w3c.dom.Document doc = builder.parse(u.openStream());
            Element root = doc.getDocumentElement();

            NodeList idl = doc.getElementsByTagName("d2p1:name");
            String idS = idl.item(0).getTextContent().substring(idl.item(0).getTextContent().length() - 4, idl.item(0).getTextContent().length());
            int id = Integer.parseInt(idS);
            System.out.println(id);

            String name = idl.item(0).getTextContent();
            System.out.println(name);

            NodeList patchTypeL = doc.getElementsByTagName("d2p1:patchType");
            String patchType = patchTypeL.item(0).getTextContent();
            System.out.println(patchType);

            NodeList descriptionL = doc.getElementsByTagName("d2p1:description");
            String description = descriptionL.item(0).getTextContent();
            System.out.println(description);

            NodeList productVersionL = doc.getElementsByTagName("d2p1:productVersion");
            String productVersion = productVersionL.item(0).getTextContent();
            System.out.println(productVersion);

            NodeList defectList = doc.getElementsByTagName("d2p1:defectIds");
            StringBuffer defect = new StringBuffer();
            for (int i = 0; i < defectList.getLength(); i++) {
                Node d = defectList.item(i);
                if (d.getNodeType() == Node.ELEMENT_NODE) {
                    Element defectE = (Element) d;
                    NodeList idlist = defectE.getChildNodes();
                    for (int j = 1; j < idlist.getLength(); j++) {
                        Node n = idlist.item(j);
                        if (n.getNodeType() == Node.ELEMENT_NODE) {
                            //System.out.println("defect: " + n.getTextContent() + ": end ");
                            defect.append(n.getTextContent());
                            defect.append(" ");
                        }
                    }
                }
            }
            System.out.println(defect);

            NodeList zendeskL = doc.getElementsByTagName("zendesk");
            StringBuffer zendesk = new StringBuffer();
            for (int i = 0; i < zendeskL.getLength(); i++) {
                Node znode = zendeskL.item(i);
                if (znode.getNodeType() == Node.ELEMENT_NODE) {
                    Element zendeskE = (Element) znode;
                    NodeList zendeskLC = zendeskE.getChildNodes();
                    for (int j = 0; j < zendeskLC.getLength(); j++) {
                        Node n = zendeskLC.item(j);
                        if (n.getNodeType() == Node.ELEMENT_NODE) {
                            zendesk.append(n.getTextContent());
                            zendesk.append(" ");
                        }
                    }
                }
            }
            System.out.println(zendesk);


            NodeList targetProductNameL = doc.getElementsByTagName("d2p1:targetProductName");
            String targetProductName = "";
            if (targetProductNameL.getLength() > 0) {
                targetProductName = targetProductNameL.item(0).getTextContent();
                System.out.println(targetProductName);
            }

            String xmlLink = u.toString();
            System.out.println(xmlLink);

            String msiLink = xmlLink.substring(0, xmlLink.length() - 3) + "msi";
            System.out.println(msiLink);

            //ЗАПИСЫВАЕМ В БАЗУ
            setTableValue(id, name, patchType, description, productVersion, defect.toString(), zendesk.toString(),
                    targetProductName, xmlLink, msiLink);

        } catch (FileNotFoundException e) {
            System.out.println("NOT FOUND : " + url);
        } catch (ParserConfigurationException | IOException | SAXException | NullPointerException e) {
            try {
                Thread.sleep(50);
            } catch (InterruptedException e1) {
                e1.printStackTrace();
            }
            e.printStackTrace();
        }
    }

    public static void setTableValue(int id, String name, String patchType, String description, String productVersion,
                                     String defectIds, String zendesk, String targetProductName, String xmlLink, String msiLink) {
        DBWorker worker = new DBWorker();
        String INSERT_NEW = "INSERT INTO patches VALUES(?,?,?,?,?,?,?,?,?,?)";
        PreparedStatement preparedStatement = null;

        try {
            preparedStatement = worker.getConnection().prepareStatement(INSERT_NEW);
            preparedStatement.setInt(1, id);
            preparedStatement.setString(2, name);
            preparedStatement.setString(3, patchType);
            preparedStatement.setString(4, description);
            preparedStatement.setString(5, productVersion);
            preparedStatement.setString(6, defectIds);
            preparedStatement.setString(7, zendesk);
            preparedStatement.setString(8, targetProductName);
            preparedStatement.setString(9, xmlLink);
            preparedStatement.setString(10, msiLink);

            preparedStatement.execute();
        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            try {
                preparedStatement.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
    }

    public static void printTable() {

        DBWorker worker = new DBWorker();
        String query = "select * from patches";
        Statement statement = null;
        try {
            statement = worker.getConnection().createStatement();
            ResultSet resultSet = statement.executeQuery(query);
            while (resultSet.next()) {
                Patch patch = new Patch();
                patch.setId(resultSet.getInt("id"));
                patch.setName(resultSet.getString("name"));
                patch.setPatchType(resultSet.getString("patchType"));
                patch.setDescription(resultSet.getString("description"));
                patch.setProductVersion(resultSet.getString("productVersion"));
                patch.setDefectIds(resultSet.getString("defectIds"));
                patch.setZendesk(resultSet.getString("zendesk"));
                patch.setTargetProductName(resultSet.getString("targetProductName"));
                patch.setXmlLink(resultSet.getString("xmlLink"));
                patch.setMsiLink(resultSet.getString("msiLink"));

                System.out.println(patch);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        } finally {
            try {
                statement.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }

    }
}