using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Consumer
    {
        public Thread thread;
        private Queue<int> tasks;
        private Semaphore read;
        private Semaphore write;
        private Semaphore access;
        private Random rand = new Random();

        public Consumer(Queue<int> tasks, Semaphore read, Semaphore write, Semaphore access)
        {
            this.tasks = tasks;
            thread = new Thread(Consume);
            this.read = read;
            this.write = write;
            this.access = access;
        }

        public void Consume()
        {
            while (true)
            {
                read.WaitOne();
                access.WaitOne();

                Console.WriteLine("Consume task: " + tasks.Dequeue() + " queue count: " + tasks.Count);

                access.Release();
                write.Release();

                Thread.Sleep(30 * rand.Next(2, 21));
            }
        }
    }
}
