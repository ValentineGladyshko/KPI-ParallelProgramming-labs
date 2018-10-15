using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Producer
    {
        public Thread thread;
        private Queue<int> tasks;
        private Semaphore read;
        private Semaphore write;
        private Semaphore access;
        private Random rand = new Random();

        public Producer(Queue<int> tasks, Semaphore read, Semaphore write, Semaphore access)
        {
            this.tasks = tasks;
            thread = new Thread(Produce);
            this.read = read;
            this.write = write;
            this.access = access;
        }

        public void Produce()
        {
            while (true)
            {
                write.WaitOne();
                access.WaitOne();

                int a = rand.Next(10000, 100000);
                tasks.Enqueue(a);
                Console.WriteLine("New task: " + a + " queue count: " + tasks.Count);

                access.Release();
                read.Release();

                Thread.Sleep(30 * rand.Next(6, 12));
            }
        }
    }
}
