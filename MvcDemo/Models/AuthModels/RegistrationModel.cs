using System.ComponentModel.DataAnnotations;

namespace MvcDemo.Models.AuthModels
{
    public class RegistrationModel : LoginModel
    {
        [Required] public string Firstname { get; set; }
        [Required] public string Lastname { get; set; }
        [Required] public string Username { get; set; }
        [Required] [Compare("Password")] public string PasswordConfirmation { get; set; }
    }
}