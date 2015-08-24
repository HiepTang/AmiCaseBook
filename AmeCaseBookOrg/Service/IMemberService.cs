using System.Collections.Generic;
using AmeCaseBookOrg.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AmeCaseBookOrg.Service
{
    public interface IMemberService
    {
        IEnumerable<ApplicationUser> GetUsers();

        IEnumerable<ApplicationUser> GetUserInRole(string roleName);
        IEnumerable<IdentityRole> GetUserRoles();
        IEnumerable<ApplicationUser> searchMember(MemberSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords);
        ApplicationUser GetUser(string UserName);
    }
}
