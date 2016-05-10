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
    public class MediaCategoryController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: MediaCategories
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TrackSortParm = sortOrder == "Tracks" ? "tracks_asc" : "Tracks";
            var categories = from t in db.MediaCategories
                         select t;

            switch (sortOrder)
            {
                case "Tracks":
                    categories = categories.OrderByDescending(s => s.MediaTypes.Sum(m => m.Tracks.Count));
                    break;
                case "tracks_asc":
                    categories = categories.OrderBy(s => s.MediaTypes.Sum(m => m.Tracks.Count));
                    break;
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;
                default:
                    categories = categories.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        // GET: MediaCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = await db.MediaCategories.FindAsync(id);
            if (mediaCategory == null)
            {
                return HttpNotFound();
            }
            return View(mediaCategory);
        }

        // GET: MediaCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MediaCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MediaCategoryId,Name")] MediaCategory mediaCategory)
        {
            if (ModelState.IsValid)
            {
                int count = db.MediaCategories.Where(mt => mt.Name.Equals(mediaCategory.Name)).Count();
                if (count > 0)
                {
                    ViewBag.Duplicate = "Media Category " + mediaCategory.Name + " already exists.";
                    return View(mediaCategory);
                }
                db.MediaCategories.Add(mediaCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mediaCategory);
        }

        // GET: MediaCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = await db.MediaCategories.FindAsync(id);
            if (mediaCategory == null)
            {
                return HttpNotFound();
            }
            return View(mediaCategory);
        }

        // POST: MediaCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(mediaCategory).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(mediaCategory);

            string[] fieldsToBind = new string[] { "MediaCategoryId","Name", "RowVersion" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var itemToUpdate = await db.MediaCategories.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                MediaCategory deletedItem = new MediaCategory(); //change here
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
                    var clientValues = (MediaCategory)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (MediaCategory)databaseEntry.ToObject(); //change here

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
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", itemToUpdate.MediaCategoryId); //change
            return View(itemToUpdate);
        }

        // GET: MediaCategories/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = await db.MediaCategories.FindAsync(id);
            if (mediaCategory == null)
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
            return View(mediaCategory);

        }

        // POST: MediaCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(MediaCategory mediaCategory)
        {
            //MediaCategory mediaCategory = await db.MediaCategories.FindAsync(id);
            //db.MediaCategories.Remove(mediaCategory);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(mediaCategory).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = mediaCategory.MediaCategoryId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(mediaCategory);
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
