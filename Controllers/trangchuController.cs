using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vnexpress.Models;

namespace Vnexpress.Controllers
{
    public class trangchuController : Controller
    {
        // GET: trangchu
        private QLtintucEntities2 db = new QLtintucEntities2();
        public ActionResult trangchu()
        {
            var articles = db.Articles.OrderByDescending(a => a.CreatedDate).ToList(); // Lấy danh sách bài viết mới nhất
            return View(articles); // Trả về View và truyền danh sách bài viết
        }
        public ActionResult thethao()
        {
            return View();
        }
        public ActionResult bongda()
        {
            return View();
        }
        public ActionResult hautruong()
        {
            return View();
        }
        public ActionResult Golf()
        {
            return View();
        }
        public ActionResult congnhe()
        {
            return View();
        }
    }
}