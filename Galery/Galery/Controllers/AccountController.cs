using Galery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Galery.Controllers
{
    public class AccountController : Controller
    {
        private GaleryContext db = new GaleryContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login!");
                    return View(logon);
                }
                var users = db.Users.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login!");
                    return View(logon);
                }
                var user = users.First();
                string salt = user.Salt;

                //    переводим пароль в байт-массив
                    byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //    создаем объект для получения средств шифрования
                   MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

                //    вычисляем хеш-представление в байтах
                   byte[] byteHash = CSP.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong password!");
                    return View(logon);
                }
                Session["User"] = user.Login;
                Session["Id"] = user.Id;
                return RedirectToAction("MainPage", "Home");
            }
            return View(logon);
        }

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

                user.Password = hash.ToString();
                user.Salt = salt;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(reg);
        }
        #endregion Register
    }
}