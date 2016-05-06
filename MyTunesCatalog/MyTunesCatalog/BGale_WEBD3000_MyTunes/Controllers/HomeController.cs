using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGale_WEBD3000_MyTunes.Models;
using BGale_WEBD3000_MyTunes.ViewModels;

namespace BGale_WEBD3000_MyTunes.Controllers
{
    public class HomeController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Statistics()
        {
            AlbumStatistics stats = new AlbumStatistics();
            IQueryable<Track> tracks = from t in db.Tracks select t;
            IQueryable<Album> albums = from t in db.Albums select t;
            IQueryable<Artist> artists = from t in db.Artists select t;
            
            stats.numberOfTracks = (from t in db.Tracks select t).Count();
                //tracks.Count();
            stats.numberOfAlbums = albums.Count();
            stats.numberOfArtists = artists.Count();
            stats.largestTrack = tracks.OrderByDescending(t => t.Bytes).First();
            stats.longestTrack = tracks.OrderByDescending(t => t.Milliseconds).First();
           
            int maxTracks = artists.Max(a => a.Albums.Sum(alb => alb.Tracks.Count));
            stats.artistsMostTracks = artists.Where(a => a.Albums.Sum(alb => alb.Tracks.Count).Equals(maxTracks));
            stats.maxTracks = maxTracks;
            return View(stats);
        }
    }
}