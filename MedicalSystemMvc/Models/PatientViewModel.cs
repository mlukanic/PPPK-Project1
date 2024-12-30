﻿using System.ComponentModel.DataAnnotations;

namespace MedicalSystemMvc.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "OIB is required")]
        public string OIB { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
    }
}
