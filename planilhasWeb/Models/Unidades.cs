﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planilhas.Models
{
    public class Unidades
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Unidades")]
        public string unidades { get; set; }

        
    }


}