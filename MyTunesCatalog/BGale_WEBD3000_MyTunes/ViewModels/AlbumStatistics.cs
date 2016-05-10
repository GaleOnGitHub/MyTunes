using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.ViewModels
{
    public class AlbumStatistics
    {
        [Display(Name = "Total Tracks")]
        public int numberOfTracks { get; set; }
        [Display(Name = "Total Albums")]
        public int numberOfAlbums { get; set; }
        [Display(Name = "Total Artists")]
        public int numberOfArtists { get; set; }
        [Display(Name = "Tracks")]
        public int maxTracks { get; set; }
        [Display(Name = "Largest Track")]
        public Track largestTrack { get; set; }
        [Display(Name = "Longest Track")]
        public Track longestTrack { get; set; }
        [Display(Name = "Most Tracks")]
        public IEnumerable<Artist> artistsMostTracks { get; set; }
    }
}