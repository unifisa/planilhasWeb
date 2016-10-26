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
using Rotativa;

namespace Planilhas.Controllers
{

    public class Listar_DNController : Controller
    {
        //banco de dados da aplicação
        private OurDbContext db = new OurDbContext();


        // GET: /Listar/
        public ActionResult Index()
        {
            var _d = db.diluicao_normal.Include(d => d.ID).Include(d => d.cliente).Include(d => d.grupo).Include(d => d.cota).Include(d => d.contrato).Include(d => d.usuario).Include(d => d.alteracao);
            return View(db.diluicao_normal.ToList());
        }

        //GetListar()
        public JsonResult GetListar(string sortdatafield, string sortOrder, int pagesize, int pagenum)
        {

            var query = Request.QueryString;
            var dbResult = db.Database.SqlQuery<diluicao_normal>(this.BuildQuery(query));
            var _d = from diluicao_normal in dbResult
                     select new diluicao_normal
                     {
                         ID = diluicao_normal.ID,
                         cliente = diluicao_normal.cliente,
                         cota = diluicao_normal.cota,
                         grupo = diluicao_normal.grupo,
                         contrato = diluicao_normal.contrato,
                         usuario = diluicao_normal.usuario,
                         alteracao = diluicao_normal.alteracao
                     };
            if (sortdatafield != null && sortOrder != "")
            {
                _d = this.SortOrders(_d, sortdatafield, sortOrder);
            }
            var total = dbResult.Count();
            _d = _d.Skip(pagesize * pagenum).Take(pagesize);

            var result = new
            {
                TotalRows = total,
                Rows = _d
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //crescente ou desc
        private IEnumerable<diluicao_normal> SortOrders(IEnumerable<diluicao_normal> collection, string sortField, string sortOrder)
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
            var queryString = @"SELECT * FROM diluicao_normal ";
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

        //filtro do Grid - Serverside
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

        diluicao_normal _d = new diluicao_normal();

        // GET: /Employees/Create
        public ActionResult Create()
        {

            return View(_d);
        }
        //
        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(diluicao_normal _d)
        {

            _d.txt_adm_desc_ad = (_d.tx_adm_total - _d.a_vista - _d.vlr_adesao_divido - _d.vlr_adesao_diluido);
            _d.bem_taxa_no_plano = ((_d.txt_adm_desc_ad + _d.fdo_reserva) / 100) + 1; //correto
            _d.vlr_adesao_a_vista = (_d.vlr_bem * (_d.a_vista / 100)); //correto

            if ((_d.vlr_bem * (_d.vlr_adesao_divido / 100)) / (25) > 0)
            {
                _d.dividido = (_d.vlr_bem * (_d.vlr_adesao_divido / 100)) / (25); //correto
            }
            else
            {
                _d.dividido = 0;
            }

            if ((_d.vlr_bem * (_d.vlr_adesao_diluido / 100)) / (25) > 0)
            { 
                _d.diluido = ((_d.vlr_bem * (_d.vlr_adesao_diluido / 100)) /  (25)); //correto
            }
            else
            {
                _d.diluido = 0;
            }
            _d.porc_por_parc_dil = (_d.vlr_adesao_diluido / 25);//correto
            _d.porc_por_parc_divid = _d.vlr_adesao_divido / 25;//correto
            _d.vlr_bem_mais_tax = (_d.vlr_bem * (((_d.tx_adm_total + _d.fdo_reserva) / 100) + 1)); //correto
            _d.taxa_real = ((_d.txt_adm_desc_ad + _d.fdo_reserva + _d.vlr_adesao_diluido) / 100) + 1;//correto
            _d.valor_real = (_d.vlr_bem * _d.taxa_real);//correto
            _d.vlr_parce_sem_seguro = _d.valor_real / _d.prazo;//correto
            _d.vlr_seguro = (_d.vlr_bem * _d.bem_taxa_no_plano) * ((_d.seguro_percent) / 100);//correto
            _d.parc_sem_ad_dilu = _d.vlr_parce_sem_seguro - _d.diluido + _d.vlr_seguro;//correto
            //_d.parc_inicial = (((_d.vlr_parce_sem_seguro) / _d.bem_taxa_no_plano) / _d.vlr_bem);
            //_d.parcelas_restantes = ((_d.vlr_parce_sem_seguro / _d.bem_taxa_no_plano) / _d.vlr_bem);
            _d.cima_per_sobre_5 = 25; //correto
            _d.per_sobre_5_parcelas_iniciais = (((_d.vlr_parce_sem_seguro - _d.diluido) / _d.bem_taxa_no_plano) / _d.vlr_bem) * 100;//correto
            _d.cima_sobre_restantes = _d.prazo - _d.cima_per_sobre_5;//correto
            _d.per_sobre_parc_restantes = ((_d.vlr_parce_sem_seguro / _d.bem_taxa_no_plano) / _d.vlr_bem) * 100;//correto

            if ((_d.cima_sobre_restantes * _d.per_sobre_parc_restantes) + (_d.per_sobre_5_parcelas_iniciais * _d.cima_per_sobre_5) > 0)//correto
            {
                _d.totalizando = (_d.cima_sobre_restantes * _d.per_sobre_parc_restantes) + (_d.per_sobre_5_parcelas_iniciais * _d.cima_per_sobre_5);
            }
            else
            {
                _d.totalizando = 0;
            }

            _d.vlr_parc_mensal_parc_restantes = _d.vlr_parce_sem_seguro + _d.vlr_seguro;//correto
            _d.vlr_parc_mensal_iniciais = _d.vlr_parc_mensal_parc_restantes + _d.dividido;//correto
            _d.vlr_parcela_inicial_mais_adesao = _d.vlr_parc_mensal_iniciais + _d.vlr_adesao_a_vista ;//correto
            _d.valor_adm = (_d.vlr_bem * _d.tx_adm_total) / 100;//correto
            _d.f_res = (_d.vlr_bem * _d.fdo_reserva) / 100;//correto
            _d.resumo_seguro = _d.vlr_seguro * _d.prazo;//correto
            _d.resumo_total = _d.vlr_bem + _d.valor_adm + _d.f_res + _d.resumo_seguro;//correto

            
            _d.ad_a_vista = _d.vlr_adesao_a_vista;//correto

            _d.parc_inicial = _d.vlr_parc_mensal_iniciais;//correto
            _d.parcelas_restantes = _d.vlr_parce_sem_seguro + _d.vlr_seguro;

            _d.parc_ini_valor1 = _d.cima_per_sobre_5;//correto
            _d.parcelas_restantes_valor1 = _d.cima_sobre_restantes;//correto
            

            _d.ad_a_vista_valor2 = _d.ad_a_vista_valor1 * _d.ad_a_vista;//correto
            _d.parc_ini_valor2 = _d.parc_ini_valor1 * _d.parc_inicial;//correto
            _d.parcelas_restantes_valor2 = _d.parcelas_restantes_valor1 * _d.vlr_parc_mensal_parc_restantes;//correto
            _d.total_quadro = _d.ad_a_vista_valor2 + _d.parc_ini_valor2 + _d.parcelas_restantes_valor2;//correto

            _d.lance_valor = _d.vlr_bem * _d.lance / 100;

            _d.saldo_devedor = _d.total_quadro - _d.parcela_1 - _d.lance_valor;

            _d._qte_meses = _d.prazo - 1;

            _d.valor_qtd_meses = _d.saldo_devedor / _d._qte_meses;

            if (_d.vlr_parc_mensal_parc_restantes > 0)
            {
                _d._j38 = Math.Round(_d.saldo_devedor - _d.vlr_parc_mensal_parc_restantes) / _d.vlr_parc_mensal_parc_restantes;


            }
            else
            {
                _d._j38 = 0;
            }



            _d._l39 = (_d.total_quadro - _d.lance_valor) / _d.prazo;

            _d.alteracao = DateTime.Now;

            _d.bem = _d.vlr_bem;//correto

            _d.usuario = User.Identity.GetUserName();

            try
            {
                db.diluicao_normal.Add(_d);
                db.SaveChanges();
                TempData["Message"] = " Salvo com Sucesso ";
                return View(_d);
            }


            catch (Exception)
            {

            }

            return View(_d);
        }

        // GET: /Orders/Details/5
        public ViewResult Details(int? ID)
        {
            diluicao_normal _d = db.diluicao_normal.Find(ID);
            return View(_d);
        }


        [HttpGet]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return HttpNotFound();
            }
            var _d = db.diluicao_normal.Find(ID);
            if (_d == null)
            {
                return HttpNotFound();
            }
            return View(_d);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID)
        {
            try
            {
                diluicao_normal _d = db.diluicao_normal.Find(ID);
                db.diluicao_normal.Remove(_d);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = ID, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }


        // GET: /Employees/Edit/5
        [HttpGet]
        public ActionResult Edit(int ID)
        {

            diluicao_normal _d = db.diluicao_normal.Find(ID);

            return View(_d);
        }
        //
        // POST: /Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(diluicao_normal _d)
        {

            _d.txt_adm_desc_ad = (_d.tx_adm_total - _d.a_vista - _d.vlr_adesao_divido - _d.vlr_adesao_diluido);
            _d.bem_taxa_no_plano = ((_d.txt_adm_desc_ad + _d.fdo_reserva) / 100) + 1; //correto
            _d.vlr_adesao_a_vista = (_d.vlr_bem * (_d.a_vista / 100)); //correto

            if ((_d.vlr_bem * (_d.vlr_adesao_divido / 100)) / (25) > 0)
            {
                _d.dividido = (_d.vlr_bem * (_d.vlr_adesao_divido / 100)) / (25); //correto
            }
            else
            {
                _d.dividido = 0;
            }

            if ((_d.vlr_bem * (_d.vlr_adesao_diluido / 100)) / (25) > 0)
            {
                _d.diluido = ((_d.vlr_bem * (_d.vlr_adesao_diluido / 100)) / (25)); //correto
            }
            else
            {
                _d.diluido = 0;
            }
            
            _d.porc_por_parc_dil = (_d.vlr_adesao_diluido / 25);//correto
            _d.porc_por_parc_divid = _d.vlr_adesao_divido / 25;//correto
            _d.vlr_bem_mais_tax = (_d.vlr_bem * (((_d.tx_adm_total + _d.fdo_reserva) / 100) + 1)); //correto
            _d.taxa_real = ((_d.txt_adm_desc_ad + _d.fdo_reserva + _d.vlr_adesao_diluido) / 100) + 1;//correto
            _d.valor_real = (_d.vlr_bem * _d.taxa_real);//correto
            _d.vlr_parce_sem_seguro = _d.valor_real / _d.prazo;//correto
            _d.vlr_seguro = (_d.vlr_bem * _d.bem_taxa_no_plano) * ((_d.seguro_percent) / 100);//correto
            _d.parc_sem_ad_dilu = _d.vlr_parce_sem_seguro - _d.diluido + _d.vlr_seguro;//correto
            //_d.parc_inicial = (((_d.vlr_parce_sem_seguro) / _d.bem_taxa_no_plano) / _d.vlr_bem);
            //_d.parcelas_restantes = ((_d.vlr_parce_sem_seguro / _d.bem_taxa_no_plano) / _d.vlr_bem);
            _d.cima_per_sobre_5 = 25; //correto
            _d.per_sobre_5_parcelas_iniciais = (((_d.vlr_parce_sem_seguro - _d.diluido) / _d.bem_taxa_no_plano) / _d.vlr_bem) * 100;//correto
            _d.cima_sobre_restantes = _d.prazo - _d.cima_per_sobre_5;//correto
            _d.per_sobre_parc_restantes = ((_d.vlr_parce_sem_seguro / _d.bem_taxa_no_plano) / _d.vlr_bem) * 100;//correto

            if ((_d.cima_sobre_restantes * _d.per_sobre_parc_restantes) + (_d.per_sobre_5_parcelas_iniciais * _d.cima_per_sobre_5) > 0)//correto
            {
                _d.totalizando = (_d.cima_sobre_restantes * _d.per_sobre_parc_restantes) + (_d.per_sobre_5_parcelas_iniciais * _d.cima_per_sobre_5);
            }
            else
            {
                _d.totalizando = 0;
            }

            _d.vlr_parc_mensal_parc_restantes = _d.vlr_parce_sem_seguro + _d.vlr_seguro;//correto
            _d.vlr_parc_mensal_iniciais = _d.vlr_parc_mensal_parc_restantes + _d.dividido;//correto
            _d.vlr_parcela_inicial_mais_adesao = _d.vlr_parc_mensal_iniciais + _d.vlr_adesao_a_vista;//correto
            _d.valor_adm = (_d.vlr_bem * _d.tx_adm_total) / 100;//correto
            _d.f_res = (_d.vlr_bem * _d.fdo_reserva) / 100;//correto
            _d.resumo_seguro = _d.vlr_seguro * _d.prazo;//correto
            _d.resumo_total = _d.vlr_bem + _d.valor_adm + _d.f_res + _d.resumo_seguro;//correto


            _d.ad_a_vista = _d.vlr_adesao_a_vista;//correto

            _d.parc_inicial = _d.vlr_parc_mensal_iniciais;//correto
            _d.parcelas_restantes = _d.vlr_parce_sem_seguro + _d.vlr_seguro;

            _d.parc_ini_valor1 = _d.cima_per_sobre_5;//correto
            _d.parcelas_restantes_valor1 = _d.cima_sobre_restantes;//correto


            _d.ad_a_vista_valor2 = _d.ad_a_vista_valor1 * _d.ad_a_vista;//correto
            _d.parc_ini_valor2 = _d.parc_ini_valor1 * _d.parc_inicial;//correto
            _d.parcelas_restantes_valor2 = _d.parcelas_restantes_valor1 * _d.vlr_parc_mensal_parc_restantes;//correto
            _d.total_quadro = _d.ad_a_vista_valor2 + _d.parc_ini_valor2 + _d.parcelas_restantes_valor2;//correto

            _d.lance_valor = _d.vlr_bem * _d.lance / 100;

            _d.saldo_devedor = _d.total_quadro - _d.parcela_1 - _d.lance_valor;

            _d._qte_meses = _d.prazo - 1;

            _d.valor_qtd_meses = _d.saldo_devedor / _d._qte_meses;

            if (_d.vlr_parc_mensal_parc_restantes > 0)
            {
                _d._j38 = Math.Round(_d.saldo_devedor - _d.vlr_parc_mensal_parc_restantes) / _d.vlr_parc_mensal_parc_restantes;


            }
            else
            {
                _d._j38 = 0;
            }



            _d._l39 = (_d.total_quadro - _d.lance_valor) / _d.prazo;

            _d.alteracao = DateTime.Now;

            _d.bem = _d.vlr_bem;//correto

            _d.usuario = User.Identity.GetUserName();

            try
            {

                db.Entry(_d).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = " Alterações Salvas ";
                return View(_d);
            }

            catch (Exception)
            {

            }


            return View(_d);

        }

        public ActionResult PrintIndex(int? ID)
        {
            diluicao_normal _d = db.diluicao_normal.Find(ID);
            return new ViewAsPdf(_d);
        }

        [HttpPost]
        public JsonResult AutocompleteCliente(string term)
        {

            var result = (from r in db.diluicao_normal
                          where r.cliente.ToLower().StartsWith(term.ToLower())
                          select new { r.cliente }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}




