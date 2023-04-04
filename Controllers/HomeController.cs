using ABCSupermarkertTask2.BlobHandler;
using ABCSupermarkertTask2.Models;
using ABCSupermarkertTask2.TableHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCSupermarkertTask2.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}