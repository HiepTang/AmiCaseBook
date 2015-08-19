using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;
using System.Data.Entity;

namespace AmeCaseBookOrg.Service
{
    public class FileService : IFileService
    {
        ApplicationDbContext dbContext;
        public FileService (IDbFactory dbFactory)
        {
            dbContext = dbFactory.Init();
        }

        public File addFile(File file)
        {
            File outFile = dbContext.Files.Add(file);
            dbContext.SaveChanges();
            return outFile;
        }

        public bool deleteFile(File file)
        {
            dbContext.Files.Remove(file);
            int result = dbContext.SaveChanges();
            return result > 0;
        }

        public File getFile(int fileId)
        {
            return dbContext.Files.First(f => f.FileId == fileId);
        }

        public void saveFile()
        {
            dbContext.SaveChanges();
        }
    }
}