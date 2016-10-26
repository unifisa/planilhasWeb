using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planilhas.Models
{
    public class Requisicao_Cemy
    {
        public int ID { get; set; }

        public string empresa { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? data { get; set; }

        public int? numero { get; set; }

        public string requisitante { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public string unidade { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public string departamento { get; set; }

        public string descricao_desp1 { get; set; }

        public string empresa1 { get; set; }

        public string documento1 { get; set; }

        public int? numero1 { get; set; }

        public decimal? valor1 { get; set; }

        public string descricao_desp2 { get; set; }

        public string empresa2 { get; set; }

        public string documento2 { get; set; }

        public int? numero2 { get; set; }

        public decimal? valor2 { get; set; }
        
        public decimal? total { get; set; }

        public string historico { get; set; }

        public double? iss { get; set; }

        public double? irrf { get; set; }

        public double? pis { get; set; }

        public double? cofins { get; set; }

        public double? csll { get; set; }

        public double? outros { get; set; }

        public double? total_deducoes { get; set; }

        public string pagamento { get; set; }

        public DateTime? vencimento1 { get; set; }

        public DateTime? vencimento2 { get; set; }

        public DateTime? vencimento3 { get; set; }

        public DateTime? vencimento4 { get; set; }

        public string favorecido { get; set; }

        public long? cnpj_cpf { get; set; }

        public string banco { get; set; }

        public int? agencia { get; set; }

        public int? conta { get; set; }

        public string corrente_ou_poup { get; set; }

        public double? valor_pag { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public string mes_despesa { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public int? ano_despesa { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public string categoria_despesa { get; set; }

        public DateTime? alteracao { get; set; }

        public string usuario { get; set; }



    }
}