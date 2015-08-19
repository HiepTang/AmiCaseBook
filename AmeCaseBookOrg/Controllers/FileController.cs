using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Controllers
{
    public class FileController : Controller
    {
        private IFileService fileService;
        public FileController (IFileService service)
        {
            fileService = service;
        }
        // GET: File
        public ActionResult Index(int id)
        {
            var fileToRetrieve = fileService.getFile(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}