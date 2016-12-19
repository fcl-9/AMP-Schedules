using System.Security.Cryptography.X509Certificates;

namespace AMPSystem.Interfaces
{
    public interface IDecorator: ITimeTableItem
    {
        ITimeTableItem Item { get; set; }
    }
}