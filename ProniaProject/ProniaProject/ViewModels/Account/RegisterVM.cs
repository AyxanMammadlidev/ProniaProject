using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProniaProject.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string Surname { get; set; }
        [MinLength(4)]
        [MaxLength(80)]
        public string UserName { get; set; }
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
