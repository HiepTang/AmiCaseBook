using AmeCaseBookOrg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Service
{
    public interface IFileService
    {
        File getFile(string fileName);
        File getFile (int fileId);
        File addFile (File file);
        bool deleteFile (File file);
        void saveFile();
    }
}