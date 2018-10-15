using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Client
    {
        public Thread thread;
        public string name;
        private WaitingRoom waitingRoom;

        public Client(string name, WaitingRoom waitingRoom)
        {
            this.name = name;
            this.waitingRoom = waitingRoom;
            thread = new Thread(GettingService);
        }

        public void GettingService()
        {
            lock (this)
            {
                Console.WriteLine(name + " go to barber shop");
                if (!waitingRoom.Add(this))
                {
                    Console.WriteLine(name + " go without haircut");
                    return;
                }

                Monitor.Wait(this);
                Console.WriteLine(name + " got a haircut");
            }
        }
    }
}
