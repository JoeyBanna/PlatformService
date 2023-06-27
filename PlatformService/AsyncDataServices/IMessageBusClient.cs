namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(Models.DTOs.PlatformPublishedDto platformPublishedDto);
    }
}
