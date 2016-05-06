using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BGale_WEBD3000_MyTunes.Models;

namespace BGale_WEBD3000_MyTunes.Controllers
{
    public class TracksAPIController : ApiController
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: api/TracksAPI
        public IQueryable<TracksDTO> GetTracks()
        {
            var tracks = from t in db.Tracks
                         select new TracksDTO()
                         {
                             Id = t.TrackId,
                             Name = t.Name,
                         };
            return tracks;
        }

        // GET: api/TracksAPI/5
        [ResponseType(typeof(TrackDetailsDTO))]
        public async Task<IHttpActionResult> GetTrack(int id)
        {
            var track = await db.Tracks.Select(t =>
               new TrackDetailsDTO()
               {
                   Id = t.TrackId,
                   Title = t.Name,
                   Genre = t.Genre.Name,
                   Price = t.UnitPrice,
                   Composer = t.Composer,
                   MediaType = t.MediaType.Name,
                   Milliseconds = t.Milliseconds,
                   Bytes = t.Bytes
               }).SingleOrDefaultAsync(t => t.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(track);
        }

        // PUT: api/TracksAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrack(int id, Track track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != track.TrackId)
            {
                return BadRequest();
            }

            db.Entry(track).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TracksAPI
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> PostTrack(Track track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tracks.Add(track);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = track.TrackId }, track);
        }

        // DELETE: api/TracksAPI/5
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> DeleteTrack(int id)
        {
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            db.Tracks.Remove(track);
            await db.SaveChangesAsync();

            return Ok(track);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrackExists(int id)
        {
            return db.Tracks.Count(e => e.TrackId == id) > 0;
        }
    }
}