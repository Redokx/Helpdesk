namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _003 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Uzytkownik", "Imie", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Uzytkownik", "Nazwisko", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Uzytkownik", "Nazwisko", c => c.String(maxLength: 50));
            AlterColumn("dbo.Uzytkownik", "Imie", c => c.String(maxLength: 20));
        }
    }
}
