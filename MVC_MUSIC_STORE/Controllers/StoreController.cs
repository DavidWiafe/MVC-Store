using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_MUSIC_STORE.Models;

namespace MVC_MUSIC_STORE.Controllers
{
    public class StoreController : Controller
    {
        //connect to DB
        MusicStoreModel db = new MusicStoreModel();
        // GET: Store
        public ActionResult Index()
        {
            var genres = db.Genres.OrderBy(o => o.Name).ToList();
            return View(genres);
        }

        public ActionResult Product(String ProductName) {

            ViewBag.ProductName = ProductName;
            return View();
        }

        public ActionResult Albums(String genre) {

            var albums = db.Albums.Where(a => a.Genre.Name == genre).OrderBy(a => a.Title).ToList();
            ViewBag.genre = genre;
            return View(albums);
           
        }

        // GET: Store/AddToCart
        public ActionResult AddToCart(int AlbumId) {

            GetCartId();
            string CurrentCartId = Session["CartId"].ToString();
            //string CurrentCartId ="Monday-1-Cart-D1";

            // check if the ablum is already in cart
            Cart cartItem = db.Carts.SingleOrDefault(c => c.CartId == CurrentCartId 
            && c.AlbumId == AlbumId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = AlbumId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    CartId = CurrentCartId
                };
                db.Carts.Add(cartItem);
        
            }
            else
            {
                cartItem.Count++;
            }
            //save
            //db.Carts.Add(cartItem);
            db.SaveChanges();

            return RedirectToAction("ShoppingCart");


        }

        private void GetCartId() {

            //is there already is a cart id
            if (Session["CartId"] == null) {

                if (User.Identity.Name == "")
                {

                    //generate unique id that is session-Specfic
                    Session["CartId"] = Guid.NewGuid().ToString();


                }
                else
                {
                    Session["CartId"] = User.Identity.Name;

                }
            }

        }

        // GET: Store/ShoppingCart
        public ActionResult ShoppingCart() {

            //Get current cart for display
            GetCartId();
            string CurrentCardId = Session["CartId"].ToString();
            var cartItems = db.Carts.Where(c => c.CartId == CurrentCardId);

            // load view pass it the list of items in the users cart
            return View(cartItems);
        }

        // GET: Store/RemvoeFromeCart
        public ActionResult RemvoeFromeCart(int RecordId) {


            // remvoe item from This user's cart
            Cart cartItem = db.Carts.SingleOrDefault(c => c.RecordId == RecordId);
            db.Carts.Remove(cartItem);
            db.SaveChanges();

            return RedirectToAction("removeFromCart");

        }
        // GET: Store/CheckOut
        [Authorize]
        public ActionResult CheckOut() {

            MigrateCart();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // POST: Store/Checkout
        public ActionResult Checkout(FormCollection values)
        {
            // create a new order and populate from the form values
            Order order = new Order();
            TryUpdateModel(order);

            // populate the 4 automatic order fields
            order.Username = User.Identity.Name;
            order.Email = User.Identity.Name;
            order.OrderDate = DateTime.Now;

            var cartItems = db.Carts.Where(c => c.CartId == order.Username);
            decimal CartTotal = (from c in cartItems
                                 select (int)c.Count * c.Album.Price).Sum();

            order.Total = CartTotal;

            // save the order
            db.Orders.Add(order);
            db.SaveChanges();

            // save the items
            foreach (Cart item in cartItems)
            {
                OrderDetail od = new OrderDetail
                {
                    OrderId = order.OrderId,
                    AlbumId = item.AlbumId,
                    Quantity = item.Count,
                    UnitPrice = item.Album.Price
                };

                db.OrderDetails.Add(od);
            }

            db.SaveChanges();
            return RedirectToAction("Details", "Orders", new { id = order.OrderId });
        }

        public ViewResult Graphs() {

            int one = 100;

            int sales = 100;

            int expenses = 2000;

            int year = 2008;

            ViewBag.Message_one = sales;

            int[] values = { 2008, 900, 320 };

            return View(values);
        }


        public void MigrateCart() {

            if (!String.IsNullOrEmpty(Session["CartId"].ToString()) && User.Identity.IsAuthenticated) {
                //IF THE USER HAS BEEN SHOPPING ANONYMOUSLY, now attach their user name
                if (Session["CartId"].ToString() != User.Identity.Name) {

                    string CurrentCartID = Session["CartId"].ToString();
                    //get items with random id
                    var cartItems = db.Carts.Where(c => c.CartId == CurrentCartID);

                    foreach (Cart item in cartItems) {
                        item.CartId = User.Identity.Name;
                    }

                    db.SaveChanges();
                    Session["CartId"] = User.Identity.Name;
                }
            }
        }
    }
}