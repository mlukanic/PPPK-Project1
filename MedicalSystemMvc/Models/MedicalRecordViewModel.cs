using System.ComponentModel.DataAnnotations;

namespace MedicalSystemMvc.Models
{
    public class MedicalRecordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Disease name is required")]
        public string DiseaseName { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
    }
}
