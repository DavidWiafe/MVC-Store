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
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(String ProductName) {



            ViewBag.ProductName = ProductName;
            return View();
        }

        /*public ActionResult Albums() {

            var album = new List<Album>();
            /*for (int i = 1; i <= 10; i++) {
                album.add( new Album { Title = "Album" + i.ToString() });
            }

            return View(ablum);
        }*/

    }
}