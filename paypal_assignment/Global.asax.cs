using paypal_assignment.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace paypal_assignment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Session_Start(object sender, EventArgs e)
        {
            SessionHelper sessionHelper = new SessionHelper();
            sessionHelper.Initialize();
        }
        void Session_End(object sender, EventArgs e)
        {
            int x = 5;
        }
    }
}
