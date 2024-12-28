namespace MedicalSystemClassLibrary.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string DiseaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}