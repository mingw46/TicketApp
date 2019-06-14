using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketEmailSender.Models
{
    class EmailConfiguration
    {
        public int id { get; set; }
        public int Port { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
