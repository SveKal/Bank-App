using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bank.Models;
using Bank.Repositories.Interfaces;
using Bank.ViewModels;

namespace Bank.Repositories.Classes
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankAppDataContext _context;

        public CustomerRepository(BankAppDataContext context)
        {
            _context = context;
        }

        public List<Customers> GetAll()
        {
            var categories = _context.Customers.ToList();
            return categories;
        }

        public Customers GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.CustomerId == id);
        }
        public bool CustomerExists(int id)
        {
            var cust = _context.Customers.Where(c => c.CustomerId == id).SingleOrDefault();

            if (cust == null)
            {
                return false;
            }

            return true;
        }
    }
}