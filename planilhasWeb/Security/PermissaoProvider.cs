//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Planilhas.Models;

//namespace Planilhas.Security
//{
//    public class PermissaoProvider : System.Web.Security.RoleProvider
//    {
//        public override string ApplicationName
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
//        {
//            throw new NotImplementedException();
//        }

//        public override void CreateRole(string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] GetAllRoles()
//        {
//            throw new NotImplementedException();
//        }

//        public override string[] GetRolesForUser(string username)
//        {
//            OurDbContext db = new OurDbContext();

//            UserRole user = db.UserRoles.FirstOrDefault(u => u.Usuario == username);

//            if (user == null)
//                return new string[] { };

//            var permissoes = (from u in db.UserRoles
//                      where u.Usuario == username
//                      select u.RoleName).ToList();


//            return permissoes.ToArray();

//        }

//        public override string[] GetUsersInRole(string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool IsUserInRole(string username, string roleName)
//        {
//            throw new NotImplementedException();
//        }

//        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool RoleExists(string roleName)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}