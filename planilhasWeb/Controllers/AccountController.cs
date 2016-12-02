using System;
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

                using (OurDbContext db = new OurDbContext())
                {
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

                    System.Web.HttpContext.Current.Session["ColaboradorId"] = usr.ColaboradorId;
                    System.Web.HttpContext.Current.Session["Usuario"] = usr.Usuario.ToString();
                    FormsAuthentication.SetAuthCookie(usr.Usuario.ToString(), createPersistentCookie: true);
                    if (System.Web.HttpContext.Current.Session["ColaboradorID"] != null)
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


            if (System.Web.HttpContext.Current.Session["Usuario"] != null)
            {

                return View();
            }

            else
            {
                FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.Session.Abandon();
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
                // ViewBag.ResultMessage = "Role created successfully !";
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
            if ((Request.Form["UserName"]) == "" || (Request.Form["RoleName"]) == "")
            {
                ViewBag.ResultMessage = "Não deixe o campo vazio!";
                return View();
            }
            var Depart = db.Roles.ToList();
            SelectList list = new SelectList(Depart, "RoleName", "RoleName");
            ViewBag.NomeRegra = list;

            db.UserRoles.Add(adiciona);
            db.SaveChanges();

            ViewBag.ResultMessage = "Regra adicionada com sucesso ao usuário !";
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
        public ActionResult GetRoles(string UserName)
        {
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
            
            return View("RoleAddToUser");
        }

        [HttpPost]
        public JsonResult AutocompletenoClick(string term)
        {

            var result = from r in db.userAccount
                          where r.Usuario == term
                          select new { r.Nome, r.Sobrenome };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult BindDepartamento(string term)
        {

            var result = from r in db.userAccount
                         where r.Usuario == term
                         select new { r.Departamentos };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult Autocomplete(string term)
        {

            var result = (from r in db.userAccount
                          where r.Usuario.ToLower().StartsWith(term.ToLower())
                          select new { r.Usuario }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Autocomplete2(string term)
        {

            var result = (from r in db.userAccount
                          where r.Nome.ToLower().StartsWith(term.ToLower())
                          select new { r.Nome, r.Sobrenome }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        //deletar usuario de uma regra
        [HttpPost]
       [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                Roles.RemoveUserFromRole(UserName, RoleName);
                ViewBag.ResultMessage = "Usuário removido deste grupo com sucesso!";
            }
            else
            {
                ViewBag.ResultMessage = "Este usuario não pertence ao grupo selecionado";
            }
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;


            return View("RoleAddToUser");
        }

    }
}