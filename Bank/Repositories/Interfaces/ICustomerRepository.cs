using System.Collections.Generic;
using Bank.Models;
using Bank.ViewModels;

namespace Bank.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customers> GetAll();
        Customers GetCustomerById(int id);
        bool CustomerExists(int id);
        public void CreateCustomer(CustomerViewModel viewModel);
        public bool CustomerExistsByNationalId(string nationalId);
        public void EditCustomer(CustomerViewModel viewModel);
    }
}