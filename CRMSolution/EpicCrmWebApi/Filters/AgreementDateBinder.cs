//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using System.Web.Http.Controllers;
//using System.Web.Http.ModelBinding;
//using System.Web.Mvc;

//namespace EpicCrmWebApi
//{
//    //https://weblogs.asp.net/melvynharbour/mvc-modelbinder-and-localization
//    // https://greatrexpectations.com/2013/01/10/custom-date-formats-and-the-mvc-model-binder
//    public class PassbookDateBinder : System.Web.Mvc.IModelBinder
//    {
//        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
//        {
//            var v = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

//            string theDate = controllerContext.HttpContext.Request.Params["PassBookReceivedDate"];
//            //string theDate = HttpContext.Current.Request.QueryString["PassBookReceivedDate"];
//            DateTime dt = new DateTime();
//            bool success = DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out dt);
//            if (success)
//            {
//                return dt;
//            }
//            else
//            {
//                return DateTime.MinValue;
//            }
//        }
        

//        //public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
//        //{
//        //    string theDate = HttpContext.Current.Request.QueryString["PassBookReceivedDate"];
//        //    DateTime dt = new DateTime();
//        //    bool success = DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out dt);
//        //    if (success)
//        //    {
//        //        return new ModelBinderResult(dt);
//        //    }
//        //    else
//        //    {
//        //        // Return an appropriate default
//        //    }
           
//        //}
//    }
//}