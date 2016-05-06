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
using System.Web.Http.Cors;

namespace BGale_WEBD3000_MyTunes.Controllers
{
    public class ArtistsAPIController : ApiController
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: api/ArtistsAPI
        [EnableCors("*", "*", "*")]
        public IQueryable<ArtistDTO> GetArtists(int? page)
        {
            int pageNumber = (page ?? 0);
            if (pageNumber < 0) {
                pageNumber = 0;
            }
            var artists = (from a in db.Artists
                        select new ArtistDTO()
                        {
                            Id = a.ArtistId,
                            Name = a.Name,
                        }).OrderBy(a => a.Name).Skip(pageNumber * 10).Take(10);
            
            return artists;
        }

        // GET: api/ArtistsAPI/5
        [ResponseType(typeof(ArtistDTO))]
        public async Task<IHttpActionResult> GetArtist(int id)
        {
            //Artist artist = await db.Artists.FindAsync(id);
            //if (artist == null)
            //{
            //    return NotFound();
            //}

            //return Ok(artist);
            var artist = await db.Artists.Include(a => a.Albums).Select(a =>
                new ArtistDetailsDTO()
                {
                    Id = a.ArtistId,
                    Name = a.Name,
                    Albums = a.Albums.Select(alb => new AlbumDTO() {
                        Id = alb.AlbumId,
                        Title = alb.Title
                    }).OrderBy(alb => alb.Title)
                }).OrderBy(a => a.Name).SingleOrDefaultAsync(a=> a.Id == id);
                        if (artist == null)
                        {
                            return NotFound();
                        }

            return Ok(artist);
        }

        // PUT: api/ArtistsAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArtist(int id, Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            db.Entry(artist).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/ArtistsAPI
        [ResponseType(typeof(Artist))]
        public async Task<IHttpActionResult> PostArtist(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Artists.Add(artist);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = artist.ArtistId }, artist);
        }

        // DELETE: api/ArtistsAPI/5
        [ResponseType(typeof(Artist))]
        public async Task<IHttpActionResult> DeleteArtist(int id)
        {
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            db.Artists.Remove(artist);
            await db.SaveChangesAsync();

            return Ok(artist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(int id)
        {
            return db.Artists.Count(e => e.ArtistId == id) > 0;
        }
    }
}