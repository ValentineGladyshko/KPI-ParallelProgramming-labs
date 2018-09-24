using System;
using System.Threading;

namespace Lab2
{
    internal sealed class Consumer
    {

        private readonly Thread thread;
        private Task task;
        private bool free;
        private readonly string name;
        public int count;
        
        public bool Free { get => free; }

        public Consumer(Task task, string name)
        {
            this.name = name;
            free = true;
            thread = new Thread(Func);
            this.task = task;
            count = 0;
        }

        private void Func()
        {
            Random random = new Random();

            while (true)
            {
                try
                {
                    lock (this)
                    {
                        free = false;
                        Console.WriteLine("executed {0}, consumer {1}", task.GetValue(), name);
                        count++;
                        Monitor.Pulse(this);
                    }

                    Thread.Sleep(20 * random.Next(1,50));
                    
                    lock (this)
                    {
                        free = true;
                        Monitor.Wait(this);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    return;
                }

            }
        }

        public string GetName()
        {
            return name;
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
