using System.ComponentModel.DataAnnotations;

namespace MedicalSystemMvc.Models
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medication is required")]
        public string Medication { get; set; }

        [Required(ErrorMessage = "Dosage is required")]
        public string Dosage { get; set; }
        public int PatientId { get; set; }
    }
}
