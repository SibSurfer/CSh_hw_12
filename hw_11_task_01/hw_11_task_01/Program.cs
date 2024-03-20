
public class ZeroEvenOdd
{
    private int n;
    private SemaphoreSlim zeroSemaphore = new SemaphoreSlim(1);
    private SemaphoreSlim evenSemaphore = new SemaphoreSlim(0);
    private SemaphoreSlim oddSemaphore = new SemaphoreSlim(0);

    public ZeroEvenOdd(int n)
    {
        this.n = n;
    }

    public void Zero(Action<int> printNumber)
    {
        for (int i = 1; i <= n; i++)
        {
            zeroSemaphore.Wait();
            printNumber(0);
            (i % 2 == 0 ? evenSemaphore : oddSemaphore).Release();
        }
    }

    public void Even(Action<int> printNumber)
    {
        for (int i = 2; i <= n; i += 2)
        {
            evenSemaphore.Wait();
            printNumber(i);
            zeroSemaphore.Release();
        }
    }

    public void Odd(Action<int> printNumber)
    {
        for (int i = 1; i <= n; i += 2)
        {
            oddSemaphore.Wait();
            printNumber(i);
            zeroSemaphore.Release();
        }
    }
}

internal class MainClass
{
    public static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine()); //enter n 
        Action<int> printNumber = Console.Write;
        ZeroEvenOdd zeroEvenOdd = new ZeroEvenOdd(n);

        Thread zeroThread = new Thread(() => zeroEvenOdd.Zero(printNumber));
        Thread evenThread = new Thread(() => zeroEvenOdd.Even(printNumber));
        Thread oddThread = new Thread(() => zeroEvenOdd.Odd(printNumber));

        zeroThread.Start();
        evenThread.Start();
        oddThread.Start();

        zeroThread.Join();
        evenThread.Join();
        oddThread.Join();
    }
}
