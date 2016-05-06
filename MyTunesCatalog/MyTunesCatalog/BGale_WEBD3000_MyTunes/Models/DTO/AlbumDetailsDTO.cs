using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGale_WEBD3000_MyTunes.Models
{
    public class AlbumDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<TracksDTO> Tracks {get;set;}
    }
}