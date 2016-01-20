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
            var watch = System.Diagnostics.Stopwatch.StartNew();

            _ticketListGenerator = new TicketListGenerator();

            var pastResults = _databaseHelper.GetAllResults();

            //var myNumbers = new Ticket();
            //myNumbers.Numbers = new List<int>{
            //    1, 6, 9, 13, 18, 
            //};            
            
            var listOfTickets = new List<Ticket>();

            for (var i = 0; i <= 100000; i++)
            {
                var ticket = GetRandomNumbers(15);
                listOfTickets.Add(ticket);
            }

            //listOfTickets.Add(myNumbers);

            var fullTicketList = new TicketList();
            fullTicketList = _ticketListGenerator.CheckAllResultsInTicketList(listOfTickets, pastResults);

            var topThousand = new TicketList();
            topThousand.Tickets = fullTicketList.Tickets.Take(1000).ToList();

            var lastTwentyResults = pastResults.OrderByDescending(t => t.Date).Take(20).ToList();

            var topHundred = _ticketListGenerator.CheckAllResultsInTicketList(topThousand.Tickets, lastTwentyResults);

            var topTen = new TicketList();
            topTen.Tickets = topHundred.Tickets.Take(10).ToList();

            var lastTenResults = pastResults.OrderByDescending(t => t.Date).Take(10).ToList();
            var winners = _ticketListGenerator.CheckAllResultsInTicketList(topTen.Tickets, pastResults);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            winners.secondsTaken = elapsedMs.ToString();

            return View(winners);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GetChart(Ticket ticketToChart)
        {
            return View(ticketToChart);
        }

        public ActionResult AddResults()
        {
            var dbHelper = new DatabaseHelper();
            var result = new Result();
            result.Date = DateTime.Today;
            result.BonusNumber = 7;
            result.Numbers = new List<int>
            {
                1,2,3,4,5,6
            };
            result.DrawType = DrawEnum.Lunchtime;
            dbHelper.AddResult(result);
            return View();
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

        private void GetLatestNumbers()
        {
            string Url = "http://something";
            var web = new HtmlWeb();
            var htmlDoc = web.Load(Url);

            HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        }
    }
}