﻿using System;
using System.Linq;
using System.Web.Services;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Planilhas.Models;
using System.Web.Security;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebMatrix.WebData;

namespace Planilhas.Controllers
{


    public class AccountController : Controller
    {
       private OurDbContext db = new OurDbContext();

        public AccountController()
        {
            db = new OurDbContext();
        }

        //Get: Account
        [Authorize(Users = "admin")]
        public ActionResult Index()
        {
            using (OurDbContext db = new OurDbContext())
            {
                return View(db.userAccount.ToList());
            }
        }


        //pagina de cadastro get os valores
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            var Depart = db.Departamentos.ToList();
            SelectList list = new SelectList(Depart, "Departamento", "Departamento");
            ViewBag.DepartamentoNome = list;
            return View();            
        }

        //pagina de Cadastro
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                var Depart = db.Departamentos.ToList();
                SelectList list = new SelectList(Depart, "Departamento", "Departamento");
                ViewBag.DepartamentoNome = list;

                using (OurDbContext db = new OurDbContext())
                {
                    var usr = db.userAccount.Where(u => u.Usuario == account.Usuario).FirstOrDefault();
                    if (usr != null)
                    {
                        ModelState.Clear();
                        ViewBag.ResultMessage = " Este usuário está indisponivel, tente outro nome de usuário! ";
                        return View();
                    }
                   
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                TempData["Message"] = " Usuário cadastrado com Sucesso ";
            }
            else
            {

                TempData["Msg"] = " Usuário não foi cadastrado. Preencha todos os campos. ";
            }

