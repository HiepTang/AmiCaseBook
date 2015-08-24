using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Models
{
    public class MemberSearchFilter
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public MemberRoles? Role { get; set; }
    }
}