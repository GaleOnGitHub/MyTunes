namespace BGale_WEBD3000_MyTunes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Track")]
    public partial class Track
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Track()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int TrackId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name="Track")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Album")]
        public int? AlbumId { get; set; }

        [Required]
        [Display(Name = "Media")]
        public int MediaTypeId { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int? GenreId { get; set; }

        [StringLength(220, MinimumLength = 3)]
        public string Composer { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid Number")]
        [Display(Name="Time(ms)")]
        public int Milliseconds { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid Number")]
        [Display(Name = "Size")]
        public int? Bytes { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        [DataType(DataType.Currency)]
        [Display(Name="Price")]
        public decimal UnitPrice { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Album Album { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual MediaType MediaType { get; set; }
    }
}
