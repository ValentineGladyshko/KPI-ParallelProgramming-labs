using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;
using Service;

namespace Server
{
    
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Service.Service)))
            {
                host.Open();
                Console.WriteLine("Host started\nPress any key to close host");
               
                Console.ReadKey();
                host.Close();
            }
        }                
    }
}
