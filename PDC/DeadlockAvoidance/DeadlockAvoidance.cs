using System;
using System.Threading;

namespace DeadlockAvoidance
{
    public class DeadlockDemo3
    {
        public static void Main(string[] args)
        {
            Util.Print("Main Thread Started");

            // Create accounts
            Account account1001 = new Account(1001, 5000);
            Account account1002 = new Account(1002, 3000);

            // Create AccountManager instances for fund transfers
            AccountManager accountManager1 = new AccountManager(account1001, account1002, 5000);
            Thread thread1 = new Thread(() => accountManager1.FundTransfer()) { Name = "Thread-A" };

            AccountManager accountManager2 = new AccountManager(account1002, account1001, 6000);
            Thread thread2 = new Thread(() => accountManager2.FundTransfer()) { Name = "Thread-B" };

            // Start threads
            thread1.Start();
            thread2.Start();

            // Wait for threads to finish (optional, but ensures proper output sequencing)
            // thread1.Join();
            // thread2.Join();

            Util.Print("Main Thread Completed");
        }
    }

    public class Account
    {
        public int ID;
        private double Balance;

        public Account(int id, double balance)
        {
            ID = id;
            Balance = balance;
        }

        public void WithdrawMoney(double amount)
        {
            Balance -= amount;
        }

        public void DepositMoney(double amount)
        {
            Balance += amount;
        }

        public void PrintBalance()
        {
            Util.Print("Rs." + Balance);
        }
    }

    public class AccountManager
    {
        private Account FromAccount;
        private Account ToAccount;
        private double TransferAmount;

        private object lock1, lock2;

        public AccountManager(Account accountFrom, Account accountTo, double amountTransfer)
        {
            FromAccount = accountFrom;
            ToAccount = accountTo;
            TransferAmount = amountTransfer;
            lock1 = new object();
            lock2 = new object();
            if (FromAccount.ID < accountTo.ID)
            {
                lock1 = FromAccount;
                lock2 = ToAccount;
            }
            else
            {
                lock2 = FromAccount;
                lock1 = ToAccount;
            }
        }

        public void FundTransfer()
        {
            Console.WriteLine($"{Util.ThreadName(Thread.CurrentThread)} trying to acquire lock on {FromAccount.ID}");

            lock(lock1)//lock (FromAccount)
            {
                Console.WriteLine($"{Util.ThreadName(Thread.CurrentThread)} acquired lock on {FromAccount.ID}");
                Console.WriteLine($"{Util.ThreadName(Thread.CurrentThread)} Doing Some work");
                Util.Sleep(1000);

                Console.WriteLine($"{Util.ThreadName(Thread.CurrentThread)} trying to acquire lock on {ToAccount.ID}");

               lock(lock2) //lock (ToAccount)
                {
                    FromAccount.WithdrawMoney(TransferAmount);
                    ToAccount.DepositMoney(TransferAmount);
                    Util.Print(Thread.CurrentThread.Name + ": DONE");
                }
            }
        }
    }

    public static class Util
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static string ThreadName(Thread thread)
        {
            return thread.Name;
        }

        public static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}

