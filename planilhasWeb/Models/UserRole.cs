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
        
        public string Usuario { get; set; }
        public string RoleName { get; set; }
    }

}