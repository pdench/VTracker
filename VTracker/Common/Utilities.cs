using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTracker.DAL;

namespace VTracker.Common
{
    public class Utilities
    {

        private HttpContextBase Context { get; set; }
        private TrackerContext db = new TrackerContext();

        public Utilities (HttpContextBase context)
        {
            this.Context = context;
        }

        public Utilities()
        {

        }

        public string GetUserId()
        {
            string userId = this.Context.Session["userId"] as string;
            return userId;
        }

        public int GetGasCatId(string acctId)
        {
            // retrieves the Id from the category table for this user's gas purchases

            var categories = db.Categories.Where(c => c.AccountId == acctId && c.Description == "Gas");
            int gasId = categories.First().Id;

            return gasId;
        }

    }
}