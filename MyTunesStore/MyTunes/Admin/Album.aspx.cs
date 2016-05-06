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
    public partial class Album : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["ProductAction"];
            if (productAction == "add")
            {
                LabelAddStatus.Text = "Album added!";
            }

            //if (productAction == "remove")
            //{
            //    LabelRemoveStatus.Text = "Product removed!";
            //}
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Add product data to DB.
            AddData albums = new AddData();
            bool addSuccess = albums.AddAlbum(
                AddName.Text,
                DropDownAddArtist.SelectedValue
                );
            if (addSuccess)
            {
                // Reload the page.
                string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?ProductAction=add");
            }
            else
            {
                LabelAddStatus.Text = "Unable to add new product to database.";
            }

        }

        public IQueryable GetArtists()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Artists;
            return query;
        }

        public IQueryable GetAlbums()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Albums;
            return query;
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                int id = Convert.ToInt16(DropDownRemove.SelectedValue);
                var myItem = (from c in _db.Albums where c.AlbumId == id select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.Albums.Remove(myItem);
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