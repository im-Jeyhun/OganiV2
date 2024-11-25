using System.ComponentModel.DataAnnotations;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
