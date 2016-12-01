//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Planilhas.Security
//{
//    public class PermissoesFiltro : AuthorizeAttribute
//    {

//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            base.OnAuthorization(filterContext);

//            if(filterContext.Result is HttpUnauthorizedResult)
//            {
//                filterContext.HttpContext.Response.Redirect("/Account/Semacesso");
//            }
//        }
//    }
//}