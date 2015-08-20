using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.Service
{
    public interface IMemberService
    {
        IEnumerable<ApplicationUser> GetUserInRole(string roleName);

    }
}
