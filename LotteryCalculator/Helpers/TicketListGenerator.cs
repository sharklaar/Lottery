using Excel;
using LotteryCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryCalculator.Helpers
{
    public class TicketListGenerator
    {
        public List<Result> GetExcelShit()
        {
            var resultsList = new List<Result>();

            foreach (var worksheet in Workbook.Worksheets(@"C:\dev\Lottery\LotteryCalculator\49s Results.xlsx"))
            {
                foreach (var row in worksheet.Rows)
                {
                    var result = new Result();

                    result.Numbers = new List<int>();

                    foreach (var cell in row.Cells)
                    {
                        if (cell.ColumnIndex == 0)
                        {
                            var dateValue = System.Convert.ToDouble(cell.Value);
                            var date = DateTime.FromOADate(dateValue);
                            result.Date = date;
                        }
                        else if (cell.ColumnIndex == 1)
                        {
                            if (cell.Value == "LUNCHTIME")
                            {
                                result.DrawType = DrawEnum.Lunchtime;
                            }
                            else
                            {
                                result.DrawType = DrawEnum.Teatime;
                            }
                        }
                        else
                        {
                            result.Numbers.Add(System.Convert.ToInt32(cell.Value));
                        }
                    }

                    resultsList.Add(result);
                }
            }

            return resultsList;
        }

        private int numberOfTickets = 0;
        private double totalProfit = 0;

        public TicketList CheckAllResultsInTicketList(List<Ticket> listOfTickets, List<Result> pastResults)
        {

            foreach (var ticket in listOfTickets)
            {
                numberOfTickets++;

                ticket.History = new List<double>();

                foreach (var result in pastResults)
                {
                    var resultMatches = result.CheckNumbers(ticket.Numbers);
                    ticket.History.Add(resultMatches);
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

                ticket.Matches = System.Convert.ToInt32(ticket.History.Sum());

                var latestResults = pastResults[pastResults.Count - 1];

                ticket.MostRecentMatches = latestResults.CheckNumbers(ticket.Numbers);

                var profit = ticket.GetProfit();
                totalProfit += profit;               
            }

            var fullTicketList = new TicketList();
            fullTicketList.Tickets = listOfTickets.OrderByDescending(x => x.Matches).ToList();
            fullTicketList.AverageProfit = totalProfit / numberOfTickets;
            return fullTicketList;
        }
    }
}