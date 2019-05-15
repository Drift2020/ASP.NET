using Music2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Music2.Code;

namespace Music2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private Music_my_db db = new Music_my_db();

        #region AdminPanel

        [HttpPost]
        public ActionResult isActive(int uid)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "0")
            {

                for (int i =0; i<db.Users.Count();i++)
                {
                    if(db.Users.ToList()[i].Id== uid)
                    {
                        db.Users.ToList()[i].isActive = !db.Users.ToList()[i].isActive;
                        db.SaveChanges();
                    }
                }

                var temp_user = new UserModel() { users = db.Users.ToList() };


                
                 return RedirectToAction("AdminPanel");
               
            }
            else
            {

                return RedirectToAction("Index");
            }
           
        }

        [HttpPost]
        public ActionResult Delete(int uid)
        {
            if (Session["Status"]!= null && Session["Status"].ToString() == "0")
            {

                for (int i = 0; i < db.Users.Count(); i++)
                {
                    if (db.Users.ToList()[i].Id == uid)
                    {
                        for(int i1 = 0; i1 < db.Pwd_salt.Count(); i1++)
                        {

                            if (db.Pwd_salt.ToList()[i1].User.Id== uid)
                            {
                                db.Pwd_salt.Remove(db.Pwd_salt.ToList()[i1]);
                                db.Users.Remove(db.Users.ToList()[i]);


                                db.SaveChanges();
                            }
                           
                        }
                       
                    }
                }

                var temp_user = new UserModel() { users = db.Users.ToList() };



                return RedirectToAction("AdminPanel");

            }
            else
            {

                return RedirectToAction("Index");
            }

        }





        public ActionResult AdminPanel()
        {
            if (Session["Status"]!= null && Session["Status"].ToString() == "0")
            {


                var temp_user = new UserModel() { users=db.Users.ToList()};

    
            return View(temp_user);
            }
            else
            {

                return RedirectToAction("Index");
            }
        }


        #endregion AdminPanel


        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexEnd()
        {
            Session["Status"] = null;
            Session["Login"] = null;
            return RedirectToAction("Index", "Home");
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login!");
                    return View(logon);
                }
                var users = db.Users.Where(a => a.Login == logon.Login && a.isActive == true);

                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login!");
                    return View(logon);
                }
               
                var user = users.First();
                var i1 = db.Pwd_salt.ToList();
                var pwds = db.Pwd_salt.Where(a => a.User.Id == user.Id);
                Pwd_salt pwd = pwds.First();

                string salt = pwd.Salt;

                //переводим пароль в байт-массив  
               byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = CSP.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Pwd != hash.ToString())
                {
                   ModelState.AddModelError("", "Wrong password!");
                    return View(logon);
                }
                Session["Login"] = user.Login;
                Session["Status"] = user.Status;
                return RedirectToAction("Page", "Home");
            }
            return View(logon);
        }
        #endregion Index

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                Pwd_salt pwd_salt = new Pwd_salt();
                user.Login = reg.Login;

                byte[] saltbuf = new byte[16];

                // Реализует криптографический генератор случайных чисел, используя реализацию, предоставляемую поставщиком служб шифрования (CSP). 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект для получения средств шифрования  
                MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = CSP.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                pwd_salt.Salt = salt;
                
                user.Pwd= hash.ToString();
                user.Status = 1;
                user.isActive = false;

                db.Users.Add(user);
                db.SaveChanges();
                pwd_salt.User = db.Users.ToList()[db.Users.ToList().Count - 1];
                db.Pwd_salt.Add(pwd_salt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reg);
        }
        #endregion Register

        #region Page
      



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pages(IEnumerable<HttpPostedFileBase> fileUpload, int[] bettingOfficeIDs)
        {

        

            PageModel temp_model = new PageModel();
            int count = 0;
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string filename = Path.GetFileName(file.FileName);
                string tempfolder = Server.MapPath("~/Music");
                if (filename != null)
                {
                    file.SaveAs(Path.Combine(tempfolder, filename));
                    var temp = new Sound();
                    temp.Name = filename; 
                    
                    db.Sounds.Add(temp);
                    db.SaveChanges();
                    List<Sound_gener> temp_s_g_list = new List<Sound_gener>();
                    if (bettingOfficeIDs != null)
                    {
                        foreach (var i in bettingOfficeIDs)
                        {
                            Sound_gener temp_s_g = new Sound_gener();
                            var temp1 = db.Genres.Where(x => x.Id == i);
                            if (temp1.ToList().Count > 0)
                            {
                                temp_s_g.Sound = db.Sounds.Where(e => e.Name == temp.Name).First();
                                temp_s_g.Id_sound = temp_s_g.Sound.Id;

                                temp_s_g.Genre = temp1.First();
                                temp_s_g.Id_gener = temp1.First().Id;

                                temp_s_g_list.Add(temp_s_g);
                            }




                        }

                        temp_s_g_list.ForEach(x => db.Sound_gener.Add(x));
                    }
                   

                    db.SaveChanges();
                    count++;
                }
            }
            ViewBag.Text = "Количество загруженных файлов: " + count;
            return RedirectToAction("Page");
        }

    
        public ActionResult Download_sound(string name)
        {

            Response.ContentType = "text";
            Response.AppendHeader("Content-Disposition", "attachment; filename="+ name);
            Response.TransmitFile(Server.MapPath("~/Music/"+ name));
            Response.End();
            return RedirectToAction("Page");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page(string path)
        {


            PageModel temp_model = new PageModel();
            temp_model.my_Genre = db.Genres.ToList();
            temp_model.my_Sound = db.Sounds.ToList();



            return View(temp_model);
        }

        [HttpGet]
        public ActionResult Page()
        {

            if (Session["Status"] != null)
            {

                PageModel temp_model = new PageModel();
                temp_model.my_Genre = db.Genres.ToList();
                temp_model.my_Sound = db.Sounds.ToList();

                string[] findFiles = System.IO.Directory.GetFiles(Server.MapPath("~/Music/"));

                bool del = true;
                foreach (var i in temp_model.my_Sound)
                {
                    foreach (string file in findFiles)
                    {
                        if (i.Name == file.Split('\\')[file.Split('\\').Length-1])
                        {
                            del = false;
                        }
                    }

                    if (del)
                    {
                        var temp = db.Sound_gener.Where(x => x.Id_sound == i.Id);
                        var temp1 = db.Sounds.Where(x => x.Id == i.Id);

                        if (temp.ToList().Count > 0)
                        {
                            temp.ToList().ForEach(x => db.Sound_gener.Remove(x));
                           
                           
                        }
                        if (temp1.ToList().Count > 0)
                        {
                            var temp_elem1 = temp1.First();
                            db.Sounds.Remove(temp_elem1);
                        }




                        db.SaveChanges();
                    }
                    del = true;
                }
                temp_model.my_Sound = db.Sounds.ToList();
                db.SaveChanges();

                return View(temp_model);
            }
            else
            {

                return RedirectToAction("Index");
            }
        }
        #endregion Page

        #region AddGener
        public ActionResult AddGener()
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "0")
            {
                return View();
            }
            else
            {

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGener(GenreModel gener)
        {
            if (ModelState.IsValid)// Значение, указывающее наличие ошибок в объекте состояния модели
            {

                var elements = db.Genres.Where(a => a.Name == gener.Name);
                if (elements.ToList().Count == 0)
                {
                    Genre temp_elem = new Genre();
                    temp_elem.Name = gener.Name;

                    db.Genres.Add(temp_elem);

                    db.SaveChanges();
                }
                return RedirectToAction("AddGener");
            }

            return View(gener);
        }
        #endregion AddGener

        #region EditGenre


        public ActionResult EditGenre()
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "0")
            {
                var temp_genres = new GenresModel() { genre = db.Genres.ToList() };

                return View(temp_genres);
            }
        
            else
            {

                return RedirectToAction("Index");
    }

}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int uid, string uname)
        {

            var temp = db.Genres.Where(x => x.Id == uid);

            if(temp.ToList().Count>0)
            {
                var temp_elem = temp.First();
                temp_elem.Name = uname;
                db.SaveChanges();
            }

           
            var temp_genres = new GenresModel() { genre = db.Genres.ToList() };

            return RedirectToAction("EditGenre");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGenre(int uid)
        {

            var temp = db.Genres.Where(x => x.Id == uid);

            if (temp.ToList().Count > 0)
            {
                var temp_elem = temp.First();

                var tempGS = db.Sound_gener.Where(x => x.Id_gener == uid);

                if (tempGS.ToList().Count > 0)
                {
                    var temp_elem2 = tempGS.First();

                    db.Sound_gener.Remove(temp_elem2);
                   
                }
                db.Genres.Remove(temp_elem);

                db.SaveChanges();
            }


            var temp_genres = new GenresModel() { genre = db.Genres.ToList() };

            return RedirectToAction("EditGenre");
        }

        #endregion EditGenre


        #region EditSound

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_genre_in_sound(int uid,int[] bettingOfficeIDs)
        {
            if(bettingOfficeIDs!=null)
            {
                var uelem = db.Sounds.Where(x => x.Id == uid).First();
                uelem.Sound_gener.ToList().ForEach(x => { db.Sound_gener.Remove(x); });
                
                foreach(var i in bettingOfficeIDs)
                {
                    var temp = new Sound_gener()
                    {
                        Id_sound = uelem.Id,
                        Id_gener = db.Genres.Where(x => x.Id == i).First().Id,
                        Sound = uelem,
                        Genre = db.Genres.Where(x => x.Id == i).First()
                    };
                    uelem.Sound_gener.Add(temp);
                    
                }
             
            }
            else
            {
                var uelem = db.Sounds.Where(x => x.Id == uid).First();
                uelem.Sound_gener.ToList().ForEach(x => { db.Sound_gener.Remove(x); });

            }
            db.SaveChanges();
            return RedirectToAction("EditMusic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMusicGenre(int uid)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "0")
            {
                EditSoundGenreModel elem = new EditSoundGenreModel();
                var temps = db.Sounds.Where(x => x.Id == uid).First();
                elem.sound = temps;
                elem.genre = db.Genres.ToList();

                return View(elem);
            }
            else
            {

                return RedirectToAction("Index");
            }
        }

        public ActionResult EditMusic()
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "0")
            {
                var temp_music = new SoundModel() { genre = db.Genres.ToList(), sound = db.Sounds.ToList(), sound_gener = db.Sound_gener.ToList() };

            return View(temp_music);
        }
            else
            {

                return RedirectToAction("Index");
    }
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSound(int uid)
        {


            string tempfolder = Server.MapPath("~/Music");


            return RedirectToAction("EditMusic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSound(int uid)
        {

            var temp = db.Sound_gener.Where(x => x.Id_sound == uid);
            var temp1 = db.Sounds.Where(x => x.Id == uid);

            if (temp.ToList().Count > 0)
            {
                temp.ToList().ForEach(x => db.Sound_gener.Remove(x));
            }
            if (temp1.ToList().Count > 0)
            {
                var temp_elem1 = temp1.First();
                string tempfolder = Server.MapPath("~/Music");
                System.IO.File.Delete(tempfolder +"\\"+ temp_elem1.Name);

                

                db.Sounds.Remove(temp_elem1);


            }




            db.SaveChanges();
            return RedirectToAction("EditMusic");
        }

        #endregion EditSound
    }
}