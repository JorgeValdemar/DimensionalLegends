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
    /// Summary description for PlayerStatus
    /// </summary>
    public class PlayerStatus : IHttpHandler, IRequiresSessionState
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
                SqlCommand cmd = new SqlCommand("get_player_status", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    Classes.Objetos.PlayerStatus RPlayerStatus = new Classes.Objetos.PlayerStatus();
                    RPlayerStatus.Coins = int.Parse(rs["Coins"].ToString());
                    RPlayerStatus.Imagem = rs["Imagem"].ToString();
                    RPlayerStatus.Level = int.Parse(rs["Level"].ToString());
                    RPlayerStatus.MaxHp = int.Parse(rs["MaxHp"].ToString());
                    RPlayerStatus.MaxMp = int.Parse(rs["MaxMp"].ToString());
                    RPlayerStatus.MaxSp = int.Parse(rs["MaxSp"].ToString());
                    RPlayerStatus.Nick = rs["Nick"].ToString();

                    feed.PlayerStatus = RPlayerStatus;

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