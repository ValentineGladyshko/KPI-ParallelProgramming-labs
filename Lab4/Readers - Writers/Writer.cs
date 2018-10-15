using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Writer
    {
        public Thread thread;        
        private Storage storage;
        public string name;
        private Random rand;

        public Writer(Storage storage, string name, Random rand)
        {
            this.storage = storage;
            this.name = name;
            this.rand = rand;
            thread = new Thread(Write);
        }

        public void Write()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (storage)
                {
                    if (!storage.access)
                    {
                        Console.WriteLine(name + " is waiting");
                        Monitor.Wait(storage);
                        Console.WriteLine(name + " is running");
                    }

                    int data = rand.Next(10000, 100000);
                    Thread.Sleep(10 * rand.Next(5, 11));
                    Console.WriteLine(name + " writed data: " + data);
                    storage.data = data;
                }

                Thread.Sleep(50 * rand.Next(5, 11));
            }
        }
    }
}
