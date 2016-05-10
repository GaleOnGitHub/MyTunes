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
    public class GenreController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Genres
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TrackSortParm = sortOrder == "Tracks" ? "tracks_asc" : "Tracks";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var genres = from t in db.Genres
                         select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                genres = genres.Where(t => t.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Tracks":
                    genres = genres.OrderByDescending(s => s.Tracks.Count);
                    break;
                case "tracks_asc":
                    genres = genres.OrderBy(s => s.Tracks.Count);
                    break;
                case "name_desc":
                    genres = genres.OrderByDescending(s => s.Name);
                    break;
                default:
                    genres = genres.OrderBy(s => s.Name);
                    break;
            }
            //var tracks = db.Tracks.Include(t => t.Album).Include(t => t.Genre).Include(t => t.MediaType);
            //return View(await tracks.ToListAsync());

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(genres.ToPagedList(pageNumber, pageSize));
        }

        // GET: Genres/Details/5
        public async Task<ActionResult> Details(int? id, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GenreTracks details = new GenreTracks();
            details.genre = await db.Genres.FindAsync(id);
            details.tracks = details.genre.Tracks.OrderBy(t => t.Name).ToPagedList(pageNumber, pageSize);
            if (details == null)
            {
                return HttpNotFound();
            }
                                           
            return View(details);
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GenreId,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                int count = db.Genres.Where(mt => mt.Name.Equals(genre.Name)).Count();
                if (count > 0)
                {
                    ViewBag.Duplicate = "Genre " + genre.Name + " already exists.";
                    return View(genre);
                }
                db.Genres.Add(genre);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(genre).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(genre);
            string[] fieldsToBind = new string[] { "GenreId","Name", "RowVersion" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var itemToUpdate = await db.Genres.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                Genre deletedItem = new Genre(); //change here
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
                    var clientValues = (Genre)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Genre)databaseEntry.ToObject(); //change here

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

        // GET: Genres/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
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

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Genre genre)
        {
            //Genre genre = await db.Genres.FindAsync(id);
            //db.Genres.Remove(genre);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(genre).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = genre.GenreId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(genre);
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
