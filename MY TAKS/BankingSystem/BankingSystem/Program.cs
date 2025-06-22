using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("\t\t╔══════════════════════════════════════════════════╗");
            Console.WriteLine("\t\t║             BANKING SYSTEM SIMULATION            ║");
            Console.WriteLine("\t\t╚══════════════════════════════════════════════════╝");
            Console.WriteLine("\n");
            Console.WriteLine("\t\t\tSubmitted To: Sir Shahid Jamil");
            Console.WriteLine("\t\t\tSubmitted By: Sajjad Shahid");
            Console.WriteLine("\t\t\tArid Number: 22-Arid-4176");
            Console.WriteLine("\n");
            Console.WriteLine("\t\t--------------------------------------------------");
            Console.WriteLine("\n");

            int accountCount = GetAccountCountFromUser();           
            List<Account> accounts = CreateAccounts(accountCount);            
            DisplayAccountBalances(accounts, "Initial Account Balances:");            
            List<(Account, Account)> transactionPairs = GenerateTransactionPairs(accounts);            
            var transactionLog = new List<TransactionLogEntry>();
            var lockObject = new object();

            Parallel.ForEach(transactionPairs, pair =>
            {
                var source = pair.Item1;
                var destination = pair.Item2;            
                var manager = new AccountManager(source, destination);                
                decimal transferAmount = CalculateTransferAmount(source);                
                var logEntry = manager.TransferAmount(transferAmount);                
                lock (lockObject)
                {
                    transactionLog.Add(logEntry);
                }
            });
            
            DisplayTransactionLog(transactionLog);            
            DisplayAccountBalances(accounts, "Final Account Balances:");
        }

        private static int GetAccountCountFromUser()
        {
            int count;
            do
            {
                Console.Write("Enter number of accounts to create (2-10): ");
            } while (!int.TryParse(Console.ReadLine(), out count) || count < 2 || count > 10);

            return count;
        }

        private static List<Account> CreateAccounts(int count)
        {
            var accounts = new List<Account>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                decimal balance = random.Next(2000, 10001); // Random balance between 2000 and 10000
                accounts.Add(new Account(i, $"Account {i}", balance));
            }

            return accounts;
        }

        private static List<(Account, Account)> GenerateTransactionPairs(List<Account> accounts)
        {
            var pairs = new List<(Account, Account)>();

            for (int i = 0; i < accounts.Count; i++)
            {
                for (int j = 0; j < accounts.Count; j++)
                {
                    if (i != j) // No self-transactions
                    {
                        pairs.Add((accounts[i], accounts[j]));
                    }
                }
            }

            return pairs;
        }

        private static decimal CalculateTransferAmount(Account source)
        {
            var random = new Random();
            decimal percentage = random.Next(1, 21) / 100m; // 1-20%
            return Math.Round(source.Balance * percentage, 2);
        }

        private static void DisplayAccountBalances(List<Account> accounts, string title)
        {
            Console.WriteLine($"\n{title}");
            foreach (var account in accounts)
            {
                account.PrintBalance();
            }
        }

        private static void DisplayTransactionLog(List<TransactionLogEntry> log)
        {
            Console.WriteLine("\nTransaction Log:");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("| From | To  | Amount  | Time                | Status    |");
            Console.WriteLine("------------------------------------------------------------");

            foreach (var entry in log)
            {
                Console.WriteLine($"| {entry.SourceAccountId,-4} | {entry.DestinationAccountId,-4} | " +
                                $"Rs.{entry.Amount,-7} | {entry.Timestamp,-19} | {entry.Status,-9} |");
            }

            Console.WriteLine("------------------------------------------------------------");
        }
    }

    public class Account
    {
        public int Id { get; }
        public string PersonName { get; }
        public decimal Balance { get; private set; }

        public Account(int id, string personName, decimal balance)
        {
            Id = id;
            PersonName = personName;
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");

            lock (this)
            {
                Balance += amount;
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            lock (this)
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    return true;
                }
                return false;
            }
        }

        public void PrintBalance()
        {
            Console.WriteLine($"{PersonName} (ID: {Id}): Rs.{Balance}");
        }
    }

    public class AccountManager
    {
        private readonly Account _sourceAccount;
        private readonly Account _destinationAccount;

        public AccountManager(Account sourceAccount, Account destinationAccount)
        {
            _sourceAccount = sourceAccount;
            _destinationAccount = destinationAccount;
        }

        public TransactionLogEntry TransferAmount(decimal amount)
        {
            if (amount <= 0)
            {
                return new TransactionLogEntry(
                    _sourceAccount.Id,
                    _destinationAccount.Id,
                    amount,
                    DateTime.Now,
                    "Failed: Invalid amount");
            }

            // Determine lock order to prevent deadlocks
            Account firstLock, secondLock;
            if (_sourceAccount.Id < _destinationAccount.Id)
            {
                firstLock = _sourceAccount;
                secondLock = _destinationAccount;
            }
            else
            {
                firstLock = _destinationAccount;
                secondLock = _sourceAccount;
            }

            bool success = false;
            string status = "Failed";

            lock (firstLock)
            {
                lock (secondLock)
                {
                    if (_sourceAccount.Withdraw(amount))
                    {
                        _destinationAccount.Deposit(amount);
                        success = true;
                        status = "Success";
                    }
                    else
                    {
                        status = "Failed: Insufficient funds";
                    }
                }
            }

            return new TransactionLogEntry(
                _sourceAccount.Id,
                _destinationAccount.Id,
                amount,
                DateTime.Now,
                status);
        }
    }

    public class TransactionLogEntry
    {
        public int SourceAccountId { get; }
        public int DestinationAccountId { get; }
        public decimal Amount { get; }
        public DateTime Timestamp { get; }
        public string Status { get; }

        public TransactionLogEntry(int sourceId, int destId, decimal amount, DateTime timestamp, string status)
        {
            SourceAccountId = sourceId;
            DestinationAccountId = destId;
            Amount = amount;
            Timestamp = timestamp;
            Status = status;
        }
    }
}