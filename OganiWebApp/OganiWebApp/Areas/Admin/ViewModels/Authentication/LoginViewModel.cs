using System.ComponentModel.DataAnnotations;

namespace OganiWebApp.Areas.Admin.ViewModels.Authentication
{
    public class LoginViewModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
