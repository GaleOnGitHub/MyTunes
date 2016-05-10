namespace BGale_WEBD3000_MyTunes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Album",
            //    c => new
            //        {
            //            AlbumId = c.Int(nullable: false, identity: true),
            //            Title = c.String(nullable: false, maxLength: 160),
            //            ArtistId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.AlbumId)
            //    .ForeignKey("dbo.Artist", t => t.ArtistId)
            //    .Index(t => t.ArtistId);
            
            //CreateTable(
            //    "dbo.Artist",
            //    c => new
            //        {
            //            ArtistId = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 120),
            //        })
            //    .PrimaryKey(t => t.ArtistId);
            
            //CreateTable(
            //    "dbo.Track",
            //    c => new
            //        {
            //            TrackId = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 200),
            //            AlbumId = c.Int(nullable: false),
            //            MediaTypeId = c.Int(nullable: false),
            //            GenreId = c.Int(nullable: false),
            //            Composer = c.String(maxLength: 220),
            //            Milliseconds = c.Int(nullable: false),
            //            Bytes = c.Int(nullable: false),
            //            UnitPrice = c.Decimal(nullable: false, precision: 10, scale: 2, storeType: "numeric"),
            //        })
            //    .PrimaryKey(t => t.TrackId)
            //    .ForeignKey("dbo.Album", t => t.AlbumId, cascadeDelete: true)
            //    .ForeignKey("dbo.Genre", t => t.GenreId, cascadeDelete: true)
            //    .ForeignKey("dbo.MediaType", t => t.MediaTypeId)
            //    .Index(t => t.AlbumId)
            //    .Index(t => t.MediaTypeId)
            //    .Index(t => t.GenreId);
            
            //CreateTable(
            //    "dbo.CartItem",
            //    c => new
            //        {
            //            ItemId = c.String(nullable: false, maxLength: 128),
            //            CartId = c.String(),
            //            DateCreated = c.DateTime(nullable: false),
            //            TrackId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ItemId)
            //    .ForeignKey("dbo.Track", t => t.TrackId, cascadeDelete: true)
            //    .Index(t => t.TrackId);
            
            //CreateTable(
            //    "dbo.Genre",
            //    c => new
            //        {
            //            GenreId = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 120),
            //        })
            //    .PrimaryKey(t => t.GenreId);
            
            //CreateTable(
            //    "dbo.MediaType",
            //    c => new
            //        {
            //            MediaTypeId = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 120),
            //            MediaCategoryId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.MediaTypeId)
            //    .ForeignKey("dbo.MediaCategory", t => t.MediaCategoryId)
            //    .Index(t => t.MediaCategoryId);
            
            //CreateTable(
            //    "dbo.MediaCategory",
            //    c => new
            //        {
            //            MediaCategoryId = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 120),
            //        })
            //    .PrimaryKey(t => t.MediaCategoryId);
            
            //CreateTable(
            //    "dbo.OrderDetail",
            //    c => new
            //        {
            //            OrderDetailId = c.Int(nullable: false, identity: true),
            //            OrderId = c.Int(nullable: false),
            //            Username = c.String(),
            //            ProductId = c.Int(nullable: false),
            //            UnitPrice = c.Double(),
            //        })
            //    .PrimaryKey(t => t.OrderDetailId)
            //    .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
            //    .Index(t => t.OrderId);
            
            //CreateTable(
            //    "dbo.Order",
            //    c => new
            //        {
            //            OrderId = c.Int(nullable: false, identity: true),
            //            OrderDate = c.DateTime(nullable: false),
            //            Username = c.String(),
            //            FirstName = c.String(nullable: false, maxLength: 160),
            //            LastName = c.String(nullable: false, maxLength: 160),
            //            Address = c.String(nullable: false, maxLength: 70),
            //            City = c.String(nullable: false, maxLength: 40),
            //            State = c.String(nullable: false, maxLength: 40),
            //            PostalCode = c.String(nullable: false, maxLength: 10),
            //            Country = c.String(nullable: false, maxLength: 40),
            //            Phone = c.String(maxLength: 24),
            //            Email = c.String(nullable: false),
            //            Total = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PaymentTransactionId = c.String(),
            //            HasBeenShipped = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            //DropForeignKey("dbo.Track", "MediaTypeId", "dbo.MediaType");
            //DropForeignKey("dbo.MediaType", "MediaCategoryId", "dbo.MediaCategory");
            //DropForeignKey("dbo.Track", "GenreId", "dbo.Genre");
            //DropForeignKey("dbo.CartItem", "TrackId", "dbo.Track");
            //DropForeignKey("dbo.Track", "AlbumId", "dbo.Album");
            //DropForeignKey("dbo.Album", "ArtistId", "dbo.Artist");
            //DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            //DropIndex("dbo.MediaType", new[] { "MediaCategoryId" });
            //DropIndex("dbo.CartItem", new[] { "TrackId" });
            //DropIndex("dbo.Track", new[] { "GenreId" });
            //DropIndex("dbo.Track", new[] { "MediaTypeId" });
            //DropIndex("dbo.Track", new[] { "AlbumId" });
            //DropIndex("dbo.Album", new[] { "ArtistId" });
            //DropTable("dbo.Order");
            //DropTable("dbo.OrderDetail");
            //DropTable("dbo.MediaCategory");
            //DropTable("dbo.MediaType");
            //DropTable("dbo.Genre");
            //DropTable("dbo.CartItem");
            //DropTable("dbo.Track");
            //DropTable("dbo.Artist");
            //DropTable("dbo.Album");
        }
    }
}
