using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 10000000;

            Random rand = new Random();

            List<int> array1 = new List<int>();
            List<int> array2 = new List<int>();
            List<int> array3 = new List<int>();

            for (int i = 0; i < count; i++)
            {
                array1.Add(rand.Next(0,1000000));
                array2.Add(rand.Next(0,1000000));
                array3.Add(rand.Next(0,1000000));
            }

            List<Task> tasks = new List<Task>();

            Task t1 = Task.Run(() => { for (int i = 0; i < array1.Count; i++) array1[i] *= 3; });
            Task t2 = Task.Run(() => array2.RemoveAll(num => num % 2 == 1));
            Task<double> t3 = Task.Run(() => array3.Average());


            Task t4 = t3.ContinueWith((average) =>
            {
                double min = average.Result * 0.8;
                double max = average.Result * 1.2;
                array3.RemoveAll(num => (num < min) || (num > max));
            }
            );

            Task t5 = t1.ContinueWith((task) => array1.Sort());
            Task t6 = t2.ContinueWith((task) => array2.Sort());
            Task t7 = t4.ContinueWith((task) => array3.Sort());

            Task<List<int>> t8 = Task.Factory.ContinueWhenAll(new Task[] { t5, t6 }, (task) => new List<int>(array1.Concat(array2)));

            Task<HashSet<int>> t9 = t7.ContinueWith((task) => new HashSet<int>(array3));

            Task<List<int>> t10 = Task.Factory.ContinueWhenAll(new Task[] { t8, t9 }, (task) =>
            {
                var result = t8.Result;

                var dict = t9.Result;
                result.RemoveAll(num => dict.Contains(num));
                result.Sort();
                return result;
            });

            var d = t10.Result;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
