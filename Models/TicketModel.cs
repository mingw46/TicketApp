using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket.Models
{
    public class TicketModel
    {
        [Key]
        public int TicketID { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string EmailFrom { get; set; }
 //       public ApplicationUser CreatedUser { get; set; }
        public ApplicationUser AssignedUser { get; set; }
        public TicketStatus TicketStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EtaDate { get; set; }

        public DateTime CreationDate { get; }
        public TicketModel()
        {
            CreationDate = DateTime.Now;
        }
    }

    public enum TicketStatus
    {
        New = 0,
        Working = 1,
        InHold = 2,
        Closed = 3,
        Annuled = 4
    }
}