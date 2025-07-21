using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vnexpress.Models;

namespace Vnexpress.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        QLtintucEntities2 db = new QLtintucEntities2();
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }
        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = db.Comments.Find(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArticleId,Name,Email,Content,CreatedAt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = db.Comments.Find(id);
            if (comment == null)
                return HttpNotFound();

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
            return View(comment);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArticleId,Name,Email,Content,CreatedAt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = db.Comments.Find(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    
}
}