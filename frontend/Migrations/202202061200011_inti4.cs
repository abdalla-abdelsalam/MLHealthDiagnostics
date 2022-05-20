namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inti4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserBarthDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.AspNetUsers", "createAt", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "createAt");
            DropColumn("dbo.AspNetUsers", "UserBarthDate");
        }
    }
}
