using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes.Objetos;
using DimensionalLegends.Models;

namespace DimensionalLegends.Controllers.Game
{
    public class PerfilController : Controller
    {
        private PlayerStatus IPlayer;
        private PlayersModel IPlayersModel;

        [HttpGet]
        public ActionResult Index(string id)
        {
            IPlayersModel = new PlayersModel();

            try
            {
                IPlayer = IPlayersModel.getPlayerByNick(id);
                ViewBag.Player = IPlayer;
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                ViewBag.Player = IPlayersModel.Player;
                //throw new Exception(ex.Message);
            }

            return View();
        }

    }
}
