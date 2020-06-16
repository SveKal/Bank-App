using System;
using System.Linq;
using Bank.Models;
using Bank.Repositories.Interfaces;
using Bank.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly BankAppDataContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly int Twenty = 20;

        public AccountController(ILogger<AccountController> logger, BankAppDataContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult AccountDetails(int id, string page)
        {
            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);
            var model = new AccountDetailsViewModel();
            model.AccountId = id;
            var transactions = _context.Transactions
                    .Where(d => d.AccountId == id)
                    .OrderByDescending(d => d.Date)
                    .ThenByDescending(d => d.TransactionId)
                    .ToList();

                
                var pageCount = (double)transactions.Count() / Twenty;
                model.PagesMax = (int)Math.Ceiling(pageCount);
                model.Transactions = transactions.Take(Twenty * currentPage).ToList();
                model.PageNow = currentPage;
            return View(model);
        }

        public IActionResult LoadMore(int id, string page)
        {
            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);
            
            var model = new AccountDetailsViewModel();

            model.AccountId = id;

            var transactions = _context.Transactions
                .Where(t => t.AccountId == id)
                .OrderByDescending(d => d.Date)
                .ToList();

            var pageCount = (double)transactions.Count() / Twenty;
            model.PagesMax = (int)Math.Ceiling(pageCount);
            model.Transactions = transactions.Take(Twenty * currentPage).ToList();
            model.PageNow = currentPage;

            return View("_LoadMorePartial", model);
        }
    }
}