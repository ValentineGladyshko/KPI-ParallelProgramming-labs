using System;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Lab1
{
    class Program
    {
        private static readonly string path = "result.txt";
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
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write("");
            }
            int[] threads = { 2, 3, 4, 8, 16 };
            ExecuteTask(10000, threads);
            ExecuteTask(100000, threads);
            ExecuteTask(1000000, threads);
            ExecuteTask(10000000, threads);
            ExecuteTask(100000000, threads);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ExecuteTask(int size, int[] threads)
        {
            double[] vector = new double[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                vector[i] = random.NextDouble() * 1000000;
            }

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Execute on vector size {0}:\n", size);
            }

            Console.WriteLine("Execute on vector size {0}:\n", size);
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
            //Console.WriteLine("\tSerial execution:\n\t\tTime: {0}ms; Min Value: {1}; Max Value: {2};",
            //    serialStopwatch.ElapsedMilliseconds, serialMinResult, serialMaxResult);

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("\tSerial execution:\n\t\ttime: {0}ms;",
                    serialStopwatch.ElapsedMilliseconds);
            }

            Console.WriteLine("\tSerial execution:\n\t\ttime: {0}ms;",
                serialStopwatch.ElapsedMilliseconds);

            // Parallel execution of Task

            for (int j = 0; j < threads.Length; j++)
            {
                int threadsCount = threads[j];

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

                //Console.WriteLine("\tParallel execution on {0} threads:\n\t\tTime: {1}ms; Min Value: {2}; Max Value: {3};",
                //threadsCount, parallelStopwatch.ElapsedMilliseconds, parallelMinResult, parallelMaxResult);

                double acceleration = (double)serialStopwatch.ElapsedMilliseconds / parallelStopwatch.ElapsedMilliseconds;
                double efficiency = acceleration / threadsCount;

                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("\tParallel execution on {0} threads:\n\t\ttime: {1}ms; acceleration: {2}; efficiency: {3};",
                    threadsCount, parallelStopwatch.ElapsedMilliseconds, acceleration.ToString("F2"), efficiency.ToString("F2"));
                }

                Console.WriteLine("\tParallel execution on {0} threads:\n\t\ttime: {1}ms; acceleration: {2}; efficiency: {3};",
                    threadsCount, parallelStopwatch.ElapsedMilliseconds, acceleration.ToString("F2"), efficiency.ToString("F2"));
            }

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
