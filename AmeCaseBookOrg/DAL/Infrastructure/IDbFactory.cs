using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;


namespace AmeCaseBookOrg.DAL.Infrastructure
{
    public interface IDbFactory : IDisposable
    {

        ApplicationDbContext Init();
    }
}
