using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.DAL.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        ApplicationDbContext dbContext;

        public ApplicationDbContext Init()
        {
            return dbContext ?? (dbContext = new ApplicationDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}