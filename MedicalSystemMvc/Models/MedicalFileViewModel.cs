using System.ComponentModel.DataAnnotations;

namespace MedicalSystemMvc.Models
{
    public class MedicalFileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "File path is required")]
        public string FilePath { get; set; }
        public int ExaminationId { get; set; }
    }
}
