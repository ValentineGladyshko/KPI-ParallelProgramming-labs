using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;


namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        HashSet<ServerUser> users = new HashSet<ServerUser>();

        public void Connect(string name)
        {

            ServerUser user = new ServerUser()
            {
                Name = name,
                operationContext = OperationContext.Current
            };

            users.Add(user);
        }

        public void Disconnect(string name)
        {
            var user = users.FirstOrDefault(item => item.Name == name);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public void SendMsg(string msg)
        {
            var names = File.ReadAllLines(@"C:\Users\Kappi\Source\Repos\KPI-ParallelProgramming-labs\Lab7\clients.txt");
            foreach (var name in names)
            {
                var usersList = users.Where(i => i.Name == name);
                if (usersList != null)
                {
                    string answer = DateTime.Now.ToShortTimeString() +": "+ msg;
                    foreach (var user in usersList)
                        user.operationContext.GetCallbackChannel<IServiceCallback>().MsgCallback(answer);
                }
            }
        }
    }
}
