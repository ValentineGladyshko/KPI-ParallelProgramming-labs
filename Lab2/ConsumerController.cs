using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab2
{
    internal sealed class ConsumerController
    {
        private readonly Thread thread;
        private List<Consumer> consumers;
        private Task task;
        public int count;

        public ConsumerController(Task task)
        {
            consumers = new List<Consumer>();
            thread = new Thread(Func);
            this.task = task;
            count = 0;
        }

        private void Func()
        {
            lock (task)
            {
                while (true)
                {
                    try
                    {
                        count++;
                        Consumer consumer = GetFreeConsumer();

                        lock (consumer)
                        {
                            Monitor.Pulse(consumer);
                            Monitor.Wait(consumer);
                            Monitor.Pulse(task);
                            Monitor.Wait(task);
                        }

                    }
                    catch (ThreadInterruptedException)
                    {
                        return;
                    }
                }
            }
        }

        private Consumer GetFreeConsumer()
        {
            foreach(Consumer consumer in consumers)
            {
                if (consumer.Free)
                {
                    //Console.WriteLine("Not new {0}", consumer.GetName());
                    return consumer;                   
                }
            }           
            Consumer result = new Consumer(task, consumers.Count.ToString());

            Console.WriteLine("\nnew consumer {0}\n", result.GetName());

            consumers.Add(result);
            result.Start();
            return result;
        }

        public void Start()
        {
            thread.Start();
        }

        public void Show()
        {
            Console.WriteLine("\nconsumers count {0}\n", consumers.Count);

            foreach (Consumer consumer in consumers)
            {
                double result = (double)consumer.count / count * 100.0;
                Console.WriteLine("consumer {0}: {1}%", consumer.GetName(), result.ToString("F2"));
            }
            Console.WriteLine();
        }

        public void Interrupt()
        {
            foreach (Consumer consumer in consumers)
            {
                consumer.Interrupt();
            }
            thread.Interrupt();
        }

        public void Join()
        {
            thread.Join();
        }
    }
}
