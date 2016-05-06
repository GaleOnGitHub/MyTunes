using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.ViewModels
{
    public class GenreTracks
    {
        public Genre genre { get; set; }
        public IPagedList<Track> tracks { get; set; }
    }
}