using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DimensionalLegends.Views
{
    public class MinhasCartasController : Controller
    {
        //
        // GET: /MinhasCartas/

        public ActionResult Index()
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }

            return View();
        }

    }
}
