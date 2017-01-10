using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTracker.Common;
using VTracker.ViewModels;

namespace VTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "Email,Comment")] ContactViewModel contact)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string apiKey = ConfigurationManager.AppSettings["sendgridkey"];
                    dynamic sg = new SendGridAPIClient(apiKey);

                    Utilities util = new Utilities();
                    Email from = new Email(contact.Email);
                    string subject = "Contact from VTracker web site";
                    string toEmail = @"pfdench@gmail.com";
                    Email to = new Email(toEmail);
                    Content content = new Content("text/html", contact.Comment);
                    Mail mail = new Mail(from, subject, to, content);

                    dynamic response = sg.client.mail.send.post(requestBody: mail.Get());

                    ViewBag.Message = "Your message was sent.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Comment cannot be sent.");
            }
            return View(contact);
        }
    }
}