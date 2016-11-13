using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.SessionState;
using Classes;
using System.Data;

namespace card.Aplicacao.Utils
{
    /// <summary>
    /// Responsável pelas informações de opções e do assistente usado pelo player
    /// </summary>
    public class Opcoes : IHttpHandler, IRequiresSessionState
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();

            if (context.Request.Cookies["UserSessionId"] == null)
            {
                feed.Erro = true;
                feed.ErroDescricao = "Usuário não está logado";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.PlayerStatus IPlayerStatus = new Classes.Objetos.PlayerStatus();

            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("get_player_opcoes", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    Classes.Objetos.Opcoes ROpcoes = new Classes.Objetos.Opcoes();
                    ROpcoes.Idioma = int.Parse(rs["Idioma"].ToString());
                    ROpcoes.Som = int.Parse(rs["Som"].ToString());
                    ROpcoes.ChibiAssistente = int.Parse(rs["ChibiAssistente"].ToString());

                    ROpcoes.Assistente = new Classes.Objetos.Assistente();
                    ROpcoes.Assistente.ChibiId = int.Parse(rs["ChibiId"].ToString());
                    ROpcoes.Assistente.Nome = rs["Nome"].ToString();
                    ROpcoes.Assistente.Imagem = rs["Imagem"].ToString();


                    feed.Opcoes = ROpcoes;

                    rs.Close();
                }

                feed.Erro = false;
            }
            catch (Exception ex)
            {
                feed.Erro = true;
                feed.ErroDescricao = ex.ToString();
            }
            finally
            {
                conex.Close();
            }

            string json = JsonConvert.SerializeObject(feed);

            context.Response.Write(json);



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}