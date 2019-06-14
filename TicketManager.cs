using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketEmailSender.Models
{
    class TicketManager
    {
        public delegate void onTicketReadHandler(object o, EventArgs e);
        public event onTicketReadHandler ReadTicket;


        public void onTicketConverted()
        {
            if (ReadTicket != null)
                ReadTicket(this, EventArgs.Empty);
        }

        public void ConvertTicket()
        {
            onTicketConverted();
        }
    }
}
