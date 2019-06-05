using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public int CreatedUserID { get; set; }
        public int AssigmentedUserID { get; set; }

        public Ticket()
        {
        }
    }
}