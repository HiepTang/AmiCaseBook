using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AmeCaseBookOrg.Service
{
    public class MemberService : IMemberService
    {
        private ApplicationDbContext context;

        public MemberService(IDbFactory dbFactory)
        {
            context = dbFactory.Init();
        }

        public ApplicationUser GetUser(string UserName)
        {
            var user = this.context.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            return user;
        }

        public IEnumerable<ApplicationUser> GetUserInRole(string roleName)
        {
            var role = context.Roles.SingleOrDefault(m => m.Name == roleName);
            var usersInRole = context.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            
            return usersInRole;
        }
        public IEnumerable<IdentityRole> GetUserRoles()
        {
            return context.Roles;
        }
    }
}