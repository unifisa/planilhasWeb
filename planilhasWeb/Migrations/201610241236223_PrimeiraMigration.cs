namespace Planilhas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeiraMigration : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleName);
            
            
        }
        
        public override void Down()
        {
           
        }
    }
}
