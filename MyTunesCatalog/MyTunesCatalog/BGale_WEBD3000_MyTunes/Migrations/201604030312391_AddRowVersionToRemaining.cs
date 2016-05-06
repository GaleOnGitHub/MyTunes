namespace BGale_WEBD3000_MyTunes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersionToRemaining : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artist", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Track", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Genre", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MediaType", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MediaCategory", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MediaCategory", "RowVersion");
            DropColumn("dbo.MediaType", "RowVersion");
            DropColumn("dbo.Genre", "RowVersion");
            DropColumn("dbo.Track", "RowVersion");
            DropColumn("dbo.Artist", "RowVersion");
        }
    }
}
