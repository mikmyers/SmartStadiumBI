using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace GAA.IoT.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static SQLDBHelper _sqlDBHelper;

        public static SQLDBHelper SqlDBHelper
        {
            get
            {
                return _sqlDBHelper;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            _sqlDBHelper = new SQLDBHelper();
        }
    }
}
