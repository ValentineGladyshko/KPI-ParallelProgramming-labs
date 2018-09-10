using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lab1
{
    class Program
    {
        private static readonly string path = "result.txt";

        // class for parallel execution

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
            int[] threads = { 2, 3, 4, 8, 16 }; // count of threads

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(string.Empty);
            }
            
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
            // Initialazing vector

            double[] vector = new double[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                vector[i] = random.NextDouble() * 1000000;
            }

            string output = new StringBuilder()
                .AppendFormat("Execute on vector size {0}:\n", size).ToString();
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(output);
                Console.WriteLine(output);
            }

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

            //output = new StringBuilder()
            //    .AppendFormat("\tSerial execution:\n\t\ttime: {0} ms;\n\t\tmin value: {1};" +
            //    " max value: {2};", serialStopwatch.Elapsed.Ticks / 10000.0,
            //    serialMinResult.ToString("F5"), serialMaxResult.ToString("F5")).ToString();

            output = new StringBuilder()
                .AppendFormat("\tSerial execution:\n\t\ttime: {0} ms;",
                serialStopwatch.Elapsed.Ticks / 10000.0).ToString();

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(output);
                Console.WriteLine(output);
            }

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
                //    Console.WriteLine("Thread #{0}:\nmin value: {1}; max value:{2}", i, threadArray[i].MinResult, threadArray[i].MaxResult);
                //}

                double acceleration = (double) serialStopwatch.Elapsed.Ticks / parallelStopwatch.Elapsed.Ticks;
                double efficiency = acceleration / threadsCount;

                //output = new StringBuilder()
                //.AppendFormat("\tParallel execution on {0} threads:\n\t\ttime: {1} ms;" +
                //" acceleration: {2}; efficiency: {3};\n\t\tmin value: {4}; max value: {5};",
                //    threadsCount, parallelStopwatch.Elapsed.Ticks / 10000.0, acceleration.ToString("F2"),
                //    efficiency.ToString("F2"), parallelMinResult.ToString("F5"),
                //    parallelMaxResult.ToString("F5")).ToString();

                output = new StringBuilder()
                .AppendFormat("\tParallel execution on {0} threads:\n\t\ttime: {1} ms;" +
                " acceleration: {2}; efficiency: {3};",
                    threadsCount, parallelStopwatch.Elapsed.Ticks / 10000.0,
                    acceleration.ToString("F2"), efficiency.ToString("F2")).ToString();

                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(output);
                    Console.WriteLine(output);
                }                
            }

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
