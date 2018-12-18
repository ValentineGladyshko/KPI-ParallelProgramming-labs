using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleServer.Service;
using Service;

namespace ConsoleServer
{
    class ServerInstance : ConsoleServer.Service.IServiceCallback
    {
        public void MsgCallback(string msg)
        {
            //Console.WriteLine(msg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var server = new ServerInstance();
            var client = new Service.ServiceClient(new System.ServiceModel.InstanceContext(server));
            while (true)
            {
                Console.WriteLine("Enter yes to continue or something another to exit");
                var key = Console.ReadLine();
                if (key == "yes")
                {
                    Console.Write("Enter message to send: ");
                    client.SendMsg(Console.ReadLine());
                }
                else
                {
                    break;
                }
            }
        }
    }
}
