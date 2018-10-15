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
        public bool access;

        public Storage()
        {
            data = 0;
            access = true;
        }
    }
}
