using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Philosopher
    {
        public Thread thread;
        private string name;
        private Fork firstFork;
        private Fork secondFork;

        public Philosopher(string name, Fork firstFork, Fork secondFork)
        {
            this.name = name;
            this.firstFork = firstFork;
            this.secondFork = secondFork;
            thread = new Thread(Run);
        }

        public void Run()
        {

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(name + " want to eat");

                firstFork.Take();
                Console.WriteLine(name + " took first fork " + firstFork);
                secondFork.Take();
                Console.WriteLine(name + " took second fork " + secondFork);

                Console.WriteLine(name + " is eating");
                Thread.Sleep(50);

                Console.WriteLine(name + " put second fork " + secondFork);
                secondFork.Put();
                Console.WriteLine(name + " put first fork " + firstFork);
                firstFork.Put();

                Thread.Sleep(100);
            }
        }
    }
}
