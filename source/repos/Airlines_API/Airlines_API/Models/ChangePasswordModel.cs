using System.ComponentModel.DataAnnotations;

namespace Airlines_API.Models
{
    public class ChangePasswordModel
    {
        [Key]
        [Required]
        public string email { get; set; }

        [Required]

        [DataType(DataType.Password)]
        public string old_password { get; set; }

        [Required]

        [DataType(DataType.Password)]

        public string new_password { get; set; }
    }
}
