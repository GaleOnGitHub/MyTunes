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
    public class MediaTypeController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: MediaTypes
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.TrackSortParm = sortOrder == "Tracks" ? "tracks_asc" : "Tracks";
            var types = from t in db.MediaTypes
                             select t;

            switch (sortOrder)
            {
                case "Tracks":
                    types = types.OrderByDescending(s => s.Tracks.Count);
                    break;
                case "tracks_asc":
                    types = types.OrderBy(s => s.Tracks.Count);
                    break;
                case "name_desc":
                    types = types.OrderByDescending(s => s.Name);
                    break;
                case "Category":
                    types = types.OrderBy(s => s.MediaCategory.Name);
                    break;
                case "category_desc":
                    types = types.OrderByDescending(s => s.MediaCategory.Name);
                    break;
                default:
                    types = types.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(types.ToPagedList(pageNumber, pageSize));
        }

        // GET: MediaTypes/Details/5
        public async Task<ActionResult> Details(int? id, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TypeTracks details = new TypeTracks();
            details.type = await db.MediaTypes.FindAsync(id);
            details.tracks = details.type.Tracks.OrderBy(t => t.Name).ToPagedList(pageNumber, pageSize);
            if (details == null)
            {
                return HttpNotFound();
            }

            return View(details);
        }

        // GET: MediaTypes/Create
        public ActionResult Create()
        {
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name");
            return View();
        }

        // POST: MediaTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MediaTypeId,Name,MediaCategoryId")] MediaType mediaType)
        {
            if (ModelState.IsValid)
            {
                int count = db.MediaTypes.Where(mt => mt.Name.Equals(mediaType.Name)).Count();
                if ( count > 0) {
                    ViewBag.Duplicate = "Media Type "+mediaType.Name+" already exists.";
                    ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
                    return View(mediaType);
                }
                db.MediaTypes.Add(mediaType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
            return View(mediaType);
        }

        // GET: MediaTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaType mediaType = await db.MediaTypes.FindAsync(id);
            if (mediaType == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
            return View(mediaType);
        }

        // POST: MediaTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(mediaType).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
            //return View(mediaType);

            string[] fieldsToBind = new string[] { "MediaTypeId","Name","MediaCategoryId", "RowVersion" }; //change

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var itemToUpdate = await db.MediaTypes.FindAsync(id); //change here

            if (itemToUpdate == null)
            {
                MediaType deletedItem = new MediaType(); //change here
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
                    var clientValues = (MediaType)entry.Entity; //change here
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else 
                    {
                        var databaseValues = (MediaType)databaseEntry.ToObject(); //change here
                        
                        if (databaseValues.Name != clientValues.Name) //change all
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.MediaCategoryId != clientValues.MediaCategoryId)
                            ModelState.AddModelError("MediaCategoryId", "Current value: "
                                + databaseValues.MediaCategoryId);

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
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", itemToUpdate.MediaCategoryId);
            return View(itemToUpdate);
        }

        // GET: MediaTypes/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaType mediaType = await db.MediaTypes.FindAsync(id);
            if (mediaType == null)
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
            return View(mediaType);
        }

        // POST: MediaTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(MediaType mediaType)
        {
            //MediaType mediaType = await db.MediaTypes.FindAsync(id);
            //db.MediaTypes.Remove(mediaType);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            try
            {
                db.Entry(mediaType).State = EntityState.Deleted; //change this
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = mediaType.MediaTypeId }); //chnage this
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(mediaType);
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
