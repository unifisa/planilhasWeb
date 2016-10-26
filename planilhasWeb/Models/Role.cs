using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Planilhas.Models
{
    public class Role
    {
        [Key]       
        public string RoleName { get; set; }
        
    }
}