using System.Collections.Generic;
using Bank.Models;

namespace Bank.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customers> GetAll();
        Customers GetCustomerById(int id);
        bool CustomerExists(int id);
    }
}