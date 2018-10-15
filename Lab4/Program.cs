using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {

            //ProduserConsumer();
            //ReadersWriters();
            DiningPhilosophers();
            //SleepingBarber();


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ProduserConsumer()
        {
            int maxCount = 10;
            Queue<int> tasks = new Queue<int>();
            Semaphore read = new Semaphore(0, maxCount);
            Semaphore write = new Semaphore(maxCount, maxCount);
            Semaphore access = new Semaphore(1, 1);

            Producer producer = new Producer(tasks, read, write, access);
            Consumer consumer = new Consumer(tasks, read, write, access);

            producer.thread.Start();
            consumer.thread.Start();

            Thread.Sleep(10000);

            producer.thread.Abort();
            consumer.thread.Abort();
        }

        static void ReadersWriters()
        {
            Random rand = new Random();

            Storage storage = new Storage();
            MyInt readersCount = new MyInt();

            List<Writer> writers = new List<Writer>();

            for (int i = 0; i < 5; i++)
            {
                writers.Add(new Writer(storage, "Writer" + (i + 1), rand));
            }

            foreach(var writer in writers)
            {
                writer.thread.Start();
            }

            for (int i = 0; i < 50; i++)
            {
                new Reader(storage, "Reader" + (i + 1), readersCount).thread.Start();

                Thread.Sleep(20);
            }

            foreach (var writer in writers)
            {
                writer.thread.Abort();
            }
        }

        static void DiningPhilosophers()
        {
            List<Fork> forks = new List<Fork>();
            for (int i = 0; i < 5; ++i)
            {
                forks.Add(new Fork("Fork" + (i + 1)));
            }

            List<Philosopher> philosophers = new List<Philosopher>();
            for (int i = 0; i < 4; ++i)
            {
                philosophers.Add(new Philosopher("Philosopher" + (i + 1), forks[i], forks[i + 1]));
            }

            philosophers.Add(new Philosopher("Philosopher5", forks[0], forks[4]));

            foreach (Philosopher philosopher in philosophers)
            {
                philosopher.thread.Start();
            }

            foreach (Philosopher philosopher in philosophers)
            {
                philosopher.thread.Join();
            }
        }

        static void SleepingBarber()
        {
            Random rand = new Random();

            int maxCount = 10;
            object sleep = new object();

            WaitingRoom waitingRoom = new WaitingRoom(maxCount, sleep);
            Barber barber = new Barber(waitingRoom, sleep);

            barber.thread.Start();

            List<Client> clients = new List<Client>();
            for (int i = 0; i < 50; i++)
            {
                Client client = new Client("Client" + (i + 1), waitingRoom);
                clients.Add(client);
                client.thread.Start();
                Thread.Sleep(30 * rand.Next(6, 12));
            }

            foreach(var client in clients)
            {
                client.thread.Join();
            }

            barber.thread.Abort();
        }
    }
}
