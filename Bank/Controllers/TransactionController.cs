using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Deposit()
        {
            return View();
        }
        public IActionResult Withdrawal()
        {
            return View();
        }
        public IActionResult Transfer()
        {
            return View();
        }
    }
}
