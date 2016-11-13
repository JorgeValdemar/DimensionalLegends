using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DimensionalLegends.Controllers.Game
{
    public class OpcoesController : Controller
    {
        //
        // GET: /Opcoes/

        public ActionResult Index()
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }

            return View();
        }

        public ActionResult Assistentes()
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }

            return View();
        }

    }
}
