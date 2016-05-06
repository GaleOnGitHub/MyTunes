using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTunes.Models;

namespace MyTunes.Logic
{
    public class AddData
    {
        public bool AddTrack(string name, string album, string price, string mediaType, string genre,
            string composer, string milliseconds, string bytes)
        {
            var newTrack = new Track();
            newTrack.Name = name;
            newTrack.AlbumId = Convert.ToInt32(album);
            newTrack.UnitPrice = Convert.ToDecimal(price);
            newTrack.MediaTypeId = Convert.ToInt32(mediaType);
            newTrack.Composer = composer;
            newTrack.Milliseconds = Convert.ToInt32(milliseconds);
            newTrack.Bytes = Convert.ToInt32(bytes);
            newTrack.GenreId = Convert.ToInt32(genre);

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.Tracks.Add(newTrack);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }
        public bool AddGenre(string name)
        {
            var newGenre = new Genre();
            newGenre.Name = name;

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.Genres.Add(newGenre);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

        public bool AddCategory(string name)
        {
            var newCategory = new MediaCategory();
            newCategory.Name = name;

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.MediaCategories.Add(newCategory);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

        public bool AddArtist(string name)
        {
            var newArtist = new Artist();
            newArtist.Name = name;

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.Artists.Add(newArtist);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

        public bool AddAlbum(string Title, string artist)
        {
            var newAlbum = new Album();
            newAlbum.Title = Title;
            newAlbum.ArtistId = Convert.ToInt32(artist);

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.Albums.Add(newAlbum);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

        public bool AddType(string name, string category)
        {
            var newType = new MediaType();
            newType.Name = name;
            newType.MediaCategoryId = Convert.ToInt32(category);

            using (MyTunesContext _db = new MyTunesContext())
            {
                // Add product to DB.
                _db.MediaTypes.Add(newType);
                _db.SaveChanges();
            }
            // Success.
            return true;
        }

    }
}