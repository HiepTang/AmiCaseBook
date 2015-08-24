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

        public IEnumerable<ApplicationUser> searchMember(MemberSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords)
        {
            IEnumerable<ApplicationUser> users = context.Users;
            if (!string.IsNullOrEmpty(filter.Email))
            {
                users = users.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
            }
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                users = users.Where(u => u.FullName.ToLower().Contains(filter.UserName.ToLower()));
            }
            if(filter.Role != null)
            {
                var role = context.Roles.SingleOrDefault(m => m.Name == filter.Role.Value.ToString());
                users = users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            }
            totalRecords = users.Count();
            return users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}