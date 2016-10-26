using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace Planilhas.Models
{
    public class Calc_Rateio
    {
        
        public int ID { get; set; }

        public double bem { get; set; }

        public double porc_paga { get; set; }

        public double quitacao { get; set; }

        public double bem_com_txs { get; set; }

        public double quit_sem_seg { get; set; }

        public int assemb_vencidas { get; set; }

        public int parc_faltam { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double taxa_adm { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}%", ApplyFormatInEditMode = true)]
        public double fundo_reserv { get; set; }

        public double porc_seguro { get; set; }

        public double seguro { get; set; }

        public DateTime alteracao { get; set; }

        public int parc_inicial { get; set; }

        public int parc_final { get; set; }

        public double amortizacao { get; set; }

        public double em_porcentagem { get; set; }

        public double total { get; set; }

        public double div_total { get; set; }

        public double zero_porc { get; set; }

        public double total_final { get; set; }

        [Required(ErrorMessage = "Precisa ser preenchido.", AllowEmptyStrings = false)]
        public string cliente { get; set; }

       //[Required(ErrorMessage = "Precisa ser preenchido.")]
       // [Range(1, 100000000, ErrorMessage = "Precisa ser preenchido")]
        public int cota { get; set; }

        //[Required(ErrorMessage = "Precisa ser preenchido.")]
        //[Range(1, 100000000, ErrorMessage = "Precisa ser preenchido")]
        public int grupo { get; set; }

        public int contrato { get; set; }

        public string usuario { get; set; }
        




    }
}