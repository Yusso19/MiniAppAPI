using AutoMapper;
using MiniApp.DTOs;
using MiniApp.Models;

namespace MiniApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Organizer, OrganizerDto>().ReverseMap();
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Title))
                .ReverseMap()
                .ForMember(dest => dest.Event, opt => opt.Ignore());
        }
    }
}
