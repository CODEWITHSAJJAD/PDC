using System;
using System.Threading;

namespace DeadLockDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");

            // Create accounts
            Account account1001 = new Account(1001, 5000);
            Account account1002 = new Account(1002, 3000);

            // Create AccountManager instances for fund transfers
            AccountManager accountManager1 = new AccountManager(account1001, account1002, 5000);
            Thread thread1 = new Thread(() => accountManager1.FundTransferInfiniteAttempt());
            thread1.Name = "Thread-A";
         

            AccountManager accountManager2 = new AccountManager(account1002, account1001, 6000);
            Thread thread2 = new Thread(() => accountManager2.FundTransferInfiniteAttempt()) { Name = "Thread-B" };

            // Start threads
            thread1.Start();
            thread2.Start();

            // Wait for threads to finish (optional, but ensures proper output sequencing)
            // thread1.Join();
            // thread2.Join();

            Console.WriteLine("Main Thread Completed");
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
            Console.WriteLine("Rs." + Balance);
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
            lock1 = FromAccount;
            lock2 = ToAccount;
        }

        /// <summary>
        /// This function will cause deadlock
        /// </summary>
        public void FundTransfer()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {FromAccount.ID}");

            lock (lock1)//lock (FromAccount)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {FromAccount.ID}");
                Console.WriteLine($"{Thread.CurrentThread.Name} Doing Some work");
                Thread.Sleep(1000);

                Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {ToAccount.ID}");

                lock (lock2) //lock (ToAccount)
                {
                    FromAccount.WithdrawMoney(TransferAmount);
                    ToAccount.DepositMoney(TransferAmount);
                    Console.WriteLine(Thread.CurrentThread.Name + ": DONE");
                }
            }
        }

        /// <summary>
        /// This function will avoid deadlock by using wait timeout, and reattempt
        /// again and again with an interval of 2 seconds.
        /// </summary>
        public void FundTransferInfiniteAttempt()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {FromAccount.ID}");
            var status1 = Monitor.TryEnter(lock1, 3000);// new TimeSpan(0, 0, 3));

            if (status1)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {FromAccount.ID}");
                Console.WriteLine($"{Thread.CurrentThread.Name} Doing Some work");
                Thread.Sleep(1000);

                Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {ToAccount.ID}");

                var status2 = Monitor.TryEnter(lock2, new TimeSpan(0, 0, 3));
                if(status2)
                {
                    FromAccount.WithdrawMoney(TransferAmount);
                    ToAccount.DepositMoney(TransferAmount);
                    Console.WriteLine(Thread.CurrentThread.Name + ": DONE");
                    Monitor.Exit(lock2);
                    Monitor.Exit(lock1);
                }
                else
                {
                    Monitor.Exit(lock1);
                    Thread.Sleep(2000);
                    FundTransferInfiniteAttempt();//selfcall.

                }
            }
            else
            {
                Thread.Sleep(2000);
                FundTransferInfiniteAttempt();//selfcall.
            }
        }

        /// <summary>
        /// This function will avoid deadlock by using wait timeout, and reattempt
        /// 5 times with an interval of 2 seconds.
        /// </summary>
        public void FundTransferWithFewAttempts()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {FromAccount.ID}");

            bool lock1Acquired = false;
            bool lock2Acquired = false;
            int maxRetries = 5; // Maximum number of retries to acquire both locks
            int retryCount = 0;
            TimeSpan retryDelay = TimeSpan.FromMilliseconds(2000); // Delay between retries

            var flag = false;//no transaction is performed.
            while (retryCount < maxRetries)
            {
                try
                {
                    // Try to acquire lock on FromAccount (lock1) with timeout
                    lock1Acquired = Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(3000)); // Try for 500ms
                    if (!lock1Acquired)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name} could not acquire lock on {FromAccount.ID}, retrying...");
                        retryCount++;
                        Thread.Sleep(retryDelay); // Wait before retrying
                        continue; // Retry acquiring lock1
                    }

                    Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {FromAccount.ID}");
                    Console.WriteLine($"{Thread.CurrentThread.Name} Doing Some work");
                    Thread.Sleep(1000); // Simulate work

                    Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {ToAccount.ID}");

                    // Try to acquire lock on ToAccount (lock2) with timeout
                    lock2Acquired = Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(500)); // Try for 500ms
                    if (!lock2Acquired)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name} could not acquire lock on {ToAccount.ID}, retrying...");
                        retryCount++;
                        Thread.Sleep(retryDelay); // Wait before retrying
                        continue; // Retry acquiring lock2
                    }

                    Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {ToAccount.ID}");

                    // Perform the fund transfer
                    FromAccount.WithdrawMoney(TransferAmount);
                    ToAccount.DepositMoney(TransferAmount);
                    Console.WriteLine(Thread.CurrentThread.Name + ": DONE");
                    flag = true;
                    break; // Break out of retry loop when both locks are successfully acquired and the transaction is done
                }
                finally
                {
                    // Ensure that the locks are released if they were acquired
                    if (lock1Acquired)
                        Monitor.Exit(lock1);
                    if (lock2Acquired)
                        Monitor.Exit(lock2);
                }
            }

            if (flag==false)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} could not complete transaction after {maxRetries} retries.");
            }
        }

    }
}