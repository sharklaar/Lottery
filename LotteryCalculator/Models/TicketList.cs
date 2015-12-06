using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryCalculator.Models
{
    public class TicketList
    {
        public List<Ticket> Tickets { get; set; }
        public string secondsTaken { get; set; }
        public double AverageProfit { get; set; }
    }
}