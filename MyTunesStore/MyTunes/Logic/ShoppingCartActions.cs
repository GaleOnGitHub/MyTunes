using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTunes.Models;

namespace MyTunes.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }

        private MyTunesContext _db = new MyTunesContext();

        public const string CartSessionKey = "CartId";

        public void AddToCart(int id)
        {
            // Retrieve the Track from the database.           
            ShoppingCartId = GetCartId();

            var cartItem = _db.CartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.TrackId == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.                 
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    TrackId = id,
                    CartId = ShoppingCartId,
                    Track = _db.Tracks.SingleOrDefault(
                   p => p.TrackId == id),
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public string GetCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();

            return _db.CartItems.Where(
                c => c.CartId == ShoppingCartId).ToList();
        }

        public decimal GetTotal()
        {
            ShoppingCartId = GetCartId();
            // Multiply product price by quantity of that product to get        
            // the current price for each of those products in the cart.  
            // Sum all product price totals to get the cart total.   
            decimal? total = decimal.Zero;
            IQueryable <CartItem> query = _db.CartItems.Where(c => c.CartId == ShoppingCartId);

            foreach (CartItem item in query)
                total += item.Track.UnitPrice;

            //total = (decimal?)(from cartItems in _db.CartItems
            //                   where cartItems.CartId == ShoppingCartId
            //                   select (int?)cartItems.Track.UnitPrice).Sum();
            return total ?? decimal.Zero;
        }

        public int GetCount()
        {
            ShoppingCartId = GetCartId();

            IQueryable<CartItem> query = _db.CartItems.Where(c => c.CartId == ShoppingCartId);
            int? count = query.Count();
            // Get the count of each item in the cart and sum them up          
            //int? count = (from cartItems in _db.CartItems
            //              where cartItems.CartId == ShoppingCartId
            //              select ).Count();
            // Return 0 if all entries are null         
            return count ?? 0;
        }


        public ShoppingCartActions GetCart(HttpContext context)
        {
            using (var cart = new ShoppingCartActions())
            {
                cart.ShoppingCartId = cart.GetCartId();
                return cart;
            }
        }

        public void UpdateShoppingCartDatabase(String cartId, ShoppingCartUpdates[] CartItemUpdates)
        {
            using (var db = new MyTunes.Models.MyTunesContext())
            {
                try
                {
                    int CartItemCount = CartItemUpdates.Count();
                    List<CartItem> myCart = GetCartItems();
                    foreach (var cartItem in myCart)
                    {
                        // Iterate through all rows within shopping cart list
                        for (int i = 0; i < CartItemCount; i++)
                        {
                            if (cartItem.Track.TrackId == CartItemUpdates[i].TrackId)
                            {
                                if (CartItemUpdates[i].RemoveItem == true)
                                {
                                    RemoveItem(cartId, cartItem.TrackId);
                                }

                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Database - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeTrackID)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                try
                {
                    var myItem = (from c in _db.CartItems where c.CartId == removeCartID && c.Track.TrackId == removeTrackID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        // Remove Item.
                        _db.CartItems.Remove(myItem);
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void UpdateItem(string updateCartID, int updateTrackID, int quantity)
        {
            using (var _db = new MyTunes.Models.MyTunesContext())
            {
                try
                {
                    var myItem = (from c in _db.CartItems where c.CartId == updateCartID && c.Track.TrackId == updateTrackID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void EmptyCart()
        {
            ShoppingCartId = GetCartId();
            var cartItems = _db.CartItems.Where(
                c => c.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                _db.CartItems.Remove(cartItem);
            }
            // Save changes.             
            _db.SaveChanges();
        }

        public struct ShoppingCartUpdates
        {
            public int TrackId;
            public bool RemoveItem;
        }

        public void MigrateCart(string cartId, string userName)
        {
            var shoppingCart = _db.CartItems.Where(c => c.CartId == cartId);
            foreach (CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }
            HttpContext.Current.Session[CartSessionKey] = userName;
            _db.SaveChanges();
        }
    }
}