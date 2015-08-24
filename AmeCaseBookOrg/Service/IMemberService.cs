using System.Collections.Generic;
using AmeCaseBookOrg.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AmeCaseBookOrg.Service
{
    public interface IMemberService
    {
        IEnumerable<ApplicationUser> GetUserInRole(string roleName);
        IEnumerable<IdentityRole> GetUserRoles();

        ApplicationUser GetUser(string UserName);
    }
}
