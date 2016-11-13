using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using Classes;
using System.Data;
using System.Web.Services;

namespace card.Aplicacao.Arena
{
    public class CriarSessoes : IRequiresSessionState
    {

        public CriarSessoes(HttpContext context, Classes.Objetos.Feedback feed)
        {
            if (context.Request.Cookies["UserSessionId"] == null)
            {
                feed.Erro = true;
                feed.ErroDescricao = "Usuário não está logado";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
            }
        }

    }
}