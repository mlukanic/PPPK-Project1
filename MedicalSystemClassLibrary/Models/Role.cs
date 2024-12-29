namespace MedicalSystemClassLibrary.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public virtual ICollection<User> Users { get; } = new List<User>();
    }
}