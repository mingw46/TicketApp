using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEmailSender.Models;

namespace TicketEmailSender
{
    class EmailManager
    {
        public delegate void onEmailReadHandler(object o, EmailArgs e);
        public event onEmailReadHandler ReadEmail;


        public void onEmailConverted(int id)
        {
            if (ReadEmail != null)
                ReadEmail(this, new EmailArgs { id = id});
        }

        public void ConvertEmail(int id)
        {
            onEmailConverted(id);
        }
    }

    class EmailArgs : EventArgs
    {
        public int id { get; set; }
    }
}
