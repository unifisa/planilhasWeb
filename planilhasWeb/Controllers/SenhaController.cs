using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Planilhas.Models;
using System.Web.Mvc;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Planilhas.Controllers
{

    public class SenhaController : Controller
    {

        public ActionResult AlterarSenha()
        {
            using (OurDbContext db = new OurDbContext())

            {
                return View();
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarSenha(int? ID, string Senha, string ConfirmeSenha)
        {
            using (OurDbContext db = new OurDbContext())
            {
                ID = (int)Session["ColaboradorId"];
                UserAccount usr = db.userAccount.Find(ID);              

                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (usr != null && Senha == ConfirmeSenha)
                {
                    if (Senha != usr.Senha && ConfirmeSenha != usr.ConfirmeSenha)
                    {
                        usr.Senha = Senha;
                        usr.ConfirmeSenha = ConfirmeSenha;
                        db.SaveChanges();
                        ModelState.Clear();
                        TempData["Message"] = "Senha alterada com sucesso ";
                        return View();
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["Msg"] = "Não coloque a senha atual. Tente novamente ";
                        return View();
                    }
                }
                else
                    {
                        if(Senha != ConfirmeSenha)
                        {
                        ModelState.Clear();
                        TempData["Msg"] = "Coloque senhas iguais";
                        return View();
                        }
                       
                       
                    
                }


                


                return View(usr);
            }
        }

    }
}
