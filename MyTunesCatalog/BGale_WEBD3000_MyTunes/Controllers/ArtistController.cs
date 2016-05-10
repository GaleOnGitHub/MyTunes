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
using BGale_WEBD3000_MyTunes.ViewModels;

namespace BGale_WEBD3000_MyTunes.Controllers
{
    public class ArtistController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Artists
        public ViewResult Index(int? ArtistId, int? AlbumId, string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            ArtistIndexData artistData = new ArtistIndexData();
            var artists = from t in db.Artists select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(t => t.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    artists = artists.OrderByDescending(s => s.Name);
                    break;
                default:
                    artists = artists.OrderBy(s => s.Name);
                    break;
            }
            
            if (ArtistId != null)
            {
                ViewBag.ArtistId = ArtistId.Value;
                artistData.albums = artists.Where(
                    i => i.ArtistId == ArtistId.Value).Single().Albums.OrderBy(i=>i.Title);
            }

            if (AlbumId != null)
            {
                ViewBag.AlbumId = AlbumId.Value;
                artistData.tracks = artistData.albums.Where(
                    x => x.AlbumId == AlbumId.Value).Single().Tracks.OrderBy(x=>x.Name);
            }

            artistData.artists = artists.ToPagedList(pageNumber, pageSize);
            return View(artistData);
        }

        // GET: Artists/Details/5
        public async Task<ActionResult> Details(int? id, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistAlbums details = new ArtistAlbums();
            details.artist = await db.Artists.FindAsync(id);
            details.albums = details.artist.Albums.OrderBy(t => t.Title).ToPagedList(pageNumber, pageSize);
            if (details == null)
            {
                return HttpNotFound();
            }

            return View(details);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ArtistId,Name")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                int count = db.Artists.Where(mt => mt.Name.Equals(artist.Name)).Count();
                if (count > 0)
                {
                    ViewBag.Duplicate = "Artist " + artist.Name + " already exists.";
                    return View(artist);
                }
                db.Artists.Add(artist);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(artist).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(artist);

            string[] fieldsToBind = new string[] { "ArtistId" ,"Name" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var itemToUpdate = await db.Artists.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                Artist deletedItem = new Artist(); //change here
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
                    var clientValues = (Artist)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Artist)databaseEntry.ToObject(); //change here

                        if (databaseValues.Name != clientValues.Name) //change all
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);

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

            return View(itemToUpdate);

        }

        // GET: Artists/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
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
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Artist artist)
        {
            //Artist artist = await db.Artists.FindAsync(id);
            //db.Artists.Remove(artist);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(artist).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = artist.ArtistId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(artist);
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
