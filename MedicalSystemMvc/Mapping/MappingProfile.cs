using AutoMapper;
using MedicalSystemClassLibrary.Models;
using MedicalSystemMvc.Models;

namespace MedicalSystemMvc.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginViewModel>().ReverseMap();
            CreateMap<Examination, ExaminationViewModel>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();
            CreateMap<MedicalFile, MedicalFileViewModel>().ReverseMap();
            CreateMap<Prescription, PrescriptionViewModel>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordViewModel>().ReverseMap();
            CreateMap<MedicalFile, MedicalFileViewModel>().ReverseMap();
        }
    }
}
