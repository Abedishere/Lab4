using AutoMapper;
using InmindLab3_4part2.DTOs;
using InmindLab3_4part2.Models;

namespace InmindLab3_4part2.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // DTO -> Entity
            CreateMap<CreateStudentDto, Student>();

            // Entity -> DTO
            CreateMap<Student, StudentViewDto>()
                .ForMember(dest => dest.FullName, 
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}