﻿namespace MedicalSystemClassLibrary.Models
{
    public class MedicalFile
    {
        public int Id { get; set; }
        public string ObjectId { get; set; }
        public int ExaminationId { get; set; }
        public virtual Examination Examination { get; set; }
    }
}