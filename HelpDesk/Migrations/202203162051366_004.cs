namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _004 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Zgloszenie", "Tresc", c => c.String(nullable: false, maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Zgloszenie", "Tresc", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
