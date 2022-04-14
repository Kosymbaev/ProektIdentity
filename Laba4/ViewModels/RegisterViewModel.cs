using System;
using System.ComponentModel.DataAnnotations;

namespace Laba4.ViewModels
{
    public class RegisterViewModel
    {
        public DateTime LastLogin { get; set; } 
        public DateTime Register { get; set; } 


        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name ="Имя")]
        public string Name { get; set; }    

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}