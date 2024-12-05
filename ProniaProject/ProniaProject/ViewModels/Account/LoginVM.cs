using System.ComponentModel.DataAnnotations;

namespace ProniaProject.ViewModels
{
    public class LoginVM
    {
        [MaxLength(256)]
        [MinLength(4)]
        public string EmailOrUserName { get; set; }
         
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }

    }
}
