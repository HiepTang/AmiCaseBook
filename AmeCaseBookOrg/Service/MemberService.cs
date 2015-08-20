using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

namespace AmeCaseBookOrg.Service
{
    public class MemberService : IMemberService
    {
        private ApplicationDbContext context;

        public MemberService(IDbFactory dbFactory)
        {
            context = dbFactory.Init();
        }

        public IEnumerable<ApplicationUser> GetUserInRole(string roleName)
        {
            var role = context.Roles.SingleOrDefault(m => m.Name == roleName);
            var usersInRole = context.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));

            return usersInRole;
        }
    }
}