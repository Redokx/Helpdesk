namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailUids",
                c => new
                    {
                        IdUids = c.Int(nullable: false, identity: true),
                        Uids = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdUids);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailUids");
        }
    }
}
