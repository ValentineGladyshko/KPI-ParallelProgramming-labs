using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Barber
    {
        public Thread thread;
        private WaitingRoom waitingRoom;
        private object sleep;
        private Random rand = new Random();

        public Barber(WaitingRoom waitingRoom, object sleep)
        {
            this.waitingRoom = waitingRoom;
            thread = new Thread(Work);
            this.sleep = sleep;
        }
        public void Work()
        {
            while (true)
            {
                lock (sleep)
                {
                    if (waitingRoom.Count() == 0)
                    {
                        Console.WriteLine("Barber started sleeping");
                        Monitor.Wait(sleep);
                        Console.WriteLine("Barber awaked");
                    }
                }

                Client client = waitingRoom.Get();
                lock (client)
                {
                    Console.WriteLine("Started cutting hair: " + client.name);
                    Thread.Sleep(30 * rand.Next(2, 21));
                    Console.WriteLine("Ended cutting hair");
                    Monitor.Pulse(client);
                }
            }
        }
    }
}
