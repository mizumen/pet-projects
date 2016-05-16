import org.zendesk.client.v2.Zendesk;
import org.zendesk.client.v2.model.*;

import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;

/**
 * Created by maks on 2/23/2016.
 Using Zendesk Java Client
 https://github.com/cloudbees/zendesk-java-client
 Tool to get content of ZD cases.
 */
public class myzd {
    public static void main(String[] args) throws InterruptedException, IOException{
        Zendesk zd = new Zendesk.Builder("https://domain.zendesk.com/")
                .setUsername("username")
                .setPassword("Password")
                .build();


        PrintWriter writer = new PrintWriter("E:\\zendesk\\zendesk-java-client-master\\AllTickets.txt");

        for (int i = 1024; i < 4450; i++) { //int i = 1023; i < 4450; i++
            System.out.println(i);
            try {
                writer.println("====================================================================================================================");
                writer.println(zd.getTicket(i).getId().toString());
                writer.println(zd.getTicket(i).getUrl());
                writer.println(zd.getTicket(i).getSubject());
                writer.println("V1 item: " + zd.getTicket(i).getCustomFields().get(0).getValue());

                writer.println();

                for (Comment ticket : zd.getTicketComments(i)) {

                    writer.println(ticket.toString());
                    writer.println();
                }
            }
            catch (NullPointerException e){
                e.printStackTrace();
                writer.print("No such case: " + i);
                //writer.close();
            }
        }
        writer.close();

        zd.close();

    }
}
