using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryCalculator.Models
{
    public class Ticket
    {
        public List<int> Numbers { get; set; }
        public int Matches { get; set; }
        public int MostRecentMatches { get; set;}
        public int Sixes { get; set; }
        public int Sevens { get; set; }
        public int Fives { get; set; }
        public int Fours { get; set; }
        public List<int> History { get; set; }

        public double GetProfit()
        {
            var outlay = 57.33 * 62 * -1;

            var foursProfit = Fours * (0.02 * 4501);

            var fivesProfit = Fives * (0.01 * 50001);

            var fivesFromSixesProfit = (0.01 * 50001 * 6 * Sixes);

            var foursFromSixesProfit = (0.02 * 4501 * 15 * Sixes);

            var fivesFromSevensProfit = (0.01 * 50001 * 21 * Sevens);

            var foursFromSevensProfit = (0.02 * 4501 * 35 * Sevens);

            var totalIncome = foursProfit +
                                fivesProfit +
                                fivesFromSixesProfit +
                                fivesFromSevensProfit +
                                foursProfit +
                                foursFromSixesProfit +
                                foursFromSevensProfit;

            return outlay + totalIncome;


        }
    }
}