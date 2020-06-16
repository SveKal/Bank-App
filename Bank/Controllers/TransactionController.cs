using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;
using Bank.Repositories.Interfaces;
using Bank.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BankAppDataContext _context;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(BankAppDataContext context, ITransactionRepository transactionRepository)
        {
            _context = context;
            _transactionRepository = transactionRepository;
        }

        public IActionResult Deposit(int id)
        {
            var account = _transactionRepository.GetAccountById(id);

            var viewModel = new TransactionViewModel
            {
                AccountId = id,
                Balance = account.Balance,
            };
            return View(viewModel);
        }
        public IActionResult Withdrawal(int id)
        {
            var account = _transactionRepository.GetAccountById(id);

            var model = new TransactionViewModel
            {
                AccountId = id,
                Balance = account.Balance,
            };
            return View(model);
        }
        public IActionResult Transfer(int id, int toId)
        {
            var account = _transactionRepository.GetAccountById(id);
            var toAccount = _transactionRepository.GetAccountById(toId);
            var model = new TransactionViewModel
            {
                AccountId = id,
                Balance = account.Balance,
                ToAccountId = toId
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(TransactionViewModel viewModel)
        {
            var account = _transactionRepository.GetAccountById(viewModel.AccountId);
            var transaction = new Transactions();
            if (ModelState.IsValid)
            {
                viewModel.Operation = "Credit in Cash";
                _transactionRepository.Deposit(viewModel, transaction, account);
                return RedirectToAction("AccountDetails", "Account", new {id = account.AccountId, page = 1});
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdrawal(TransactionViewModel viewModel)
        {
            var account = _transactionRepository.GetAccountById(viewModel.AccountId);
            var transaction = new Transactions();
            if (ModelState.IsValid)
            {
                if (_transactionRepository.HasCoverage(account.Balance, viewModel.Amount))
                {
                    viewModel.Operation = "Withdrawal in Cash";
                    _transactionRepository.Withdrawal(viewModel,transaction, account);
                    return RedirectToAction("AccountDetails", "Account", new { id = account.AccountId, page = 1 });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Account has not enough coverage for withdrawal.");
                }
                
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(TransactionViewModel viewModel)
        {
            var fromAccount = _transactionRepository.GetAccountById(viewModel.AccountId);
            var toAccount = _transactionRepository.GetAccountById(viewModel.ToAccountId);
            var fromTransaction = new Transactions();
            var toTransaction = new Transactions();
            if (ModelState.IsValid)
            {
                if (_transactionRepository.AccountExists(viewModel.ToAccountId))
                {
                    if (_transactionRepository.SameAccount(viewModel.AccountId, viewModel.ToAccountId))
                    {
                        if (_transactionRepository.HasCoverage(fromAccount.Balance, viewModel.Amount))
                        {
                            _transactionRepository.Transfer(viewModel, fromAccount, toAccount, fromTransaction, toTransaction);
                            return RedirectToAction("AccountDetails", "Account", new { id = fromAccount.AccountId, page = 1 });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Account has not enough coverage.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "You can not transfer to the same account.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Account does not exist.");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
