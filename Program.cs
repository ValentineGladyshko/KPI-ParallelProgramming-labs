using System;
using System.Threading;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        private static int size = 50000000;
        private static int threadsCount = 4;

        internal sealed class ThreadMinMax
        {
            private readonly Thread thread;
            private readonly double[] vector;
            private readonly int startIndex;
            private readonly int endIndex;
            private double minResult;
            private double maxResult;

            public double MaxResult { get => maxResult; }
            public double MinResult { get => minResult; }


            public ThreadMinMax(double[] _vector, int _startIndex, int _endIndex)
            {
                thread = new Thread(Func);
                vector = _vector;
                startIndex = _startIndex;
                endIndex = _endIndex;
                minResult = vector[startIndex];
                maxResult = vector[startIndex];
            }

            private void Func()
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (vector[i] < minResult)
                        minResult = vector[i];
                    if (vector[i] > maxResult)
                        maxResult = vector[i];
                }
            }

            public void Start()
            {
                thread.Start();
            }

            public void Join()
            {
                thread.Join();
            }
        }

        static void Main(string[] args)
        {
            double[] vector = new double[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                vector[i] = random.NextDouble() * 1000000;
            }
            Console.WriteLine("Executing...\n");
            // Serial execution of Task

            Stopwatch serialStopwatch = new Stopwatch();
            serialStopwatch.Start();

            double serialMinResult = vector[0];
            double serialMaxResult = vector[0];

            for (int i = 0; i < size; i++)
            {
                if (vector[i] < serialMinResult)
                    serialMinResult = vector[i];
                if (vector[i] > serialMaxResult)
                    serialMaxResult = vector[i];
            }
            serialStopwatch.Stop();
            Console.WriteLine("Serial execution:\nTime: {0}ms; Min Value: {1}; Max Value: {2}\n",
                serialStopwatch.ElapsedMilliseconds, serialMinResult, serialMaxResult);

            // Parallel execution of Task

            Stopwatch parallelStopwatch = new Stopwatch();
            parallelStopwatch.Start();

            double parallelMinResult = vector[0];
            double parallelMaxResult = vector[0];
            ThreadMinMax[] threadArray = new ThreadMinMax[threadsCount];
            
            for (int i = 0; i < threadsCount; i++)
            {
                threadArray[i] = new ThreadMinMax(vector, size / threadsCount * i, 
                    i == threadsCount - 1 ? size : size / threadsCount * (i + 1));
                threadArray[i].Start();
            }

            for (int i = 0; i < threadsCount; i++)
            {
                threadArray[i].Join();
            }
            
            for (int i = 0; i < threadsCount; i++)
            {
                if (threadArray[i].MinResult < parallelMinResult)
                    parallelMinResult = threadArray[i].MinResult;
                if (threadArray[i].MaxResult > parallelMaxResult)
                    parallelMaxResult = threadArray[i].MaxResult;
            }

            parallelStopwatch.Stop();

            //Showing results for each step
            
            //for (int i = 0; i < threadsCount; i++)
            //{
            //    Console.WriteLine("Thread #{0}:\nMin Value: {1}; Max Value:{2}", i, threadArray[i].MinResult, threadArray[i].MaxResult);
            //}

            Console.WriteLine("Parallel execution:\nTime: {0}ms; Min Value: {1}; Max Value: {2}\n",
            parallelStopwatch.ElapsedMilliseconds, parallelMinResult, parallelMaxResult);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
