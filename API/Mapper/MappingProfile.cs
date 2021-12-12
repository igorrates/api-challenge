using AutoMapper;
using Entities.DTO;
using Entities.Models;

namespace API.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Application, ApplicationDTO>();
            CreateMap<ApplicationDTO, Application>()
                .ForMember(dest => dest.PathLocal, opts => opts.Condition(src => src.PathLocal != null))
                .ForMember(dest => dest.Url, opts => opts.Condition(src => src.Url != null))
                .ForMember(dest => dest.Id, opts => opts.Condition(src => src.Id > 0))
                .ForMember(dest => dest.DebuggingMode, opts => opts.Condition(src => src.DebuggingMode != null))
                .ForMember(dest => dest.DebuggingMode, opts => opts.MapFrom(src => src.DebuggingMode.HasValue ? src.DebuggingMode.Value : false));        
        }
    }
}
