using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
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

        private async Task configSendGridAsync(IdentityMessage message)
        {
            try
            {

                string apiKey = ConfigurationManager.AppSettings["sendgridkey"];
                dynamic sg = new SendGridAPIClient(apiKey);

                Email from = new Email("vtracker@pauldench.com");
                String subject = message.Subject;
                Email to = new Email(message.Destination);
                Content content = new Content("text/html", message.Body);
                Mail mail = new Mail(from, subject, to, content);

                dynamic response = sg.client.mail.send.post(requestBody: mail.Get());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}