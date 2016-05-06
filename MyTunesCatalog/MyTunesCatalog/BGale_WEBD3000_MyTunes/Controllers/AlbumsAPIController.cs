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
    public class AlbumsAPIController : ApiController
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: api/AlbumsAPI
        public IQueryable<AlbumDTO> GetAlbums()
        {
            var albums = from a in db.Albums
                          select new AlbumDTO()
                          {
                              Id = a.AlbumId,
                              Title = a.Title,
                          };
            return albums;
        }

        // GET: api/AlbumsAPI/5
        [ResponseType(typeof(AlbumDetailsDTO))]
        public async Task<IHttpActionResult> GetAlbum(int id)
        {
            var album = await db.Albums.Include(a => a.Tracks).Select(a =>
               new AlbumDetailsDTO()
               {
                   Id = a.AlbumId,
                   Title = a.Title,
                   Artist = a.Artist.Name,
                   Tracks = a.Tracks.Select(t => new TracksDTO()
                   {
                       Id = t.TrackId,
                       Name = t.Name
                   }).OrderBy(t => t.Name)
               }).OrderBy(a => a.Title).SingleOrDefaultAsync(a => a.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // PUT: api/AlbumsAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            db.Entry(album).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // POST: api/AlbumsAPI
        [ResponseType(typeof(Album))]
        public async Task<IHttpActionResult> PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Albums.Add(album);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumId }, album);
        }

        // DELETE: api/AlbumsAPI/5
        [ResponseType(typeof(Album))]
        public async Task<IHttpActionResult> DeleteAlbum(int id)
        {
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            db.Albums.Remove(album);
            await db.SaveChangesAsync();

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return db.Albums.Count(e => e.AlbumId == id) > 0;
        }
    }
}