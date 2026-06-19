using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserEntity, UserDto>();
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UpdateUserDto, UserEntity>()
            .ForAllMembers(opt => opt.Condition((_, _, src) => src != null));

        // Skill
        CreateMap<SkillEntity, SkillDto>()
            .ForMember(d => d.LearnerCount, opt => opt.MapFrom(s => s.UserSkills.Count));
        CreateMap<CreateSkillDto, SkillEntity>();

        // UserSkill
        CreateMap<UserSkillEntity, UserSkillDto>()
            .ForMember(d => d.UserFullName, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.SkillName, opt => opt.MapFrom(s => s.Skill.Name));
        CreateMap<CreateUserSkillDto, UserSkillEntity>();

        // Booking
        CreateMap<BookingEntity, BookingDto>()
            .ForMember(d => d.RequesterName, opt => opt.MapFrom(s => s.Requester.FullName))
            .ForMember(d => d.ProviderName, opt => opt.MapFrom(s => s.Provider.FullName))
            .ForMember(d => d.SkillName, opt => opt.MapFrom(s => s.Skill.Name));
        CreateMap<CreateBookingDto, BookingEntity>();
    }
}