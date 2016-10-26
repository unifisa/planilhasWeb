using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planilhas.Models;
using System.Net;
using System.Data.Entity;

namespace Planilhas.Controllers
{
    public class ListaPlanilhasController : Controller
    {

        //GET ListaPlanilhas
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult ListaDiretoria()
        {
            return View();
        }

        public ActionResult ListaAtendimento()
        {
            return View();
        }

        public ActionResult ListaImovel()
        {
            return View();
        }
        public ActionResult ListaRh()
        {
            return View();
        }

        public ActionResult ListaTi()
        {
            return View();
        }

        public ActionResult ListaAuditoria()
        {
            return View();

        }
        public ActionResult ListaCobranca()
        {
            return View();
        }

        public ActionResult ListaReativacao()
        {
            return View();
        }

        public ActionResult ListaOperacoes()
        {
            return View();
        }

        [AccessDeniedAuthorize(Roles = "Contabilidade")]
        public ActionResult ListaContabilidade()
        {
            return View();
        }

        public ActionResult ListaCompras()
        {
            return View();
        }

        public ActionResult ListaFinanceiro()
        {
            return View();
        }

        public ActionResult ListaCadastro()
        {
            return View();
        }

        public ActionResult ListaJuridico()
        {
            return View();
        }


        public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                base.OnAuthorization(filterContext);

                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectResult("~/Account/Semacesso");
                }
            }
        }

    }
}