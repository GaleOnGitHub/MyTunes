namespace BGale_WEBD3000_MyTunes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReleaseDateToAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "ReleaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Album", "ReleaseDate");
        }
    }
}
