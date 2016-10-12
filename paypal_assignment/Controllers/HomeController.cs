using paypal_assignment.BusinessLayer;
using paypal_assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace paypal_assignment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionHelper sessionHelper = new SessionHelper();
            if (Request.Cookies["ASP.NET_SessionId"] == null)
            {
                sessionHelper.Initialize();
            }
            else
            {
                sessionHelper.UpdateSession();
            }

            ViewBag.SessionID = sessionHelper.SessionID;
            return View();
        }

        public ActionResult PayPal()
        {
            Paypal_IPN paypalResponse = new Paypal_IPN("test");

            if (paypalResponse.TXN_ID != null)
            {
                paypalEntities context = new paypalEntities();
                IPN ipn = new IPN();
                ipn.transactionID = paypalResponse.TXN_ID;
                decimal amount = Convert.ToDecimal(paypalResponse.PaymentGross);
                int quantity = (int)Convert.ToInt64(paypalResponse.Quantity);
                ipn.amount = amount;
                ipn.quantity = quantity;
                ipn.firstname = paypalResponse.PayerFirstName;
                ipn.lastname = paypalResponse.PayerLastName;
                ipn.buyerEmail = paypalResponse.PayerEmail;
                ipn.txTime = DateTime.Now;
                ipn.custom = paypalResponse.Custom;
                ipn.paymentStatus = paypalResponse.PaymentStatus;
                context.IPNs.Add(ipn);
                context.SaveChanges();
            }
            return View();
        }

        public ActionResult Attendees()
        {
            paypalEntities context = new paypalEntities();
            return View(context.IPNs);
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}