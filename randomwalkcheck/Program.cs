using System;
using System.Threading.Tasks;

namespace randomwalkcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Rank:");
            var rank = int.Parse(Console.ReadLine());
            System.Numerics.BigInteger[] startpos = new System.Numerics.BigInteger[rank];
            System.Numerics.BigInteger[] nowpos = new System.Numerics.BigInteger[rank];
            Array.Copy(startpos, nowpos, startpos.Length);
            Console.WriteLine("Press ESC to stop");
            var j = 0;
            int findedcount = 0;
            foreach (var one in System.Linq.Enumerable.Range(0, 100))
            {
                Task.Factory.StartNew(() =>
                {
                    WorkMethod(startpos, rank);
                    System.Threading.Interlocked.Increment(ref findedcount);
                    Console.WriteLine($"Now {findedcount} threads come back");
                }, TaskCreationOptions.LongRunning);
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                System.Threading.Thread.Sleep(100);
            }
        }
        [ThreadStatic]
        static Random random;
        public static void WorkMethod(System.Numerics.BigInteger[] startpos, int rank)
        {
            if (random == null)
                random = new Random();
            try
            {
                long j = 0;
                System.Numerics.BigInteger[] nowpos = new System.Numerics.BigInteger[rank];
                while (true)
                {
                    for (var i = 0; i < nowpos.Length; i++)
                    {
                        if (GetRandomBool())
                        {
                            nowpos[i]++;
                        }
                        else
                        {
                            nowpos[i]--;
                        }
                        if (System.Linq.Enumerable.SequenceEqual(nowpos, startpos))
                        {
                            Console.WriteLine($"one time back,about {j} steps");
                            return;
                        }
                    }
                }
            }
            finally
            {
                random = null;
            }
        }
        public static long GetRandomPos()
        {
            return random.Next();
        }
        public static bool GetRandomBool()
        {
            return GetRandomPos() % 2 == 0;
        }
    }
}
