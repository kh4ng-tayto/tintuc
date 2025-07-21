using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
        public ActionResult Create(Article article, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        imageFile.SaveAs(path);
                        article.ImageUrl = "/Uploads/" + fileName;
                    }

                    // Gán ngày giờ tạo bài viết
                    article.CreatedDate = DateTime.Now.Date;
                    article.CreatedTime = DateTime.Now.TimeOfDay;

                    db.Articles.Add(article);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    // Ghi lại lỗi xác thực
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    ModelState.AddModelError("", $"Lỗi khi lưu bài viết: {fullErrorMessage}");
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi chung
                    ModelState.AddModelError("", $"Lỗi khi lưu bài viết: {ex.Message}");
                }
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", article.UserId);
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
        public ActionResult Edit(Article model)
        {
            if (!ModelState.IsValid)
            {
                // Nếu model có lỗi, load lại dropdowns và return view
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
                ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", model.UserId);
                return View(model);
            }

            var articleToUpdate = db.Articles.Find(model.Id);
            if (articleToUpdate == null)
            {
                return HttpNotFound();
            }

            // Cập nhật các trường người dùng được chỉnh sửa
            articleToUpdate.Title = model.Title;
            articleToUpdate.Summary = model.Summary;
            articleToUpdate.Content = model.Content;
            articleToUpdate.ImageUrl = model.ImageUrl;
            articleToUpdate.CategoryId = model.CategoryId;
            articleToUpdate.UserId = model.UserId;


            // Gán ngày giờ hiện tại vào trường chỉnh sửa
            articleToUpdate.CreatedDate = DateTime.Now.Date;
            articleToUpdate.CreatedTime = DateTime.Now.TimeOfDay;

            db.SaveChanges();

            return RedirectToAction("Index");
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