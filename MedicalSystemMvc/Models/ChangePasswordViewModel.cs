﻿namespace MedicalSystemMvc.Models
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
