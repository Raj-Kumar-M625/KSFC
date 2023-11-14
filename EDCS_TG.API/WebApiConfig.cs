//using System.Web.Http;
//using System.Web.Http.Cors;

//namespace EDCS_TG.API
//{
//    public class WebApiConfig
//    {
//        public static void Register(HttpConfiguration config)
//        {
//            // New code
//            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
//            config.EnableCors(cors);
//            config.MapHttpAttributeRoutes();    

//            config.Routes.MapHttpRoute(
//                name: "DefaultApi",
//                routeTemplate: "api/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//            );
//        }
//    }
//}
