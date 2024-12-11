using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace TodoAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<User, UserDto>()
        //     .ForCtorParam("FullName", opt => opt.MapFrom(u => $"{u.Name}({u.Username})"));
        
        CreateMap<User, UserDto>();
        CreateMap<Todo, TodoDto>();
        CreateMap<UserForCreationDto, User>();
        CreateMap<TodoForCreationDto, Todo>();
        CreateMap<TodoForUpdateDto, Todo>();
    }
}