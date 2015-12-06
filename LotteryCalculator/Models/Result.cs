using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryCalculator.Models
{
    public class Result
    {
        public List<int> Numbers { get; set; }
        public int BonusNumber { get; set; }
        public DateTime Date { get; set; }
        public DrawEnum DrawType { get; set; }

        public int CheckNumbers(List<int> numbersToCheck)
        {
            var numberMatched = 0;

            foreach (var number in numbersToCheck)
            {
                foreach (var resultNumber in Numbers)
                {
                    if (number == resultNumber)
                        numberMatched++;
                }
            }
            
            return numberMatched;
        }

    }


    public enum DrawEnum
    {
        Lunchtime,
        Teatime
    }
}