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
        private readonly ILogger<TransactionController> _logger;
        private readonly BankAppDataContext _context;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ILogger<TransactionController> logger, BankAppDataContext context, ITransactionRepository transactionRepository)
        {
            _logger = logger;
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
                account.Balance += viewModel.Amount;
                _transactionRepository.Update(account);
                transaction.AccountId = viewModel.AccountId;
                transaction.Date = DateTime.Now;
                transaction.Type = "Credit";
                transaction.Operation = "Credit in Cash";
                transaction.Amount = viewModel.Amount;
                transaction.Balance = account.Balance;
                _transactionRepository.CreateTransaction(transaction);
                return RedirectToAction("AccountDetails", "Account", new {id = account.AccountId, page = 1});
            }
            return View("Deposit", viewModel);
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
                    account.Balance -= viewModel.Amount;
                    _transactionRepository.Update(account);
                    transaction.AccountId = viewModel.AccountId;
                    transaction.Date = DateTime.Now;
                    transaction.Type = "Debit";
                    transaction.Operation = "Debit in Cash";
                    transaction.Amount = -viewModel.Amount;
                    transaction.Balance = account.Balance;
                    _transactionRepository.CreateTransaction(transaction);
                    return RedirectToAction("AccountDetails", "Account", new { id = account.AccountId, page = 1 });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Account has not enough coverage for withdrawal.");
                }
                
            }
            return View("Withdrawal", viewModel);
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
                            fromAccount.Balance -= viewModel.Amount;
                            toAccount.Balance += viewModel.Amount;
                            _transactionRepository.Update(fromAccount);
                            _transactionRepository.Update(toAccount);
                            fromTransaction.AccountId = viewModel.AccountId;
                            fromTransaction.Date = DateTime.Now;
                            fromTransaction.Type = "Debit";
                            fromTransaction.Operation = "Transfer to other Account";
                            fromTransaction.Amount = -viewModel.Amount;
                            fromTransaction.Balance = fromAccount.Balance;
                            _transactionRepository.CreateTransaction(fromTransaction);
                            toTransaction.AccountId = viewModel.ToAccountId;
                            toTransaction.Date = DateTime.Now;
                            toTransaction.Type = "Credit";
                            toTransaction.Operation = "Transfer from other Account";
                            toTransaction.Amount = viewModel.Amount;
                            toTransaction.Balance = toAccount.Balance;
                            _transactionRepository.CreateTransaction(toTransaction);
                            return RedirectToAction("AccountDetails", "Account", new { id = fromAccount.AccountId, page = 1 });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Account has not enough coverage for withdrawal.");
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
            return View("Transfer", viewModel);
        }
    }
}
