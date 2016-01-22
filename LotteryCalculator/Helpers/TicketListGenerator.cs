using LotteryCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LotteryCalculator.Helpers
{
    public class TicketListGenerator
    {
        private int _numberOfTickets = 0;
        private double _totalProfit = 0;

        public TicketList CheckAllResultsInTicketList(List<Ticket> listOfTickets, List<Result> pastResults)
        {
            var isVirgin = false;
            foreach (var ticket in listOfTickets)
            {
                _numberOfTickets++;
                if (ticket.History == null || ticket.History.Count == 0)
                {
                    isVirgin = true;
                    ticket.History = new List<double>();
                }

                foreach (var result in pastResults)
                {
                    var resultMatches = result.CheckNumbers(ticket.Numbers);
                    if (isVirgin)
                    {
                        ticket.History.Add(resultMatches);
                    }
                    if (resultMatches == 6)
                    {
                        ticket.Sixes++;
                    }
                    if (resultMatches == 7)
                    {
                        ticket.Sevens++;
                    }
                    if (resultMatches == 5)
                    {
                        ticket.Fives++;
                    }
                    if (resultMatches == 4)
                    {
                        ticket.Fours++;
                    }
                }

                ticket.Matches = Convert.ToInt32(ticket.History.Sum());

                var latestResults = pastResults[pastResults.Count - 1];

                ticket.MostRecentMatches = latestResults.CheckNumbers(ticket.Numbers);

                var profit = ticket.GetProfit();
                _totalProfit += profit;               
            }

            var fullTicketList = new TicketList();
            fullTicketList.Tickets = listOfTickets.OrderByDescending(x => x.Matches).ToList();
            fullTicketList.AverageProfit = _totalProfit / _numberOfTickets;
            return fullTicketList;
        }
    }
}