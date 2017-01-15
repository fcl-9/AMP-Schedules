using System.Threading.Tasks;

namespace AMPSchedules.Services
{
    public interface IGraphService {
        Task<string> GetMyEmailAddress();

        Task<byte[]> GetPhoto();

        Task<byte[]> GetUserPhoto( string aUser );

        Task<string> SendEmail( IMessageRequest aMessage );
    }
}