using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Controllers
{
    public class CommunicationController : Controller
    {
        // GET: Communication
        [Route("Communication/Index/{code}/{countryCode}")]
        public ActionResult Index()
        {
            return View();
        }
    }
}