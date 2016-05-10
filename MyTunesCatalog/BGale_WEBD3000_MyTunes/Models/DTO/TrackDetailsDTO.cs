using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGale_WEBD3000_MyTunes.Models
{ 
    public class TrackDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Composer { get; set; }
        public string MediaType { get; set; }
        public int Milliseconds { get; set; }
        public int? Bytes { get; set; }
    }
}