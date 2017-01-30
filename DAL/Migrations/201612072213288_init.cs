namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumReviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Note = c.String(),
                        Rating = c.Double(nullable: false),
                        Album_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .Index(t => t.Album_ID);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        AlbumImgUri = c.String(maxLength: 1024),
                        IsPublic = c.Boolean(nullable: false),
                        Interpret_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Interprets", t => t.Interpret_ID, cascadeDelete: true)
                .Index(t => t.Interpret_ID);
            
            CreateTable(
                "dbo.Interprets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        InterpretImgUri = c.String(maxLength: 1024),
                        Language = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1024),
                        Time = c.DateTime(nullable: false),
                        Place = c.String(nullable: false, maxLength: 255),
                        EventLink = c.String(maxLength: 1024),
                        Interpret_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Interprets", t => t.Interpret_ID, cascadeDelete: true)
                .Index(t => t.Interpret_ID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Genre = c.Int(nullable: false),
                        Added = c.DateTime(nullable: false),
                        YTLink = c.String(maxLength: 1024),
                        Lyrics = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        Album_ID = c.Int(nullable: false),
                        CreatedBy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy_ID)
                .Index(t => t.Album_ID)
                .Index(t => t.CreatedBy_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Account_Key = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserAccounts", t => t.Account_Key)
                .Index(t => t.Account_Key);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 64),
                        LastName = c.String(maxLength: 64),
                        BirthDate = c.DateTime(nullable: false),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 254),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        IsAccountClosed = c.Boolean(nullable: false),
                        AccountClosed = c.DateTime(),
                        IsLoginAllowed = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastFailedLogin = c.DateTime(),
                        FailedLoginCount = c.Int(nullable: false),
                        PasswordChanged = c.DateTime(),
                        RequiresPasswordReset = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 254),
                        IsAccountVerified = c.Boolean(nullable: false),
                        LastFailedPasswordReset = c.DateTime(),
                        FailedPasswordResetCount = c.Int(nullable: false),
                        MobileCode = c.String(maxLength: 100),
                        MobileCodeSent = c.DateTime(),
                        MobilePhoneNumber = c.String(maxLength: 20),
                        MobilePhoneNumberChanged = c.DateTime(),
                        AccountTwoFactorAuthMode = c.Int(nullable: false),
                        CurrentTwoFactorAuthStatus = c.Int(nullable: false),
                        VerificationKey = c.String(maxLength: 100),
                        VerificationPurpose = c.Int(),
                        VerificationKeySent = c.DateTime(),
                        VerificationStorage = c.String(maxLength: 100),
                        HashedPassword = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccountClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.PasswordResetSecrets",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        PasswordResetSecretID = c.Guid(nullable: false),
                        Question = c.String(nullable: false, maxLength: 150),
                        Answer = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.TwoFactorAuthTokens",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100),
                        Issued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.UserCertificates",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Thumbprint = c.String(nullable: false, maxLength: 150),
                        Subject = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.SongReviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Note = c.String(),
                        Rating = c.Double(nullable: false),
                        Song_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Songs", t => t.Song_ID, cascadeDelete: true)
                .Index(t => t.Song_ID);
            
            CreateTable(
                "dbo.SongLists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Playlist_ID = c.Int(nullable: false),
                        Song_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Playlists", t => t.Playlist_ID, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.Song_ID, cascadeDelete: true)
                .Index(t => t.Playlist_ID)
                .Index(t => t.Song_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SongLists", "Song_ID", "dbo.Songs");
            DropForeignKey("dbo.SongLists", "Playlist_ID", "dbo.Playlists");
            DropForeignKey("dbo.AlbumReviews", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.SongReviews", "Song_ID", "dbo.Songs");
            DropForeignKey("dbo.Songs", "CreatedBy_ID", "dbo.Users");
            DropForeignKey("dbo.Playlists", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Users", "Account_Key", "dbo.UserAccounts");
            DropForeignKey("dbo.UserCertificates", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.TwoFactorAuthTokens", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.PasswordResetSecrets", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccounts", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccountClaims", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.UserClaims", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.Songs", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.Albums", "Interpret_ID", "dbo.Interprets");
            DropForeignKey("dbo.Events", "Interpret_ID", "dbo.Interprets");
            DropIndex("dbo.SongLists", new[] { "Song_ID" });
            DropIndex("dbo.SongLists", new[] { "Playlist_ID" });
            DropIndex("dbo.SongReviews", new[] { "Song_ID" });
            DropIndex("dbo.Playlists", new[] { "User_ID" });
            DropIndex("dbo.UserCertificates", new[] { "ParentKey" });
            DropIndex("dbo.TwoFactorAuthTokens", new[] { "ParentKey" });
            DropIndex("dbo.PasswordResetSecrets", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccounts", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccountClaims", new[] { "ParentKey" });
            DropIndex("dbo.UserClaims", new[] { "ParentKey" });
            DropIndex("dbo.Users", new[] { "Account_Key" });
            DropIndex("dbo.Songs", new[] { "CreatedBy_ID" });
            DropIndex("dbo.Songs", new[] { "Album_ID" });
            DropIndex("dbo.Events", new[] { "Interpret_ID" });
            DropIndex("dbo.Albums", new[] { "Interpret_ID" });
            DropIndex("dbo.AlbumReviews", new[] { "Album_ID" });
            DropTable("dbo.SongLists");
            DropTable("dbo.SongReviews");
            DropTable("dbo.Playlists");
            DropTable("dbo.UserCertificates");
            DropTable("dbo.TwoFactorAuthTokens");
            DropTable("dbo.PasswordResetSecrets");
            DropTable("dbo.LinkedAccounts");
            DropTable("dbo.LinkedAccountClaims");
            DropTable("dbo.UserClaims");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Users");
            DropTable("dbo.Songs");
            DropTable("dbo.Events");
            DropTable("dbo.Interprets");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumReviews");
        }
    }
}
