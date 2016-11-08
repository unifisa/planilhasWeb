using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Planilhas.Models
{
    public class OurDbContext : DbContext
    {
        public OurDbContext() : base(System.Configuration.ConfigurationManager.ConnectionStrings["OurConnectionString"].ConnectionString)
        {
            Database.SetInitializer<OurDbContext>(new CreateDatabaseIfNotExists<OurDbContext>());
        }
        public DbSet<UserAccount> userAccount { get; set; }

        public DbSet<diluicao_normal> diluicao_normal { get; set; }

        public DbSet<Calc_Rateio> Calc_Rateio { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Requisicao> Requisicao { get; set; }

        public DbSet<Requisicao_Fisa> Requisicao_Fisa { get; set; }

        public DbSet<Requisicao_Cemy> Requisicao_Cemy { get; set; }

        public DbSet<Unidades> Unidades { get; set; }

        public DbSet<Departamentos> Departamentos { get; set; }










    }
}