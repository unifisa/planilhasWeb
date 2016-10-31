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
            ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name");

            List<SelectListItem> li = new List<SelectListItem>();

            li.Add(new SelectListItem { Text = "Diretoria", Value = "Diretoria" });
            li.Add(new SelectListItem { Text = "Atendimento", Value = "Atendimento" });
            li.Add(new SelectListItem { Text = "Imóvel", Value = "Imóvel" });
            li.Add(new SelectListItem { Text = "RH", Value = "RH" });
            li.Add(new SelectListItem { Text = "TI", Value = "TI" });
            li.Add(new SelectListItem { Text = "Auditoria", Value = "Auditoria" });
            li.Add(new SelectListItem { Text = "Cobranca", Value = "Cobranca" });
            li.Add(new SelectListItem { Text = "Reativação", Value = "Reativação" });
            li.Add(new SelectListItem { Text = "Operações", Value = "Operações" });
            li.Add(new SelectListItem { Text = "Contabilidade", Value = "Contabilidade" });
            li.Add(new SelectListItem { Text = "Compras", Value = "Compras" });
            li.Add(new SelectListItem { Text = "Financeiro", Value = "Financeiro" });
            li.Add(new SelectListItem { Text = "Cadastro", Value = "Cadastro" });
            li.Add(new SelectListItem { Text = "Jurídico", Value = "Jurídico" });
            ViewData["departamentos"] = li;


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

                TempData["Msg"] = " Usuário nao foi cadastrado. Preencha todos os campos. ";
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
                    FormsAuthentication.SetAuthCookie(usr.Usuario.ToString(), true);
                    if (Session["ColaboradorID"] != null)
                    {


                        return RedirectToAction("Logado");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuario ou senha estao errados");
                }
            }
            return View();
        }





        //pagina Logado

        public ActionResult Logado()
        {


            if (Session["ColaboradorId"] != null)
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
        public ActionResult RoleCreate(string RoleName)
        {
            if ((Request.Form["RoleName"]) == "")
            {
                ViewBag.ResultMessage = "Não deixe o campo vazio!";
                return View();
            }

            if (Roles.RoleExists(RoleName))
            {
                ViewBag.ResultMessage = "Este grupo já existe!";
            }
            else { 
            Roles.CreateRole(Request.Form["RoleName"]);
               
            return RedirectToAction("RoleIndex", "Account");

            }
            // ViewBag.ResultMessage = "Role created successfully !";

            return View();
        }

        //lista todas as regras
        [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        //Excluir regra
        [Authorize(Roles = "Admin")]
        public ActionResult RoleDelete(string RoleName)
        {

            Roles.DeleteRole(RoleName);
            // ViewBag.ResultMessage = "Role deleted succesfully !";


            return RedirectToAction("RoleIndex", "Account");
        }

        //Adiciona regra a determinado usuario
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;

            return View();
        }



        /// <summary>
        /// Add role to the user
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string RoleName, string UserName)
        {
            if ((Request.Form["UserName"]) == "" || (Request.Form["RoleName"]) == "")
            {
                ViewBag.ResultMessage = "Não deixe o campo vazio!";
                return View();
            }

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                ViewBag.ResultMessage = "Este usuário ja pertence a esse grupo!";
            }
            else
            {
                Roles.AddUserToRole(UserName, RoleName);
                ViewBag.ResultMessage = "Usuário adicionado ao grupo com sucesso!";
            }

            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;
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
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
            }
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