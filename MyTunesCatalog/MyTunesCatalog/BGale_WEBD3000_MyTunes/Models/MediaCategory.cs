namespace BGale_WEBD3000_MyTunes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MediaCategory")]
    public partial class MediaCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MediaCategory()
        {
            MediaTypes = new HashSet<MediaType>();
        }

        public int MediaCategoryId { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        [Display(Name = "Media")]
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MediaType> MediaTypes { get; set; }
    }
}
