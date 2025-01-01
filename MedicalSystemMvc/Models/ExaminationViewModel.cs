using MedicalSystemClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemMvc.Models
{
    public class ExaminationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public ExaminationType Type { get; set; }
        public int PatientId { get; set; }
    }
}
