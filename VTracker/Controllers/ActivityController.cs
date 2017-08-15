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
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

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

            var gasOutput = activities.OrderBy(a=> a.Vehicle.VehicleName).ThenByDescending(a => a.ActivityDate).ThenBy(a => a.Category.Description)
                .Where(a => a.Vehicle.AccountId == accountId && a.CategoryId == gasId);

            var nonGasOutput = activities.OrderBy(a => a.Vehicle.VehicleName).ThenByDescending(a => a.ActivityDate).ThenBy(a => a.Category.Description)
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
                    a.MPG = a.Miles.GetValueOrDefault() / a.Gallons.GetValueOrDefault();
                    totalMiles += a.Miles.GetValueOrDefault();
                    totalGallons += a.Gallons.GetValueOrDefault();
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
            model.Mileage = 0;
            model.Miles = 0;
            model.Gallons = 0;
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
        
        // POST: Activity/GetGas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult GetGas([Bind(Include = "VehicleId,CategoryId,ActivityDate,Mileage,Miles,Gallons,Description,Comments")] Activity activity)
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


        // GET: Activity/Download
        [Authorize]
        public HttpResponseMessage Download()
        {

            accountId = GetUserId();
            int gasId = GetGasCatId(accountId);

            var activities = db.Activities.Include(a => a.Category).Include(a => a.Vehicle)
                .Where(a => a.Vehicle.AccountId == accountId)
                .OrderByDescending(a => a.DateCreated);

            HttpResponseMessage output = CreateDownloadCSV(activities.ToList());

            ViewBag.Message = "Download Completed.";
            return output;
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

        public HttpResponseMessage CreateDownloadCSV(List<Activity> input)
        {


            StringBuilder sb = new StringBuilder();

            foreach (Activity activity in input)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    activity.Vehicle.VehicleName,
                    activity.ActivityDate,
                    activity.Category,
                    activity.Gallons,
                    activity.Miles,
                    activity.Mileage,
                    activity.Description,
                    activity.Comments, 
                    Environment.NewLine
                    );
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;

        }
            
    }
}
