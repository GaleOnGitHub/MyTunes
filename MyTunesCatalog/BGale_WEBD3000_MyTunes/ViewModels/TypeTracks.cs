using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.ViewModels
{
    public class TypeTracks
    {
        public MediaType type { get; set; }
        public IPagedList<Track> tracks { get; set; }
    }
}