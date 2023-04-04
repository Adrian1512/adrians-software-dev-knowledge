using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private IServiceScopeFactory _scopeFactory;
        private IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determined the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exist");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Could not add platform to the db: {ex.Message}");
                }
            }
        }

    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}