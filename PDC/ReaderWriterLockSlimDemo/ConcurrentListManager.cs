using System;
using System.Collections.Generic;
using System.Threading;

class ConcurrentListManager
{
    private readonly List<int> list;
    private readonly ReaderWriterLockSlim rwLock;
    /// <summary>
    /// This iis constructor
    /// </summary>

    public ConcurrentListManager()
    {
        list = new List<int>();// an empty list is intialized.
        rwLock = new ReaderWriterLockSlim();//Reader Writer Lock object
    }


    /// <summary>
    /// This will add value to the list
    /// </summary>
    /// <param name="value">A non negative Integer value is required.</param>

    public void Add(int value)
    {
        rwLock.EnterWriteLock();
        try
        {
            list.Add(value);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}->Added:{value}");
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }

    public IEnumerable<int> Read()
    {
        rwLock.EnterReadLock();
        try
    {
            foreach (var item in list)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}->Reading:{item}");
                yield return item;
            }
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public bool Search(int value)
    {
        rwLock.EnterReadLock();
        try
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}->Searching:{value}");
            return list.Contains(value);
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public void Update(int oldValue, int newValue)
    {
        rwLock.EnterWriteLock();
        try
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}->Updating:{oldValue}");
            int index = list.IndexOf(oldValue);//-1 if does not exist
            if (index != -1)
            {
                list[index] = newValue;
            }
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }

    public void AddOrUpdate(int value)
    {
        rwLock.EnterUpgradeableReadLock();
        try
        {
            if (list.Contains(value))
            {
                Update(value, value); // Example: Updating with the same value
            }
            else
            {
                Add(value);
            }
        }
        finally
        {
            rwLock.ExitUpgradeableReadLock();
        }
    }

    public void Delete(int value)
    {
        rwLock.EnterWriteLock();
        try
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}->Removing:{value}");
            list.Remove(value);
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
}
