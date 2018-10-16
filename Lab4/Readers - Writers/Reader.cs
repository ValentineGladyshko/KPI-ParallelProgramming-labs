using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Reader
    {
        public Thread thread;
        private Storage storage;
        public string name;

        public Reader(Storage storage, string name)
        {
            this.storage = storage;
            this.name = name;
            thread = new Thread(Read);
        }

        public void Read()
        {
            lock (storage)
            {
                storage.readersCount++;
            }

            Console.WriteLine(name + " started reading");
            Thread.Sleep(50);
            lock (storage)
            {
                Console.WriteLine(name + " readed data: " + storage.data);
            }

            lock (storage)
            {
                storage.readersCount--;
                if (storage.readersCount == 0)
                {
                    Monitor.Pulse(storage);
                }
            }
        }
    }
}
