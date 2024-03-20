public class H2O
{
    public H2O()
    {
        hydroAct = new Semaphore(2, 2);
        oxyAct = new AutoResetEvent(true);
        blocking = new object();
    }

    public void Hydrogen(Action releaseHydrogen)
    {
        hydroAct.WaitOne();
        releaseHydrogen();
        UpdateState();
    }

    public void Oxygen(Action releaseOxygen)
    {
        oxyAct.WaitOne();
        releaseOxygen();
        UpdateState();
    }

    private void UpdateState()
    {
        lock (blocking)
        {
            if (++state == 3)
            {
                hydroAct.Release(2);
                oxyAct.Set();
                state = 0;
            }
        }
    }
    private Semaphore hydroAct;
    private AutoResetEvent oxyAct;
    private object blocking;
    private int state = 0;
}
class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter your molecule: ");
        string moleculeStr = Console.ReadLine();
        H2O molecule = new H2O();

        Action releaseHydrogen = () => { Console.Write("H"); };
        Action releaseOxygen = () => { Console.Write("O"); };
        List<Thread> threads = new List<Thread>();
        foreach (char a in moleculeStr)
        {
            if (a == 'H')
            {
                threads.Add(new Thread(() => molecule.Hydrogen(releaseHydrogen)));
            }
            else if (a == 'O')
            {
                threads.Add(new Thread(() => molecule.Oxygen(releaseOxygen)));
            }
        }

        threads.ForEach(t => t.Start());
        threads.ForEach(t => t.Join());
    }
}
