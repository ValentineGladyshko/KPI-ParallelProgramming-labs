using System;
using System.Threading;

namespace Lab2
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task();
            Producer producer = new Producer(task);
            ConsumerController consumerController = new ConsumerController(task);

            producer.Start();
            consumerController.Start();
            
            Thread.Sleep(10000);

            producer.Interrupt();
            consumerController.Interrupt();

            consumerController.Show();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
