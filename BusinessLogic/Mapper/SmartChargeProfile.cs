using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class SmartChargeProfile : Profile
    {
        public SmartChargeProfile()
        {
            CreateMap<GroupDTO, Group>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name}")
                )
                .ForMember(
                    dest => dest.Capacity,
                    opt => opt.MapFrom(src => $"{src.Capacity}")
                )
                .ForMember(
                    dest => dest.ChargeStations,
                    opt => opt.MapFrom(src => $"{src.ChargeStations}")
                ).ReverseMap();

            CreateMap<ConnectorDTO, Connector>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ForMember(
                    dest => dest.MaxCurrent,
                    opt => opt.MapFrom(src => $"{src.MaxCurrent}")
                )
                .ForMember(
                    dest => dest.ChargeStation.Id,
                    opt => opt.MapFrom(src => $"{src.ChargeStationId}")
                ).ReverseMap();

            CreateMap<ChargeStationDTO, ChargeStation>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => $"{src.Id}")
               )
               .ForMember(
                   dest => dest.Name,
                   opt => opt.MapFrom(src => $"{src.Name}")
               )
               .ForMember(
                   dest => dest.Connectors,
                   opt => opt.MapFrom(src => $"{src.Connectors}")
               ).ReverseMap();
        }
    }
}
