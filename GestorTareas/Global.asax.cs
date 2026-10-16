using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GestorTareas
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
