using System;
using System.Threading;

namespace Lab2
{
    internal sealed class Producer
    {
        private readonly Thread thread;
        private Task task;

        public Producer(Task task)
        {
            thread = new Thread(Func);
            this.task = task;
        }

        private void Func()
        {
            lock (task)
            {
                Random random = new Random();
                while (true)
                {
                    try
                    {

                        task.RandomValue();
                        Console.WriteLine("generated Value {0}", task.GetValue());

                        Monitor.Pulse(task);
                        Monitor.Wait(task);

                        Thread.Sleep(30 * random.Next(1, 10));

                    }
                    catch (ThreadInterruptedException)
                    {
                        return;
                    }
                }           
            }
        }

        public void Start()
        {
            thread.Start();
        }

        public void Interrupt()
        {
            thread.Interrupt();
        }

        public void Join()
        {
            thread.Join();
        }
    }
}
