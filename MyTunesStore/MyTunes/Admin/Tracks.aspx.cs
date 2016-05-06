using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyTunes.Models;
using MyTunes.Logic;

namespace MyTunes.Admin
{
    public partial class Tracks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["TrackAction"];
            if (productAction == "add")
            {
                LabelAddStatus.Text = "Track added!";
            }

            if (productAction == "remove")
            {
                LabelRemoveStatus.Text = "Track removed!";
            }
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Add product data to DB.
            AddData tracks = new AddData();
            bool addSuccess = tracks.AddTrack(
                AddName.Text,
                DropDownAddAlbum.SelectedValue,
                AddPrice.Text,
                DropDownAddMedia.SelectedValue,
                DropDownAddGenre.SelectedValue,
                AddComposer.Text,
                AddTime.Text,
                AddBytes.Text
                );
            if (addSuccess)
            {
                // Reload the page.
                string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?TrackAction=add");
            }
            else
            {
                LabelAddStatus.Text = "Unable to add new product to database.";
            }

        }

        public IQueryable GetAlbums()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Albums;
            return query;
        }

        public IQueryable GetMedia()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.MediaTypes;
            return query;
        }

        public IQueryable GetGenre()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Genres;
            return query;
        }

        public IQueryable GetTracks()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Tracks;
            return query;
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                int id = Convert.ToInt16(DropDownRemove.SelectedValue);
                var myItem = (from c in _db.Tracks where c.TrackId == id select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.Tracks.Remove(myItem);
                    _db.SaveChanges();

                    // Reload the page.
                    string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                    Response.Redirect(pageUrl + "?RemoveAction=remove");
                }
                else
                {
                    LabelRemoveStatus.Text = "Unable to locate product.";
                }
            }
        }
    }
}