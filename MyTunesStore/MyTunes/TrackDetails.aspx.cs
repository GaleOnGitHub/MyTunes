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
    public partial class TrackDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Track> GetTrack([QueryString("trackID")] int? trackID)
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable<Track> query = _db.Tracks;
            if (trackID.HasValue && trackID > 0)
            {
                query = query.Where(t => t.TrackId == trackID);
            }
            else
            {
                query = null;
            }
            return query;
        }
    }
}