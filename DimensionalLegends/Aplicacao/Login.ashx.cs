using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.SessionState;
using Classes;
using System.Data;

namespace card.Aplicacao
{
    /// <summary>
    /// Summary description for Login1
    /// </summary>
    public class Login1 : IHttpHandler, IRequiresSessionState
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private gamefunctions gameFunc = new gamefunctions();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();

            var data = context.Request.Form["data"];

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dados nulos";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.Login ILogin = new Classes.Objetos.Login();
            ILogin = JsonConvert.DeserializeObject<Classes.Objetos.Login>(data);


            // classe de conexão
            SqlConnection conex = new SqlConnection(conn);

            // data readers
            SqlDataReader rs = null;
            SqlDataReader rsUser = null;

            conex.Open();

            try
            {

                // verifica login valido
                SqlCommand cmd = new SqlCommand("get_login_existe", conex);
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 200).Value = gameFunc.validaSQL(ILogin.Email).ToString();
                cmd.Parameters.Add("@senha", SqlDbType.VarChar, 200).Value = gameFunc.validaSQL(ILogin.Senha).ToString();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    Classes.Objetos.Login RLogin = new Classes.Objetos.Login();

                    RLogin.id = rs["id"].ToString();

                    rs.Close();

                    //verifica se char existe para o usuario
                    SqlCommand cmd2 = new SqlCommand("get_char_existe", conex);
                    cmd2.Parameters.Add("@InternautaId", SqlDbType.VarChar, 200).Value = gameFunc.validaSQL(RLogin.id);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    rsUser = cmd2.ExecuteReader();

                    if (rsUser.Read())
                    {
                        RLogin.PlayerStatus = new Classes.Objetos.PlayerStatus();
                        RLogin.PlayerStatus.InternautaId = rsUser["InternautaId"].ToString();
                    }


                    context.Session.Clear();

                    if (RLogin.PlayerStatus != null)
                    {
                        var cookie = new HttpCookie("UserSessionId", RLogin.PlayerStatus.InternautaId);
                        cookie.Expires.AddYears(1);
                        context.Response.SetCookie(cookie);
                        RLogin.PlayerStatus.InternautaId = string.Empty;
                    }
                    else
                    {
                        var cookie = new HttpCookie("UserSessionIdTemp", RLogin.id);
                        cookie.Expires.AddHours(1);
                        context.Response.SetCookie(cookie);
                    }

                    //context.Session.Timeout = 90000;

                    // a senha não deve sair do servidor, nunca!
                    RLogin.id = string.Empty;
                    RLogin.Senha = string.Empty;

                    feed.Login = RLogin;

                    rsUser.Close();
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