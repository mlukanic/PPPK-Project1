using AutoMapper;
using MedicalSystemClassLibrary.Models;
using MedicalSystemWebAPI.Dtos;

namespace MedicalSystemWebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        protected MappingProfile() 
        {
            CreateMap<User, UserDto>();
            CreateMap<Patient, PatientDto>();
        }
    }
}
