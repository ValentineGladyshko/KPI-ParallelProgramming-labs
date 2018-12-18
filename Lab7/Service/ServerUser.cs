using System.ServiceModel;
namespace Service
{
    public class ServerUser
    {
        public string Name { get; set; }

        public OperationContext operationContext { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
