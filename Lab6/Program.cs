using System;
using System.Threading.Tasks;

namespace Lab6
{
    internal class Program
    {
        private static void Main()
        {
            const int size = 10000000;

            var vector = new double[size];
            var random = new Random();

            Parallel.For(0, size, index =>
            {
                vector[index] = random.NextDouble() * 1000000;
            });

            var min = vector[0];
            var max = vector[0];

            Parallel.For(0, vector.Length, index =>
            {
                if (vector[index] < min)
                    min = vector[index];
                if (vector[index] > max)
                    max = vector[index];
            });

            Console.WriteLine("min element: {0} max element: {1}", min, max);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
    

