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

namespace card.Aplicacao.Assistentes
{
    /// <summary>
    /// Summary description for Listar
    /// </summary>
    public class Listar : IHttpHandler, IRequiresSessionState
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
                SqlCommand cmd = new SqlCommand("get_player_assistente_lista", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                List<Classes.Objetos.Assistente> IListaAssistente = new List<Classes.Objetos.Assistente>();

                while (rs.Read())
                {
                    Classes.Objetos.Assistente RAssistente = new Classes.Objetos.Assistente();
                    RAssistente.ChibiId = int.Parse(rs["ChibiId"].ToString());
                    RAssistente.Nome = rs["Nome"].ToString();
                    RAssistente.Imagem = rs["Imagem"].ToString();
                    RAssistente.Existe = bool.Parse(rs["Existe"].ToString());

                    IListaAssistente.Add(RAssistente);

                }

                rs.Close();

                feed.ListaAssistentes = IListaAssistente;
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