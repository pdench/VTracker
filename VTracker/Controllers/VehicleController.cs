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
using VTracker.Extensions;
using VTracker.Models;
using VTracker.ViewModels;

namespace VehicleTracker.Controllers
{
    [SessionExpires]
    public class VehicleController : Controller
    {
        private TrackerContext db = new TrackerContext();
        private string accountId = "";
        private Utilities util;

        // GET: Vehicle
        [Authorize]
        public ActionResult Index()
        {
            accountId = GetUserId();

            var vehicles = from v in db.Vehicles
                           select v;

            vehicles = vehicles.Where(v => v.AccountId == accountId);

            return View(vehicles);

        }

        // GET: Vehicle/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            int gasId = GetGasCatId(accountId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (vehicle == null)
            {
                return HttpNotFound();
            }

            var activities = db.Activities.Include(a => a.Category).Include(a => a.Vehicle);

            var gasOutput = activities.OrderByDescending(a => a.VehicleId).ThenBy(a => a.ActivityDate).ThenBy(a => a.CategoryId)
                .Where(a => a.Vehicle.AccountId == accountId && a.CategoryId == gasId);

            var nonGasOutput = activities.OrderByDescending(a => a.ActivityDate).ThenBy(a => a.CategoryId)
                .Where(a => a.Vehicle.AccountId == accountId && a.CategoryId != gasId);

            VehicleViewModel vModel = new VehicleViewModel();
            vModel.GasPurchases = gasOutput.ToList();
            vModel.NonGasPurchases = nonGasOutput.ToList();
            vModel.VehicleName = vehicle.VehicleName;

            decimal totalMiles = 0;
            decimal totalGallons = 0;

            foreach (Activity a in vModel.GasPurchases)
            {
                if (a.Gallons > 0)
                {
                    a.MPG = a.Miles / a.Gallons;
                    totalMiles += a.Miles;
                    totalGallons += a.Gallons;
                }
            }

            return View(vModel);
            //return View(vehicle);
        }

        // GET: Vehicle/Create
        [Authorize]
        public ActionResult Create(bool? firstLogin)
        {
            var model = new VehicleViewModel();
            model.AccountId = GetUserId();
            if (firstLogin.GetValueOrDefault())
            {
                model.FirstLogin = true;
            }
            
            return View(model);
        }

        // POST: Vehicle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "VehicleName,AccountId,FirstLogin")] Vehicle vehicle)
        {

            vehicle.Deleted = false;
            vehicle.DateCreated = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    if (Request.QueryString["FirstVehicle"] == "True")
                    {
                        return RedirectToAction("Create", "Activity");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
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

            var vehicle = db.Vehicles.Find(id);
            if (vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (TryUpdateModel(vehicle, "", new string[] { "VehicleName" }))
            {
                try
                {
                    vehicle.DateUpdated = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException ex)
                {
                    ModelState.AddModelError("", "Could not update vehicle.");
                }
            }

            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.";
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            if (vehicle.AccountId != GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                Vehicle vehicle = db.Vehicles.Find(id);
                db.Vehicles.Remove(vehicle);
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

        private int GetGasCatId(string acctId)
        {
            // retrieves the Id from the category table for this user's gas purchases

            util = new Utilities();
            return util.GetGasCatId(acctId);

        }
    }
}
