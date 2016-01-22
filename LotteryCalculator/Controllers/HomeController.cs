using Excel;
using HtmlAgilityPack;
using LotteryCalculator.Helpers;
using LotteryCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryCalculator.Controllers
{
    public class HomeController : Controller
    {
        private Random _random = new Random();
        private TicketListGenerator _ticketListGenerator;
        private DatabaseHelper _databaseHelper = new DatabaseHelper();
        public ActionResult Index()
        {
            return View(GetWinnersForDrawType(DrawEnum.All));
        }

        public ActionResult LunchtimeStats()
        {
            ViewBag.Message = "Your application description page.";

            return View(GetWinnersForDrawType(DrawEnum.Lunchtime));

        }

        public ActionResult TeatimeStats()
        {
            ViewBag.Message = "Your application description page.";

            return View(GetWinnersForDrawType(DrawEnum.Teatime));
        }

        public ActionResult GetChart(Ticket ticketToChart)
        {
            return View(ticketToChart);
        }

        public ActionResult AddResults()
        {
            return View();
        }

        private TicketList GetWinnersForDrawType(DrawEnum drawType)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            _ticketListGenerator = new TicketListGenerator();

            List<Result> pastResults;

            if (drawType == DrawEnum.All)
            {
                pastResults = _databaseHelper.GetAllResults();
            }
            else
            {
                pastResults = _databaseHelper.GetAllResults().Where(x => x.DrawType == drawType).ToList();
            }

            var listOfTickets = new List<Ticket>();

            for (var i = 0; i <= 100000; i++)
            {
                var ticket = GetRandomNumbers(15);
                listOfTickets.Add(ticket);
            }

            var fullTicketList = new TicketList();
            fullTicketList = _ticketListGenerator.CheckAllResultsInTicketList(listOfTickets, pastResults);

            var topThousand = new TicketList();
            topThousand.Tickets = fullTicketList.Tickets.Take(1000).ToList();

            var lastTwentyResults = pastResults.OrderByDescending(t => t.Date).Take(20).ToList();

            var topHundred = _ticketListGenerator.CheckAllResultsInTicketList(topThousand.Tickets, lastTwentyResults);

            var topTen = new TicketList();
            topTen.Tickets = topHundred.Tickets.Take(10).ToList();

            var lastTenResults = pastResults.OrderByDescending(t => t.Date).Take(10).ToList();
            var winners = _ticketListGenerator.CheckAllResultsInTicketList(topTen.Tickets, lastTenResults);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            winners.secondsTaken = elapsedMs.ToString();

            return winners;
        }

        [HttpGet]
        public ActionResult AddResult()
        {
            var result = new Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult AddResult(Result result)
        {
            _databaseHelper.AddResult(result);
            return View();
        }

        private Ticket GetRandomNumbers(int count)
        {
            var ticket = new Ticket();
            ticket.Numbers = new List<int>();

            for (var j = 1; j <= count; j++)
            {
                var number = GetRandomNumber(1, 49, ticket);
                ticket.Numbers.Add(number);
            }

            return ticket;
        }

        public int GetRandomNumber(int minimum, int maximum, Ticket ticket)
        {
            
            var number = _random.Next(minimum, maximum);

            if (ticket.Numbers.IndexOf(number) == -1)
            {
                return number;
            }

            return GetRandomNumber(1, 49, ticket);

        }
    }
}