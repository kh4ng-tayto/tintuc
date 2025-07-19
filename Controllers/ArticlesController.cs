using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vnexpress.Models;

namespace Vnexpress.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: Articles
        QLtintucEntities2 db = new QLtintucEntities2();
        public ActionResult Index()
        {
            return View(db.Articles.ToList());
        }
        public ActionResult Create()
        {
            var categories = db.Categories.Select(c => new { c.Id, c.Name }).ToList();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            var users = db.Users.Select(u => new { u.Id, u.FullName }).ToList();
            ViewBag.UserId = new SelectList(users, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            var culture = new System.Globalization.CultureInfo("vi-VN");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            var categories = db.Categories.Select(c => new { c.Id, c.Name }).ToList();
            var users = db.Users.Select(u => new { u.Id, u.FullName }).ToList();

            if (ModelState.IsValid)
            {
                article.CreatedDate = DateTime.Now.Date;
                article.CreatedTime = DateTime.Now.TimeOfDay;
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", article.CategoryId);
            ViewBag.UserId = new SelectList(users, "Id", "FullName", article.UserId);
            return View(article);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            var categories = db.Categories.Select(c => new { c.Id, c.Name }).ToList();
            var users = db.Users.Select(u => new { u.Id, u.FullName }).ToList();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", article.CategoryId);
            ViewBag.UserId = new SelectList(users, "Id", "FullName", article.UserId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            var categories = db.Categories.Select(c => new { c.Id, c.Name }).ToList();
            var users = db.Users.Select(u => new { u.Id, u.FullName }).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArticle = db.Articles.AsNoTracking().FirstOrDefault(a => a.Id == article.Id);
                    if (existingArticle != null)
                    {
                        // Giữ nguyên ngày tạo ban đầu
                        article.CreatedDate = existingArticle.CreatedDate;
                        article.CreatedTime = existingArticle.CreatedTime;

                        // Cập nhật ngày giờ chỉnh sửa
                        article.UpdatedDate = DateTime.Now.Date;
                        article.UpdatedTime = DateTime.Now.TimeOfDay;
                    }

                    db.Entry(article).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in error.ValidationErrors)
                        {
                            ModelState.AddModelError("", $"Thuộc tính: {validationError.PropertyName}, Lỗi: {validationError.ErrorMessage}");
                        }
                    }
                    ViewBag.CategoryId = new SelectList(categories, "Id", "Name", article.CategoryId);
                    ViewBag.UserId = new SelectList(users, "Id", "FullName", article.UserId);
                    return View(article);
                }
            }

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", article.CategoryId);
            ViewBag.UserId = new SelectList(users, "Id", "FullName", article.UserId);
            return View(article);
        }
        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}