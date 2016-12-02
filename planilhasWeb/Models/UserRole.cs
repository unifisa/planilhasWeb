using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planilhas.Models
{
    public class UserRole
    {
        [Key]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Grupo")]
        public string RoleName { get; set; }
    }

}