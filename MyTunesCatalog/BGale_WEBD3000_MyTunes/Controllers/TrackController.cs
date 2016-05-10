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
    public class TrackController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Tracks
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrackSortParm = String.IsNullOrEmpty(sortOrder) ? "track_desc" : "";
            ViewBag.AlbumSortParm = sortOrder == "Album" ? "album_desc" : "Album";
            ViewBag.TypeSortParm = sortOrder == "MediaType" ? "type_desc" : "MediaType";
            ViewBag.GenreSortParm = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.ComposerSortParm = sortOrder == "Composer" ? "composer_desc" : "Composer";
            ViewBag.SizeSortParm = sortOrder == "Size" ? "size_desc" : "Size";
            ViewBag.DurationSortParm = sortOrder == "Duration" ? "duration_desc" : "Duration";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tracks = from t in db.Tracks
                           select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tracks = tracks.Where(t => t.Name.Contains(searchString));
            }

            tracks = sortTracks(tracks, sortOrder);
            //var tracks = db.Tracks.Include(t => t.Album).Include(t => t.Genre).Include(t => t.MediaType);
            //return View(await tracks.ToListAsync());

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(tracks.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tracks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // GET: Tracks/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name");
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TrackId,Name,AlbumId,MediaTypeId,GenreId,Composer,Milliseconds,Bytes,UnitPrice")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Tracks.Add(track);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", track.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", track.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", track.MediaTypeId);
            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", track.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", track.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", track.MediaTypeId);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(track).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", track.AlbumId);
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", track.GenreId);
            //ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", track.MediaTypeId);
            //return View(track);

            string[] fieldsToBind = new string[] { "TrackId","Name","AlbumId","MediaTypeId","GenreId","Composer","Milliseconds","Bytes","UnitPrice", "RowVersion" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var itemToUpdate = await db.Tracks.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                Track deletedItem = new Track(); //change here
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
                    var clientValues = (Track)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Track)databaseEntry.ToObject(); //change here

                        if (databaseValues.Name != clientValues.Name) //change all
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.Milliseconds != clientValues.Milliseconds) //change all
                            ModelState.AddModelError("Milliseconds", "Current value: "
                                + databaseValues.Milliseconds);
                        if (databaseValues.UnitPrice != clientValues.UnitPrice) //change all
                            ModelState.AddModelError("UnitPrice", "Current value: "
                                + databaseValues.UnitPrice);
                        if (databaseValues.AlbumId != clientValues.AlbumId)
                            ModelState.AddModelError("AlbumId", "Current value: "
                                + databaseValues.AlbumId);
                        if (databaseValues.MediaTypeId != clientValues.MediaTypeId)
                            ModelState.AddModelError("MediaTypeId", "Current value: "
                                + databaseValues.MediaTypeId);
                        if (databaseValues.GenreId != clientValues.GenreId)
                            ModelState.AddModelError("GenreId", "Current value: "
                                + databaseValues.GenreId);
                        if (databaseValues.Composer != clientValues.Composer)
                            ModelState.AddModelError("Composer", "Current value: "
                                + databaseValues.Composer);
                        if (databaseValues.Bytes != clientValues.Bytes)
                            ModelState.AddModelError("Bytes", "Current value: "
                                + databaseValues.Bytes);

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
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", itemToUpdate.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", itemToUpdate.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", itemToUpdate.MediaTypeId);
            return View(itemToUpdate);
        }

        // GET: Tracks/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
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

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Track track)
        {
            //Track track = await db.Tracks.FindAsync(id);
            //db.Tracks.Remove(track);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(track).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = track.TrackId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(track);
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

        private IQueryable<Track> sortTracks(IQueryable<Track> tracks, string sortString)
        {
            switch (sortString)
            {
                case "track_desc":
                    tracks = tracks.OrderByDescending(s => s.Name);
                    break;
                case "Album":
                    tracks = tracks.OrderBy(s => s.Album.Title);
                    break;
                case "album_desc":
                    tracks = tracks.OrderByDescending(s => s.Album.Title);
                    break;
                case "MediaType":
                    tracks = tracks.OrderBy(s => s.MediaType.Name);
                    break;
                case "type_desc":
                    tracks = tracks.OrderByDescending(s => s.MediaType.Name);
                    break;
                case "Genre":
                    tracks = tracks.OrderBy(s => s.Genre.Name);
                    break;
                case "genre_desc":
                    tracks = tracks.OrderByDescending(s => s.Genre.Name);
                    break;
                case "Composer":
                    tracks = tracks.OrderBy(s => s.Composer);
                    break;
                case "composer_desc":
                    tracks = tracks.OrderByDescending(s => s.Composer);
                    break;
                case "Size":
                    tracks = tracks.OrderBy(s => s.Bytes);
                    break;
                case "size_desc":
                    tracks = tracks.OrderByDescending(s => s.Bytes);
                    break;
                case "Duration":
                    tracks = tracks.OrderBy(s => s.Milliseconds);
                    break;
                case "duration_desc":
                    tracks = tracks.OrderByDescending(s => s.Milliseconds);
                    break;
                case "Price":
                    tracks = tracks.OrderBy(s => s.UnitPrice);
                    break;
                case "price_desc":
                    tracks = tracks.OrderByDescending(s => s.UnitPrice);
                    break;
                default:
                    tracks = tracks.OrderBy(s => s.Name);
                    break;
            }
            return tracks;
        }
    }
}
