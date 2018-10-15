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
        private MyInt readersCount;

        public Reader(Storage storage, string name, MyInt readersCount)
        {
            this.storage = storage;
            this.name = name;
            this.readersCount = readersCount;
            thread = new Thread(Read);
        }

        public void Read()
        {
            lock (readersCount)
            {
                readersCount.readersCount++;
                if (readersCount.readersCount == 1)
                {
                    lock (storage)
                    {
                        storage.access = false;
                    }
                }
            }

            Console.WriteLine(name + " started reading");
            Thread.Sleep(50);
            lock (storage)
            {
                Console.WriteLine(name + " readed data: " + storage.data);
            }

            lock (readersCount)
            {
                readersCount.readersCount--;
                if (readersCount.readersCount == 0)
                {
                    lock (storage)
                    {
                        storage.access = true;
                        Monitor.Pulse(storage);
                    }
                }
            }
        }
    }
}
