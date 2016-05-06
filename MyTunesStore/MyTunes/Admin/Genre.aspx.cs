using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyTunes.Logic;
using MyTunes.Models;

namespace MyTunes.Admin
{
    public partial class Genre : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["GenreAction"];
            if (productAction == "add")
            {
                LabelAddStatus.Text = "Genre added!";
            }

            if (productAction == "remove")
            {
                LabelRemoveStatus.Text = "Genre removed!";
            }
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Add product data to DB.
            AddData genre = new AddData();
            bool addSuccess = genre.AddGenre(
                AddName.Text
                );
            if (addSuccess)
            {
                // Reload the page.
                string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?GenreAction=add");
            }
            else
            {
                LabelAddStatus.Text = "Unable to add new product to database.";
            }

        }

        public IQueryable GetGenres()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.Genres;
            return query;
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                int id = Convert.ToInt16(DropDownRemove.SelectedValue);
                var myItem = (from c in _db.Genres where c.GenreId == id select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.Genres.Remove(myItem);
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