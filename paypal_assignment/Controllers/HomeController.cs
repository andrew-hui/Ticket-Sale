using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using paypal_assignment.BusinessLayer;
using paypal_assignment.Models;
using paypal_assignment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName,
                                                             login.Password);

            if (ModelState.IsValid)
            {
                if (identityUser != null)
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext().Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);
                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    return RedirectToAction("Attendees", "Home");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUser newUser)
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            string captchaResponse = captchaHelper.CheckRecaptcha();
            if (captchaResponse != "Valid")
            {
                ViewBag.CaptchaResponse = "Please complete the reCAPTCHA.";
            }
            else
            {
                var userStore = new UserStore<IdentityUser>();
                var manager = new UserManager<IdentityUser>(userStore);
                var identityUser = new IdentityUser()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email
                };
                IdentityResult result = manager.Create(identityUser, newUser.Password);

                if (result.Succeeded)
                {
                    var authenticationManager
                         = HttpContext.Request.GetOwinContext().Authentication;
                    var userIdentity = manager.CreateIdentity(identityUser,
                                               DefaultAuthenticationTypes.ApplicationCookie);
                    //authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    ViewBag.Success = "<b>Success!</b> Your account has been created.";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Home");
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
        [Authorize]
        public ActionResult Attendees()
        {
            paypalEntities context = new paypalEntities();
            return View(context.IPNs);
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        public ActionResult Cancel()
        {
            return View();
        }
    }
}