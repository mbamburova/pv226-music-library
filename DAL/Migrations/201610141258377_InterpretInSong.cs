namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterpretInSong : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Songs", "AlbumId", "dbo.Albums");
            DropIndex("dbo.Songs", new[] { "AlbumId" });
            RenameColumn(table: "dbo.Songs", name: "AlbumId", newName: "Album_Id");
            AddColumn("dbo.Songs", "Interpret_Id", c => c.Int());
            AlterColumn("dbo.Songs", "Album_Id", c => c.Int());
            CreateIndex("dbo.Songs", "Interpret_Id");
            CreateIndex("dbo.Songs", "Album_Id");
            AddForeignKey("dbo.Songs", "Interpret_Id", "dbo.Interprets", "Id");
            AddForeignKey("dbo.Songs", "Album_Id", "dbo.Albums", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.Songs", "Interpret_Id", "dbo.Interprets");
            DropIndex("dbo.Songs", new[] { "Album_Id" });
            DropIndex("dbo.Songs", new[] { "Interpret_Id" });
            AlterColumn("dbo.Songs", "Album_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Songs", "Interpret_Id");
            RenameColumn(table: "dbo.Songs", name: "Album_Id", newName: "AlbumId");
            CreateIndex("dbo.Songs", "AlbumId");
            AddForeignKey("dbo.Songs", "AlbumId", "dbo.Albums", "Id", cascadeDelete: true);
        }
    }
}
