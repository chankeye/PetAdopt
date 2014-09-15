using System.Web.Mvc;
using System.Web.Routing;
using PetAdopt.DTO;
using PetAdopt.Logic;

namespace PetAdopt
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            new UserLogic(new Operation { Display = "_guest" }).HasAnyUser();
        }
    }
}
