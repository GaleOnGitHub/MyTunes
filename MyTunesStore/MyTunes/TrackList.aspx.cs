using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyTunes.Models;
using System.Web.ModelBinding;

namespace MyTunes
{
    public partial class TrackList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Track> GetTracks()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable<Track> query = _db.Tracks;
            return query;
        }
    }
}