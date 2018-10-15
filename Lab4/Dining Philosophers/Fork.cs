﻿using System;
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
        private bool free;

        public Fork(string name)
        {
            this.name = name;
            free = true;
        }

        public bool Take()
        {
            bool status = false;
            if (free)
            {
                locker.EnterWriteLock();
                free = false;
                status = true;
            }

            return status;
        }

        public void Put()
        {
            locker.ExitWriteLock();
            free = true;
        }

        public override string ToString()
        {
            return name;
        }
    }
}