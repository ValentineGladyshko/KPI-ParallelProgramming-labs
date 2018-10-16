using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab4
{
    internal sealed class Fork
    {
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private string name;

        public Fork(string name)
        {
            this.name = name;
        }

        public void Take()
        {
            locker.EnterWriteLock();
        }

        public void Put()
        {          
            locker.ExitWriteLock();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
