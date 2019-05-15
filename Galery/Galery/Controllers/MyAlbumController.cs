using Galery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Galery.Controllers
{
    public class MyAlbumController : Controller
    {
        private GaleryContext db = new GaleryContext();
        // GET: MyAlbum
        public ActionResult EditAlbum()
        {
            Album my_album= null;
            if(Session["IdA"]!=null)
            {
                int my_id = Convert.ToInt32(Session["IdA"].ToString());
                my_album = db.Albums.Where(x => x.Id == my_id).First();
            }

            return View(my_album);
        }

        [HttpPost]
        public ActionResult Edit(int uid,string uname)
        {
            var i =  db.Photos.Where(x => x.Id == uid);

            if(i.Count()>0)
            {
                db.Photos.Where(x => x.Id == uid).First().Name = uname;
                db.SaveChanges();
            }

            return RedirectToAction("EditAlbum", "MyAlbum");
        }

        [HttpPost]
        public ActionResult Delete(int uid)
        {
            var i = db.Photos.Where(x => x.Id == uid);

            if (i.Count() > 0)
            {
                var i2 = i.First();
                i.First().Album.Photos.Remove(i2);
                db.Photos.Remove(db.Photos.Where(x => x.Id == uid).First());
            
                db.SaveChanges();
            }

            return RedirectToAction("EditAlbum", "MyAlbum");
        }
    }
}