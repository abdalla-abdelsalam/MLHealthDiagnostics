namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inti453 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "UserNationalID", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AspNetUsers", "UserNationalID", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
