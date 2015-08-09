using System.Web.Mvc;
using System.Web.Http;

namespace Web.Areas.Mongo
{
    public class MongoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mongo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // WebApi Routes Mapping
            context.Routes.MapHttpRoute(
                name: "Mongo_WebApiRoute",
                routeTemplate: "Mongo/api/Catalog/{action}/{id}",
                defaults: new { controller = "MongoCatalog", id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Mongo_default",
                "Mongo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}