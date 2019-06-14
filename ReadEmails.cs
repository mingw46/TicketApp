using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TicketEmailSender
{
    class EmailLog
    {
        public string u_id { get; set; }

        public EmailLog(string u_id)
        {
            this.u_id = u_id;
        }
    }
    class ReadEmails
    {
        //List<EmailLog> emailLogs = new List<EmailLog>();
        public bool SaveReadEmails(int id, string u_id, DateTime messageDate) 
        {
            var msgDate = messageDate.ToString("yyyy-dd-M");
            string directoryName = id.ToString();
            string lineValue = u_id + Environment.NewLine;

            System.IO.Directory.CreateDirectory(directoryName);
            var filepath = Path.Combine(directoryName, msgDate + ".txt");
           // Console.WriteLine(filepath);
            // Folder
            if (!File.Exists(filepath)) File.Create(filepath).Close();

            if (!File.ReadLines(filepath).Any(line => line.Contains(u_id)))
            {
                File.AppendAllText(filepath, lineValue);
                return true;
            }
            return false;

        }

        public void CreateTicket()
        {

        }
    }
}
