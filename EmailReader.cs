using OpenPop.Pop3;
using OpenPop.Mime;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEmailSender.Models;

namespace TicketEmailSender
{
    class EmailReader
    {
        public delegate void ConvertEmails();
        Pop3Client pop3Client = null;
        ConvertEmails convertEmails;
        public void readEmail(EmailConfiguration emailConfiguration)
        {
            EmailManager ticketManager = new EmailManager();
            try
            {
                if (pop3Client == null)
                {
                    pop3Client = new Pop3Client();
                    pop3Client.Connect(emailConfiguration.Hostname, emailConfiguration.Port, true);
                    pop3Client.Authenticate(emailConfiguration.Username, emailConfiguration.Password);
                }
                
                ticketManager.ReadEmail += TicketManager_ReadTicket;
              //  ticketManager.ReadTicket += TicketManager_CheckAsRead;
                ticketManager.ConvertEmail(emailConfiguration.id);
            }
            catch(Exception e) {
                Console.WriteLine(e);
            }
           
        }

        private void TicketManager_ReadTicket(object o, EmailArgs e)
        {
            int count = pop3Client.GetMessageCount();
           // List<EmailLog> emailLogs = new List<EmailLog>();
            ReadEmails readEmails = new ReadEmails();
            for (int i = 300; i <= count; i++)
            {
                string u_id = pop3Client.GetMessageUid(i);
                Message message = pop3Client.GetMessage(i);
                var messageDate = message.Headers.DateSent;
                //   emailLogs.Add(new EmailLog(u_id));
                if (readEmails.SaveReadEmails(e.id, u_id, messageDate))
                { 
                    MessagePart messagePart = message.MessagePart.MessageParts[0];
                    //  Console.WriteLine(message);

                    Console.WriteLine("Treść wiadomości");
                    var msg = messagePart.BodyEncoding.GetString(messagePart.Body);
                    
                }
            }
            
            Console.WriteLine(count);
        }
    }
}
