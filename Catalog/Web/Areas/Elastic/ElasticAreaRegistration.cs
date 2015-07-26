using System.Web.Mvc;
using System.Web.Http;

namespace Web.Areas.Elastic
{
    public class ElasticAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Elastic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.Routes.MapHttpRoute(
                name: "Elastic_WebApiRoute",
                routeTemplate: "Elastic/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Elastic_default",
                "Elastic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}