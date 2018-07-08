using AutoMapper;
using HTEC.ChampionsLeague.Dto;
using System;

namespace HTEC.ChampionsLeague
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<MatchDto, Match>()
                .ForMember(u => u.KickoffAt, opts => opts.MapFrom(ext => Convert.ToDateTime(ext.KickoffAt)))
                .ForMember(u => u.Id, opts => opts.Ignore());


            CreateMap<Table, TableDto>();

            CreateMap<Standing, StandingDto>()
                .ForMember(u => u.Rank, opts => opts.Ignore());
        }
    }
}
