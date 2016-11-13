using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DimensionalLegends.Controllers.Game
{
    public class CadastroController : Controller
    {
        //
        // GET: /cadastro/

        public ActionResult Index()
        {
            if (Request.Cookies["UserSessionIdTemp"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

    }
}
