using System;
using Bank.Controllers;
using Xunit;
using Bank.Models;
using Bank.ViewModels;
using Microsoft.EntityFrameworkCore;
using Bank.Repositories.Classes;
using Bank.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;


namespace BankUnitTests
{
    public class TransactionUnitTest
    {
        private readonly BankAppDataContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionController _transactionController;

        public TransactionUnitTest()
        {
            DbContextOptionsBuilder<Bank.Models.BankAppDataContext> optionsBuilder = new DbContextOptionsBuilder<Bank.Models.BankAppDataContext>();
            optionsBuilder.UseSqlServer("Server=localhost; Database=BankAppData3 ;Trusted_Connection=True;");
            _context = new BankAppDataContext(optionsBuilder.Options);
            _transactionRepository = new TransactionRepository(_context);
            _transactionController = new TransactionController(_context, _transactionRepository);
        }
        [Fact]
        public void Withdrawal_More_Than_Balance_Returns_False()
        {
            var transaction = new Transactions();
            var viewModel = new TransactionViewModel();
            var account = new Accounts();

            viewModel.AccountId = 402;
            viewModel.Amount = 50000.00m;
           
            
            Assert.False(_transactionRepository.Withdrawal(viewModel, transaction, account));
        }
        [Fact]
        public void Transfer_More_Than_Balance_Returns_False()
        {
            var fromTransaction = new Transactions();
            var toTransaction = new Transactions();
            var viewModel = new TransactionViewModel();
            var fromAccount = new Accounts();
            var toAccount = new Accounts();

            fromAccount.AccountId = 402;
            viewModel.Amount = 50000.00m;

            Assert.False(_transactionRepository.Transfer(viewModel, fromAccount, toAccount, fromTransaction, toTransaction));
        }
        [Fact]
        public void Can_Not_Deposit_Negative_Amount()
        {
            var vm = new TransactionViewModel();
            vm.Amount = -100.00m;
            var controller = new TransactionController(_context, _transactionRepository);
            controller.ModelState.AddModelError(String.Empty, "Amount can not be lower than 1 or higher than 50000. For higher transactions please contact the bank.");
            var result = controller.Deposit(vm);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
        [Fact]
        public void Can_Not_Withdraw_Negative_Amount()
        {
            var vm = new TransactionViewModel();
            vm.Amount = -100.00m;
            var controller = new TransactionController(_context, _transactionRepository);
            controller.ModelState.AddModelError(String.Empty, "Amount can not be lower than 1 or higher than 50000. For higher transactions please contact the bank.");
            var result = controller.Withdrawal(vm);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Deposit_Created_Correctly()
        {
            var transaction = new Transactions();
            var viewModel = new TransactionViewModel();
            var account = new Accounts();

            account.AccountId = 402;
            account.Frequency = "Monthly";
            viewModel.AccountId = 402;
            viewModel.Amount = 1000.00m;
            viewModel.Operation = "Test";
            
            Assert.True(_transactionRepository.Deposit(viewModel, transaction, account));
        }
        [Fact]
        public void Withdrawal_Created_Correctly()
        {
            var transaction = new Transactions();
            var viewModel = new TransactionViewModel();
            var account = new Accounts();

            account.AccountId = 402;
            account.Frequency = "Monthly";
            viewModel.AccountId = 402;
            viewModel.Amount = -100.00m;
            viewModel.Operation = "Test";

            Assert.True(_transactionRepository.Withdrawal(viewModel, transaction, account));
        }
    }
}
