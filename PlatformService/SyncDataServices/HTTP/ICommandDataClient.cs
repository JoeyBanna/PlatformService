using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.HTTP
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(Models.DTOs.PlatformDTO platform); 
    }
}
