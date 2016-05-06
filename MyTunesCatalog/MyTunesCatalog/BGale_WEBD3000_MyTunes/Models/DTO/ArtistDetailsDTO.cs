using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.Models
{
    public class ArtistDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<AlbumDTO> Albums { get; set; }
    }
}