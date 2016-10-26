using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Planilhas.Models;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;



namespace Planilhas
{
    //tutorial sobre relatório no youtube = www.youtube.com/watch?v=dr8iM9hQ7gM
    public partial class relatorio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowReport();
                //ShowReport2();

            }
        }

        private void ShowReport()
        {
            ReportViewer2.Reset();
            ReportDataSource requisicao = new ReportDataSource("Requisicao", GetRequisicao_Fisa());

            ReportViewer2.LocalReport.DataSources.Add(requisicao);

            ReportViewer2.LocalReport.ReportPath = "Relatorio.rdlc";

            ReportViewer2.LocalReport.Refresh();
        }

        private DataTable GetRequisicao_Fisa()
        {
            DataTable dt = new DataTable();
            string conn = @"Data Source=UNIFISA-PC\SQLEXPRESS;Initial Catalog=PlanilhasDB;Integrated Security=true";
            using (SqlConnection cn = new SqlConnection(conn))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM Requisicaos WHERE ID = 55 ", cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                dt.Load(reader);
            }

            return dt;
        }

        //private void ShowReport2()
        //{
        //    ReportViewer2.Reset();
        //    ReportDataSource requisicao2 = new ReportDataSource("Requisicao", GetRequisicao_Fisa2());

        //    ReportViewer2.LocalReport.DataSources.Add(requisicao2);

        //    ReportViewer2.LocalReport.ReportPath = "Relatorio.rdlc";

        //    ReportViewer2.LocalReport.Refresh();
        //}

        //private DataTable GetRequisicao_Fisa2()
        //{
        //    DataTable dt = new DataTable();
        //    string conn = @"Data Source=UNIFISA-PC\SQLEXPRESS;Initial Catalog=PlanilhasDB;Integrated Security=true";
        //    using (SqlConnection cn = new SqlConnection(conn))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM Requisicaos WHERE ID = 51", cn);
        //        cn.Open();


        //        SqlDataReader reader = cmd.ExecuteReader();

        //        dt.Load(reader);
        //    }

        //    return dt;
        //}



    }
}