﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Common.Paging;
using Bank.Models;
using Bank.Repositories.Interfaces;
using Bank.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Controllers
{
    [Authorize(Roles = "Cashier, Admin")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly BankAppDataContext _context;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ILogger<CustomerController> logger, BankAppDataContext context, ICustomerRepository cust)
        {
            _logger = logger;
            _context = context;
            _customerRepository = cust;
        }

        public IActionResult Customers()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Customers(string searchString, int? pageNumber, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            var customer = from s in _context.Customers
                select s;
            if (!string.IsNullOrEmpty(searchString))
                customer = customer.Where(c => c.Surname.Contains(searchString)
                                               || c.Givenname.Contains(searchString)
                                               || c.City.Contains(searchString));

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            var pageSize = 50;
            return View(await PaginatedList<Customers>.CreateAsync(customer.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public  IActionResult CustomersById(string searchString)
        {
            int.TryParse(searchString, out var searchInt);
            
            if (!string.IsNullOrEmpty(searchString))
            {
                if(_customerRepository.CustomerExists(searchInt))
                    return RedirectToAction("CustomerDetails", new { id = searchInt });
            }

            ModelState.AddModelError(string.Empty, "Account does not exist");
            return RedirectToAction("Customers");
        }

        public IActionResult CustomerDetails(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId == id);

            var accounts = _context.Dispositions
                .Where(d => d.CustomerId == id)
                .Select(s => s.Account)
                .ToList();

            var model = new CustomerDetailsViewModel
            {
                Customer = customer,
                Accounts = accounts
            };

            return View(model);
        }

        public IActionResult CreateCustomer()
        {
            var viewModel = new CustomerViewModel();
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_customerRepository.CustomerExistsByNationalId(viewModel.NationalId))
                {
                    _customerRepository.CreateCustomer(viewModel);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Customer with this National ID already exists.");
                    return View("CreateCustomer", viewModel);
                }
                return RedirectToAction("Customers");
            }
            return View("CreateCustomer", viewModel);
        }
        public IActionResult EditCustomer(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            var viewModel = new CustomerViewModel()
            {
                CustomerId = customer.CustomerId,
                Gender = customer.Gender,
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                Streetaddress = customer.Streetaddress,
                City = customer.City,
                Zipcode = customer.Zipcode,
                Country = customer.Country,
                CountryCode = customer.CountryCode,
                Birthday = customer.Birthday,
                NationalId = customer.NationalId,
                Telephonecountrycode = customer.Telephonecountrycode,
                Telephonenumber = customer.Telephonenumber,
                Emailaddress = customer.Emailaddress
            };
            return View("EditCustomer", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               _customerRepository.EditCustomer(viewModel);
                return RedirectToAction("Customers");
            }

            return View("EditCustomer", viewModel);
        }

    }
}