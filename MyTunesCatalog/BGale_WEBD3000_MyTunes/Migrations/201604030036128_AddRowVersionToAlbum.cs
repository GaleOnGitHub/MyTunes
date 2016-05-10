namespace BGale_WEBD3000_MyTunes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersionToAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Album", "RowVersion");
        }
    }
}
