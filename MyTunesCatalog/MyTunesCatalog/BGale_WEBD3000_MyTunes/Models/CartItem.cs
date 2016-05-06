namespace BGale_WEBD3000_MyTunes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartItem")]
    public partial class CartItem
    {
        [Key]
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public DateTime DateCreated { get; set; }

        public int TrackId { get; set; }

        public virtual Track Track { get; set; }
    }
}
