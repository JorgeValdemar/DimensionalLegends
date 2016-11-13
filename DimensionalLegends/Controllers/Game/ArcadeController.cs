using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DimensionalLegends.Controllers.Game
{
    public class ArcadeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index3D()
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult SelecaoCartas(string id)
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            int result = 0;
            if (int.TryParse(id.ToString(), out result))
            {
                ViewBag.faseId = result;
            }
            else
            {
                ViewBag.faseId = 0;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Arena(string faseId, string faseListaCartas)
        {
            if (Request.Cookies["UserSessionId"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            ViewBag.faseId = int.Parse(faseId.ToString());
            ViewBag.faseListaCartas = faseListaCartas;

            return View();
        }


    }
}
