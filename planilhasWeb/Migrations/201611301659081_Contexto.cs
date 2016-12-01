namespace Planilhas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Contexto : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Usuario = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(),
                    })
                ;
            
        }
        
        public override void Down()
        {
          
        }
    }
}
