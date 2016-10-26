using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planilhas.Models
{
    public class UserAccount
    {
        [Key]
        public int ColaboradorId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9](([_.-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([.-]?[a-zA-Z0-9]+)*)([.][A-Za-z]{2,4})$", ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Escolha um departamento", AllowEmptyStrings = false)]
        [Display(Name = "Depart.")]
        public string Departamentos { get; set; }
        
        [Required]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Senha precisa ser preenchida.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A {0} deve conter no minimo {2} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Compare("Senha", ErrorMessage = "Por favor repita sua senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string ConfirmeSenha { get; set; }

      
    }


 

}