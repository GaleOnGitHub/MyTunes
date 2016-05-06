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
    public partial class MediaType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["AddAction"];
            if (productAction == "add")
            {
                LabelAddStatus.Text = "Media Type added!";
            }

            if (productAction == "remove")
            {
                LabelRemoveStatus.Text = "Media Type removed!";
            }
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            // Add product data to DB.
            AddData type = new AddData();
            bool addSuccess = type.AddAlbum(
                AddName.Text,
                DropDownAddCategory.SelectedValue
                );
            if (addSuccess)
            {
                // Reload the page.
                string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?AddAction=add");
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

        public IQueryable GetTypes()
        {
            var _db = new MyTunes.Models.MyTunesContext();
            IQueryable query = _db.MediaTypes;
            return query;
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                int id = Convert.ToInt16(DropDownRemove.SelectedValue);
                var myItem = (from c in _db.MediaTypes where c.MediaTypeId == id select c).FirstOrDefault();
                if (myItem != null)
                {
                    _db.MediaTypes.Remove(myItem);
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