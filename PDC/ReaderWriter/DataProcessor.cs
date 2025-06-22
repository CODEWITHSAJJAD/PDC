using System;
namespace ReaderWriter
{
    public class DataProcessor
    {
        Cell cell;
        int attemptCount;
        public DataProcessor(Cell c,int count)
        {
            cell = c;
            attemptCount = count;
        }

        public void WriteAttempt()
        {
                for (int i = 1; i <= attemptCount; i++)
                {
                    cell.Write(i);
                }
        }

        public void ReadAttempt()
        {
             for (int i = 1; i <= attemptCount; i++)
                {
                    cell.Read();
                }
        }
    }
}
