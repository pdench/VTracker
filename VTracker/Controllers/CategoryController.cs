using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VTracker.DAL;
using VTracker.Models;
using VTracker.Common;
using VTracker.Extensions;
using VTracker.ViewModels;

namespace VehicleTracker.Controllers
{
    [SessionExpires]
    public class CategoryController : Controller
    {
        private TrackerContext db = new TrackerContext();
        private string accountId = "";
        private Utilities util;

        // GET: Category
        [Authorize]
        public ActionResult Index(bool? firstLogin = false)
        {

            //if (firstLogin.GetValueOrDefault())
            //{
            //    cvModel.FirstLogin = true;
            //}
            return View();
        }

        // GET: Category
        [Authorize]
        public JsonResult GetCategories()
        {

            accountId = GetUserId();
            var categories = from c in db.Categories                             
                             select c;

            var output = categories.OrderByDescending(cat => cat.BuiltIn).ThenBy(cat=>cat.Description)
                .Where(cat => cat.AccountId == accountId);

            CategoryViewModel cvModel = new CategoryViewModel();

            cvModel.Categories = output.ToList();

            //categories = categories.Where(c => c.AccountId == accountId );

            return Json(output.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Category/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);

            //make sure the category Id really belongs to the logged-in account
            if (category.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        [Authorize]
        public ActionResult Create()
        {
            var model = new Category();
            model.AccountId = GetUserId();
            return View(model);
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Description,AccountId")] Category category)
        {

            category.DateCreated = DateTime.Now;
            category.BuiltIn = false;
            category.Deleted = false;

            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(category);
        }

        // GET: Category/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            //make sure the category Id really belongs to the logged-in account
            if (category.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (category.BuiltIn == true)
            {
                ModelState.AddModelError("", "Builtin categories may not be modified.");
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = db.Categories.Find(id);
            if (TryUpdateModel(category, "", new string[] { "Description" }))
            {
                try
                {
                    category.DateUpdated = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException ex)
                {
                    ModelState.AddModelError("", "Could not update category.");
                }
            }
            
            return View(category);
        }

        // GET: Category/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.";
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //make sure the category Id really belongs to the logged-in account
            if (category.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (category.BuiltIn == true)
            {
                ModelState.AddModelError("", "Builtin categories may not be deleted.");
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
            }
            catch (DataException ex)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetUserId()
        {
            util = new Utilities(this.HttpContext);
            accountId = util.GetUserId();
            return accountId;
        }
    }
}
