using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using PagedList;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.Controllers
{
    public class AlbumController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Albums
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.ArtistSortParm = sortOrder == "Artist" ? "artist_desc" : "Artist";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var album = from t in db.Albums
                         select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                album = album.Where(t => t.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    album = album.OrderByDescending(s => s.Title);
                    break;
                case "Artist":
                    album = album.OrderBy(s => s.Artist.Name);
                    break;
                case "artist_desc":
                    album = album.OrderByDescending(s => s.Artist.Name);
                    break;
                default:
                    album = album.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(album.ToPagedList(pageNumber, pageSize));
        }

        // GET: Albums/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AlbumId,Title,ArtistId,ReleaseDate")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(album).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            //return View(album);
            string[] fieldsToBind = new string[] { "AlbumId", "Title", "ArtistId", "ReleaseDate", "RowVersion" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var itemToUpdate = await db.Albums.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                Album deletedItem = new Album(); //change here
                TryUpdateModel(deletedItem, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                return View(deletedItem);
            }


            if (TryUpdateModel(itemToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(itemToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Album)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Album)databaseEntry.ToObject(); //change here

                        if (databaseValues.Title != clientValues.Title) //change all
                            ModelState.AddModelError("Title", "Current value: "
                                + databaseValues.Title);
                        if (databaseValues.ArtistId != clientValues.ArtistId)
                            ModelState.AddModelError("Artist", "Current value: "
                                + databaseValues.Artist.Name);
                        if (databaseValues.ReleaseDate != clientValues.ReleaseDate)
                            ModelState.AddModelError("Release Date", "Current value: "
                                + String.Format("{0:d}", databaseValues.ReleaseDate));

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        itemToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", itemToUpdate.ArtistId); //change
            return View(itemToUpdate);
        }

        // GET: Albums/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                if (concurrencyError.GetValueOrDefault()) //this
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault()) //this
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Album album)
        {
            //Album album = await db.Albums.FindAsync(id);
            //db.Albums.Remove(album);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(album).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = album.AlbumId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(album);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
