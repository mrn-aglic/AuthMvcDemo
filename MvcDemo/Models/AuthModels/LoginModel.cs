using System.ComponentModel.DataAnnotations;

namespace MvcDemo.Models.AuthModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}