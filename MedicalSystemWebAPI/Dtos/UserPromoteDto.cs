using System.ComponentModel.DataAnnotations;

namespace MedicalSystemWebAPI.Dtos
{
    public class UserPromoteDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username
        {
            get; set;
        }
    }
}
