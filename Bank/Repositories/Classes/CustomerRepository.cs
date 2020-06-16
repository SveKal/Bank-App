using System.Collections.Generic;
using System.Linq;
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

            if (cust == null) return false;

            return true;
        }

        public bool CustomerExistsByNationalId(string nationalId)
        {
            var customer = _context.Customers.Where(c => c.NationalId == nationalId && c.NationalId != "" && c.NationalId != null)
                .SingleOrDefault();

            if (customer == null) return false;

            return true;
        }

        public void CreateCustomer(CustomerViewModel viewModel)
        {
            if (viewModel.NationalId == null)
                viewModel.NationalId = "";
            var customer = new Customers
            {
                Gender = viewModel.Gender,
                Givenname = viewModel.Givenname,
                Surname = viewModel.Surname,
                Streetaddress = viewModel.Streetaddress,
                City = viewModel.City,
                Zipcode = viewModel.Zipcode,
                Country = viewModel.Country,
                CountryCode = viewModel.CountryCode,
                Birthday = viewModel.Birthday,
                NationalId = viewModel.NationalId,
                Telephonecountrycode = viewModel.Telephonecountrycode,
                Telephonenumber = viewModel.Telephonenumber,
                Emailaddress = viewModel.Emailaddress
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void EditCustomer(CustomerViewModel viewModel)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == viewModel.CustomerId);

            customer.Gender = viewModel.Gender;
            customer.Givenname = viewModel.Givenname;
            customer.Surname = viewModel.Surname;
            customer.Streetaddress = viewModel.Streetaddress;
            customer.City = viewModel.City;
            customer.Zipcode = viewModel.Zipcode;
            customer.Country = viewModel.Country;
            customer.CountryCode = viewModel.CountryCode;
            customer.Birthday = viewModel.Birthday;
            customer.NationalId = viewModel.NationalId;
            customer.Telephonecountrycode = viewModel.Telephonecountrycode;
            customer.Telephonenumber = viewModel.Telephonenumber;
            customer.Emailaddress = viewModel.Emailaddress;

            _context.SaveChanges();
        }
    }
}