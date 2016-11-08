namespace Planilhas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projeto_pla : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.Departamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Departamento = c.String(),
                    })
                .PrimaryKey(t => t.Id);
          
            
        }
        
        public override void Down()
        {
          
        }
    }
}
