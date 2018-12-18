using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IService
    {
        [OperationContract]
        void Connect(string name);

        [OperationContract]
        void Disconnect(string name);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg);
    }

    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}
