using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DimensionalLegends.Controllers.Game
{
    public class MenuPrincipalController : Controller
    {
        //
        // GET: /MenuPrincipal/

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
