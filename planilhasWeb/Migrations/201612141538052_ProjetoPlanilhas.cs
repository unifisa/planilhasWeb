namespace Planilhas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjetoPlanilhas : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario = c.String(),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
           
        }
    }
}
