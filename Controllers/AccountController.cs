using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vnexpress.Models;

namespace Vnexpress.Controllers
{
    public class AccountController : Controller
    {
        QLtintucEntities2 db = new QLtintucEntities2();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
            if (user != null)
            {
                // Lưu vào Session nếu bạn vẫn muốn
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;

                // Tạo Cookie lưu tên đăng nhập (có thể thêm ID nếu muốn)
                HttpCookie cookie = new HttpCookie("Username", user.Username);
                cookie.Expires = DateTime.Now.AddDays(7); // tồn tại trong 7 ngày
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Articles");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();

            if (Request.Cookies["Username"] != null)
            {
                var cookie = new HttpCookie("Username");
                cookie.Expires = DateTime.Now.AddDays(-1); // hết hạn
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }
    }
}