namespace MedicalSystemClassLibrary.Models
{
    public class Examination
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<MedicalFile> MedicalFiles { get; set; }
    }
}