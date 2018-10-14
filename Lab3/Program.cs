using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab3
{
    class Program
    {
        public class Pair
        {
            public int index;
            public int value;

            public Pair()
            {
                index = 0;
                value = 0;
            }

            public Pair(int _value)
            {
                index = 0;
                value = _value;
            }

            public Pair(int _value, int _index)
            {
                index = _index;
                value = _value;
            }
        }

        private static int countOf;
        private static Pair minIndex;
        private static Pair maxIndex;
        private static int hash;

        internal sealed class ThreadTask
        {            
            private readonly Thread thread;
            private readonly int[] vector;
            private readonly int startIndex;
            private readonly int endIndex;
            private readonly int toFound;

            public ThreadTask(int[] _vector, int _startIndex, int _endIndex, int _toFound)
            {
                thread = new Thread(Func);
                vector = _vector;
                startIndex = _startIndex;
                endIndex = _endIndex;
                toFound = _toFound;
                thread.Start();
            }

            private void Func()
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    int oldValue, newValue;
                    Pair newPair, oldPair;
                    //Counter
                    if (vector[i] == toFound)
                    {
                        do
                        {
                            oldValue = countOf;
                            newValue = oldValue + 1;
                        }
                        while (oldValue != Interlocked.CompareExchange(
                            ref countOf, newValue, oldValue));
                    }

                    //Min
                    do
                    {
                        oldPair = minIndex;
                        if (vector[i] >= oldPair.value)
                        {
                            break;
                        }
                        newPair = new Pair(vector[i], i);
                    }
                    while (oldPair != Interlocked.CompareExchange(
                            ref minIndex, newPair, oldPair));

                    //Max
                    do
                    {
                        oldPair = maxIndex;
                        if (vector[i] <= oldPair.value)
                        {
                            break;
                        }
                        newPair = new Pair(vector[i], i);
                    }
                    while (oldPair != Interlocked.CompareExchange(
                            ref maxIndex, newPair, oldPair));

                    ////Hash
                    do
                    {
                        oldValue = hash;
                        newValue = oldValue ^ vector[i];
                        //Console.WriteLine(newValue);
                    }
                    while (oldValue != Interlocked.CompareExchange(
                            ref hash, newValue, oldValue));
                }
            }

            public void Join() { thread.Join(); }
        }

        static void Main(string[] args)
        {
            countOf = 0;
            
            hash = 0;

            int size = 20000000;
            int threads = 4;
            int toFound = 50;
            int[] vector = new int[size];
            Random random = new Random();
            ThreadTask[] threadArray = new ThreadTask[threads];

            for (int i = 0; i < size; i++)
            {
                vector[i] = random.Next(0,100001);
            }

            minIndex = new Pair(vector[0]);
            maxIndex = new Pair(vector[0]);

            for (int i = 0; i < threads; i++)
            {
                threadArray[i] = new ThreadTask(vector, size / threads * i,
                    i == threads - 1 ? size : size / threads * (i + 1), toFound);
            }

            for (int i = 0; i < threads; i++)
            {
                threadArray[i].Join();
            }

            Console.WriteLine("count of " + toFound + " : " + countOf);
            Console.WriteLine("min value " + minIndex.value + " at index " + minIndex.index);
            Console.WriteLine("max value " + maxIndex.value + " at index " + maxIndex.index);
            Console.WriteLine("hash " + hash);
            Console.ReadKey();
        }
    }
}
