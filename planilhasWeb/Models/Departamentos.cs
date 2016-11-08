using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planilhas.Models
{
    public class Departamentos
    {
        [Key]
        public int Id { get; set; }

        public string Departamento { get; set; }
    }
}