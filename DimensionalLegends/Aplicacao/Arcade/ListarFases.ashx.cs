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

namespace card.Aplicacao.Arcade
{
    /// <summary>
    /// Summary description for ListarFases
    /// </summary>
    public class ListarFases : IHttpHandler, IRequiresSessionState
    {
        private Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            new Aplicacao.Arena.CriarSessoes(context, feed);

            Classes.Objetos.PlayerStatus IPlayerStatus = new Classes.Objetos.PlayerStatus();


            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();

            try
            {
                List<Classes.Objetos.Fase> ListaFases = new List<Classes.Objetos.Fase>();

                SqlCommand cmd = new SqlCommand("get_player_fases", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    Classes.Objetos.Fase RFase = new Classes.Objetos.Fase();
                    RFase.Id = int.Parse(rs["Id"].ToString());
                    RFase.LiderId = int.Parse(rs["LiderId"].ToString());
                    RFase.FaseNome = rs["Fase"].ToString();

                    RFase.ArcadeLiderStatus = new Classes.Objetos.PlayerStatus();
                    RFase.ArcadeLiderStatus.Level = int.Parse(rs["Level"].ToString());
                    RFase.ArcadeLiderStatus.Nick = rs["Nome"].ToString();
                    RFase.ArcadeLiderStatus.Imagem = rs["Imagem"].ToString();


                    RFase.ListaDrop = ListaDrop(RFase.Id);
                    ListaFases.Add(RFase);

                
                }

                feed.ListaFases = ListaFases;
                
                rs.Close();
                

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


        private List<Classes.Objetos.Card> ListaDrop(int FaseId)
        {

            List<Classes.Objetos.Card> ListaCards = new List<Classes.Objetos.Card>();
            
            // classe de conexão
            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs2 = null;
            conex.Open();

            try
            {

                SqlCommand cmd2 = new SqlCommand("get_arcade_drop", conex);
                cmd2.Parameters.Add("@FaseId", SqlDbType.Int, 60).Value = FaseId;
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                rs2 = cmd2.ExecuteReader();

                while (rs2.Read())
                {
                    Classes.Objetos.Card ICardDrop = new Classes.Objetos.Card();

                    ICardDrop.Numero = int.Parse(rs2["Numero"].ToString());
                    ICardDrop.Nome = rs2["Nome"].ToString();
                    ICardDrop.Rank = int.Parse(rs2["Rank"].ToString());

                    ListaCards.Add(ICardDrop);
                }

                rs2.Close();
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

            return ListaCards;
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