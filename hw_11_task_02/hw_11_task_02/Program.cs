class AdvancedArray
{
    public AdvancedArray(int[] arr)
    {
        this.buf = arr;
        this.sync = new ReaderWriterLockSlim();
        this.random = new Random();
    }

    public void Sort()
    {
        sync.EnterWriteLock();
        Array.Sort(buf);
        sync.ExitWriteLock();
    }

    public void Swap()
    {
        sync.EnterWriteLock();
        int i = random.Next(buf.Length);
        int j = random.Next(buf.Length);
        int tmp = buf[i];
        buf[i] = buf[j];
        buf[j] = tmp;
        sync.ExitWriteLock();

    }
    public int Min()
    {
        int res = 0;
        sync.EnterReadLock();
        res = buf.Min();
        sync.ExitReadLock();
        return res;
    }

    public double Avg()
    {
        double res = 0;
        sync.EnterReadLock();

        Array.Sort(buf);
        int middleIndex = buf.Length / 2;
        if (buf.Length % 2 == 0)
        {
            res = (buf[middleIndex] + buf[middleIndex - 1]) / 2;
        }
        else
        {
            res = buf[middleIndex];
        }

        sync.ExitReadLock();
        return res;
    }
    private ReaderWriterLockSlim sync;
    private Random random;
    private int[] buf;
}

class Program
{
    static void Main(String[] args)
    {
        int[] arr = { 0, 5, 7, 3, 2, 9, 20};
        var AdvArr = new AdvancedArray(arr);
        var TMin = new Thread(() =>
        {
            var random = new Random();
            while (true)
            {
                Thread.Sleep(random.Next(2000));
                Console.WriteLine($"min={AdvArr.Min()}");
            }
        });
        var TAvg = new Thread(() =>
        {
            var random = new Random();
            while (true)
            {
                Thread.Sleep(random.Next(2000));
                Console.WriteLine($"avg={AdvArr.Avg()}");
            }
        });
        var TSort = new Thread(() =>
        {
            var random = new Random();
            while (true)
            {
                Thread.Sleep(random.Next(2000));
                AdvArr.Sort();
                Console.WriteLine("Sort");
            }
        });
        var TSwap = new Thread(() =>
        {
            var random = new Random();
            while (true)
            {
                Thread.Sleep(random.Next(2000));
                AdvArr.Swap();
                Console.WriteLine("Swap");
            }
        });
        TMin.Start();
        TAvg.Start();
        TSort.Start();
        TSwap.Start();
    }
}