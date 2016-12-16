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
    public class RequisicaoController : Controller
    {      

        private OurDbContext db = new OurDbContext();

        // GET: /Listar/
        public ActionResult Index()
        {
            

            var req = db.Requisicao.Include(d => d.ID).Include(d => d.ano_despesa).Include(d => d.mes_despesa).Include(d => d.categoria_despesa).Include(d => d.empresa1).Include(d => d.usuario).Include(d => d.unidade).Include(d => d.alteracao);
            return View(db.Requisicao.ToList());           

        }

        //pega os dados do banco de dados e retorna na view Index
        //obs: o grid esta no modo virtual 
        //qualquer duvida acessar www.jqwidgets.com e ver a documentação para entender melhor
        public JsonResult GetListar(string sortdatafield, string sortOrder, int pagesize, int pagenum)
        {
            var query = Request.QueryString;
            var dbResult = db.Database.SqlQuery<Requisicao>(this.BuildQuery(query));
            var req = from Requisicao in dbResult
                      select new Requisicao
                      {
                          ID = Requisicao.ID,
                          ano_despesa = Requisicao.ano_despesa,
                          mes_despesa = Requisicao.mes_despesa,
                          categoria_despesa = Requisicao.categoria_despesa,
                          empresa1 = Requisicao.empresa1,
                          unidade = Requisicao.unidade,
                          usuario = Requisicao.usuario,
                          alteracao = Requisicao.alteracao


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
        //ordenar crescente e decrescente
        private IEnumerable<Requisicao> SortOrders(IEnumerable<Requisicao> collection, string sortField, string sortOrder)
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


        //seleciona a tabela do banco Requisicaos
        private string BuildQuery(System.Collections.Specialized.NameValueCollection query)
        {
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var queryString = @"SELECT * FROM Requisicaos ";
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

        //filtro do Grid 
        private string GetFilterCondition(string filterCondition, string filterDataField, string filterValue)
        {
            switch (filterCondition)
            {
                case "NOT_EMPTY": //caso nao vazio
                case "NOT_NULL":  //caso nao for nulo
                    return " " + filterDataField + " NOT LIKE '" + "" + "'";
                case "EMPTY":  //caso for vazio
                case "NULL":  //caso for nulo
                    return " " + filterDataField + " LIKE '" + "" + "'";
                case "CONTAINS_CASE_SENSITIVE":  //caso for case sensitive(diferença entre minusculas e maiusculas)
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

        Requisicao req = new Requisicao(); //instancio

        //CRIAR

        // GET - Retorna a view Create para o usuario inserir as informações
        public ActionResult Create()
        {
            var Unidade = db.Unidades.ToList();
            SelectList list = new SelectList(Unidade, "unidades", "unidades");
            ViewBag.UnidadeNome = list;

            return View(req);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Requisicao req) //insere as informações no banco de dados e automoaticamente no Grid(tabela da view Index)
        {
            using (OurDbContext db = new OurDbContext()) //usa o model OurDbContext para fazer a conexao com o banco de dados
            {

                if (req.valor1 == null)
                {
                    req.valor1 = 0;
                }
                if (req.valor2 == null)
                {
                    req.valor2 = 0;
                }

                if (req.valor1 == null)
                {
                    req.valor1 = 0;
                }
                if (req.valor2 == null)
                {
                    req.valor2 = 0;
                }
                //deixando a formatação mais ajeitada para mostrar na tabela
                if (req.categoria_despesa == "conserto_equip")
                {
                    req.categoria_despesa = "Conserto de Equipamentos";
                }
                if (req.categoria_despesa == "internet")
                {
                    req.categoria_despesa = "Internet";
                }
                if (req.categoria_despesa == "telefone")
                {
                    req.categoria_despesa = "Telefone";
                }
                if (req.categoria_despesa == "servicos")
                {
                    req.categoria_despesa = "Servicos";
                }
                if (req.categoria_despesa == "site_hosp")
                {
                    req.categoria_despesa = "Site de Hospedagem";
                }
                if (req.categoria_despesa == "pacote_serv")
                {
                    req.categoria_despesa = "Pacote de Serviços";
                }
                if (req.categoria_despesa == "conserto_equip")
                {
                    req.categoria_despesa = "Conserto de Equipamentos";
                }
                if (req.categoria_despesa == "compra_equip")
                {
                    req.categoria_despesa = "Compra de Equipamentos";
                }


                req.total = (req.valor1 + req.valor2);
                req.alteracao = DateTime.Now;
                req.usuario = User.Identity.GetUserName();

                try
                {
                    db.Requisicao.Add(req);
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");
                }

                catch (Exception)
                {
                    TempData["Msg"] = " Preencha os campos solicitados "; //retorna mensagem de erro nos campos obrigatórios
                    return View(req);
                }
            }
        }

       
        public ViewResult Details(int? id) //quando o usuario clica no botao Ver
        {
            Requisicao req = db.Requisicao.Find(id);
            return View(req);
        }


        //EXCLUIR

        [HttpGet]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return HttpNotFound();
            }
            var req = db.Requisicao.Find(ID);
            if (req == null)
            {
                return HttpNotFound();
            }
            return View(req);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDeniedAuthorize(Users = "Anderson")] //nega acesso a todos menos aos usuarios entre aspas
        public ActionResult Delete(int id)
        {
            try
            {
                Requisicao req = db.Requisicao.Find(id); //acha a tabela Requisicao no banco de dados
                db.Requisicao.Remove(req); //remove o registro
                db.SaveChanges();  //salva as alterações
            }
            catch
            {

            }
            return RedirectToAction("Index"); //redireciona para a view Index
        }


        //EDITAR

        [HttpGet]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Edit(int ID)
        {
            Requisicao req = db.Requisicao.Find(ID);
            return View(req);
        }

        [HttpPost]
        [AccessDeniedAuthorize(Users = "Anderson")]
        public ActionResult Edit(Requisicao req)
        {
            if (req.valor1 == null)
            {
                req.valor1 = 0;
            }
            if (req.valor2 == null)
            {
                req.valor2 = 0;
            }
            req.alteracao = DateTime.Now;  //pega a data atual
            req.usuario = User.Identity.GetUserName();  //pega o nome do Usuario
            req.total = (req.valor1 + req.valor2);
            try
            {
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = " Alterações Salvas ";
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
