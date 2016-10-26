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
    public class Listar_CRController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: /Listar/
        public ActionResult Index()
        {
            var _calc = db.Calc_Rateio.Include(d => d.ID).Include(d => d.cliente).Include(d => d.grupo).Include(d => d.cota).Include(d => d.contrato).Include(d => d.usuario).Include(d => d.alteracao);
            return View(db.Calc_Rateio.ToList());
        }

        public JsonResult GetListar(string sortdatafield, string sortOrder, int pagesize, int pagenum)
        {
            var query = Request.QueryString;
            var dbResult = db.Database.SqlQuery<Calc_Rateio>(this.BuildQuery(query));
            var _calc = from Calc_Rateio in dbResult
                        select new Calc_Rateio
                        {
                            ID = Calc_Rateio.ID,
                            cliente = Calc_Rateio.cliente,
                            cota = Calc_Rateio.cota,
                            grupo = Calc_Rateio.grupo,
                            contrato = Calc_Rateio.contrato,
                            usuario = Calc_Rateio.usuario,
                            alteracao = Calc_Rateio.alteracao


                        };
            if (sortdatafield != null && sortOrder != "")
            {
                _calc = this.SortOrders(_calc, sortdatafield, sortOrder);
            }
            var total = dbResult.Count();
            _calc = _calc.Skip(pagesize * pagenum).Take(pagesize);

            var result = new
            {
                TotalRows = total,
                Rows = _calc
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<Calc_Rateio> SortOrders(IEnumerable<Calc_Rateio> collection, string sortField, string sortOrder)
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
            var queryString = @"SELECT * FROM Calc_Rateio ";
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
        Calc_Rateio _calc = new Calc_Rateio();


        // GET: Calc_RateiosemAdesao
        public ActionResult Create()
        {
            return View(_calc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calc_Rateio _calc)
        {
            
                    _calc.quitacao = (100 - _calc.porc_paga);
                    _calc.bem_com_txs = _calc.bem * (1 + (_calc.taxa_adm / 100) + (_calc.fundo_reserv / 100));
                    _calc.quit_sem_seg = ((100 - _calc.porc_paga) / 100) * _calc.bem_com_txs;
                    _calc.seguro = _calc.bem_com_txs * (_calc.porc_seguro / 100);
                    _calc.parc_inicial = _calc.assemb_vencidas + 1;
                    _calc.parc_final = _calc.parc_inicial + _calc.parc_faltam - 1;
                    _calc.amortizacao = _calc.quit_sem_seg / _calc.parc_faltam;
                    _calc.em_porcentagem = (_calc.amortizacao / _calc.bem_com_txs) * 100;
                    _calc.total = _calc.amortizacao + _calc.seguro;
                    _calc.div_total = (_calc.parc_final - _calc.parc_inicial + 1) * (_calc.em_porcentagem);
                    _calc.zero_porc = _calc.porc_paga;
                    _calc.total_final = _calc.div_total + _calc.zero_porc;


                    _calc.alteracao = DateTime.Now;
                    _calc.usuario = User.Identity.GetUserName();

            try
            {
                db.Calc_Rateio.Add(_calc);
                db.SaveChanges();
                TempData["Message"] = " Salvo com Sucesso ";
                return View(_calc);
            }

              

        catch (Exception)
            {

            }
               

                return View(_calc);
            }
    


// GET: /Orders/Details/5
public ViewResult Details(int? id)
        {
            Calc_Rateio _calc = db.Calc_Rateio.Find(id);
            return View(_calc);
        }


        [HttpGet]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return HttpNotFound();
            }
            var _calc = db.Calc_Rateio.Find(ID);
            if (_calc == null)
            {
                return HttpNotFound();
            }
            return View(_calc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Calc_Rateio _calc = db.Calc_Rateio.Find(id);
                db.Calc_Rateio.Remove(_calc);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }


        // GET: /Employees/Edit/5
        [HttpGet]
        public ActionResult Edit(int ID)
        {

            Calc_Rateio _calc = db.Calc_Rateio.Find(ID);
            return View(_calc);
        }
        //
        // POST: /Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(Calc_Rateio _calc)
        {
                _calc.bem =_calc.bem;
                _calc.quitacao = (100 - _calc.porc_paga);
                _calc.bem_com_txs = _calc.bem * (1 + (_calc.taxa_adm / 100) + (_calc.fundo_reserv / 100));
                _calc.quit_sem_seg = ((100 - _calc.porc_paga) / 100) * _calc.bem_com_txs;
                _calc.seguro = _calc.bem_com_txs * (_calc.porc_seguro / 100);
                _calc.parc_inicial = _calc.assemb_vencidas + 1;
                _calc.parc_final = _calc.parc_inicial + _calc.parc_faltam - 1;
                _calc.amortizacao = _calc.quit_sem_seg / _calc.parc_faltam;
                _calc.em_porcentagem = (_calc.amortizacao / _calc.bem_com_txs) * 100;
                _calc.total = _calc.amortizacao + _calc.seguro;
                _calc.div_total = (_calc.parc_final - _calc.parc_inicial + 1) * Math.Round(_calc.em_porcentagem);
                _calc.zero_porc = _calc.porc_paga;
                _calc.total_final = _calc.div_total + _calc.zero_porc;


                _calc.alteracao = DateTime.Now;
                _calc.usuario = User.Identity.GetUserName();

                try
                {

                    db.Entry(_calc).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = " Alterações Salvas ";
                    return View(_calc);
                }
                catch (Exception)
                {

                
                }

                return View(_calc);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }

}
