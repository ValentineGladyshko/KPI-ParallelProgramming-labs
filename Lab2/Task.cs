using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab2
{
    internal sealed class Task
    {
        private int value;
        public Task() { }

        public void RandomValue()
        {
            Random random = new Random();
            value = random.Next();
        }

        public int GetValue()
        {
            return value;
        }
    }
}
