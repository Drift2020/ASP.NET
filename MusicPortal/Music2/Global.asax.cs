using Music2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Music2
{
    public class MvcApplication : System.Web.HttpApplication
    {
       
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
          //  Database.SetInitializer(new MusicDbInitializer());
     
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
