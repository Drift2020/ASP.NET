using Galery.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Galery.Controllers
{
    public class HomeController : Controller
    {
        private GaleryContext db = new GaleryContext();
        // GET: Home
        #region main
        public ActionResult MainPage()
        {
            MainModel mainModel=new MainModel();

            mainModel = new MainModel();
            mainModel.six_albums = new List<Album>();
            for (int i = 0; i < 7 && i < db.Albums.Count(); i++)
            {
                mainModel.six_albums.Add(db.Albums.ToList()[db.Albums.ToList().Count - 1 - i]);
            }

            return View(mainModel);
        }

        [HttpPost]
        public ActionResult MainPage(MainModel mainModel)
        {
            mainModel = new MainModel();

            for(int i=0;i<7 && i< db.Albums.Count(); i++)
            {
                mainModel.six_albums.Add(db.Albums.ToList()[db.Albums.ToList().Count - 1 - i]);
            }
         



            return View(mainModel);
        }
       
        [HttpPost]
        public ActionResult Exit()
        {
            Session["User"] = "";
            Session["Id"] = "";
            return RedirectToAction("MainPage", "Home");
        }
        #endregion main


        #region outher album
        public ActionResult OthersAlbum()
        {
            OtherAlbumModel otherAlbumModel = new OtherAlbumModel();


            otherAlbumModel.albums = new List<Album>();
            for (int i = 0; i < 5 && i < db.Albums.Count(); i++)
            {
                otherAlbumModel.albums.Add(db.Albums.ToList()[db.Albums.ToList().Count - 1 - i]);
            }

            return View(otherAlbumModel);
        }

        [HttpPost]
        public JsonResult ChoiceCount(OtherAlbumModel rs)
        {
          //  db.Configuration.ProxyCreationEnabled = false;
            rs.albums = new List<Album>();
            if (rs.count != "All")
            {
                int count = int.Parse(rs.count);
                for (int i = 0; i < count && i < db.Albums.Count(); i++)
                {
                    var my_db = db.Albums.ToList()[i];
                    var my_photo = my_db.Photos;

                    var elem = new Album()
                    {
                        Id = my_db.Id,
                        Name = my_db.Name,
                        number = my_db.number,
                        Photos = new List<Photo>(),
                        User = new User() { Id = my_db.User.Id, Login = my_db.User.Login, Password = my_db.User.Password }
                    };

                    foreach (var elem_photo in my_photo)
                    {
                        elem.Photos.Add(new Photo() { Id = elem_photo.Id, Name = elem_photo.Name, Path = elem_photo.Path, UpLoad = elem_photo.UpLoad });
                    }



                    rs.albums.Add(elem);

                }

            }
            else if (rs.count == "All")
            {
                for (int i = 0;  i < db.Albums.Count(); i++)
                {
                    var my_db = db.Albums.ToList()[i];
                    var my_photo = my_db.Photos;

                    var elem = new Album()
                    {
                        Id = my_db.Id,
                        Name = my_db.Name,
                        number = my_db.number,
                        Photos = new List<Photo>(),
                        User = new User() { Id = my_db.User.Id, Login = my_db.User.Login, Password = my_db.User.Password }
                    };

                    foreach (var elem_photo in my_photo)
                    {
                        elem.Photos.Add(new Photo() { Id = elem_photo.Id, Name = elem_photo.Name, Path = elem_photo.Path, UpLoad = elem_photo.UpLoad });
                    }



                    rs.albums.Add(elem);

                }
            }
            // db.Configuration.ProxyCreationEnabled = true;

            // string JSONString = string.Empty;
            // JSONString = JsonConvert.SerializeObject(rs);
            //return Json(JSONString);



  

            return Json(rs, JsonRequestBehavior.AllowGet);

           // string json = JsonConvert.SerializeObject(rs, Formatting.Indented);
           // return Json(json);
        }




        #endregion outher album

        #region my album


        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> fileUpload, string uName)
        {
            if (Session["User"] != null && Session["User"].ToString() != "" && Session["Id"].ToString() != "")
            {
                var temp_album = new Album();
            temp_album.Name = uName;

            Photo temp_photo = null;

            if (!Directory.Exists(Server.MapPath("~/image"))) // "!" забыл поставить
            {
                Directory.CreateDirectory(Server.MapPath("~/image"));
            }

            foreach (var file in fileUpload)
            {
              

                if (file == null) continue;
                string filename = Path.GetFileName(file.FileName);
                string tempfolder = Server.MapPath("~/image");

                if (filename != null)
                {

                    file.SaveAs(Path.Combine(tempfolder, filename));
                    temp_photo = new Photo();
                    temp_photo.UpLoad = DateTime.Now;
                    temp_photo.Album = temp_album;
                    temp_photo.Name = filename;
                    temp_photo.Path = "/image/" + filename;


                }
                if (temp_photo != null)
                {
                    db.Photos.Add(temp_photo);
                    temp_album.Photos.Add(temp_photo);
                }
            }
            var number = Convert.ToInt32(Session["Id"].ToString());
            temp_album.User = db.Users.Where(x => x.Id == number).First();
         
            db.Albums.Add(temp_album);
            db.SaveChanges();

            return RedirectToAction("MyAlbum", "Home");
            }
            return RedirectToAction("MainPage", "Home");
        }

        [HttpPost]
        public ActionResult Delete(int uid)
        {
            var temp = db.Albums.Where(x => x.Id == uid).First();



           

            for (int i = 0; i < temp.Photos.Count; )
            {
               
                temp.Photos.ToList()[i].Album = null;
                temp.Photos.Remove(temp.Photos.ToList()[i]);
               
            }

            temp.User.Albums.Remove(temp);
            temp.User = null;
            


            db.Albums.Remove(temp);

            db.SaveChanges();
            return RedirectToAction("MyAlbum", "Home");
        }

        [HttpPost]
        public ActionResult Edit(int uid)
        {
            Session["IdA"] = uid;
            return RedirectToAction("EditAlbum", "MyAlbum");
        }

        public ActionResult MyAlbum()
        {
            if (Session["User"] != null && Session["User"].ToString() != "" && Session["Id"].ToString() != "")
            {
                MyAlbumModel temp = new MyAlbumModel();
                var number = Convert.ToInt32(Session["Id"].ToString());
                temp.my_albums = (db.Users.Where(x => x.Id == number).First().Albums.ToList());

                return View(temp);
            }
            return RedirectToAction("MainPage", "Home");
        }
        #endregion my album


        #region view album
        public ActionResult ViewAlbum()
        {
            ViewAlbumModel temp = new ViewAlbumModel();

            return View(temp);
        }

        [HttpPost]
        public ActionResult ViewAlbum(int uid)
        {
            ViewAlbumModel temp = new ViewAlbumModel();
            temp.id = uid;
            temp.number = 0;
            temp.src = db.Albums.Where(x => x.Id == uid).First().Photos.ToList()[temp.number].Path;

            return View(temp);
        }
       
        [HttpPost]
        public JsonResult Next(ViewAlbumModel rs)
        {
            if (db.Albums.Where(x => x.Id == rs.id).First().number == -1)
            {
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number = 0;
                
            }
            else  if (db.Albums.Where(x => x.Id == rs.id).First().number  < db.Albums.Where(x => x.Id == rs.id).First().Photos.Count-1)
            {
                db.Albums.Where(x => x.Id == rs.id).First().number++;
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number;

            }
             
            else
            {
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number=0;
            }
            db.SaveChanges();
            rs.src = db.Albums.Where(x => x.Id == rs.id).First().Photos.ToList()[rs.number].Path;

            return Json(rs);            
        }

        [HttpPost]
        public JsonResult Back(ViewAlbumModel rs)
        {
            if (db.Albums.Where(x => x.Id == rs.id).First().number == -1)
            {
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number = 0;
               
            }
            else if (db.Albums.Where(x => x.Id == rs.id).First().number >0)
            {
                db.Albums.Where(x => x.Id == rs.id).First().number--;
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number;

            }
             
            else
            {
                rs.number = db.Albums.Where(x => x.Id == rs.id).First().number = db.Albums.Where(x => x.Id == rs.id).First().Photos.Count-1;
            }
            db.SaveChanges();
            rs.src = db.Albums.Where(x => x.Id == rs.id).First().Photos.ToList()[rs.number].Path;
            

            return Json(rs);
        }

        public class SlideshoModel
        {
            public List<string> src { set; get; }
            public int number { set; get; }
        }

        [HttpPost]
        public JsonResult Slidesho(ViewAlbumModel rs)
        {
            SlideshoModel temp = new SlideshoModel() { src=new List<string>(), number=0 };

            if (db.Albums.Where(x => x.Id == rs.id).First().number != -1)
            {
                db.Albums.Where(x => x.Id == rs.id).First().number = -1;
                db.Albums.Where(x => x.Id == rs.id).First().Photos.ToList().ForEach(
                x =>
                {
                 temp.src.Add(x.Path);
                }
                );
                db.SaveChanges();
                return Json(temp);
            }
            else
            {
                db.Albums.Where(x => x.Id == rs.id).First().number = 0;
                temp.number = -1;
                db.SaveChanges();
                return Json(temp);
            }

        }

    }

        #endregion view album
}