            return RedirectToAction("Register", "Account");

        }

        //Login
        public ActionResult Login()
        {
            if (!Request.IsAuthenticated)
            {

                return View();
            }
            return RedirectToAction("Logado", "Account");
        }

        //login Post
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (OurDbContext db = new OurDbContext())
            {

                var usr = db.userAccount.Where(u => u.Usuario == user.Usuario && u.Senha == user.Senha).FirstOrDefault();
                if (usr != null)
                {

                    Session["ColaboradorId"] = usr.ColaboradorId;
                    Session["Usuario"] = usr.Usuario.ToString();
                    FormsAuthentication.SetAuthCookie(usr.Usuario.ToString(), false);
                    if (Session["ColaboradorID"] != null)
                    {


                        return RedirectToAction("Logado");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuário ou senha estão errados");
                }
            }
            return View();
        }

        //pagina Logado

        public ActionResult Logado()
        {


            if (User.Identity.IsAuthenticated)
            {

                return View();
            }

            else
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login");
            }
        }

        //Sign out
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }


        public ActionResult Semacesso()
        {
            return View();
        }


        //ROLES

        //cria novas regras --GET
        [Authorize(Roles = "Admin")]
        public ActionResult RoleCreate()
        {
            return View();
        }

        //cria novas regras --POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(Role r)
        {
            using (OurDbContext db = new OurDbContext()) //usa o model OurDbContext para fazer a conexao com o banco de dados
            {
                Role rol = (from u in db.Roles
                            where u.RoleName == r.RoleName
                            select u).SingleOrDefault();
                if(rol != null)
                {
                    ViewBag.ResultMessage = "Este grupo já existe";
                    return View();
                }
             
                try {

                    db.Roles.Add(r);
                    db.SaveChanges();

                    return RedirectToAction("RoleIndex", "Account");

                }
                catch(Exception)
                {
                    if(Request.Form["RoleName"] == "") { 
                    ViewBag.ResultMessage = "Não deixe o campo vazio!";
                    }
                    
                    return View();
                }
              
            }
           
        }

        //lista todas as regras
        [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            Role r = new Role();
            var allRoles = db.Roles.ToList();         
            return View(allRoles);
       
        }

        //Excluir regra
        [Authorize(Roles = "Admin")]
        public ActionResult RoleDelete(string RoleName)
        {
            Role rol = db.Roles.Find(RoleName);
            db.Roles.Remove(rol);
            db.SaveChanges();


            return RedirectToAction("RoleIndex", "Account");
        }

        //Adiciona regra a determinado usuario
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            var Depart = db.Roles.ToList();
            SelectList list = new SelectList(Depart, "RoleName", "RoleName");
            ViewBag.NomeRegra = list;

            return View();
        }

         //adiciona regra ao usuario     
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(UserRole adiciona)
        {
            //verifica se o campo está vazio
            if ((Request.Form["UserName"]) == "" || (Request.Form["RoleName"]) == "")
            {
                ViewBag.ResultMessage = "Não deixe o campo vazio!";
                return View();
            }
            //verifica se o usuario ja esta no grupo escolhido
            var verifica = (from v in db.UserRoles
                            where (v.Usuario == adiciona.Usuario) && (v.RoleName == adiciona.RoleName)
                            select v).FirstOrDefault();

            //lista as regras direto do banco de dados
            var Depart = db.Roles.ToList();
            SelectList list = new SelectList(Depart, "RoleName", "RoleName");
            ViewBag.NomeRegra = list;

            //adiciona no banco e salva
            if(verifica == null)
            {
                db.UserRoles.Add(adiciona);
                db.SaveChanges();

                ViewBag.ResultMessage = "Regra adicionada com sucesso ao usuário !";
                return View();

            }
            else
            {
                ViewBag.ResultMessage = "Este usuário já está nesse grupo!";
            }
            return View();
            
        }

        /// <summary>
        /// Get all the roles for a particular user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]    //autoriza somente quem for Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string Usuario)
        {
            //lista as regras direto do banco de dados
            var Depart = db.Roles.ToList();
            SelectList list = new SelectList(Depart, "RoleName", "RoleName");
            ViewBag.NomeRegra = list;

            //seleciona o nome da regra e joga em um array
            var ur = (from u in db.UserRoles
                     where u.Usuario == Usuario
                     select u.RoleName).ToArray();
            ViewBag.RolesForThisUser = ur;
            
            return View("RoleAddToUser");
        }



        //deletar usuario de uma regra
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string Username, string RoleName)
        {
            var Depart = db.Roles.ToList();
            SelectList list = new SelectList(Depart, "RoleName", "RoleName");
            ViewBag.NomeRegra = list;

            var del = (from d in db.UserRoles
                      where (d.Usuario == Username) && (d.RoleName == RoleName)
                      select d).SingleOrDefault();

            if(del != null)
            {            
            db.UserRoles.Remove(del);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.ResultMessage = "Usuário excluido com sucesso deste grupo!";
            }
            else
            {
                ViewBag.ResultMessage = "Este usuário não pertence a este grupo!";
            }

            return View("RoleAddToUser");
        }

        //da sugestoes de usuarios de acordo com as letras digitadas, autocompleta
        [HttpPost]
        public JsonResult AutocompletenoClick(string term)
        {

            var result = from r in db.userAccount
                          where r.Usuario == term
                          select new { r.Nome, r.Sobrenome };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //quando o usuario muda de campo, preenche automaticamente o departamento dele - busca no banco de dados
        [HttpPost]
        public JsonResult BindDepartamento(string term)
        {

            var result = from r in db.userAccount
                         where r.Usuario == term
                         select new { r.Departamentos };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //da sugestoes de usuarios de acordo com as letras digitadas, autocompleta
        [HttpPost]
        public JsonResult Autocomplete(string term)
        {

            var result = (from r in db.userAccount
                          where r.Usuario.ToLower().StartsWith(term.ToLower())
                          select new { r.Usuario }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //preenche o nome automaticamente de acordo com o usuario escolhido
        [HttpPost]
        public JsonResult Autocomplete2(string term)
        {

            var result = (from r in db.userAccount
                          where r.Nome.ToLower().StartsWith(term.ToLower())
                          select new { r.Nome, r.Sobrenome }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        //checa se esse nome está disponivel no banco de dados - Cadastro de usuários na view Register
        [AllowAnonymous]
        public string ChecarUsuario(string input)
        {
           
            var ifuser = (from u in db.userAccount
                        where u.Usuario == input
                        select u).SingleOrDefault();
     
                if (ifuser == null)
            {
                return "Disponivel";
            }
            if (ifuser != null)
            {
                return "Indisponivel";
            }

            return "";
        }

    }
}