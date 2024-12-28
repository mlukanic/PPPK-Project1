namespace MedicalSystemClassLibrary.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}