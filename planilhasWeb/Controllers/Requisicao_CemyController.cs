using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Planilhas.Models;
using Microsoft.AspNet.Identity;

namespace Planilhas.Controllers
{
    public class Requisicao_CemyController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: /Listar/
        public ActionResult Index()
        {
            var req = db.Requisicao_Cemy.Include(d => d.ID).Include(d => d.ano_despesa).Include(d => d.mes_despesa).Include(d => d.categoria_despesa).Include(d => d.empresa1).Include(d => d.usuario).Include(d => d.unidade).Include(d => d.alteracao);
            return View(db.Requisicao_Cemy.ToList());
        }

        public JsonResult GetListar(string sortdatafield, string sortOrder, int pagesize, int pagenum)
        {
            var query = Request.QueryString;
            var dbResult = db.Database.SqlQuery<Requisicao_Cemy>(this.BuildQuery(query));
            var req = from Requisicao_Cemy in dbResult
                      select new Requisicao_Cemy
                      {
                          ID = Requisicao_Cemy.ID,
                          ano_despesa = Requisicao_Cemy.ano_despesa,
                          mes_despesa = Requisicao_Cemy.mes_despesa,
                          categoria_despesa = Requisicao_Cemy.categoria_despesa,
                          empresa1 = Requisicao_Cemy.empresa1,
                          unidade = Requisicao_Cemy.unidade,
                          usuario = Requisicao_Cemy.usuario,
                          alteracao = Requisicao_Cemy.alteracao


                      };
            if (sortdatafield != null && sortOrder != "")
            {
                req = this.SortOrders(req, sortdatafield, sortOrder);
            }
            var total = dbResult.Count();
            req = req.Skip(pagesize * pagenum).Take(pagesize);

            var result = new
            {
                TotalRows = total,
                Rows = req
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<Requisicao_Cemy> SortOrders(IEnumerable<Requisicao_Cemy> collection, string sortField, string sortOrder)
        {
            if (sortOrder == "asc")
            {
                collection = collection.OrderBy(c => c.GetType().GetProperty(sortField).GetValue(c, null));
            }
            else
            {
                collection = collection.OrderByDescending(c => c.GetType().GetProperty(sortField).GetValue(c, null));
            }
            return collection;
        }



        private string BuildQuery(System.Collections.Specialized.NameValueCollection query)
        {
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var queryString = @"SELECT * FROM Requisicao_Cemy ";
            var tmpDataField = "";
            var tmpFilterOperator = "";
            var where = "";
            if (filtersCount > 0)
            {
                where = " WHERE (";
            }
            for (var i = 0; i < filtersCount; i += 1)
            {
                var filterValue = query.GetValues("filtervalue" + i)[0];
                var filterCondition = query.GetValues("filtercondition" + i)[0];
                var filterDataField = query.GetValues("filterdatafield" + i)[0];
                var filterOperator = query.GetValues("filteroperator" + i)[0];
                if (tmpDataField == "")
                {
                    tmpDataField = filterDataField;
                }
                else if (tmpDataField != filterDataField)
                {
                    where += ") AND (";
                }
                else if (tmpDataField == filterDataField)
                {
                    if (tmpFilterOperator == "")
                    {
                        where += " AND ";
                    }
                    else
                    {
                        where += " OR ";
                    }
                }
                // build the "WHERE" clause depending on the filter's condition, value and datafield.
                where += this.GetFilterCondition(filterCondition, filterDataField, filterValue);
                if (i == filtersCount - 1)
                {
                    where += ")";
                }
                tmpFilterOperator = filterOperator;
                tmpDataField = filterDataField;
            }
            queryString += where;
            return queryString;
        }

        private string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
        {
            switch (filterCondition)
            {
                case "NOT_EMPTY":
                case "NOT_NULL":
                    return " " + filterDataField + " NOT LIKE '" + "" + "'";
                case "EMPTY":
                case "NULL":
                    return " " + filterDataField + " LIKE '" + "" + "'";
                case "CONTAINS_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS";
                case "CONTAINS":
                    return " " + filterDataField + " LIKE '%" + filterValue + "%'";
                case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "DOES_NOT_CONTAIN":
                    return " " + filterDataField + " NOT LIKE '%" + filterValue + "%'";
                case "EQUAL_CASE_SENSITIVE":
                    return " " + filterDataField + " = '" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "EQUAL":
                    return " " + filterDataField + " = '" + filterValue + "'";
                case "NOT_EQUAL_CASE_SENSITIVE":
                    return " BINARY " + filterDataField + " <> '" + filterValue + "'";
                case "NOT_EQUAL":
                    return " " + filterDataField + " <> '" + filterValue + "'";
                case "GREATER_THAN":
                    return " " + filterDataField + " > '" + filterValue + "'";
                case "LESS_THAN":
                    return " " + filterDataField + " < '" + filterValue + "'";
                case "GREATER_THAN_OR_EQUAL":
                    return " " + filterDataField + " >= '" + filterValue + "'";
                case "LESS_THAN_OR_EQUAL":
                    return " " + filterDataField + " <= '" + filterValue + "'";
                case "STARTS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "STARTS_WITH":
                    return " " + filterDataField + " LIKE '" + filterValue + "%'";
                case "ENDS_WITH_CASE_SENSITIVE":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'" + " COLLATE SQL_Latin1_General_CP1_CS_AS"; ;
                case "ENDS_WITH":
                    return " " + filterDataField + " LIKE '%" + filterValue + "'";
            }
            return "";
        }
        Requisicao_Cemy req = new Requisicao_Cemy();


        // GET
        public ActionResult Create()
        {
            var Unidade = db.Unidades.ToList();
            SelectList list = new SelectList(Unidade, "unidades", "unidades");
            ViewBag.UnidadeNome = list;
            return View(req);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Requisicao_Cemy req)
        {
            using (OurDbContext db = new OurDbContext())
            {

                if (req.valor1 == null)
                {
                    req.valor1 = 0;
                }
                if (req.valor2 == null)
                {
                    req.valor2 = 0;
                }
               
                req.total = (req.valor1 + req.valor2);
                req.alteracao = DateTime.Now;
                req.usuario = User.Identity.GetUserName();

                try
                {
                    db.Requisicao_Cemy.Add(req);
                    db.SaveChanges();
                    ViewData["Message"] = " Salvo com Sucesso ";
                    return RedirectToAction("Index");
                }

                catch (Exception)
                {
                    ViewData["Msg"] = " Preencha os campos solicitados ";
                    return View(req);
                }
            }
        }

        // GET: /Orders/Details/5
        public ViewResult Details(int? id)
        {
            Requisicao_Cemy req = db.Requisicao_Cemy.Find(id);
            return View(req);
        }


        [HttpGet]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return HttpNotFound();
            }
            var req = db.Requisicao_Cemy.Find(ID);
            if (req == null)
            {
                return HttpNotFound();
            }
            return View(req);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Delete(int id)
        {
            try
            {
                Requisicao_Cemy req = db.Requisicao_Cemy.Find(id);
                db.Requisicao_Cemy.Remove(req);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Edit(int ID)
        {
            Requisicao_Cemy req = db.Requisicao_Cemy.Find(ID);
            return View(req);
        }


        [HttpPost]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Edit(Requisicao_Cemy req)
        {
            if (req.valor1 == null)
            {
                req.valor1 = 0;
            }
            if (req.valor2 == null)
            {
                req.valor2 = 0;
            }
           
            req.alteracao = DateTime.Now;
            req.usuario = User.Identity.GetUserName();
            req.total = (req.valor1 + req.valor2);
            try
            {
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
                ViewData["Message"] = " Alterações Salvas ";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(req);
            }

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // classe para negar acesso e redirecionar para pagina Semacesso
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
