using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystemClassLibrary.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OIB { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }

}
