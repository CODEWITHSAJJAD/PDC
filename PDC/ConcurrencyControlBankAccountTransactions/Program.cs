using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyControlBankAccountTransactions
{
class BankAccount
    {
        private decimal balance;
        private readonly object balanceLock = new object();

        public BankAccount(decimal initialBalance) => balance = initialBalance;

        public void Deposit(decimal amount)
        {
            Monitor.Enter(balanceLock);
            try
            {
                Console.WriteLine($"Depositing {amount:C}... (Thread {Thread.CurrentThread.ManagedThreadId})");
                balance += amount;
                Console.WriteLine($"New balance: {balance:C}");
            }
            finally
            {
                Monitor.Exit(balanceLock);
            }
        }

        public bool Withdraw(decimal amount)
        {
            bool success = false;
            Monitor.Enter(balanceLock);
            try
            {
                if (balance >= amount)
                {
                    Console.WriteLine($"Withdrawing {amount:C}... (Thread {Thread.CurrentThread.ManagedThreadId})");
                    balance -= amount;
                    success = true;
                    Console.WriteLine($"New balance: {balance:C}");
                }
                else
                {
                    Console.WriteLine($"Failed to withdraw {amount:C}. Insufficient funds.");
                }
            }
            finally
            {
                Monitor.Exit(balanceLock);
            }
            return success;
        }

        public decimal GetBalance()
        {
            Monitor.Enter(balanceLock);
            try
            {
                return balance;
            }
            finally
            {
                Monitor.Exit(balanceLock);
            }
        }
    }

    class MainClass
    {
        static void Main()
        {
            var account = new BankAccount(1000m);
            var rnd = new Random();

            // Simulate 5 ATMs accessing the same account
            Parallel.For(0, 5, Process);

            Console.WriteLine($"\nFinal balance: {account.GetBalance():C}");
        }

        public static void Process(int i)
        {
            var account = new BankAccount(1000m);
            var rnd = new Random();
            //i =>
            //{
            for (int j = 0; j < 3; j++)
                {
                    Thread.Sleep(rnd.Next(300, 800)); // Random delay

                    if (rnd.NextDouble() > 0.5)
                    {
                        account.Deposit(rnd.Next(50, 301));
                    }
                    else
                    {
                        account.Withdraw(rnd.Next(20, 401));
                    }
                    Task.Delay(1000).Wait();
                }
            //}
        }
    }
}
