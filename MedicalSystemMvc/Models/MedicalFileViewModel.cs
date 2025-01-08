using System.ComponentModel.DataAnnotations;

public class MedicalFileViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "File path is required")]
    public string ObjectId { get; set; }

    public int ExaminationId { get; set; }
}
