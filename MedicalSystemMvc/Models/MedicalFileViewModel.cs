using System.ComponentModel.DataAnnotations;

public class MedicalFileViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "File path is required")]
    public string FilePath { get; set; }

    public int ExaminationId { get; set; }

    public string RelativeFilePath =>
        !string.IsNullOrEmpty(FilePath) ? Path.GetRelativePath(Directory.GetCurrentDirectory(), FilePath) : string.Empty;
}
