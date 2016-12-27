using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VTracker.Common;
using VTracker.DAL;
using VTracker.Models;
using VTracker.Extensions;
using VTracker.ViewModels;

namespace VehicleTracker.Controllers
{
    [SessionExpires]
    public class ActivityController : Controller
    {
        private TrackerContext db = new TrackerContext();
        private string accountId = "";
        private Utilities util;

        // GET: Activity
        [Authorize]
        public ActionResult Index(int? page, int? vehid)
        {
            accountId = GetUserId();
            int gasId = GetGasCatId(accountId);

            var activities = db.Activities.Include(a => a.Category).Include(a => a.Vehicle);

            var gasOutput = activities.OrderByDescending(a=> a.VehicleId).ThenBy(a => a.ActivityDate).ThenBy(a => a.CategoryId)
                .Where(a => a.Vehicle.AccountId == accountId && a.CategoryId == gasId);

            var nonGasOutput = activities.OrderByDescending(a => a.ActivityDate).ThenBy(a => a.CategoryId)
                .Where(a => a.Vehicle.AccountId == accountId && a.CategoryId != gasId);

            if (vehid != null)
            {
                gasOutput = gasOutput.Where(a => a.VehicleId == vehid);
                nonGasOutput = nonGasOutput.Where(a => a.VehicleId == vehid);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ActivityViewModel avModel = new ActivityViewModel();
            avModel.GasPurchases = gasOutput.ToList();
            avModel.NonGasPurchases = nonGasOutput.ToList();

            decimal totalMiles = 0;
            decimal totalGallons = 0;

            foreach (Activity a in avModel.GasPurchases)
            {
                if (a.Gallons > 0)
                {
                    a.MPG = a.Miles / a.Gallons;
                    totalMiles += a.Miles;
                    totalGallons += a.Gallons;
                }
            }

            


            return View(avModel);
        }

        // GET: Activity/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            if (activity.Vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(activity);
        }

        // GET: Activity/Create
        [Authorize]
        public ActionResult Create()
        {

            var categories = GetCategories();
            var vehicles = GetVehicles();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Description");
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "VehicleName");

            var model = new Activity();
            model.ActivityDate = DateTime.Now;
            //model.Vehicle.AccountId = accountId;
            return View(model);
        }

        // GET: Activity/Create
        [Authorize]
        public ActionResult GetGas()
        {

            var categories = GetCategories();
            var vehicles = GetVehicles();

            Utilities util = new Utilities();
            int gasId = util.GetGasCatId(accountId);

            //ViewBag.CategoryId = new SelectList(categories, "Id", "Description");
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "VehicleName");
            ViewBag.CategoryId = new SelectList(categories, "Id", "Description", gasId);

            var model = new Activity();
            model.ActivityDate = DateTime.Now;
            //model.Vehicle.AccountId = accountId;
            return View("Create", model);
        }

        // POST: Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "VehicleId,CategoryId,ActivityDate,Mileage,Miles,Gallons,Description,Comments")] Activity activity)
        {

            activity.DateCreated = DateTime.Now;
            activity.Deleted = false;
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Activities.Add(activity);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            var categories = GetCategories();
            var vehicles = GetVehicles();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Description", activity.CategoryId);
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "VehicleName", activity.VehicleId);
            return View(activity);
        }

        // GET: Activity/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            var categories = GetCategories();
            var vehicles = GetVehicles();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Description", activity.CategoryId);
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "VehicleName", activity.VehicleId);
            return View(activity);
        }

        // POST: Activity/Edit/5
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

            var activity = db.Activities.Find(id);
            if (TryUpdateModel(activity, "", new string[] { "VehicleId", "CategoryId", "ActivityDate", "Mileage", "Miles", "Gallons", "Description", "Comments"}))
            {
                try
                {
                    activity.DateUpdated = DateTime.Now;
                    db.SaveChanges();
                    var categories = GetCategories();
                    var vehicles = GetVehicles();

                    ViewBag.CategoryId = new SelectList(categories, "Id", "Description", activity.CategoryId);
                    ViewBag.VehicleId = new SelectList(vehicles, "Id", "VehicleName", activity.VehicleId);
                    return RedirectToAction("Index");
                }
                catch (DataException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }          
            return View(activity);
        }

        // GET: Activity/Delete/5
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
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            if (activity.Vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                Activity activity = db.Activities.Find(id);
                db.Activities.Remove(activity);
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

        private IQueryable<Category> GetCategories()
        {

            accountId = GetUserId();
            var categories = from c in db.Categories
                             select c;
            categories = categories.Where(c => c.AccountId == accountId);
            return categories;

        }

        private IQueryable<Vehicle> GetVehicles()
        {

            accountId = GetUserId();
            var vehicles = from v in db.Vehicles
                           select v;
            vehicles = vehicles.Where(v => v.AccountId == accountId);
            return vehicles;

        }

        private string GetUserId()
        {
            util = new Utilities(this.HttpContext);
            accountId = util.GetUserId();
            return accountId;
        }

        private int GetGasCatId (string acctId)
        {
            // retrieves the Id from the category table for this user's gas purchases

            util = new Utilities();
            return util.GetGasCatId(acctId);

        }
    }
}
