using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.ViewModels
{
    public class ArtistIndexData
    {
        public IPagedList<Artist>  artists { get; set; }
        public IEnumerable<Album> albums { get; set; }
        public IEnumerable<Track> tracks { get; set; }
    }
}