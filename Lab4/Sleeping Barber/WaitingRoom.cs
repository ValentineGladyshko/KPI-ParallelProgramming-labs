using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class WaitingRoom
    {
        private Queue<Client> clients;
        private int maxCount;
        private object sleep;

        public WaitingRoom(int maxCount, object sleep)
        {
            clients = new Queue<Client>();
            this.maxCount = maxCount;
            this.sleep = sleep;
        }

        public bool Add(Client client)
        {
            lock (clients)
            {
                if (clients.Count >= maxCount)
                    return false;
                clients.Enqueue(client);
                lock (sleep)
                {
                    Monitor.Pulse(sleep);
                }
                return true;
            }
        }

        public int Count()
        {
            lock (clients)
            {
                return clients.Count;
            }
        }

        public Client Get()
        {
            lock (clients)
            {
                return clients.Dequeue();
            }
        }
    }
}
