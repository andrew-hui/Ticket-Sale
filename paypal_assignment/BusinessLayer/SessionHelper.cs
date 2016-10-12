using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace paypal_assignment.BusinessLayer
{
    public class SessionHelper
    {
        public const string SESSION_START = "Session_Start";
        public const string SESSION_END = "Session_End";

        // Get data stored under the current session.
        // This data is stored on the server in a collection.
        public DateTime Start
        {
            get
            {
                try
                {
                    return (DateTime)HttpContext.Current.Session[SESSION_START];
                }
                catch
                {
                    Initialize();
                }
                return (DateTime)HttpContext.Current.Session[SESSION_START];
            }
        }
        public DateTime End
        {
            get { return (DateTime)HttpContext.Current.Session[SESSION_END]; }
        }

        // Return value from session cookie manually if the session does not exist.
        public string SessionID
        {
            get
            {
                if (HttpContext.Current.Session.SessionID != null)
                    return HttpContext.Current.Session.SessionID;
                return null;
            }
        }
        public void Initialize()
        {
            HttpContext.Current.Session[SESSION_START] = DateTime.Now;
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(1);
        }
        public void UpdateSession()
        {
            if (SessionID == null)
                Initialize();
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(1);
        }
        public void Clear()
        {
            if (SessionID != null)
            {
                HttpContext.Current.Session.Clear(); // remove stored items
                HttpContext.Current.Session.Abandon();
            }
        }

    }
}