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

namespace card.Aplicacao.Cartas
{
    /// <summary>
    /// Summary description for LerPlayerDeck
    /// </summary>
    public class LerPlayerDeck : IHttpHandler, IRequiresSessionState
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
                SqlCommand cmd = new SqlCommand("get_player_deck", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                List<Classes.Objetos.Card> IListaCard = new List<Classes.Objetos.Card>();

                while (rs.Read())
                {
                    Classes.Objetos.Card RCard = new Classes.Objetos.Card();

                    RCard.Numero = int.Parse(rs["Numero"].ToString());
                    RCard.Rank = int.Parse(rs["Rank"].ToString());
                    RCard.Nome = rs["Nome"].ToString();

                    RCard.A1 = rs["A1"].ToString();
                    RCard.A2 = rs["A2"].ToString();
                    RCard.A3 = rs["A3"].ToString();
                    RCard.B1 = rs["B1"].ToString();
                    RCard.B2 = rs["B2"].ToString();
                    RCard.C1 = rs["C1"].ToString();
                    RCard.C2 = rs["C2"].ToString();
                    RCard.C3 = rs["C3"].ToString();

                    RCard.Elemento = new Classes.Objetos.Elementos();
                    RCard.Elemento.ElementoId = int.Parse(rs["ElementoId"].ToString());
                    RCard.Elemento.Elemento = rs["Elemento"].ToString();

                    IListaCard.Add(RCard);

                }

                rs.Close();

                feed.ListaCards = IListaCard;
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