using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

namespace AmeCaseBookOrg.Service
{
    public class FileService : IFileService
    {
        ApplicationDbContext dbContext;
        public FileService (IDbFactory dbFactory)
        {
            dbContext = dbFactory.Init();
        }
        public File getFile(int fileId)
        {
            return dbContext.Files.First(f => f.FileId == fileId);
        }
    }
}