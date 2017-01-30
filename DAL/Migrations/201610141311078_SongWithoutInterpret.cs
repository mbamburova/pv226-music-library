namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongWithoutInterpret : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Songs", "Interpret_Id", "dbo.Interprets");
            DropIndex("dbo.Songs", new[] { "Interpret_Id" });
            DropColumn("dbo.Songs", "Interpret_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "Interpret_Id", c => c.Int());
            CreateIndex("dbo.Songs", "Interpret_Id");
            AddForeignKey("dbo.Songs", "Interpret_Id", "dbo.Interprets", "Id");
        }
    }
}
