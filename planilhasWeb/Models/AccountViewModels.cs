using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planilhas.Models
{
  
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Salvar login")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }

        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Departamentos")]
        public string Departamentos { get; set; }

        
        [Display(Name = "Digite seu nome de usuario")]
        public string Usuario { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O {0} deve ser no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A senha e a combinação de senha nao combinam")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Digite seu nome de usuario")]
        public string Roles { get; set; }
    }

    //public class ResetPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "O {0} deve ser no minimo {2} caracteres.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Senha")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirme a senha")]
    //    [Compare("Password", ErrorMessage = "A senha e a combinação de senha nao combinam.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Code { get; set; }
    //}

    //public class ForgotPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}
}
