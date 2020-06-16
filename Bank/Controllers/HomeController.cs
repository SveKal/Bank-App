using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bank.Models;
using Bank.ViewModels;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankAppDataContext _context;

        public HomeController(ILogger<HomeController> logger, BankAppDataContext context)
        {
            _logger = logger;
            _context = context;
        }
        [ResponseCache(Duration = 30)]
        public IActionResult IndexDefault()
        {
            return View("Index");
        }
        [ResponseCache(Duration = 30)]
        public IActionResult Index()
        {
            var amountCustomers = _context.Customers.Select(x => x.CustomerId).Distinct().Count();
            var amountAccounts = _context.Accounts.Select(x => x.AccountId).Distinct().Count();
            var sumAccounts = _context.Accounts.Select(x => x.Balance).Sum();
            var homeViewModel = new HomeViewModel()
            {
                AmountCustomers = amountCustomers,
                AmountAccounts = amountAccounts,
                SumAccounts = sumAccounts
            };

            return View(homeViewModel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
