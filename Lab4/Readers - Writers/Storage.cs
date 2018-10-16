using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal sealed class Storage
    {
        public int data;
        public int readersCount;

        public Storage()
        {
            data = 0;
            readersCount = 0;
        }
    }
}
