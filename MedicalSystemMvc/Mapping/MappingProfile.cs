using AutoMapper;
using MedicalSystemClassLibrary.Models;
using MedicalSystemMvc.Models;

namespace MedicalSystemMvc.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginViewModel>();
            //CreateMap<Examination, ExaminationViewModel>();
            //CreateMap<Patient, PatientViewModel>();
            //CreateMap<MedicalFile, MedicalFileViewModel>();
        }
    }
}
