using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
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
    /// Summary description for AtualizarPlayerDeck
    /// </summary>
    public class AtualizarPlayerDeck : IHttpHandler, IRequiresSessionState
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

            var data = context.Request["data"];

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dados nulos";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.Feedback IFeedEnvio = new Classes.Objetos.Feedback();
            //IFeedEnvio = JsonConvert.DeserializeObject<Classes.Objetos.Feedback>(data);
            
            IFeedEnvio.ListaCards = JsonConvert.DeserializeObject<List<Classes.Objetos.Card>>(data);

            if (IFeedEnvio.ListaCards.Count != 10)
            {
                feed.Erro = true;
                feed.ErroDescricao = "O numero de cartas no deck deve ser exatamente 10.";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }




            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();


            try
            {

                SqlCommand cmd = new SqlCommand("de_player_deck", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                for( int i = 0; i < IFeedEnvio.ListaCards.Count; i++ )
                {

                    SqlCommand cmd2 = new SqlCommand("ex_atualizar_player_deck", conex);
                    cmd2.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                    cmd2.Parameters.Add("@Numero", SqlDbType.Int).Value = IFeedEnvio.ListaCards[i].Numero;
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    rs = cmd2.ExecuteReader();

                    while (rs.Read())
                    {
                        int resultado;
                        resultado = int.Parse(rs["resultado"].ToString());
                        if (resultado == 1)
                        {
                            continue;
                        }
                        else
                        {
                            feed.Erro = true;
                            feed.ErroDescricao = "ID incorreto encontrado.";

                            string jsonErroID = JsonConvert.SerializeObject(feed);
                            context.Response.Write(jsonErroID);
                            return;
                        }
                    }

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